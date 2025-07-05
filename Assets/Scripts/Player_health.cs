using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_health : MonoBehaviour
{
    public int current_health;
    public int max_health;
    public int damage;

    public Health_bar healthbar;

    // Start is called before the first frame update
    void Start()
    {
        current_health = max_health;
        healthbar.setMaxHealth(current_health);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetDamage()
    {
        current_health -= damage;
        healthbar.setHealth(current_health);
        if (current_health <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
