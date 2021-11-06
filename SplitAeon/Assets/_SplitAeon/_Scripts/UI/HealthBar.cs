using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Health health;

    public Image[] bars;

    void Start()
    {
        foreach (Image bar in bars)
        {
            bar.fillAmount = 1;
        }
    }

    void Update()
    {
        foreach (Image bar in bars)
        {
            bar.fillAmount = health.health / health.maxHealth;
        }
    }
}
