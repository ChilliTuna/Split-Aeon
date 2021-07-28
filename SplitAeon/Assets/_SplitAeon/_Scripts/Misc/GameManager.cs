using System.Collections;
using System.Collections.Generic;
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

    public bool ShouldEnemiesSpawn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void WinGame()
    {
        //Do stuff (TBD)
    }
}
