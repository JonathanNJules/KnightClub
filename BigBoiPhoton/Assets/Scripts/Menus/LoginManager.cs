using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField emailIPF, passwordIPF;
    public CanvasFader cf, failedCF;
    public void Login()
    {
        string e = emailIPF.text;
        string p = passwordIPF.text;

        KnightClubAPI.LoginWithUsernamePassword(GotLogin, e, p);
    }

    public void GotLogin(string res)
    {
        User u = JsonUtility.FromJson<User>(res);
        if (u != null)
        {
            GameManager.user = u;
            StartCoroutine(TransitionToMainMenu());
        }
        else
            StartCoroutine(GiveFailedMessage());
    }

    public IEnumerator TransitionToMainMenu()
    {
        cf.Fade(false, 2);
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene("MainMenu");
    }

    public IEnumerator GiveFailedMessage()
    {
        failedCF.Fade(true, 2);
        yield return new WaitForSeconds(3);
        failedCF.Fade(false, 0.5f);
    }
}