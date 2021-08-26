using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    /*
    Checkpoints save on objective completion;
    Things that need to be kept from checkpoints:
    - Ammo
    - Respawn Position
    - Health
    - Enemies
    - Weapons Obtained
    */

    private GameObject player;

    public List<int[]> ammoCounts;

    public float health;

    public Vector3 respawnPos;
    public Quaternion respawnRot;

    private void Start()
    {
        player = GetComponent<Timewarp>().player;
    }

    public void SetCheckpoint()
    {
        health = player.GetComponent<Health>().health;
        SaveAmmoCounts();
        respawnPos = player.transform.position;
        respawnRot = player.transform.rotation;
        //Save defeated zones
    }

    public void LoadCheckpoint()
    {
        player.GetComponent<Health>().health = health;
        LoadAmmoCounts();
        player.transform.position = respawnPos;
        player.transform.rotation = respawnRot;
        //Reset non-saved zones
    }

    private void SaveAmmoCounts()
    {
        ammoCounts.Clear();
        Gun[] guns = player.transform.GetComponentsInChildren<Gun>();
        foreach (Gun gun in guns)
        {
            ammoCounts.Add(new int[] { gun.ammoLoaded, gun.ammoPool });
        }
    }

    private void LoadAmmoCounts()
    {
        Gun[] guns = player.transform.GetComponentsInChildren<Gun>();
        for (int i = 0; i < ammoCounts.Count; i++)
        {
            guns[i].ammoLoaded = ammoCounts[i][0];
            guns[i].ammoPool = ammoCounts[i][1];
        }
    }
}