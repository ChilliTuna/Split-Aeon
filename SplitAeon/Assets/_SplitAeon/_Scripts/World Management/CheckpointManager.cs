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

    public List<AmmoData> ammoCounts = new List<AmmoData>();

    public int cardCounts;

    public float health;

    public Vector3 respawnPosition;
    public Quaternion respawnRotation;

    private void Start()
    {
        player = GetComponent<Timewarp>().player;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            Respawn();
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            SetCheckpoint();
        }
    }

    public void SetCheckpoint()
    {
        health = player.GetComponent<Health>().health;
        SaveAmmoCounts();
        respawnPosition = player.transform.position;
        respawnRotation = player.transform.rotation;
        //Save defeated zones
    }

    public void Respawn()
    {
        player.GetComponent<Health>().health = health;
        LoadAmmoCounts();
        player.GetComponent<CharacterController>().enabled = false;
        player.transform.position = respawnPosition;
        player.GetComponent<CharacterController>().enabled = true;
        player.transform.rotation = respawnRotation;
        //Reset non-saved zones
    }

    private void SaveAmmoCounts()
    {
        cardCounts = player.GetComponentInChildren<CardManager>().cardLethalPool;
        ammoCounts.Clear();
        Gun[] guns = player.transform.GetComponentsInChildren<Gun>();
        foreach (Gun gun in guns)
        {
            AmmoData adata = new AmmoData();
            adata.weaponName = gun.weaponName;
            adata.ammoLoaded = gun.ammoLoaded;
            adata.ammoPool = gun.ammoPool;
            ammoCounts.Add(adata);
        }
        
    }

    private void LoadAmmoCounts()
    {
        player.GetComponentInChildren<CardManager>().cardLethalPool = cardCounts;
        Gun[] guns = player.transform.GetComponentsInChildren<Gun>();
        foreach (Gun gun in guns)
        {
            foreach(AmmoData ammoData in ammoCounts)
            {
                if (ammoData.weaponName == gun.weaponName)
                {
                    gun.ammoLoaded = ammoData.ammoLoaded;
                    gun.ammoPool = ammoData.ammoPool;
                }
            }
        }
    }
}

public struct AmmoData
{
    public string weaponName;
    public int ammoLoaded;
    public int ammoPool;
}