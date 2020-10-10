using UnityEngine;

public class KnightClubAPI : MonoBehaviour
{
    public static string LoginWithUsernamePassword(string username, string password)
    {
        if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password)) return null;

        // Temporary
        return "thisisafakejwt";
    }

    public static string LoginWithJWT(string jwt)
    {
        if (string.IsNullOrEmpty(jwt)) return null;

        // Temporary
        return "RestoredUsernameFromJWT";
    }
}
