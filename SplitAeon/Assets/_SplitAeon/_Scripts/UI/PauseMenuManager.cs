using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuManager : MonoBehaviour
{
    public void ClosePauseMenu()
    {
        MenuTools.SetIsPaused(false);
        gameObject.SetActive(false);
    }
}
