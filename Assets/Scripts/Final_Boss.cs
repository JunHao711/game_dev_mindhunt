using UnityEngine;

public class FinalBossEnemy : Enemy
{
    // Boss 专属属性
    public float attackCooldown = 2f;   // 普通攻击冷却时间
    public GameObject specialAttackPrefab; // 特殊技能的子弹
    public Transform firePoint;          // 技能发射点

    private float nextAttackTime = 0f;

    public override void Update()
    {
        base.Update(); // 保留 Enemy 的 Update，可加逻辑

        if (player != null)
        {
            // 朝向玩家
            Vector3 direction = player.position - transform.position;
            if (direction.x < 0)
                sr.flipX = true;
            else
                sr.flipX = false;

            // 攻击逻辑
            if (Time.time >= nextAttackTime)
            {
                if (currentHealth > maxHealth / 2)
                {
                    NormalAttack();
                }
                else
                {
                    SpecialAttack();
                }
                nextAttackTime = Time.time + attackCooldown;
            }
        }
    }

    void NormalAttack()
    {
        anim.SetTrigger("attack");
        DoDamage(); // 调用父类的攻击，让玩家掉血
    }

    void SpecialAttack()
    {
        anim.SetTrigger("special");
        if (specialAttackPrefab != null && firePoint != null)
        {
            Instantiate(specialAttackPrefab, firePoint.position, firePoint.rotation);
        }
    }
}
