using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageScreen : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string scene = SceneManager.GetActiveScene().name;

        // Tutorial scene → start with everything locked; triggers will unlock 1 by 1
        if (scene == "Tutorial" || scene == "tutorial")
        {
            PlayerAbilityLock.ResetAll();
        }
        // Level_1 (and any other gameplay level) → everything unlocked by default
        else if (scene == "Level_1" || scene == "level_1")
        {
            PlayerAbilityLock.UnlockAll();
        }
        else
        {
            // Fallback: default to unlocked in other scenes
            PlayerAbilityLock.UnlockAll();
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
