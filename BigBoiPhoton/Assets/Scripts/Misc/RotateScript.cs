using UnityEngine;

public class RotateScript : MonoBehaviour
{
    public Vector3 r;
    void Update()
    {
        transform.Rotate(r * Time.deltaTime * 10);
    }
}