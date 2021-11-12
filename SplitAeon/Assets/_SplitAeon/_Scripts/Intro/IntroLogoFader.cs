using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroLogoFader : MonoBehaviour
{
    CanvasGroup canvasg;

    public CanvasGroup textcg;

    public float startAfter = 12;
    public float logoTextFadeDelay = 0.5f;
    public float fadeSpeed = 3;
    public float scaleSpeed = 10;

    float timer;
    bool doTimer = true;

    private void Awake()
    {
        canvasg = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        if (doTimer)
        {
            timer += Time.deltaTime;
    
            if (timer > startAfter)
            {
                FadeText();
                StartCoroutine(BeginFadeImage());
                doTimer = false;
            }
        }
    }
    
    public void FadeIn()
    {
        StartCoroutine(FadeCanvasGroupIn(canvasg, fadeSpeed));
    }
    
    public void FadeOut()
    {
        StartCoroutine(FadeCanvasGroupOut(canvasg, fadeSpeed));
    }

    void FadeText()
    {
        StartCoroutine(FadeCanvasGroupOut(textcg, fadeSpeed));
    }

    public IEnumerator FadeCanvasGroupIn(CanvasGroup cg, float time)
    {
        while(cg.alpha < 1)
        {
            cg.alpha += Time.deltaTime / time;
            yield return 0;
        }
    }
    
    public IEnumerator FadeCanvasGroupOut(CanvasGroup cg, float time)
    {
        while(cg.alpha > 0)
        {
            cg.alpha -= Time.deltaTime / time;
            yield return 0;
        }
    }

    public IEnumerator BeginFadeImage()
    {
        yield return new WaitForSecondsRealtime(logoTextFadeDelay + fadeSpeed);
        StartCoroutine(ScaleCanvasGroup(scaleSpeed));
        StartCoroutine(FadeCanvasGroupIn(canvasg, fadeSpeed));
    }

    public IEnumerator ScaleCanvasGroup(float speed)
    {
        RectTransform rectran = GetComponent<RectTransform>();
        float currentTime = 0;
        while(true)
        {
            rectran.sizeDelta = rectran.sizeDelta + new Vector2(Time.deltaTime * speed, Time.deltaTime * speed);
            currentTime += Time.deltaTime;
            yield return null;
        }
    }
}
