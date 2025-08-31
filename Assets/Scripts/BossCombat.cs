using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCombat : MonoBehaviour
{
    public Transform attackPoint;
    public float attackRadius = 1f;
    public LayerMask playerLayer;
    public int playerDamageAmount;

    // Update is called once per frame
    void Update()
    {
        Collider2D[] hitPlayers = Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerLayer);
        
        foreach(Collider2D player in hitPlayers)
        {
            Player_health playerHealth = player.GetComponent<Player_health>();
            if (playerHealth != null)
            {
                playerHealth.GetDamage(playerDamageAmount);
            }
        }
            
            
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
