using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneDoor : MonoBehaviour
{
    public string newScene;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
            other.GetComponent<Player>().ChangeScenes(newScene);
    }
}
