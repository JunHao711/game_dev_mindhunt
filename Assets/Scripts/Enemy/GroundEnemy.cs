using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    
    [Header("Portal Settings")]
    public GameObject portalPrefab;
    public Transform portalSpawnPoint;
    public float portalDelay = 0.3f;

    public override void Start()
    {
        base.Start();
        if (pointB != null)
        {
            currentTarget = pointB.position;
        }
    }

    public override void Die()
    {
        base.Die();
        isDead = true;

        // Check if the current scene is "tutorial"
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            // If scene is "tutorial", execute the portal spawning logic
            if (portalPrefab && portalDelay > 0f)
                StartCoroutine(SpawnPortalThenDestroy());
            else
            {
                // Immediately spawn and destroy portal
                Vector3 pos = portalSpawnPoint ? portalSpawnPoint.position : transform.position;
                if (portalPrefab) Instantiate(portalPrefab, pos, Quaternion.identity);
            }
        }
        else
        {
        }
    }
    private System.Collections.IEnumerator SpawnPortalThenDestroy()
    {
        yield return new WaitForSeconds(portalDelay);
        Vector3 pos = portalSpawnPoint ? portalSpawnPoint.position : transform.position;
        Instantiate(portalPrefab, pos, Quaternion.identity);
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
        base.takeDamage();
    }
}
