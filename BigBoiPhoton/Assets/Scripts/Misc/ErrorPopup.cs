using System.Collections;
using UnityEngine;

public class ErrorPopup : MonoBehaviour
{
    private CanvasFader cf;

    void Start()
    {
        cf = GetComponent<CanvasFader>();
        StartCoroutine(MyWholeLife());
    }

    private IEnumerator MyWholeLife()
    {
        cf.Fade(true, 1);
        yield return new WaitForSeconds(4);
        cf.Fade(false, .5f);
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
