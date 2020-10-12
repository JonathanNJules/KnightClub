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
            StartCoroutine(HideStore());
    }

    private void ShowStore()
    {
        store.SetActive(true);
        cf.Fade(true, 2);
    }

    private IEnumerator HideStore()
    {
        cf.Fade(false, 2);
        yield return new WaitForSeconds(.5f);
        store.SetActive(false);
    }
}
