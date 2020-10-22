using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField emailIPF, passwordIPF;
    public CanvasFader cf;

    private void Start()
    {
        //Login();
    }
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
    }

    public IEnumerator TransitionToMainMenu()
    {
        cf.Fade(false, 2);
        yield return new WaitForSeconds(0.8f);
        SceneManager.LoadScene("MainMenu");
    }
}