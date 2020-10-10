using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviourPunCallbacks
{
    private Rigidbody rb;
    private Vector3 moveVector;
    public float moveSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        PhotonNetwork.ConnectUsingSettings();
    }

    void Update()
    {
        moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (moveVector.sqrMagnitude > 1) moveVector = moveVector.normalized;
    }

    void FixedUpdate()
    {
        rb.velocity = moveVector * moveSpeed;
        float ang;
        if (rb.velocity.sqrMagnitude > 0.1f)
            ang = Mathf.Atan2(rb.velocity.z, rb.velocity.x) * Mathf.Rad2Deg - 90;
        else
            ang = 0;
        transform.eulerAngles = new Vector3(0, -ang, 0);
    }
}
