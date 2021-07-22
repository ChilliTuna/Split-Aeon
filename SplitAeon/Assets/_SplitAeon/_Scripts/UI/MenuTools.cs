using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            Cursor.lockState = CursorLockMode.None;
            Time.timeScale = 0;
            pauseMenu.SetActive(true);
        }
        else
        {
            pauseMenu.GetComponent<PauseMenuManager>().ClosePauseMenu();
        }
    }
}
