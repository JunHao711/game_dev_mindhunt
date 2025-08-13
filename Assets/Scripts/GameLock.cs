using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLock : MonoBehaviour
{
    public static bool InputLocked { get; private set; }

    public static void Lock()   => InputLocked = true;
    public static void Unlock() => InputLocked = false;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
