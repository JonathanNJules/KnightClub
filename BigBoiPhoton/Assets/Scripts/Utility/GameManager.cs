using JetBrains.Annotations;
using Photon.Pun;
using System.Collections;
using System.Threading;
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

    private static string targetShirt;

    void Start()
    {
        if (inst == null) inst = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        if (SceneManager.GetActiveScene().name == "LoginMenu")
        {
            PlayerPrefs.DeleteAll();
            string j = PlayerPrefs.GetString("jwt", "");
            string e = KnightClubAPI.LoginWithJWT(jwt);
            if (e != null)
            {
                jwt = j;
                StartCoroutine(GameObject.Find("Login Manager").GetComponent<LoginManager>().TransitionToMainMenu());
            }
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "LoginMenu" && scene.name != "MainMenu")
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
        player = PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
        player.name = "Player";
        KnightClubAPI.GetItem(SetShirtFromAPI, user.username, "shirt");
    }

    public void SetShirtFromAPI(string shirt)
    {
        user.shirt = shirt.Replace("\"", "");
        player.GetComponent<Player>().photonView.RPC("SetHeadwear", RpcTarget.AllBuffered, user.shirt);
    }

    public void UpdateCurrency(int newCurrency)
    {
        user.currency = newCurrency;
        currencyText.text = "$" + user.currency;
    }

    public static void SetHeadwear(string itemname, int cost)
    {
        targetShirt = itemname;
        KnightClubAPI.ChangeCurrency(SetHeadwearP2, -cost, user.username);
    }

    private static void SetHeadwearP2(string res)
    {
        string newMoniesS = res.Substring(res.IndexOf(':') + 1);
        newMoniesS = newMoniesS.Remove(newMoniesS.Length - 2);

        int newMonies = int.Parse(newMoniesS);

        if (newMonies != -1)
        {
            inst.UpdateCurrency(newMonies);
            KnightClubAPI.SetItem(SetShirtCompleted, user.username, "shirt", targetShirt);
        }
    }

    private static void SetShirtCompleted(string res)
    {
        KnightClubAPI.GetItem(inst.SetShirtFromAPI, user.username, "shirt");
    }
}