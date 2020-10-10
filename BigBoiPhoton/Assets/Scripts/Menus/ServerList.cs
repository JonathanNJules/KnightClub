using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ServerList : MonoBehaviourPunCallbacks
{
    public Transform serverListUIParent;
    public GameObject serverElementPrefab;
    public MainMenuManager mmm;

    public override void OnEnable()
    {
        UpdateServerListUI();
    }

    public void UpdateServerListUI()
    {
        int i;

        for (i = serverListUIParent.childCount - 1; i >= 0; i--)
            Destroy(serverListUIParent.GetChild(i).gameObject);

        for (i = 0; i < mmm.savedRoomList.Count; i++)
        {
            GameObject s = Instantiate(serverElementPrefab, serverListUIParent);
            s.transform.GetChild(0).GetComponent<TMP_Text>().text = mmm.savedRoomList[i].Name;
            s.transform.GetChild(1).GetComponent<TMP_Text>().text = $"{mmm.savedRoomList[i].PlayerCount}/{mmm.savedRoomList[i].MaxPlayers} Players";
            print("len: " + mmm.savedRoomList.Count + ", i: " + i);
            string theOne = mmm.savedRoomList[i].Name;
            s.GetComponent<Button>().onClick.AddListener(delegate { mmm.JoinServer(theOne); });
        }
    }


}
