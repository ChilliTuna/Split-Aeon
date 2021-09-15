using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using TMPro;

using System;

public class ObjectiveManager : MonoBehaviour
{

    public Objective[] objectives;

    public TextMeshProUGUI text;

    private Objective currentObjective;
    private int index;

    private void Start()
    {
        SetObjective(0);
    }

    public void CompleteCurrentObjective()
    {
        currentObjective.EndObjective();
        NextObjective();
    }

    public void NextObjective()
    {
        index++;

        if (index > objectives.Length)
        {
            //Win game
            Debug.LogWarning("All Objectives Completed!");
            return;
        }

        SetObjective(index);
    }

    public void PreviousObjective()
    {
        index--;

        if (index < 0)
        {
            index = 0;
        }

        SetObjective(index);
    }

    public void SetObjective(int index)
    {
        currentObjective = objectives[index];
        UpdateObjectiveDisplay();
        currentObjective.StartObjective();
    }

    public void UpdateObjectiveDisplay()
    {
        text.text = currentObjective.description;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            NextObjective();
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            PreviousObjective();
        }
       
        if (Input.GetKeyDown(KeyCode.J))
        {
            CompleteCurrentObjective();
        }

    }

}

[Serializable]
public class Objective
{
    [Header("Data")]
    public string name;

    [TextArea]
    public string description;

    public UnityEvent onStartObjective;
    public UnityEvent onCompleteObjective;

    public void StartObjective()
    {
        onStartObjective.Invoke();
    }

    public void EndObjective()
    {
        onCompleteObjective.Invoke();
    }

}

