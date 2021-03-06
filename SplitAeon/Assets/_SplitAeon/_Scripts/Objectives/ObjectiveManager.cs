using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Events;

using TMPro;

using System;

public class ObjectiveManager : MonoBehaviour
{

    public Objective[] objectives;

    private TextMeshProUGUI text;
    private ObjectivePointer pointer;

    private Objective currentObjective;
    private int index;

    public UnityEvent onFinalObjectiveCompleted;

    private void Start()
    {
        text = GameObject.Find("ObjectiveTextReadout").GetComponent<TextMeshProUGUI>();
        pointer = GameObject.Find("PlayerObjectivePointer").GetComponent<ObjectivePointer>();

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

        if (index >= objectives.Length)
        {
            //Win game
            Debug.LogWarning("All Objectives Completed!");
            onFinalObjectiveCompleted.Invoke();
            return;
        }

        currentObjective = objectives[index];
        UpdateObjectiveDisplay();
        currentObjective.StartObjective();
        pointer.target = currentObjective.objectiveLocation;
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

    public void RestartCurrentObjective()
    {
        SetObjective(index);
    }
}

[Serializable]
public class Objective
{
    [Header("Data")]
    public string name;

    [TextArea]
    public string description;

    [Space(10)]

    [Header("Objective Marker")]
    public Transform objectiveLocation;

    [Space(10)]

    [Header("Events")]

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

