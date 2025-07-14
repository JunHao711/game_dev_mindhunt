using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy_health_bar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxHealth(int enemy_health)
    {
        slider.value = enemy_health;
        slider.maxValue = enemy_health;
    }

    public void SetHealth(int enemy_health)
    {
        slider.value = enemy_health;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
