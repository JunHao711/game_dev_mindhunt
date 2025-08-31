using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bosshealthbar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(int bosshealth)
    {
        slider.maxValue = bosshealth;
        slider.value = bosshealth;
    }

    public void SetHealth(int bosshealth)
    {
        slider.value = bosshealth;
    }
}
