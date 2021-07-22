using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonScript : MonoBehaviour
{
    public GameObject thisMenu;

    public void SwitchActiveMenu(GameObject newActiveMenu)
    { 
        newActiveMenu.SetActive(true);
        thisMenu.SetActive(false);
    }
}
