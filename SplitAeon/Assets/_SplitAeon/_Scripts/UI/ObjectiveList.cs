using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ObjectiveList : MonoBehaviour
{
    public string[] objectives;

    public bool gameWin = false;

    [Header("Transition Times")]
    public float stage0Time = 0.2f;

    public float stage1Time = 0.2f;
    public float stage2Time = 0.2f;
    public float stage3Time = 0.2f;
    public float stage4Time = 0.2f;

    [Space]
    public float fadeDuration = 0.1f;

    private int currentObjective = -1;

    private TextMeshProUGUI objectiveText;

    private bool isVisible = false;

    public UnityEvent onObjectiveComplete;

    public UnityEvent onWin;

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
        if (currentObjective < objectives.Length)
        {
            onObjectiveComplete.Invoke();
            StartCoroutine("TransitionObjective");
        }
        if (gameWin)
        {
            //do fancy stuff
            onWin.Invoke();
        }
    }

    public void ToggleObjectives(bool state)
    {
        CanvasGroup cg = GetComponent<CanvasGroup>();
        if (cg.alpha == 0 || cg.alpha == 1)
        {
            isVisible = state;
            if (isVisible)
            {
                StartCoroutine(FadeIn(cg, fadeDuration));
            }
            else
            {
                StartCoroutine(FadeOut(cg, fadeDuration));
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleObjectives(!isVisible);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            CompleteObjective();
        }
    }

    private IEnumerator TransitionObjective()
    {
        bool wasVisible = isVisible;
        CanvasGroup objectiveBlock = gameObject.transform.Find("Objective").gameObject.GetComponent<CanvasGroup>();
        if (!isVisible)
        {
            ToggleObjectives(true);
        }
        int i = 0;
        while (i < 4)
        {
            switch (i)
            {
                case (0):
                    yield return new WaitForSeconds(stage0Time);

                    objectiveBlock.gameObject.transform.Find("Complete image").gameObject.SetActive(true);
                    objectiveText.fontStyle = FontStyles.Strikethrough;
                    yield return new WaitForSeconds(stage1Time);
                    i++;
                    goto case (1);

                case (1):
                    StartCoroutine(FadeOut(objectiveBlock, 0.5f));
                    if (objectiveBlock.GetComponent<CanvasGroup>().alpha == 0)
                    {
                        i++;
                        yield return new WaitForSeconds(stage2Time);
                    }
                    yield return null;
                    break;

                case (2):
                    if (currentObjective < objectives.Length - 1)
                    {
                        NextObjective();
                        objectiveBlock.gameObject.transform.Find("Complete image").gameObject.SetActive(false);
                        objectiveText.fontStyle = FontStyles.Normal;
                        i++;
                        yield return new WaitForSeconds(stage3Time);
                    }
                    else
                    {
                        i++;
                    }
                    break;

                case (3):
                    if (!gameWin)
                    {
                        StartCoroutine(FadeIn(objectiveBlock, 0.5f));
                        if (objectiveBlock.GetComponent<CanvasGroup>().alpha == 1)
                        {
                            if (currentObjective >= objectives.Length - 1)
                            {
                                gameWin = true;
                            }
                            i++;
                            yield return new WaitForSeconds(stage4Time);
                        }
                        yield return null;
                    }
                    else
                    {
                        i++;
                    }
                    break;

                default:
                    i = 5;
                    break;
            }
        }
        if (!wasVisible)
        {
            ToggleObjectives(false);
        }
    }

    private bool IsApproximatelyEqual(float targetVal, float actualVal, float acceptableVariance = 0.1f)
    {
        return (targetVal - acceptableVariance <= actualVal && actualVal + acceptableVariance >= actualVal);
    }

    private IEnumerator FadeOut(TextMeshProUGUI text, float period = 1)
    {
        while (text.alpha > 0)
        {
            text.alpha -= 1 / period * Time.deltaTime;
            yield return 0;
        }
    }

    private IEnumerator FadeOut(Image image, float period = 1)
    {
        while (image.color.a > 0)
        {
            Color tempCol = image.color;
            tempCol.a -= 1 / period * Time.deltaTime;
            image.color = tempCol;
            yield return 0;
        }
    }

    private IEnumerator FadeOut(CanvasGroup canvasGroup, float period = 1)
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 1 / period * Time.deltaTime;
            yield return 0;
        }
    }

    private IEnumerator FadeIn(TextMeshProUGUI text, float period = 1)
    {
        while (text.alpha < 1)
        {
            text.alpha += 1 / period * Time.deltaTime;
            yield return 0;
        }
    }

    private IEnumerator FadeIn(Image image, float period = 1)
    {
        while (image.color.a < 1)
        {
            Color tempCol = image.color;
            tempCol.a += 1 / period * Time.deltaTime;
            image.color = tempCol;
            yield return 0;
        }
    }

    private IEnumerator FadeIn(CanvasGroup canvasGroup, float period = 1)
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += 1 / period * Time.deltaTime;
            yield return 0;
        }
    }
}