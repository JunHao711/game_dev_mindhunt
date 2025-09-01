using System.Collections;
using UnityEngine;

public class Final_Boss : MonoBehaviour
{
    public GameObject player;

    [Header("Ranges")]
    public float detectingRange = 8f;
    public float attackRange = 2f;
    public float boxHeight = 4f;
    public float forwardGap = 0f;

    [Header("Attack")]
    public float timeBetweenAttacks = 1f;

    [Header("Health & VFX")]
    public int maxHealth = 10;
    public int damageAmount = 1;                 // 每次受击减少的血量
    public GameObject explosionEffect;           // 在 Inspector 赋值

    [Header("Layer Settings")]
    public LayerMask playerLayer;

    public FinalBoss_healthBar FinalBoss_Healthbar;

    private int currentHealth;
    private Animator anim;
    private SpriteRenderer sr;
    private bool isAttacking = false;

    private static readonly int HASH_IN_RANGE = Animator.StringToHash("Player_In_Range");
    private static readonly int HASH_ATTACK = Animator.StringToHash("Attack");

    void Start()
    {
        currentHealth = maxHealth;

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        FinalBoss_Healthbar.SetMaxHealth(currentHealth);
    }

    void Update()
    {
        if (anim == null) return;

        int facingDir = (sr != null && sr.flipX) ? -1 : 1;

        Vector2 detectCenter = (Vector2)transform.position +
                               Vector2.right * facingDir * (detectingRange * 0.5f + forwardGap);
        Vector2 detectSize = new Vector2(detectingRange, boxHeight);

        Vector2 attackCenter = (Vector2)transform.position +
                               Vector2.right * facingDir * (attackRange * 0.5f + forwardGap);
        Vector2 attackSize = new Vector2(attackRange, boxHeight);

        bool inDetect = Physics2D.OverlapBox(detectCenter, detectSize, 0f, playerLayer);
        bool inAttack = Physics2D.OverlapBox(attackCenter, attackSize, 0f, playerLayer);

        anim.SetBool(HASH_IN_RANGE, inDetect);

        if (inDetect && inAttack && !isAttacking)
            StartCoroutine(AttackAfterDelay());
    }

    private IEnumerator AttackAfterDelay()
    {
        isAttacking = true;
        anim.SetTrigger(HASH_ATTACK);
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    private void OnDrawGizmosSelected()
    {
        if (sr == null) sr = GetComponent<SpriteRenderer>();
        int facingDir = (sr != null && sr.flipX) ? -1 : 1;

        Gizmos.color = Color.yellow;
        Vector2 detectCenter = (Vector2)transform.position +
                               Vector2.right * facingDir * (detectingRange * 0.5f + forwardGap);
        Gizmos.DrawWireCube(detectCenter, new Vector2(detectingRange, boxHeight));

        Gizmos.color = Color.red;
        Vector2 attackCenter = (Vector2)transform.position +
                               Vector2.right * facingDir * (attackRange * 0.5f + forwardGap);
        Gizmos.DrawWireCube(attackCenter, new Vector2(attackRange, boxHeight));
    }

    // —— 受伤/死亡 —— //
    public void TakeDamage()
    {
        currentHealth -= damageAmount;
        FinalBoss_Healthbar.SetHealth(currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        anim.SetBool("isDead", true);
        anim.speed = 1f;                      // just in case
        anim.Play("Die", 0, 0f);             // force play the 'dead' state on layer 0

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
