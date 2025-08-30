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


    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
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
}
