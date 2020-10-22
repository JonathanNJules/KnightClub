using Photon.Pun;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static User user;
    public static string jwt;
    public static GameManager inst;
    private bool startedGame;
    public static GameObject player;
    public static string usersScene = "Main";

    private TMP_Text currencyText;

    public TMP_Text TestText;

    void Start()
    {
        if (inst == null) inst = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        if(SceneManager.GetActiveScene().name == "LoginMenu")
        {
            PlayerPrefs.DeleteAll();
            string j = PlayerPrefs.GetString("jwt", "");
            string e = KnightClubAPI.LoginWithJWT(jwt);
            if(e != null)
            {
                jwt = j;
                StartCoroutine(GameObject.Find("Login Manager").GetComponent<LoginManager>().TransitionToMainMenu());
            }
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if(scene.name != "LoginMenu" && scene.name != "MainMenu")
        {
            if (!startedGame)
                StartGame();

            currencyText = GameObject.Find("Currency Text").GetComponent<TMP_Text>();
            UpdateCurrency(user.currency);
        }
    }

    private void StartGame()
    {
        startedGame = true;
        PhotonNetwork.LocalPlayer.NickName = user.username;
        GameObject g = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
        g.name = "Player";
        g.GetComponent<Player>().photonView.RPC("SetHeadwear", RpcTarget.AllBuffered, user.shirt);
    }

    public void UpdateCurrency(int newCurrency)
    {
        user.currency = newCurrency;
        currencyText.text = "$" + user.currency;
    }

    public void SetTestText(string t)
    {
        TestText.text = t;
    }
}