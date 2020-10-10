using UnityEngine;

public class CanvasFader : MonoBehaviour
{
    private CanvasGroup cg;
    private bool fadingIn, fadingOut;
    private float fs;

    void Start()
    {
        cg = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if(fadingIn)
        {
            cg.alpha += fs * Time.deltaTime;
            if (cg.alpha >= 1) fadingIn = false;
        }
        if (fadingOut)
        {
            cg.alpha -= fs * Time.deltaTime;
            if (cg.alpha <= 0) fadingOut = false;
        }
    }

    public void Fade(bool fadeIn, float fadeSpeed)
    {
        fadingIn = fadeIn;
        fadingOut = !fadeIn;
        fs = fadeSpeed;
    }
}
