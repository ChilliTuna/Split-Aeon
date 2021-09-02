using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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

    private List<AmmoData> ammoCounts = new List<AmmoData>();

    private int cardCounts;

    private float health;

    private Vector3 respawnPosition;
    private Quaternion respawnRotation;

    private List<Zone> completedZones = new List<Zone>();

    public UnityEvent onCheckpoint;
    public UnityEvent onRespawn;

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
        SaveZones();
        onCheckpoint.Invoke();
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
        LoadZones();
        onRespawn.Invoke();
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
            foreach (AmmoData ammoData in ammoCounts)
            {
                if (ammoData.weaponName == gun.weaponName)
                {
                    gun.ammoLoaded = ammoData.ammoLoaded;
                    gun.ammoPool = ammoData.ammoPool;
                    break;
                }
            }
        }
    }

    private void SaveZones()
    {
        completedZones.Clear();
        GameManager gm = gameObject.GetComponent<GameManager>();
        foreach (Zone zone in gm.pastZoneManager.zones)
        {
            if (zone.isComplete)
            {
                completedZones.Add(zone);
            }
        }
        foreach (Zone zone in gm.presentZoneManager.zones)
        {
            if (zone.isComplete)
            {
                completedZones.Add(zone);
            }
        }
    }

    private void LoadZones()
    {
        GameManager gm = gameObject.GetComponent<GameManager>();
        List<AIAgent> allAgents = gm.gameObject.GetComponent<SilhouetteGenerator>().aiManager.GetAllActiveAgents();
        foreach (AIAgent agent in allAgents)
        {
            agent.DisablePoolObject();
        }
        foreach (Zone zone in gm.pastZoneManager.zones)
        {
            if (!completedZones.Contains(zone))
            {
                zone.ResetZone();
            }
        }
        foreach (Zone zone in gm.presentZoneManager.zones)
        {
            if (!completedZones.Contains(zone))
            {
                zone.ResetZone();
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