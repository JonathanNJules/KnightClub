using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChatMessageRelayer : MonoBehaviour
{
    public Player p;
    public TMP_InputField chatMessageIPF;
    private EventSystem e;
    public bool inChatBox;

    void Start()
    {
        e = EventSystem.current;
    }

    void Update()
    {
        inChatBox = e.currentSelectedGameObject == chatMessageIPF.gameObject;
        if (Input.GetKeyDown(KeyCode.Return))
            GiveChatMessageToPlayer();
    }

    public void GiveChatMessageToPlayer()
    {
        if (p == null) return;
        if (string.IsNullOrEmpty(chatMessageIPF.text)) return;

        p.NetworkSendChatMessage(chatMessageIPF.text);
        chatMessageIPF.text = "";
    }
}
