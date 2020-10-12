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

        var form = new Dictionary<string, string>
        {
            {"email", email },
            {"password", password}
        };

        string response = MakeRequest("login", false, form);

        User u = new User { username = email }; //JsonUtility.FromJson<User>(response);

        return u;
    }

    public static void ChangeCurrency(int money, string username)
    {
        return;
        var form = new Dictionary<string, string>
        {
            { "username", username },
            { "money", money.ToString() }
        };
        
        string response = MakeRequest("changeBalance", false, form);

        string newMoniesS = response.Substring(response.IndexOf(':') + 1);
        newMoniesS = newMoniesS.Remove(newMoniesS.Length - 2);

        print("s: " + newMoniesS);

        int newMonies = int.Parse(newMoniesS);

        if (newMonies != -1)
            GameManager.inst.UpdateCurrency(newMonies);

        print("new money: " + response);
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


    private static string MakeRequest(string route, bool isPost = false, Dictionary<string, string> form = null)
    {
        return null;
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

        HttpWebRequest request = (HttpWebRequest)WebRequest.Create($"{serverLocation}/{route}");
        request.Method = isPost ? "POST" : "GET";

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