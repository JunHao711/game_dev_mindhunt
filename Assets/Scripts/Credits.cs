using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    public GameObject creditScreen;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(creditsRun());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator creditsRun()
    {
        yield return new WaitForSeconds(0.5f);
        creditScreen.SetActive(true);
        yield return new WaitForSeconds(15f);
        SceneManager.LoadScene(0);
    }
}
