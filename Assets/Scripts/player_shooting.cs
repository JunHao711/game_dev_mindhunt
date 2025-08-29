using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player_shooting : MonoBehaviour
{
    [Header("Bullet Prefabs")]
    public GameObject bullet;      // horizontal bullet prefab
    public GameObject bulletUp;    // upward bullet prefab

    [Header("Fire Positions")]
    public Transform firePos;     // left/right fire point
    public Transform fireUpPos;   // upward fire point

    [Header("Cooldown")]
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

    void Update()
    {
        if (GameLock.InputLocked || !PlayerAbilityLock.allowShoot || !shooting) return;

        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
            FireBullet();
        }
        else if (Input.GetMouseButtonDown(1))
        {
            ShootUp();
            FireBulletUp();
        }
    }


    private void Shoot()
    {
        anim.SetTrigger("Shoot");
        AudioManager.Instance.PlaySFX(3);
        StartCoroutine(ShootDelay());
    }

    private void ShootUp()
    {
        anim.SetTrigger("ShootUp");
        AudioManager.Instance.PlaySFX(3);
        StartCoroutine(ShootDelay());
    }

    public void FireBullet()
    {
        GameObject newBullet = Instantiate(bullet, firePos.position, Quaternion.identity);

        Vector2 dir = player.isFacingRight() ? Vector2.right : Vector2.left;

        newBullet.transform.right = dir;

        newBullet.GetComponent<player_bullet>().SetDirection(dir);
    }



    public void FireBulletUp()
    {
        if (bulletUp != null && fireUpPos != null)
        {
            GameObject newBullet = Instantiate(bulletUp, fireUpPos.position, Quaternion.identity);

            newBullet.transform.right = Vector2.up;

            newBullet.GetComponent<player_bullet>().SetDirection(Vector2.up);
        }
    }

    IEnumerator ShootDelay()
    {
        shooting = false;
        yield return new WaitForSeconds(timeShots);
        shooting=true;

    }

}
