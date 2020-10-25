using Photon.Pun;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviourPunCallbacks, IPunObservable
{
    private Rigidbody rb;
    public Transform model;

    public GameObject canvas;
    private Animator anim;
    public Transform camLookT;

    private Vector3 moveVector;
    public float moveSpeed;
    private bool moveLock;
    private float moveLockTimer, maxMoveLockTimer = 0.5f;
    public bool isReal;

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

    private string oldScene;
    private string currentSceneName = "Main";
    private bool deactivated;

    public Transform headwearParent;

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        print("just loaded " + scene.name +", and my scene is " + currentSceneName);

        GameManager.usersScene = scene.name;
        if (!photonView.IsMine)
        {
            CorrectSceneMismatch();
            return;
        }

        moveLock = true;
        moveLockTimer = maxMoveLockTimer;

        if (!scene.name.Contains("Game"))
        {
            Vector3 scenePos = GameObject.Find(oldScene + " Spawn").transform.position;
            photonView.RPC("HardSetPosition", RpcTarget.All, scenePos.x, scenePos.y, scenePos.z);
        }

        InitializeScene();
    }

    public override void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        DontDestroyOnLoad(gameObject);

        nameText.text = photonView.Owner.NickName;
        anim = model.GetChild(0).GetComponent<Animator>();

        if (!photonView.IsMine) return;

        rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;

        isReal = photonView.IsMine;

        InitializeScene();
    }

    private void InitializeScene()
    {
        cmr = GameObject.Find("Main Canvas").GetComponent<ChatMessageRelayer>();
        cmr.p = this;

        CameraController cc = GameObject.Find("Main Camera").GetComponent<CameraController>();
        cc.target = transform;
        cc.targetLook = camLookT;
        cc.enabled = true;
    }

    void Update()
    {
        anim.SetFloat("MovementMag", networkedMovementMag);
        anim.SetBool("CutEmote", networkedCutEmote);
        anim.SetInteger("EmoteIndex", networkedEmoteIndex);

        if (photonView.IsMine) OwnerUpdate();
        else if (deactivated == false) NetworkedUpdate();
    }

    void OwnerUpdate()
    {
        if (moveLock)
        {
            moveLockTimer -= Time.deltaTime;
            if(moveLockTimer <= 0)
                moveLock = false;
        }

        // Only move if not in chat box
        moveVector = (cmr.inChatBox || moveLock) ? Vector3.zero : new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

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

    [PunRPC]
    public void ChangeNetworkScene(string newScene)
    {
        currentSceneName = newScene;
        if (photonView.IsMine == false)
            CorrectSceneMismatch();
    }

    [PunRPC]
    public void HardSetPosition(float newX, float newY, float newZ)
    {
        transform.position = new Vector3(newX, newY, newZ);
    }

    [PunRPC]
    public void SetHeadwear(string headwear)
    {
        for(int i = 0; i < headwearParent.childCount; i++)
        {
            GameObject g = headwearParent.GetChild(i).gameObject;
            g.gameObject.SetActive(g.name == headwear);
        }
    }

    public void ChangeScenes(string newScene)
    {
        if (!photonView.IsMine) return;

        oldScene = SceneManager.GetActiveScene().name;
        photonView.RPC("ChangeNetworkScene", RpcTarget.All, newScene);
        SceneManager.LoadScene(newScene);
    }

    private void CorrectSceneMismatch()
    {
        print("isDeactivated? " + deactivated + " | in game? " + GameManager.usersScene.Contains("Game"));
        if (currentSceneName != GameManager.usersScene || GameManager.usersScene.Contains("Game"))
            Deactivate();
        else if (currentSceneName == GameManager.usersScene)
            Activate();
    }

    private void Deactivate()
    {
        deactivated = true;
        model.gameObject.SetActive(false);
        canvas.SetActive(false);
    }
    private void Activate()
    {
        deactivated = false;
        model.gameObject.SetActive(true);
        canvas.SetActive(true);
    }
}