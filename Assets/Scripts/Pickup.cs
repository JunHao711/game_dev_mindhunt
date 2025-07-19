using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;
using UnityEngine.UI; // use and work with UI

public class Pickup : MonoBehaviour
{
    public int coins = 0;
    public int coinAmount = 100; // default by 1

    public UnityEngine.UI.Text scoreText;


    // Start is called before the first frame update
    void Start()
    {
        scoreText.text = "0";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Coins")
        {
            coins += coinAmount;
            AudioManager.Instance.PlaySFX(0);
            scoreText.text = coins.ToString();
            Destroy(collision.gameObject);
        }
    }
}
