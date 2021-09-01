using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuTools : MonoBehaviour
{
    public void ToggleActive(GameObject target)
    {
        target.SetActive(!target.activeInHierarchy);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void TogglePauseMenu(GameObject pauseMenu)
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

    public void SetIsPaused(bool shouldBePaused)
    {
        if (shouldBePaused)
        {
            Globals.isGamePaused = true;
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
        }
        else
        {
            Globals.isGamePaused = false;
            Cursor.lockState = CursorLockMode.Locked;
            Time.timeScale = 1;
        }
    }

    public void ChangeScene(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }
}