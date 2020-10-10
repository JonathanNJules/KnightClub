using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChatMessageRelayer : MonoBehaviour
{
    public Player p;
    public TMP_InputField chatMessageIPF;
    public EventSystem e;
    public bool inChatBox;

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
