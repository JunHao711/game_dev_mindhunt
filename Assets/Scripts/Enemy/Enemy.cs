using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("General Stats")]
    public int currentHealth;
    public int maxHealth;
    public int damageAmount;

    public Enemy_health_bar enemy_health_bar;
    protected Animator anim;
    protected SpriteRenderer sr;
    protected Transform player;
    protected Player_health playerHealth;



    public virtual void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        currentHealth = maxHealth;
        enemy_health_bar.SetMaxHealth(maxHealth);
        enemy_health_bar.SetHealth(currentHealth);

        player = GameObject.FindGameObjectWithTag("Player").transform;
        if (player != null)
            playerHealth = player.GetComponent<Player_health>();
    }

    public virtual void Update() { }

    public virtual void DoDamage()
    {
        if (player != null)
        {
            playerHealth.GetDamage();
        }
    }

    public virtual void takeDamage()
    {
        currentHealth -= damageAmount;
        enemy_health_bar.SetHealth(currentHealth);
        if (currentHealth <= 0)
            Die();
    }

    public virtual void Die()
    {
        anim.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(gameObject, 2f);
    }
}