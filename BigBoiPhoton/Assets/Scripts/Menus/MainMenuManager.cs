using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class MainMenuManager : MonoBehaviourPunCallbacks
{
    public CanvasFader buttonsAndTitleCF;
    public Transform embellishment1, embellishment2;
    private Vector3 e1pos, e2pos;
    private Quaternion e1rot, e2rot;
    private bool moveEmb1, moveEmb2;
    public TMP_Text welcomeText;

    public GameObject[] submenus;

    public List<RoomInfo> savedRoomList;
    public TMP_InputField newServerNameIPF, newServerMaxPlayersIPF;
    public UnityEngine.UI.Toggle newServerVisibleToggle;
    public TMP_InputField manualServerNameIPF;
    public ServerList sl;

    public GameObject errorPopupPrefab;
    public Transform errorsParent;

    void Start()
    {
        buttonsAndTitleCF.Fade(true, 2);
        welcomeText.text = $"Welcome, {GameManager.username}!";

        e1pos = new Vector3(-356, 62, -5);
        e1rot = new Quaternion(-0.03201738f, 0.06985322f, 0.05429835f, 0.9955637f);
        e2pos = new Vector3(-528.95f, -69.2f, -12.39f);
        e2rot = new Quaternion(0.04392721f, 0.03913203f, 0.1983917f, 0.9783557f);

        StartCoroutine(AnimateEmbellishments());

        PhotonNetwork.ConnectUsingSettings();
        savedRoomList = new List<RoomInfo>();
    }

    void Update()
    {
        if (moveEmb1)
        {
            embellishment1.localPosition = Vector3.Lerp(embellishment1.localPosition, e1pos, 12 * Time.deltaTime);
            embellishment1.localRotation = Quaternion.Lerp(embellishment1.localRotation, e1rot, 12 * Time.deltaTime);
        }

        if (moveEmb2)
        {
            embellishment2.localPosition = Vector3.Lerp(embellishment2.localPosition, e2pos, 12 * Time.deltaTime);
            embellishment2.localRotation = Quaternion.Lerp(embellishment2.localRotation, e2rot, 12 * Time.deltaTime);
        }
    }

    private IEnumerator AnimateEmbellishments()
    {
        yield return new WaitForSeconds(.1f);
        moveEmb1 = true;
        yield return new WaitForSeconds(.1f);
        moveEmb2 = true;
        yield return new WaitForSeconds(1);
        moveEmb1 = moveEmb2 = false;
    }

    public override void OnConnectedToMaster()
    {
        print("just connected");
        PhotonNetwork.JoinLobby();
    }

    public void Logout()
    {
        PlayerPrefs.DeleteKey("jwt");
        SceneManager.LoadScene("LoginMenu");
    }

    public void PickSubmenu(int menu)
    {
        menu--;

        for (int i = 0; i < 3; i++)
        {
            if (menu == i)
            {
                submenus[i].SetActive(true);
                submenus[i].GetComponent<CanvasFader>().Fade(true, 2.5f);
            }
            else
            {
                submenus[i].GetComponent<CanvasGroup>().alpha = 0;
                submenus[i].SetActive(false);
            }
        }
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        savedRoomList = roomList;
        if (sl.gameObject.activeSelf) UpdateServerListUI();
    }

    private void UpdateServerListUI()
    {
        sl.UpdateServerListUI();
    }

    public void CreateServer()
    {
        if(PhotonNetwork.NetworkClientState != ClientState.JoinedLobby)
        {
            GiveErrorMessage("Tried to join a bit too early! Still connecting to lobby");
            return;
        }

        string name = newServerNameIPF.text;
        int maxPlayers = int.Parse(newServerMaxPlayersIPF.text);
        bool isVisible = newServerVisibleToggle.isOn;

        RoomOptions ro = new RoomOptions()
        {
            MaxPlayers = (byte)maxPlayers,
            IsVisible = isVisible
        };

        PhotonNetwork.CreateRoom(name, ro);
    }

    public void JoinServer(string serverName)
    {
        if (PhotonNetwork.NetworkClientState != ClientState.JoinedLobby)
        {
            GiveErrorMessage("Tried to join a bit too early! Still connecting to lobby");
            return;
        }

        PhotonNetwork.JoinRoom(serverName);
    }

    public void JoinServerManually()
    {
        JoinServer(manualServerNameIPF.text);
    }

    public override void OnJoinedRoom()
    {
        SceneManager.LoadScene("Main");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        GiveErrorMessage(message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        GiveErrorMessage(message);
    }

    private void GiveErrorMessage(string message)
    {
        GameObject g = Instantiate(errorPopupPrefab, errorsParent);
        g.transform.localPosition = Vector3.zero;
        g.transform.GetChild(1).GetComponent<TMP_Text>().text = "Message: " + message;
    }
}
