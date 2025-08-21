using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_shooting : MonoBehaviour
{
    public GameObject bullet;
    public Transform firePos;

    public float timeShots;
    private bool shooting = true;

    private Animator anim;
    private Player player;


    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        // GameLock.InputLocked lock the motion of player
        if(!GameLock.InputLocked && PlayerAbilityLock.allowShoot && Input.GetMouseButtonDown(0) && shooting)
        {

            Shoot();
        }
    }

    private void Shoot()
    {
        anim.SetTrigger("Shoot");
        AudioManager.Instance.PlaySFX(3);
        StartCoroutine(ShootDelay());
    }

    public void FireBullet()
    {
        GameObject newBullet = Instantiate(bullet, firePos.position, Quaternion.identity);
        //Vector2 direction = player.isFacingRight() ? Vector2.right : Vector2.left;
        //newBullet.GetComponent<player_bullet>().SetDirection(direction);

        // If Up Arrow is held while shooting, fire straight up.
        // Otherwise, fire horizontally based on facing.
        Vector2 direction = Input.GetKey(KeyCode.E)
            ? Vector2.up
            : (player.isFacingRight() ? Vector2.right : Vector2.left);

        newBullet.GetComponent<player_bullet>().SetDirection(direction);
    }

    IEnumerator ShootDelay()
    {
        shooting = false;
        yield return new WaitForSeconds(timeShots);
        shooting=true;

    }

}
