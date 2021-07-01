using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ObjectiveList : MonoBehaviour
{
    public string[] objectives;

    private int currentObjective = -1;

    private TextMeshProUGUI text;

    private void Start()
    {
        text = gameObject.transform.Find("Objective").Find("Objective text").gameObject.GetComponent<TextMeshProUGUI>();
        text.text = "";
    }

    public void NextObjective()
    {
        if (currentObjective < objectives.Length - 1)
        {
            currentObjective++;
            text.text = objectives[currentObjective];
        }
        else
        {
            //Win Game!
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            NextObjective();
        }
    }
}
