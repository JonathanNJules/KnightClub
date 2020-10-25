using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pet : MonoBehaviour
{
    public int health = 100;
    public int hunger = 100;
    public int hygine = 100;
    public int happiness = 100;

    private bool moving;
    public Transform target;


    void Start()
    {
        
    }

    void Update()
    {
        if (!moving && Vector3.Distance(transform.position, target.position) > 0.5f)
            moving = true;

        if (moving && Vector3.Distance(transform.position, target.position) < 0.5f)
            moving = false;
    }
}
