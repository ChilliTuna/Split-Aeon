using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveList : MonoBehaviour
{
    public string[] objectives;

    [Header("Transition Times")]
    public float initialTime = 0.2f;

    public float stage1Time = 0.2f;
    public float stage2Time = 0.2f;
    public float stage3Time = 0.2f;

    [Space]
    public float fadeSpeed = 0.1f;

    private int currentObjective = -1;

    private TextMeshProUGUI objectiveText;

    private bool isVisisble = false;

    private void Start()
    {
        objectiveText = gameObject.transform.Find("Objective").Find("Objective text").gameObject.GetComponent<TextMeshProUGUI>();
        objectiveText.text = "";
    }

    public void NextObjective()
    {
        currentObjective++;
        objectiveText.text = objectives[currentObjective];
    }

    public void CompleteObjective()
    {
        if (currentObjective < objectives.Length - 1)
        {
            if (!isVisisble)
            {
                ToggleObjectives(!isVisisble);
            }
            StartCoroutine("TransitionObjective");
        }
        else
        {
            //Win Game!
        }
    }

    public void ToggleObjectives(bool state)
    {
        isVisisble = state;
        if (!isVisisble)
        {
            StartCoroutine(FadeOut(GetComponent<CanvasGroup>(), fadeSpeed));
        }
        else
        {
            StartCoroutine(FadeIn(GetComponent<CanvasGroup>(), fadeSpeed));
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleObjectives(!isVisisble);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            CompleteObjective();
        }
    }

    private IEnumerator TransitionObjective()
    {
        int i = 0;
        while (i < 5)
        {
            switch (i)
            {
                case (0):
                    i++;
                    yield return new WaitForSeconds(initialTime);
                    break;

                case (1):
                    gameObject.transform.Find("Objective").Find("Complete image").gameObject.SetActive(true);
                    objectiveText.fontStyle = FontStyles.Strikethrough;
                    i++;
                    yield return new WaitForSeconds(stage1Time);
                    break;

                default:
                    break;
            }
            i++;
            yield return null;
        }
        NextObjective();
    }

    private bool IsApproximatelyEqual(float targetVal, float actualVal, float acceptableVariance = 0.1f)
    {
        return (targetVal - acceptableVariance <= actualVal && actualVal + acceptableVariance >= actualVal);
    }

    private IEnumerator FadeOut(TextMeshProUGUI text, float rate = 1)
    {
        while (text.alpha > 0)
        {
            text.alpha -= rate * Time.deltaTime;
            yield return 0;
        }
    }

    private IEnumerator FadeOut(Image image, float rate = 1)
    {
        while (image.color.a > 0)
        {
            Color tempCol = image.color;
            tempCol.a -= rate * Time.deltaTime;
            image.color = tempCol;
            yield return 0;
        }
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup, float rate = 1)
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= rate * Time.deltaTime;
            yield return 0;
        }
    }

    private IEnumerator FadeIn(TextMeshProUGUI text, float rate = 1)
    {
        while (text.alpha < 1)
        {
            text.alpha += rate * Time.deltaTime;
            yield return 0;
        }
    }

    private IEnumerator FadeIn(Image image, float rate = 1)
    {
        while (image.color.a < 1)
        {
            Color tempCol = image.color;
            tempCol.a += rate * Time.deltaTime;
            image.color = tempCol;
            yield return 0;
        }
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup, float rate = 1)
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += rate * Time.deltaTime;
            yield return 0;
        }
    }
}