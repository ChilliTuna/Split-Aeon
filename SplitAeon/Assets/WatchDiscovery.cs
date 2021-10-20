using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchDiscovery : MonoBehaviour
{

    public Animator anim;
    Player player;

    public float freezeTime;

    private void Awake()
    {
        player = GetComponent<Player>();
    }

    public void Discover()
    {
        //player.DisableInputs();

        anim.SetTrigger("Discovery");

        Invoke("UnlockPlayer", freezeTime);

    }

    void UnlockPlayer()
    {
        //player.EnableInputs();
    }


}
