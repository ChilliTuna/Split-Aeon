using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Health health;

    public Slider slider;

    void Start()
    {
        slider.maxValue = health.maxHealth;
    }

    void Update()
    {
        slider.value = health.health;
    }
}
