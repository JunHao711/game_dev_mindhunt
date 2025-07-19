using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public string level1;
    public string credit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGame()
    {
        SceneManager.LoadScene(level1);
    }

    public void creditPage()
    {
        SceneManager.LoadScene(credit);
    }

    public void quitGame()
    {
        Application.Quit();
        UnityEngine.Debug.Log("Quit Game");
    }
}
