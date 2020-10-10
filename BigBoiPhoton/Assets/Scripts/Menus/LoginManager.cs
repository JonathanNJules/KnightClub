using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameIPF, passwordIPF;
    public void Login()
    {
        string u = usernameIPF.text;
        string p = usernameIPF.text;

        string j = KnightClubAPI.LoginWithUsernamePassword(u, p);

        if(j != null)
        {
            GameManager.username = u;
            GameManager.jwt = j;
            SceneManager.LoadScene("MainMenu");
        }
    }
}