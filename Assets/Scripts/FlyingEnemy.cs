using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    protected float speed = 3f;
    public float followRange = 6f;

    private Transform player;
    private Vector3 spawnPosition;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        spawnPosition = transform.position;

        currentHealth = maxHealth;
        // ✅ 你可以不assign enemy_health_bar 和 anim，安全不报错
    }

    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= followRange)
        {
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (player.position.x < transform.position.x ? -1 : 1);
            transform.localScale = scale;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, spawnPosition, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Player_health playerHealth = collision.GetComponent<Player_health>();
            if (playerHealth != null) playerHealth.GetDamage();
        }
    }

    public override void Die()
    {
        // ✅ 完全跳过anim和血条，直接销毁
        GetComponent<Collider2D>().enabled = false;
        Destroy(gameObject);
    }
}