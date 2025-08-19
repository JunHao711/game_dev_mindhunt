using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TutorialAction
{
    None,
    MoveRight,
    MoveLeft,
    Jump,
    Fire,
    FireUp, 
    Health, 
    Coin
}

[RequireComponent(typeof(Collider2D))]
public class TutorialTrigger : MonoBehaviour
{
    [TextArea] public string message = "Tutorial hint.";
    public HintPopupUI popup;                  // Assign Canvas → HintPopup
    public TutorialAction action = TutorialAction.None;
    public bool oneShot = true;

    private bool playerInside = false;
    private bool consumed = false;
    private bool armed = false;                // becomes true when popup is shown

    void Reset()
    {
        GetComponent<Collider2D>().isTrigger = true;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (consumed || !other.CompareTag("Player")) return;

        playerInside = true;
        armed = true;                          // require a fresh press after showing
        popup?.Show(message);

        // ✅ Immediately unlock when you hit the trigger
        switch (action)
        {
            case TutorialAction.MoveRight: PlayerAbilityLock.UnlockRight(); break;
            case TutorialAction.MoveLeft:  PlayerAbilityLock.UnlockLeft();  break;
            case TutorialAction.Jump:      PlayerAbilityLock.UnlockJump();  break;
            case TutorialAction.Fire:      PlayerAbilityLock.UnlockShoot(); break;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerInside = false;
        armed = false;
        popup?.Hide();
        if (oneShot) consumed = true;
    }

    void Update()
    {
        if (!playerInside || consumed || !armed) return;

        switch (action)
        {
            case TutorialAction.Fire:
                if (Input.GetMouseButtonDown(0)) Dismiss();
                break;

            case TutorialAction.FireUp:
                // Hold Up (W or ↑) and click LMB
                if ((Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) &&
                    Input.GetMouseButtonDown(0))
                {
                    Dismiss();
                }
                break;

            case TutorialAction.MoveRight:
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
                    Dismiss();
                break;

            case TutorialAction.MoveLeft:
                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
                    Dismiss();
                break;

            case TutorialAction.Jump:
                if (Input.GetKeyDown(KeyCode.Space)) Dismiss();
                break;

            case TutorialAction.None:
            default:
                // Remains visible until leaving the zone.
                break;
        }
    }

    private void Dismiss()
    {
        armed = false;
        popup?.Hide();
        if (oneShot) consumed = true;

        switch (action)
        {
            case TutorialAction.MoveRight: PlayerAbilityLock.UnlockRight(); break;
            case TutorialAction.MoveLeft:  PlayerAbilityLock.UnlockLeft();  break;
            case TutorialAction.Jump:      PlayerAbilityLock.UnlockJump();  break;
            case TutorialAction.Fire:      PlayerAbilityLock.UnlockShoot(); break;
        }
    }
}
