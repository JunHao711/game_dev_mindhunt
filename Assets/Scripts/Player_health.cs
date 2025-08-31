using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Player_health : MonoBehaviour
{
    public int current_health;
    public int max_health;
    //public int damage;
    public int healthPickupAmount = 1;

    public Health_bar healthbar;

    public float immortalTime = 2f;
    private float immortalCounter;

    private SpriteRenderer spriteRenderer;
    private Animator animator;

    public float blinkInterval = 0.1f;
    private bool isDead = false;

    public GameObject gameoverScreen;

    // Start is called before the first frame update
    void Start()
    {
        current_health = max_health;
        healthbar.setMaxHealth(current_health);
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>(); 

    }

    // Update is called once per frame
    void Update()
    {
        if(immortalCounter > 0 )
        {
            immortalCounter -= Time.deltaTime;
        }
    }

    public void GetDamage(int damage)
    {
        // avoid starting coroutines on inactive/dead player
        if (isDead || !isActiveAndEnabled || !gameObject.activeInHierarchy)
            return;

        if (immortalCounter <= 0)
        {
            current_health -= damage;
            healthbar.setHealth(current_health);

            if (current_health <= 0)
            {
                StartCoroutine(PlayDeathAnimation());
            }
            else
            {
                AudioManager.Instance.PlaySFX(6);
                StartCoroutine(Blink());
            }
        }
    }

    IEnumerator PlayDeathAnimation()
    {
        if (isDead) yield break;
        isDead = true;
        animator.SetTrigger("Die");
        AudioManager.Instance.PlaySFX(5);
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

        if (gameoverScreen != null) gameoverScreen.SetActive(true);


        gameObject.SetActive(false);
    }

    IEnumerator Blink()
    {
        immortalCounter = immortalTime;

        while (immortalCounter > 0)
        {
            spriteRenderer.enabled = false;
            yield return new WaitForSeconds(blinkInterval);
            spriteRenderer.enabled = true;
            yield return new WaitForSeconds(blinkInterval);

            immortalCounter -= blinkInterval * 2;
        }

        spriteRenderer.enabled = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Immortal_Potion")
        {
            immortalCounter = immortalTime;
            Destroy(collision.gameObject);
            //immortalEffect.SetActive(true);
        }
        if (collision.gameObject.tag == "Healing_Potion")
        {
            current_health += healthPickupAmount;
            healthbar.setHealth(current_health);
            AudioManager.Instance.PlaySFX(4);
            if (current_health > max_health)
            {
                current_health = max_health;
            }

            Destroy(collision.gameObject);
        }
    }
}
