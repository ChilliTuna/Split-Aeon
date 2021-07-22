using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public GameObject optionsMenu;
    public GameObject mainPauseMenu;

    public void ClosePauseMenu()
    {
        if (optionsMenu.activeInHierarchy)
        {
            mainPauseMenu.SetActive(true);
            optionsMenu.SetActive(false);
        }
    }
}
