using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class canoonShooter : MonoBehaviour
{
    public GameObject rockPrefab;
    public Transform muzzle;
    public float interval = 1.5f;
    public float speed = 10f;
    public bool useGravityArc = false;
    public bool onlyWhenPlayerInRange = true;
    public float detectRange = 8f;
    public LayerMask playerMask;

    Rigidbody2D rbTemp;

    IEnumerator Start()
    {
        if (!rockPrefab || !muzzle)
        {
            Debug.LogError("CannonShooter: rockPrefab 或 muzzle 未设置", this);
            yield break;
        }
        if (rockPrefab.scene.IsValid())
        {
            Debug.LogError("CannonShooter: rockPrefab 指向场景对象，请拖入 Project 面板里的 prefab 资产", rockPrefab);
            yield break;
        }

        while (true)
        {
            if (!onlyWhenPlayerInRange || PlayerInFront())
                Shoot();

            yield return new WaitForSeconds(interval);
        }
    }

    bool PlayerInFront()
    {
        var hit = Physics2D.OverlapCircle(muzzle.position, detectRange, playerMask);
        return hit != null;
    }

    void Shoot()
    {
        var go = Instantiate(rockPrefab, muzzle.position, muzzle.rotation);
        var rb = go.GetComponent<Rigidbody2D>();
        if (rb)
        {
            rb.gravityScale = useGravityArc ? Mathf.Max(1f, rb.gravityScale) : 0f;
            rb.velocity = (Vector2)muzzle.right * speed;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (muzzle)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(muzzle.position, muzzle.right * 0.6f);
            Gizmos.DrawWireSphere(muzzle.position, detectRange);
        }
    }
}
