using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class TutorialManager : MonoBehaviour
{

    TextMeshProUGUI text;

    void Start()
    {
        text = GameObject.Find("TutorialText").GetComponent<TextMeshProUGUI>();
    }

    public void SetTutorialText(string t)
    {
        text.text = t;
    }

    public void ClearTutorialText()
    {
        text.text = "";
    }

    public void ClearTutorialTextAfterTime(float time)
    {
        Invoke("ClearTutorialText", time);
    }

}
