using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class AgentAudio : MonoBehaviour
{
    [Header("Head Emitter")]
    public StudioEventEmitter idleEmitter;

    public StudioEventEmitter attackEmitter;
    public StudioEventEmitter hurtEmitter;
    public StudioEventEmitter deathEmitter;

    [Header("Foot Emitter")]
    public StudioEventEmitter footEmitter;
}
