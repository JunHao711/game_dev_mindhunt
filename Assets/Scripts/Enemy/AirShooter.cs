using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AirShooter : Enemy   
{
    [Header("Wander (random move in air)")]
    public float moveSpeed = 2.2f;
    public float roamWidth = 8f;
    public float roamHeight = 5f;
    public float retargetMin = 0.8f;
    public float retargetMax = 1.8f;
    public bool faceMoveDir = true;

    [Header("Shoot (rock prefab)")]
    public GameObject rockPrefab;
    public Transform firePoint;
    public float bulletSpeed = 7f;
    public float shootRadius = 12f;
    public float shootIntervalMin = 0.7f;
    public float shootIntervalMax = 1.8f;
    public float bulletLife = 5f;

    [Header("Damage intake")]
    public LayerMask playerBulletLayers;   
    public int damageFromBullet = 1;        
    public bool destroyBulletOnHit = true;
    private bool isDead = false;

    private Vector3 origin, target;
    private float nextRetargetAt = 0f;


    public override void Start()
    {
        base.Start();                       
        origin = transform.position;
        PickNewTarget();
        StartCoroutine(ShootLoop());
    }

    void Update()
    {
        if (Time.time >= nextRetargetAt || Vector2.Distance(transform.position, target) < 0.2f)
            PickNewTarget();

        Vector2 toTarget = (target - transform.position);
        Vector2 step = toTarget.normalized * moveSpeed * Time.deltaTime;
        transform.position += (Vector3)step;

        if (faceMoveDir && Mathf.Abs(step.x) > 0.0001f)
        {
            float dir = Mathf.Sign(step.x);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * dir,
                                               transform.localScale.y, transform.localScale.z);
        }
    }

    void PickNewTarget()
    {
        float rx = Random.Range(-roamWidth * 0.5f, roamWidth * 0.5f);
        float ry = Random.Range(-roamHeight * 0.5f, roamHeight * 0.5f);
        target = origin + new Vector3(rx, ry, 0f);
        nextRetargetAt = Time.time + Random.Range(retargetMin, retargetMax);
    }

    IEnumerator ShootLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(shootIntervalMin, shootIntervalMax));
            if (player == null) continue; 
            if (Vector2.Distance(transform.position, player.position) > shootRadius) continue;
            FireOne();
        }
    }

    void FireOne()
    {
        if (rockPrefab == null || firePoint == null || player == null) return;

        Vector2 dir = (player.position - firePoint.position).normalized;

        GameObject rock = Instantiate(rockPrefab, firePoint.position, Quaternion.identity);
        var rb = rock.GetComponent<Rigidbody2D>();
        if (rb == null) rb = rock.AddComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Kinematic;
        rb.gravityScale = 0f;
        rb.velocity = dir * bulletSpeed;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rock.transform.rotation = Quaternion.Euler(0, 0, angle);
        Destroy(rock, bulletLife);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Vector3 c = Application.isPlaying ? origin : transform.position;
        Gizmos.DrawWireCube(c, new Vector3(roamWidth, roamHeight, 0));
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, shootRadius);
    }
    public override void takeDamage()
    {
        base.takeDamage();
    }
}
