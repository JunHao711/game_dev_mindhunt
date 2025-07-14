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

    public float attackRange = 1.5f;
    public float followPlayerRange = 5f;

    private Transform player;
    private Player_health playerHealth;
    private bool isAttacking = false;
    private bool isChasing = false;


    // Start is called before the first frame update
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        currentTarget = pointB.position;
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
        enemy_health_bar.SetMaxHealth(currentHealth);
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerHealth = player.GetComponent<Player_health>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            anim.SetBool("isAttacking", true);
            isAttacking = true;
            isChasing = false;

            // 朝向玩家
            sr.flipX = player.position.x < transform.position.x;
        }
        else if (distance <= followPlayerRange)
        {
            anim.SetBool("isAttacking", false);
            isAttacking = false;
            isChasing = true;

            // ✅ 只追踪 X 方向
            currentTarget = new Vector3(player.position.x, transform.position.y, transform.position.z);
            sr.flipX = player.position.x < transform.position.x;
        }
        else
        {
            anim.SetBool("isAttacking", false);
            isAttacking = false;
            isChasing = false;
            PatrolLogic();
        }

        if (!isAttacking)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
        }
    }


    void PatrolLogic()
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
    }



    public void DoDamage()
    {
        if (player != null)
        {
            float distance = Vector2.Distance(transform.position, player.position);
            if (distance <= attackRange) // 确保攻击时玩家还在范围内
            {
                playerHealth.GetDamage();
            }
        }
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

    public virtual void Die()
    {
        anim.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(gameObject, 2f);
    }


}
