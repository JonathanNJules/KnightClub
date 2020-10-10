using System.Collections;
using TMPro;
using UnityEngine;

public class ChatMessage : MonoBehaviour
{
    public TMP_Text chatMessageTextBox;

    public void DoChatMessage(string message)
    {
        chatMessageTextBox.text = message;
        StartCoroutine(ChatMessageLifeTime());
    }

    private IEnumerator ChatMessageLifeTime()
    {
        var cf = GetComponent<CanvasFader>();
        cf.Fade(true, 2);
        yield return new WaitForSeconds(4);
        cf.Fade(false, .5f);
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
