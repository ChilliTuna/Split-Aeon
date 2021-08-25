using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AI/Scriptables/ProjectileSettings")]
public class ProjectileSettings : ScriptableObject
{
    public float force = 10.0f;
    public float lifeTime = 5.0f;
    [Range(-90.0f, 90.0f)]public float upAngle = 15.0f;
}
