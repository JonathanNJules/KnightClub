using System.Collections;
using UnityEngine;

public class StoreTrigger : MonoBehaviour
{
    public GameObject store;
    private CanvasFader cf;

    void Start()
    {
        cf = store.GetComponent<CanvasFader>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && other.GetComponent<Player>().isReal)
            ShowStore();
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player") && other.GetComponent<Player>().isReal)
            HideStore();
    }

    private void ShowStore()
    {
        store.SetActive(true);
        cf.Fade(true, 2);
    }

    public void HideStore()
    {
        StartCoroutine(HideStoreCo());
    }

    private IEnumerator HideStoreCo()
    {
        cf.Fade(false, 2);
        yield return new WaitForSeconds(.5f);
        store.SetActive(false);
    }
}
