using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ObjectiveList : MonoBehaviour
{
    public string[] objectives;

    private int currentObjective = -1;

    private TextMeshProUGUI text;

    private bool isVisisble = true;

    private void Start()
    {
        text = gameObject.transform.Find("Objective").Find("Objective text").gameObject.GetComponent<TextMeshProUGUI>();
        text.text = "";
        ToggleObjectives(false);
    }

    public void NextObjective()
    {
        currentObjective++;
        text.text = objectives[currentObjective];
    }

    public void CompleteObjective()
    {
        if (currentObjective < objectives.Length - 1)
        {
            NextObjective();
        }
        else
        {
            //Win Game!
        }
    }

    public void ToggleObjectives(bool state)
    {
        isVisisble = state;
        GetComponent<Image>().enabled = state;
        transform.Find("Header text").gameObject.SetActive(state);
        transform.Find("Objective").gameObject.SetActive(state);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleObjectives(!isVisisble);
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            CompleteObjective();
        }
    }
}
