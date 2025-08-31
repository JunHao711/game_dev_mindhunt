using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_bullet : MonoBehaviour
{
    public float bulletSpeed;
    private Rigidbody2D rb;

    public float maxDistance = 10f;
    private Vector3 spawnPosition;

    private Player playerController;
    private GameObject playerObj;
    public int damageAmount = 1;

    public GameObject hitEffect;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        //Destroy(gameObject, 2f);
    }

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spawnPosition = transform.position;
    }

    public void SetDirection(Vector2 direction)
    {
        rb.velocity = direction.normalized * bulletSpeed;

        // 翻转子弹外观（sprite）方向
        if (direction.x < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x = -Mathf.Abs(scale.x); // 向左：负值
            transform.localScale = scale;
        }
        else
        {
            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x); // 向右：正值
            transform.localScale = scale;
        }
    }

    //// Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(transform.position, spawnPosition) >= maxDistance)
        {
            Destroy(gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            Enemy enemy = collision.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.takeDamage();
            }

            SpawnerTower tower = collision.GetComponent<SpawnerTower>();
            if (tower != null)
            {
                tower.TakeDamage(damageAmount);
            }

            Destroy(gameObject);
        }
        else if (collision.gameObject.CompareTag("Box"))
        {
            Destroy(gameObject);
        }

        else if(collision.gameObject.CompareTag("boss_level2"))
        {
            if(hitEffect != null)
            {
                Instantiate(hitEffect, collision.transform.position, collision.transform.rotation);
            }

            var boss = collision.GetComponent<boss_level2>();
            if(boss != null)
            {
                boss.TakeDamage();
            }
            Destroy(gameObject);
        
        }

        else if (collision.gameObject.CompareTag("Final_Boss"))
        {
            if (hitEffect != null)
            {
                Instantiate(hitEffect, collision.transform.position, collision.transform.rotation);
            }

            var Final_boss = collision.GetComponent<Final_Boss>();
            if (Final_boss != null)
            {
                Final_boss.TakeDamage();
            }
            Destroy(gameObject);

        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
