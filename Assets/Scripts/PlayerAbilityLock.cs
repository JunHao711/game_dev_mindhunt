using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAbilityLock : MonoBehaviour
{
    public static bool allowMoveLeft  = true;
    public static bool allowMoveRight = true;
    public static bool allowJump      = true;
    public static bool allowShoot     = true;

    public static void ResetAll()
    {
        allowMoveLeft = allowMoveRight = allowJump = allowShoot = false;
    }

    public static void UnlockAll()
    {
        allowMoveLeft = allowMoveRight = allowJump = allowShoot = true;
    }

    public static void UnlockLeft()  => allowMoveLeft  = true;
    public static void UnlockRight() => allowMoveRight = true;
    public static void UnlockJump()  => allowJump      = true;
    public static void UnlockShoot() => allowShoot     = true;
}
