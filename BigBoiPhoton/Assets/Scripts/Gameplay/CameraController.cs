using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target, targetLook;
    public Vector3  followDirection;
    public float followDistance;
    public float minZoom = 5, maxZoom = 25;
    public float scrollSpeed;
    public float followL;

    void Update()
    {
        followDistance -= Input.GetAxis("Mouse ScrollWheel") * scrollSpeed * Time.deltaTime * 100;
        followDistance = Mathf.Clamp(followDistance, minZoom, maxZoom);
    }

    void FixedUpdate()
    {
        transform.LookAt(targetLook);
        Vector3 targetPos = target.position + followDirection.normalized * followDistance;
        transform.position = Vector3.Lerp(transform.position, targetPos, followL * Time.deltaTime);
    }
}
