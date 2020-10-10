using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    private Rigidbody rb;
    public Transform model;
    public Transform camLookT;
    private Vector3 moveVector;
    public float moveSpeed;

    private Vector3 networkedPos;
    private Quaternion networkedRot;
    public float networkSmoothingSpeed;

    public TMP_Text nameText;
    public Transform chatMessageParent;
    public GameObject chatMessagePrefab;
    private ChatMessageRelayer cmr;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        nameText.text = photonView.Owner.NickName;

        if (!photonView.IsMine) return;

        rb = GetComponent<Rigidbody>();
        cmr = GameObject.Find("Main Canvas").GetComponent<ChatMessageRelayer>();
        print($"1: {GameObject.Find("Main Canvas")} | 2: {GameObject.Find("Main Canvas").GetComponent<ChatMessageRelayer>()} | 3: {cmr}");
        cmr.p = this;

        CameraController cc = GameObject.Find("Main Camera").GetComponent<CameraController>();
        cc.target = transform;
        cc.targetLook = camLookT;
        cc.enabled = true;

        rb.isKinematic = false;
    }

    void Update()
    {
        if (photonView.IsMine) OwnerUpdate();
        else NetworkedUpdate();
    }

    void OwnerUpdate()
    {
        // Only move if not in chat box
        print($"later = 1: {GameObject.Find("Main Canvas")} | 2: {GameObject.Find("Main Canvas").GetComponent<ChatMessageRelayer>()} | 3: {cmr}");
        moveVector = cmr.inChatBox ? Vector3.zero : new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (moveVector.sqrMagnitude > 1) moveVector = moveVector.normalized;
        moveVector *= moveSpeed;
    }

    void NetworkedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, networkedPos, networkSmoothingSpeed * Time.deltaTime);
        model.rotation = Quaternion.Lerp(model.rotation, networkedRot, networkSmoothingSpeed * Time.deltaTime);
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        rb.velocity = moveVector * Time.deltaTime;
        if (moveVector.sqrMagnitude > 0.5f)
        {
            float ang = Mathf.Atan2(moveVector.z, moveVector.x) * Mathf.Rad2Deg - 90;
            model.eulerAngles = new Vector3(0, Mathf.LerpAngle(model.eulerAngles.y, -ang, 15 * Time.deltaTime), 0);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(model.rotation);
        }
        else
        {
            networkedPos = (Vector3)stream.ReceiveNext();
            networkedRot = (Quaternion)stream.ReceiveNext();
        }
    }

    public void NetworkSendChatMessage(string message)
    {
        photonView.RPC("SendChatMessage", RpcTarget.All, message);
    }

    [PunRPC]
    public void SendChatMessage(string message)
    {
        if (chatMessageParent.childCount > 0)
            Destroy(chatMessageParent.GetChild(0).gameObject);

        Instantiate(chatMessagePrefab, chatMessageParent).GetComponent<ChatMessage>().DoChatMessage(message);
    }
}