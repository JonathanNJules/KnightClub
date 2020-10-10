using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameIPF, passwordIPF;
    public CanvasFader cf;
    public void Login()
    {
        string u = usernameIPF.text;
        string p = passwordIPF.text;

        string j = KnightClubAPI.LoginWithUsernamePassword(u, p);

        if(j != null)
        {
            GameManager.username = u;
            GameManager.jwt = j;
            StartCoroutine(TransitionToMainMenu());
        }
    }

    public IEnumerator TransitionToMainMenu()
    {
        cf.Fade(false, 2);
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene("MainMenu");
    }
}