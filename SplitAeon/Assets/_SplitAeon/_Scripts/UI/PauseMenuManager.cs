using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject mainPauseMenu;

    public void ClosePauseMenu()
    {
        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.Locked;
        if (optionsMenu.activeInHierarchy)
        {
            mainPauseMenu.SetActive(true);
            optionsMenu.SetActive(false);
        }
        gameObject.SetActive(false);
    }
}
