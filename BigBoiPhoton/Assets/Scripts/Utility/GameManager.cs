using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviourPunCallbacks
{
    public static string username;
    public static string jwt;
    public static GameManager inst;
    private bool startedGame;
    public static GameObject player;

    void Start()
    {
        if (inst == null) inst = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
        PlayerPrefs.DeleteAll();

        //PhotonNetwork.AutomaticallySyncScene = true;

        if(SceneManager.GetActiveScene().name == "LoginMenu")
        {
            string j = PlayerPrefs.GetString("jwt", "");
            string u = KnightClubAPI.LoginWithJWT(jwt);
            if(u != null)
            {
                jwt = j;
                username = u;
                StartCoroutine(GameObject.Find("Login Manager").GetComponent<LoginManager>().TransitionToMainMenu());
            }
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Main" && !startedGame)
            StartGame();
    }

    void Update()
    {
        
    }

    private void StartGame()
    {
        startedGame = true;
        PhotonNetwork.LocalPlayer.NickName = username;
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
    }
}