using UnityEngine;

public class Flopper : MonoBehaviour
{
    public GameEnder e;

    private int reward = 15;
    private string returnScene = "Balcony";

    void Start()
    {
        GameObject.Find("Player").transform.position = Vector3.zero;
        e.reward = reward;
        e.returnScene = returnScene;
    }
}