using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ThreeHeadCannon : MonoBehaviour
{
    [System.Serializable]
    public class MuzzleSlot
    {
        public Transform muzzle;
        public bool useTransformRotation = true;   // 勾选=用 muzzle.right；不勾=用下面的 directionOverride
        public Vector2 directionOverride = Vector2.right; // 例如 (0,1)=Up, (-1,0)=Left
    }

    [Header("Projectile")]
    public GameObject rockPrefab;
    public float speed = 10f;
    public bool useGravityArc = false;
    public float angleJitterDeg = 0f;
    public float speedJitter = 0f;
    public float interval = 1.2f;

    [Header("Muzzles")]
    public MuzzleSlot[] muzzles;

    [Header("When to shoot")]
    public bool onlyWhenPlayerInRange = false;
    public float detectRange = 8f;
    public LayerMask playerMask;

    public enum FireMode { RandomOne, RoundRobin, AllAtOnce }
    public FireMode fireMode = FireMode.RandomOne;

    int rrIndex;

    IEnumerator Start()
    {
        while (true)
        {
            if (!onlyWhenPlayerInRange || PlayerInRange()) FireOnce();
            yield return new WaitForSeconds(interval);
        }
    }

    bool PlayerInRange()
    {
        Vector3 c = (muzzles[0]?.muzzle ? muzzles[0].muzzle.position : transform.position);
        return Physics2D.OverlapCircle(c, detectRange, playerMask) != null;
    }

    void FireOnce()
    {
        List<MuzzleSlot> list = new List<MuzzleSlot>();
        if (fireMode == FireMode.AllAtOnce)
        {
            list.AddRange(muzzles);
        }
        else if (fireMode == FireMode.RoundRobin)
        {
            var m = muzzles[rrIndex % muzzles.Length]; rrIndex++; if (m != null) list.Add(m);
        }
        else
        { // RandomOne
            var m = muzzles[Random.Range(0, muzzles.Length)]; if (m != null) list.Add(m);
        }
        foreach (var slot in list) ShootFrom(slot);
    }

    void ShootFrom(MuzzleSlot slot)
    {
        if (!slot.muzzle) return;
        var go = Instantiate(rockPrefab, slot.muzzle.position, slot.muzzle.rotation);
        var rb = go.GetComponent<Rigidbody2D>();
        if (rb)
        {
            Vector2 dir = slot.useTransformRotation
                ? (Vector2)slot.muzzle.right
                : (slot.directionOverride.sqrMagnitude > 0 ? slot.directionOverride.normalized : Vector2.right);

            // 应用抖动
            float ang = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            ang += Random.Range(-angleJitterDeg, angleJitterDeg);
            Vector2 dirJittered = new Vector2(Mathf.Cos(ang * Mathf.Deg2Rad), Mathf.Sin(ang * Mathf.Deg2Rad));

            float v = speed + Random.Range(-speedJitter, speedJitter);
            rb.gravityScale = useGravityArc ? Mathf.Max(1f, rb.gravityScale) : 0f;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
            rb.velocity = dirJittered * v;
            rb.angularVelocity = 0f;
        }
    }

    void OnDrawGizmosSelected()
    {
        if (muzzles == null) return;
        foreach (var slot in muzzles)
        {
            if (!slot?.muzzle) continue;
            Vector2 d = slot.useTransformRotation ? (Vector2)slot.muzzle.right
                                                  : (slot.directionOverride.sqrMagnitude > 0 ? slot.directionOverride.normalized : Vector2.right);
            Gizmos.color = Color.red;
            Gizmos.DrawRay(slot.muzzle.position, (Vector3)d * 0.7f);
        }
        Gizmos.color = new Color(0, 1, 0, 0.2f);
        Vector3 c = (muzzles.Length > 0 && muzzles[0]?.muzzle) ? muzzles[0].muzzle.position : transform.position;
        Gizmos.DrawWireSphere(c, detectRange);
    }
}
