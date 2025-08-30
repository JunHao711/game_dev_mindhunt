using UnityEngine;

public class SuicideCar : MonoBehaviour
{
    [Header("Start / Trigger")]
    public bool autoStart = false;
    public float detectRange = 8f;         
    public float windupTime = 0.25f;       

    [Header("Movement")]
    public float moveSpeed = 6f;        

    [Header("Damage / Dodge")]
    public float jumpClearance = 0.15f;    
    public bool damageOnce = true;      
    [Header("Cleanup")]
    public bool destroyWhenInvisible = true;

    private Rigidbody2D rb;
    private Collider2D myCol;
    private Transform player;

    private bool started = false;
    private bool hitOnce = false;
    private float windupTimer;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        myCol = GetComponent<Collider2D>();

        rb.gravityScale = 0f;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        foreach (var sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.flipX = false;
            var t = sr.transform;
            t.localScale = new Vector3(Mathf.Abs(t.localScale.x), Mathf.Abs(t.localScale.y), Mathf.Abs(t.localScale.z));
            t.localRotation = Quaternion.identity;
        }
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), Mathf.Abs(transform.localScale.y), Mathf.Abs(transform.localScale.z));
        transform.localRotation = Quaternion.identity;
    }

    void Start()
    {
        var p = GameObject.FindGameObjectWithTag("Player");
        if (p) player = p.transform;

        windupTimer = windupTime;

        if (autoStart) BeginMove();
    }

    void Update()
    {
        if (!started)
        {
            if (!autoStart && player)
            {
                float dist = Vector2.Distance(transform.position, player.position);
                if (dist <= detectRange)
                {
                    windupTimer -= Time.deltaTime;
                    if (windupTimer <= 0f) BeginMove();
                }
                else
                {
                    windupTimer = windupTime;
                }
            }
            return; 
        }

        rb.velocity = new Vector2(-moveSpeed, 0f);
    }

    void BeginMove()
    {
        started = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!started) return;
        if (!other.CompareTag("Player")) return;
        TryHit(other);
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        if (!started) return;
        if (!c.collider.CompareTag("Player")) return;
        TryHit(c.collider);
    }

    void TryHit(Component playerComp)
    {
        if (damageOnce && hitOnce) return;

        var playerCol = playerComp.GetComponent<Collider2D>();
        if (playerCol && myCol)
        {
            float carTop = myCol.bounds.max.y;
            float playerBottom = playerCol.bounds.min.y;
            if (playerBottom > carTop + jumpClearance) return;
        }

        var hp = playerComp.GetComponent<Player_health>();
        if (hp) hp.GetDamage();
        hitOnce = true; 
    }

    void OnBecameInvisible()
    {
        if (destroyWhenInvisible && started) Destroy(gameObject);
    }

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        if (!autoStart)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, detectRange);
        }
    }
#endif
}