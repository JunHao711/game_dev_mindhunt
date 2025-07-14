using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public int speed;
    private Vector3 currentTarget;

    private SpriteRenderer sr;

    public int currentHealth;
    public int maxHealth;
    public int damageAmount;

    private Animator anim;

    public Enemy_health_bar enemy_health_bar;

    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        currentTarget = pointB.position;
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        enemy_health_bar.SetMaxHealth(currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToA = Vector3.Distance(transform.position, pointA.position);
        float distanceToB = Vector3.Distance(transform.position, pointB.position);

        if (distanceToA < 0.1f)
        {
            currentTarget = pointB.position;
            sr.flipX = false;
        }
        else if (distanceToB < 0.1f)
        {
            currentTarget = pointA.position;
            sr.flipX = true;
        }

        transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
    }

    public void takeDamage()
    {
        currentHealth -= damageAmount;
        enemy_health_bar.SetHealth(currentHealth);
        
        if(currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        anim.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(gameObject, 2f);
    }


}
