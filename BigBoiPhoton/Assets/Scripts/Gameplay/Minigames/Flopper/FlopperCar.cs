using UnityEngine;

public class FlopperCar : MonoBehaviour
{
    private Vector3 startPos;
    private float currentSpeed;

    public float minSpeed, maxSpeed;

    void Start()
    {
        startPos = transform.position;
        Roll();
    }

    void Update()
    {
        transform.position -= new Vector3(currentSpeed * Time.deltaTime, 0, 0);
        if (transform.position.x < -30)
            Roll();
    }

    void Roll()
    {
        transform.position = startPos;
        currentSpeed = Random.Range(minSpeed, maxSpeed);

        int chosenChild = Random.Range(0, transform.childCount);
        for (int i = 0; i < transform.childCount; i++)
            transform.GetChild(i).gameObject.SetActive(i == chosenChild);
    }

    private void OnCollisionEnter(Collision collision)
    {
        print("Collided! " + collision.gameObject.tag);
        if (collision.gameObject.tag.Equals("Player"))
            collision.transform.position = Vector3.zero;
    }
}
