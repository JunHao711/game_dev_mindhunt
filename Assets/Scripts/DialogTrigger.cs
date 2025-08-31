using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public Dialog dialog;   // assign your Dialog component in Inspector
    public int dialogId = 1; // which dialog index to play

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            dialog.ShowDialog(dialogId);
            GetComponent<Collider2D>().enabled = false; // avoid double fire
            // Optionally disable trigger after first use:
            // Destroy(gameObject);
        }
    }
}
