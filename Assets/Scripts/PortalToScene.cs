using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalToScene : MonoBehaviour
{
    public string nextSceneName = "Level2";
    public bool useBuildIndexNext = false;
    public bool requirePress = false;
    public KeyCode interactKey = KeyCode.E;
    public float enterDelay = 0f;

    bool playerInside, loading;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            if (!requirePress) TryLoad();
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player")) playerInside = false;
    }
    void Update()
    {
        if (requirePress && playerInside && Input.GetKeyDown(interactKey)) TryLoad();
    }

    void TryLoad()
    {
        if (loading) return;
        loading = true;
        StartCoroutine(LoadRoutine());
    }

    IEnumerator LoadRoutine()
    {
        if (enterDelay > 0) yield return new WaitForSeconds(enterDelay);

        // Check if the current scene is "Tutorial"
        if (SceneManager.GetActiveScene().name == "Tutorial")
        {
            // If in the "Tutorial" scene, load "Menu" scene
            nextSceneName = "Menu";
        }

        // Load the determined next scene
        if (useBuildIndexNext)
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        else
            SceneManager.LoadScene(nextSceneName);
    }
}
