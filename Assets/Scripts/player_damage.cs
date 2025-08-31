using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class player_damage : MonoBehaviour
{
    public int damage;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Player_health playerhealth = collision.gameObject.GetComponent<Player_health>();
            if (playerhealth != null)
            {
                playerhealth.GetDamage(damage);
            }
        }
    }

}
