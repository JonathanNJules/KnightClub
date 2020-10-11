using Photon.Pun;
using System.Linq;
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
        }
        
    }

    private void StartGame()
    {
        startedGame = true;
        PhotonNetwork.LocalPlayer.NickName = user.username;
        PhotonNetwork.Instantiate("Player", Vector3.zero, Quaternion.identity, 0);
    }
}