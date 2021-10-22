using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;

public class MenuTools : MonoBehaviour
{
    public static void ToggleActive(GameObject target)
    {
        target.SetActive(!target.activeInHierarchy);
    }

    public void SetInactive()
    {
        gameObject.SetActive(false);
    }

    public static void ExitGame()
    {
        Application.Quit();
    }

    public static void TogglePauseMenu(GameObject pauseMenu)
    {
        if (!pauseMenu.activeInHierarchy)
        {
            SetIsPaused(true);
            pauseMenu.SetActive(true);
        }
        else
        {
            pauseMenu.GetComponent<PauseMenuManager>().ClosePauseMenu();
        }
    }

    public static void SetIsPaused(bool shouldBePaused)
    {
        Globals.isGamePaused = shouldBePaused;
        if (shouldBePaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }

    public static void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public static bool IsApproximatelyEqual(float targetVal, float actualVal, float acceptableVariance = 0.1f)
    {
        return (targetVal - acceptableVariance <= actualVal && actualVal + acceptableVariance >= actualVal);
    }

    public static IEnumerator FadeOut(TextMeshProUGUI text, float period = 1)
    {
        while (text.alpha > 0)
        {
            text.alpha -= 1 / period * Time.deltaTime;
            yield return 0;
        }
    }

    public static IEnumerator FadeOut(Image image, float period = 1)
    {
        while (image.color.a > 0)
        {
            Color tempCol = image.color;
            tempCol.a -= 1 / period * Time.deltaTime;
            image.color = tempCol;
            yield return 0;
        }
    }

    public static IEnumerator FadeOut(CanvasGroup canvasGroup, float period = 1)
    {
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= 1 / period * Time.deltaTime;
            yield return 0;
        }
    }

    public static IEnumerator FadeIn(TextMeshProUGUI text, float period = 1)
    {
        while (text.alpha < 1)
        {
            text.alpha += 1 / period * Time.deltaTime;
            yield return 0;
        }
    }

    public static IEnumerator FadeIn(Image image, float period = 1)
    {
        while (image.color.a < 1)
        {
            Color tempCol = image.color;
            tempCol.a += 1 / period * Time.deltaTime;
            image.color = tempCol;
            yield return 0;
        }
    }

    public static IEnumerator FadeIn(CanvasGroup canvasGroup, float period = 1)
    {
        while (canvasGroup.alpha < 1)
        {
            canvasGroup.alpha += 1 / period * Time.deltaTime;
            yield return 0;
        }
    }

}