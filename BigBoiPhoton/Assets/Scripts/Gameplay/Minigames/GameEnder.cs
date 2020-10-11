using UnityEngine;

public class GameEnder : MonoBehaviour
{
    public string returnScene = "";
    public int reward = 0;

    public void OnTriggerEnter(Collider other)
    {
        if(other.tag.Equals("Player"))
        {
            KnightClubAPI.ChangeCurrency(reward, GameManager.user.username);
            other.GetComponent<Player>().ChangeScenes(returnScene);
        }
    }
}
