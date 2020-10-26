using UnityEngine;

public class Pet : MonoBehaviour
{
    public int health = 100;
    public int hunger = 100;
    public int hygine = 100;
    public int happiness = 100;

    private bool moving;
    public Transform target;
    private Transform model;
    private bool initialized;

    public float jumpHeight = 0.4f;
    public float hopSpeed = 40;
    public float moveInfluence = 1;

    void Start()
    {
        DontDestroyOnLoad(this);
        model = transform.GetChild(0);
    }

    void Update()
    {
        //if (!initialized) return;

        Vector3 moveV = target.position - transform.position;

        if (!moving && moveV.magnitude > 0.5f)
            moving = true;

        if (moving && moveV.magnitude < 0.5f)
        {
            moving = false;
            model.localPosition = Vector3.zero;
        }

        if(moving)
        {
            transform.position += moveV * Time.deltaTime;
            transform.LookAt(target);
            model.localPosition = new Vector3(0, jumpHeight * (Mathf.Sin(Time.timeSinceLevelLoad * hopSpeed + (moveV.magnitude * moveInfluence)) + 1), 0);
        }
    }

    public void Initialize(Transform newTarget)
    {
        target = newTarget;
        initialized = true;
    }
}
