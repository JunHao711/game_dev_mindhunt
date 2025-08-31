using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_level2 : MonoBehaviour
{
    private GameObject player;
    public float detectingRange;
    public float timeBetweenattacks = 1f;
    private Animator anim;

    public float moveSpeed;
    public float attackRange;
    private bool isattacking = false;

    public int currentHealth, maxHealth, damageAmount;
    private bool isDead = false;

    public Bosshealthbar bosshealthbar;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
        bosshealthbar.SetMaxHealth(currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        float distance=Vector2.Distance(transform.position, player.transform.position);

        if (distance < detectingRange)
        {
            anim.SetBool("playerInRange", true);
            
            if(distance <= attackRange && !isattacking)
            {
                StartCoroutine(AttackAfterDelay());
            }

            else if (!isattacking)
            {
                MoveTowardsPlayer();
            }

        }

        else
        {
            anim.SetBool("playerInRange", false);
        }
    }

    private void MoveTowardsPlayer()
    {
        Vector2 playerPosition = new Vector2(player.transform.position.x, transform.position.y);
        Vector2 direction = (playerPosition - (Vector2)transform.position).normalized;

        if(direction.x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }

        else if(direction.x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        
        transform.Translate(direction * moveSpeed * Time.deltaTime);
    }

    IEnumerator AttackAfterDelay()
    {
        isattacking = true;
        anim.SetTrigger("Attack");
        yield return new WaitForSeconds(timeBetweenattacks);
        isattacking = false;
    }

    public void TakeDamage()
    {
        currentHealth -= damageAmount;
        bosshealthbar.SetHealth(currentHealth);

        if(currentHealth <= 0)
        {
            Die();
        }
    }

    //private void Die()
    //{
    //    isDead = true;
    //    anim.SetBool("isDead", true);
    //    GetComponent<Collider2D>().enabled = false;
    //    this.enabled = false;
    //    Destroy(gameObject, 2f);
    //}

    private void Die()
    {
        anim.SetBool("isDead", true);
        anim.speed = 1f;                      // just in case
        anim.Play("dead", 0, 0f);             // force play the 'dead' state on layer 0

        var col = GetComponent<Collider2D>();
        if (col) col.enabled = false;

        var rb = GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            rb.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        this.enabled = false;                  // stops your Translate() loop :contentReference[oaicite:0]{index=0}
        Destroy(gameObject, 2f);               // match your dead clip length
    }

}
