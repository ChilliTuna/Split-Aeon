using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    //Things this script needs to deliver
    /*
        -When should spawners start. This is most likely going to be a trigger box.
        -When should spawners stop. This "might" be a kill limit. or another trigger box.
        -When the player has switched zones. Call the function that will "pause" the AI manager for that zone
        -How are the enemies going to hit the player. Invoking a damage event currently, with a collider attached to their arm. It works horribly right now.
    */
    public bool isInDebug;

    public ZoneManager presentZoneManager;
    public ZoneManager pastZoneManager;
    public AIManager pastAIManager;
    public AIManager futureAIManager;

    public UnityEvent onStart;

    // Start is called before the first frame update
    private void Start()
    {
        Globals.isInDebug = isInDebug;

        AssignToZoneManagers();

        // This must be in start as aiManager must init variables in awake
        futureAIManager.TogglePlayerInsideState();

        onStart.Invoke();
    }

    public void WinGame()
    {
        //Do stuff (TBD)
    }

    private void AssignToZoneManagers()
    {
        presentZoneManager.gameManger = this;
        pastZoneManager.gameManger = this;
    }

    public void LoseGame()
    {
        Debug.Log("you are die");
    }

    public void AIManagerTimeSwap()
    {
        pastAIManager.TogglePlayerInsideState();
        futureAIManager.TogglePlayerInsideState();
    }

    public void DebugString(string message)
    {
        Debug.Log(message);
    }
}

public static class Globals
{
    public static bool isInDebug = false;
    public static bool isGamePaused = false;
    public static bool isInPast = true;
}