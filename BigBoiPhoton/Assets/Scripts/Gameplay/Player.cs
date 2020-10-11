using Photon.Pun;
using System.Reflection;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    private Rigidbody rb;
    public Transform model;
    private Animator anim;
    public Transform camLookT;
    private Vector3 moveVector;
    public float moveSpeed;

    private Vector3 networkedPos;
    private Quaternion networkedRot;
    private float networkedMovementMag;
    private int networkedEmoteIndex;
    private bool networkedCutEmote;
    public float networkSmoothingSpeed;

    public TMP_Text nameText;
    public Transform chatMessageParent;
    public GameObject chatMessagePrefab;
    private ChatMessageRelayer cmr;

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        nameText.text = photonView.Owner.NickName;
        anim = model.GetChild(0).GetComponent<Animator>();

        if (!photonView.IsMine) return;

        rb = GetComponent<Rigidbody>();
        cmr = GameObject.Find("Main Canvas").GetComponent<ChatMessageRelayer>();
        cmr.p = this;

        CameraController cc = GameObject.Find("Main Camera").GetComponent<CameraController>();
        cc.target = transform;
        cc.targetLook = camLookT;
        cc.enabled = true;

        rb.isKinematic = false;
    }

    void Update()
    {
        anim.SetFloat("MovementMag", networkedMovementMag);
        anim.SetBool("CutEmote", networkedCutEmote);
        anim.SetInteger("EmoteIndex", networkedEmoteIndex);
        if (photonView.IsMine) OwnerUpdate();
        else NetworkedUpdate();
    }

    void OwnerUpdate()
    {
        // Only move if not in chat box
        moveVector = cmr.inChatBox ? Vector3.zero : new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

        if (moveVector.sqrMagnitude > 1) moveVector = moveVector.normalized;
        networkedMovementMag = moveVector.magnitude;

        Emotes();
        networkedCutEmote = moveVector.magnitude > 0.02f;

        moveVector *= moveSpeed;
    }

    void NetworkedUpdate()
    {
        transform.position = Vector3.Lerp(transform.position, networkedPos, networkSmoothingSpeed * Time.deltaTime);
        model.rotation = Quaternion.Lerp(model.rotation, networkedRot, networkSmoothingSpeed * Time.deltaTime);
    }

    void Emotes() {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            networkedEmoteIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2))
            networkedEmoteIndex = 2;
        if (Input.GetKeyDown(KeyCode.Alpha3))
            networkedEmoteIndex = 3;

        if (Input.GetKeyUp(KeyCode.Alpha1) && networkedEmoteIndex == 1)
            networkedEmoteIndex = 0;
        if (Input.GetKeyUp(KeyCode.Alpha2) && networkedEmoteIndex == 2)
            networkedEmoteIndex = 0;
        if (Input.GetKeyUp(KeyCode.Alpha3) && networkedEmoteIndex == 3)
            networkedEmoteIndex = 0;
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        rb.velocity = moveVector * Time.deltaTime;
        if (moveVector.magnitude > .5f)
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
            stream.SendNext(networkedMovementMag);
            stream.SendNext(networkedEmoteIndex);
            stream.SendNext(networkedCutEmote);
        }
        else
        {
            networkedPos = (Vector3)stream.ReceiveNext();
            networkedRot = (Quaternion)stream.ReceiveNext();
            networkedMovementMag = (float)stream.ReceiveNext();
            networkedEmoteIndex = (int)stream.ReceiveNext();
            networkedCutEmote = (bool)stream.ReceiveNext();
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