using UnityEngine;

public class GameEnder : MonoBehaviour
{
    public string returnScene = "";
    public int reward = 0;

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            KnightClubAPI.ChangeCurrency(AddedCurrency, reward, GameManager.user.username);
            other.GetComponent<Player>().ChangeScenes(returnScene);
        }
    }

    public void AddedCurrency(string res)
    {
        string newMoniesS = res.Substring(res.IndexOf(':') + 1);
        newMoniesS = newMoniesS.Remove(newMoniesS.Length - 2);

        print("s: " + newMoniesS);

        int newMonies = int.Parse(newMoniesS);

        if (newMonies != -1)
            GameManager.inst.UpdateCurrency(newMonies);

        print("new money: " + res);
    }
}