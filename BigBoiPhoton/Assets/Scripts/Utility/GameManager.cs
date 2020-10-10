using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static string username;
    public static string jwt;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        print("Start");
        if(SceneManager.GetActiveScene().name == "LoginMenu")
        {
            string j = PlayerPrefs.GetString("jwt", "");
            string u = KnightClubAPI.LoginWithJWT(jwt);
            if(u != null)
            {
                jwt = j;
                username = u;
                SceneManager.LoadScene("MainMenu");
            }
        }
    }

    void Update()
    {
        
    }
}