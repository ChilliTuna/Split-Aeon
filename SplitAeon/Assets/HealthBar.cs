using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Health health;

    public Image bar;

    void Start()
    {
        bar.fillAmount = 1;
    }

    void Update()
    {
        bar.fillAmount = health.health / health.maxHealth;
    }
}
