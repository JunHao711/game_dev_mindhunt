using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemy : Enemy
{
    [Header("Flying Enemy Settings")]
    public float speed = 3f;
    public float followRange = 6f;
    private Vector3 spawnPosition;

    public override void Start()
    {
        base.Start();
        spawnPosition = transform.position;
    }

    public override void Update()
    {
        if (currentHealth <= 0) return;
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= followRange)
        {
            // ✅ 追踪玩家
            transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);

            // ✅ 翻转朝向
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) * (player.position.x < transform.position.x ? -1 : 1);
            transform.localScale = scale;
        }
        else
        {
            // ✅ 回到出生点
            transform.position = Vector3.MoveTowards(transform.position, spawnPosition, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        //if (collision.CompareTag("Player"))
        //{
        //    Player_health playerHealth = collision.GetComponent<Player_health>();
        //    if (playerHealth != null) playerHealth.GetDamage(damageAmount);
        //}

        var hp = other.GetComponentInParent<Player_health>();
        if (hp != null)
        {
            Debug.Log("[FlyingEnemy] body hit");
            hp.GetDamage(damageAmount);
        }
    }

    public override void Die()
    {
        anim.SetBool("isDead", true);
        GetComponent<Collider2D>().enabled = false;
        this.enabled = false;
        Destroy(gameObject, 2f);
    }
}