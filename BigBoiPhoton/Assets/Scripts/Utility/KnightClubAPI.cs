using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using UnityEngine;

public class KnightClubAPI : MonoBehaviour
{
    private static readonly string serverLocation = "https://knightclub-qpzebhklia-ue.a.run.app/api";

    public static User LoginWithUsernamePassword(string email, string password)
    {
        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password)) return null;

        // TEST
        //User u = new User();
        //u.username = email;
        //return u;

        var form = new Dictionary<string, string>
        {
            {"email", email },
            {"password", password}
        };

        string response = MakeRequest("login", false, form);
        return JsonUtility.FromJson<User>(response);
    }

    public static void ChangeCurrency(int change)
    {
        var form = new Dictionary<string, string>
        {
            { "currencyChange", change.ToString() }
        };
        MakeRequest("changeCurrency", true, form);
    }

    public static string LoginWithJWT(string jwt)
    {
        if (string.IsNullOrEmpty(jwt)) return null;

        // Temporary
        return "RestoredUsernameFromJWT";
    }


    private static string MakeRequest(string route, bool isPost = false, Dictionary<string, string> form = null)
    {
        string jsonResponse;

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

        print(route);

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{serverLocation}/{route}");
        request.Method = isPost ? "POST" : "GET";
        //request.Headers.Add(HttpRequestHeader.Authorization, $"Bearer {jwt}");

        if (isPost == true && form != null)
        {
            request.ContentType = "application/json";
            using (var streamWriter = new StreamWriter(request.GetRequestStream()))
            {
                StringBuilder formString = new StringBuilder();
                formString.Append('{');
                foreach (KeyValuePair<string, string> formEntry in form)
                {
                    formString.Append($"\"{formEntry.Key}\":\"{formEntry.Value}\",");
                }
                if (formString.Length > 1)
                    formString.Remove(formString.Length - 1, 1);
                formString.Append('}');

                streamWriter.Write(formString.ToString());
            }
        }

        try
        {
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            using (StreamReader reader = new StreamReader(response.GetResponseStream()))
            {
                jsonResponse = reader.ReadToEnd();
            }

            return jsonResponse;

        }
        catch
        {
            return null;
        }
    }
}
