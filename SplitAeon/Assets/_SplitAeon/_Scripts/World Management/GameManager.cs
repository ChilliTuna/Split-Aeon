using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Things this script needs to deliver
    /*
        -When should spawners start. This is most likely going to be a trigger box.
        -When should spawners stop. This "might" be a kill limit. or another trigger box.
        -When the player has switched zones. Call the function that will "pause" the AI manager for that zone
        -How are the enemies going to hit the player. Invoking a damage event currently, with a collider attached to their arm. It works horribly right now.
    */

    public ZoneManager presentZoneManager;
    public ZoneManager pastZoneManager;

    // Start is called before the first frame update
    private void Start()
    {
        AssignToZoneManagers();
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
}