using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    public TutorialManager manager;

    public enum TriggerType
    {
        DisplayText,
        ClearText
    }

    public TriggerType type;

    public string tutorialText;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (type == TriggerType.DisplayText)
            {
                manager.SetTutorialText(tutorialText);
            }

            if (type == TriggerType.ClearText)
            {
                manager.ClearTutorialText();
            }

        }    



    }

}
