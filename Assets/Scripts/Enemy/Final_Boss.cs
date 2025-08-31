using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Final_Boss : MonoBehaviour
{
    public GameObject player;
    public float detectingRange;
    public float timeBetweenattacks = 1f;
    private Animator anim;

    public float attackRange;
    private bool isAttacking = false;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);

        if (distance < detectingRange)
        {
            anim.SetBool("Player_In_Range", true);

            if(distance = attackRangee && !isAttacking)
            {
                //continue
            }

        }

        else
        {
            anim.SetBool("Player_In_Range", false);
        }
    }

    private void 
}
