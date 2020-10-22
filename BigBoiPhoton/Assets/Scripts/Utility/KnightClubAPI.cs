using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class KnightClubAPI : MonoBehaviour
{
    private static readonly string serverLocation = "https://knightclub-qpzebhklia-ue.a.run.app/api";

    public static KnightClubAPI instance;

    private void Start()
    {
        instance = this;
    }

    public static void LoginWithUsernamePassword(Action<string> postRequestMethod, string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            return;

        var form = new Dictionary<string, string>
        {
            {"email", email },
            {"password", password}
        };

        instance.StartCoroutine(MakeRequest("login", postRequestMethod, false, form));
    }

    public static void ChangeCurrency(Action<string> postRequestMethod,  int money, string username)
    {
        var form = new Dictionary<string, string>
        {
            { "username", username },
            { "money", money.ToString() }
        };

        instance.StartCoroutine(MakeRequest("changeBalance", postRequestMethod, false, form));
    }

    public static void BuyHeadwear()
    {
        //string response = "";
    }

    public static string LoginWithJWT(string jwt)
    {
        if (string.IsNullOrEmpty(jwt)) return null;

        // Temporary
        return "RestoredUsernameFromJWT";
    }

    private static IEnumerator MakeRequest(string route, Action<string> postRequestMethod, bool isPost = false, Dictionary<string, string> form = null)
    {
        List<IMultipartFormSection> formData = null;

        if (isPost == false && form != null)
        {
            route += "?";
            bool firsted = false;
            foreach (KeyValuePair<string, string> pair in form)
            {
                if (firsted == true) route += "&";
                else firsted = true;
                route += pair.Key;
                route += "=";
                route += pair.Value;
            }
        }

        if (isPost == true && form != null)
        {
            foreach (KeyValuePair<string, string> formEntry in form)
            {
                formData = new List<IMultipartFormSection>();
                formData.Add(new MultipartFormFileSection($"\"{formEntry.Key}\"", "\"{formEntry.Value}\""));
            }
        }

        UnityWebRequest uwr;
        if (isPost == false)
            uwr = UnityWebRequest.Get($"{serverLocation}/{route}");
        else
            uwr = UnityWebRequest.Post($"{serverLocation}/{route}", formData);
        yield return uwr.SendWebRequest();

        if (uwr.isNetworkError || uwr.isHttpError)
        {
            postRequestMethod(null);
            Debug.Log(uwr.error);
        }
        else
        {
            postRequestMethod(uwr.downloadHandler.text);
            print(uwr.downloadHandler.text);
        }
    }
}