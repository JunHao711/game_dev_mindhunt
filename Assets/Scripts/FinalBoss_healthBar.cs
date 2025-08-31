using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalBoss_healthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(int FinalBossHealth)
    {
        slider.maxValue = FinalBossHealth;
        slider.value = FinalBossHealth;

    }

    public void SetHealth(int FinalBossHealth)
    {
        slider.value = FinalBossHealth;
    }
}
