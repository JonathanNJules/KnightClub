using Photon.Pun;
using UnityEngine;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    private Rigidbody rb;
    public Transform camLookT;
    private Vector3 moveVector;
    public float moveSpeed;

    private Vector3 networkedPos;
    private Quaternion networkedRot;
    public float networkSmoothingSpeed;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        CameraController cc = GameObject.Find("Main Camera").GetComponent<CameraController>();
        cc.target = transform;
        cc.targetLook = camLookT;
        cc.enabled = true;

        if (photonView.IsMine) rb.isKinematic = false;
    }

    void Update()
    {
        if (photonView.IsMine) OwnerUpdate();
        else NetworkedUpdate();
    }

    void OwnerUpdate()
    {
        moveVector = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        if (moveVector.sqrMagnitude > 1) moveVector = moveVector.normalized;
        moveVector *= moveSpeed;
    }

    void NetworkedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, networkedPos, networkSmoothingSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, networkedRot, networkSmoothingSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        rb.velocity = moveVector * Time.deltaTime;
        float ang = (moveVector.sqrMagnitude > 0.1f) ? Mathf.Atan2(moveVector.z, moveVector.x) * Mathf.Rad2Deg - 90 : 180;
        transform.eulerAngles = new Vector3(0, -ang, 0);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else
        {
            networkedPos = (Vector3)stream.ReceiveNext();
            networkedRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
