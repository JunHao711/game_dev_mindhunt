using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundEnemy : Enemy
{
    [Header("Ground Enemy Settings")]
    public Transform pointA;
    public Transform pointB;
    public int speed;
    public float attackRange = 1.5f;
    public float followPlayerRange = 5f;

    private Vector3 currentTarget;
    private bool isAttacking = false;
    private bool isDead = false;

    public override void Start()
    {
        base.Start();
        currentTarget = pointB.position;
    }

    public override void Die()
    {
        base.Die();
        isDead = true;
    }

    public override void Update()
    {
        if (isDead) return;
        if (currentHealth <= 0) return;
        if (player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= attackRange)
        {
            anim.SetBool("isAttacking", true);
            isAttacking = true;
            sr.flipX = player.position.x < transform.position.x;
        }
        else if (distance <= followPlayerRange)
        {
            anim.SetBool("isAttacking", false);
            isAttacking = false;
            currentTarget = new Vector3(player.position.x, transform.position.y, transform.position.z);
            sr.flipX = player.position.x < transform.position.x;
        }
        else
        {
            anim.SetBool("isAttacking", false);
            isAttacking = false;
            PatrolLogic();
        }

        if (!isAttacking)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentTarget, speed * Time.deltaTime);
        }
    }

    private void PatrolLogic()
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

    public override void DoDamage()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        if (distance <= attackRange)
        {
            base.DoDamage();
        }
    }
    public override void takeDamage()
    {
        base.takeDamage();  // ✅ 确保死后播放动画
    }
}
