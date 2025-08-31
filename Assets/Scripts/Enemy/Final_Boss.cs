using System.Collections;
using UnityEngine;

public class Final_Boss : MonoBehaviour
{
    [Header("Detection & Attack")]
    public float detectingRange = 8f;      // start reacting to player
    public float attackRange = 2f;      // trigger attack when player is this close
    public float timeBetweenAttacks = 1f;  // cooldown between attacks

    [Header("Refs (auto if left empty)")]
    public GameObject player;              // will auto-find by tag "Player" if null

    // Animator parameters (keep names exactly the same in Animator)
    private const string PARAM_IN_RANGE = "Player_In_Range";
    private const string PARAM_ATTACK = "Attack";

    private Animator anim;
    private bool isAttacking;

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");

        anim = GetComponent<Animator>();
        if (anim == null)
            Debug.LogError("[Final_Boss] Missing Animator component.");
    }

    void Update()
    {
        if (player == null || anim == null) return;

        float distance = Vector2.Distance(transform.position, player.transform.position);

        // Set bool for Animator (used for idle/alert logic or movement)
        bool inRange = distance <= detectingRange;
        anim.SetBool(PARAM_IN_RANGE, inRange);

        // If close enough and not on cooldown -> trigger attack once
        if (inRange && distance <= attackRange && !isAttacking)
            StartCoroutine(AttackAfterDelay());
    }

    private IEnumerator AttackAfterDelay()
    {
        isAttacking = true;           // lock until cooldown finishes
        anim.SetTrigger(PARAM_ATTACK);// Animator will transition idle -> Attack (Trigger)
        yield return new WaitForSeconds(timeBetweenAttacks);
        isAttacking = false;
    }

    // Editor helper: visualize ranges
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectingRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

    // Keep values sane if edited in Inspector
    private void OnValidate()
    {
        detectingRange = Mathf.Max(0f, detectingRange);
        attackRange = Mathf.Clamp(attackRange, 0f, detectingRange);
        timeBetweenAttacks = Mathf.Max(0.05f, timeBetweenAttacks);
    }
}
