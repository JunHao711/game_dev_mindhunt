using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTower : MonoBehaviour
{
    [Header("Spawner Settings")]
    public GameObject enemyPrefab;
    public float spawnCooldown = 3f;
    private float nextSpawnTime;

    [Header("Spawn Position Settings")]
    public Transform spawnPoint;  // ✅ 可选：刷怪用的位置
    public Vector3 spawnOffset = new Vector3(0, 1f, 0);  // ✅ 默认往上偏移

    [Header("Ground Enemy Patrol Points")]
    public Transform groundPointA;
    public Transform groundPointB;

    [Header("Health Settings")]
    public int maxHealth = 20;
    private int currentHealth;
    public Enemy_health_bar healthBar;

    private void Start()
    {
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnEnemy();
            nextSpawnTime = Time.time + spawnCooldown;
        }
    }

    private void SpawnEnemy()
    {
        Vector3 spawnPos = (spawnPoint != null) ? spawnPoint.position : transform.position + spawnOffset;
        GameObject newEnemy = Instantiate(enemyPrefab, spawnPos, Quaternion.identity);

        // ✅ 生成GroundEnemy时，重设pointA、pointB
        GroundEnemy groundEnemy = newEnemy.GetComponent<GroundEnemy>();
        if (groundEnemy != null && groundPointA != null && groundPointB != null)
        {
            groundEnemy.pointA = groundPointA;
            groundEnemy.pointB = groundPointB;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Destroy(gameObject);
    }
}