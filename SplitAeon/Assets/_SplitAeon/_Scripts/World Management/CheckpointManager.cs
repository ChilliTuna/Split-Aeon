using System.Collections;
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

    private Player player;

    public List<float[]> ammoCounts;

    public float health;

    public Vector3 respawnPos;

    private void Start()
    {
        player = GetComponent<Timewarp>().player.GetComponent<Player>();
    }

    public void SetCheckpoint()
    {

    }
}
