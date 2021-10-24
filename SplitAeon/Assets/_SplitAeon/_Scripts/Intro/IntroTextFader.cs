using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class IntroTextFader : MonoBehaviour
{

    TextMeshProUGUI text;

    public float startAfter;

    float timer;
    bool doTimer = true;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    private void Update()
    {
        if (doTimer)
        {
            timer += Time.deltaTime;

            if (timer > startAfter)
            {
                FadeIn();
                doTimer = false;
            }
        }
    }

    public void FadeIn()
    {
        StartCoroutine(FadeTextIn(2f));
    }

    public void FadeOut()
    {
        StartCoroutine(FadeTextOut(2f));
    }

    public IEnumerator FadeTextIn(float time)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 0);

        while (text.color.a < 1.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a + (Time.deltaTime / time));
            yield return null;
        }
    }

    public IEnumerator FadeTextOut(float time)
    {
        text.color = new Color(text.color.r, text.color.g, text.color.b, 1);

        while (text.color.a > 0.0f)
        {
            text.color = new Color(text.color.r, text.color.g, text.color.b, text.color.a - (Time.deltaTime / time));
            yield return null;
        }
    }

}
