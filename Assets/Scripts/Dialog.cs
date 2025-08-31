using System.Collections;
using UnityEngine;
using TMPro;

public class Dialog : MonoBehaviour
{
    // [SerializeField] — Allows to set the value in the Inspector even though it’s private.
    [SerializeField] private TextMeshProUGUI textComponent;   // can still drag in Inspector
    [SerializeField] private float textSpeed = 0.02f;
    [SerializeField] private GameObject healthBarUI;
    [SerializeField] private GameObject coinImageUI;
    [SerializeField] private GameObject coinTextUI;

    // Store multiple dialogs here (each entry is a set of lines)
    private readonly string[][] dialogs = new string[][]
    {
        // Level 1
        new string[] {
            "Commander: Shadow Wolf, respond if you hear me.",
            "Shadow Wolf: Copy that, Commander. I am in position. The outskirts of the city are calm, but something feels off.",
            "Commander: You arrived just in time. We have reliable intel that in the past 48 hours, more than 30 people have gone missing in the city — no witnesses, no surveillance footage, not even a trace.",
            "Shadow Wolf: Confirmed. For so many people to vanish without anyone noticing... Could the police be hiding something?",
            "Commander: Unknown. The local law enforcement is in disarray, and some have already 'resigned voluntarily.' Your mission is to uncover the truth and find the cause behind these disappearances.",
            "Shadow Wolf: Target location?",
            "Commander: Initial suspicion points to the East District — the old industrial area. Several missing persons were last seen there, and then vanished without a word.",
            "Shadow Wolf: Understood. I’ll start searching in that area. If someone is pulling the strings behind this, I’ll find them.",
            "Commander: Maintain stealth and do not reveal your identity. Operation Shadow Veil is officially underway. Good luck, Shadow Wolf."
        },
        new string[] {
            "What's that? A flying monster? And a minotaur that looks like the Minotaur?"
        },
        new string[] {
            "Are those traps? Better be careful."
        },
        new string[] {
            "What's that tower? Monsters seem to keep spawning from it.",
            "It seems I need to destroy that tower to deal with the surrounding monsters."
        },
        // Level 2
        new string[] {
            "Boss: You dare challenge me?",
            ""
        },
        new string[] {
            "Boss: You dare challenge me?",
            "Player: I will stop you no matter what!"
        },
        new string[] {
            "Boss: You dare challenge me?",
            "Player: I will stop you no matter what!"
        },
    };

    private string[] lines;
    private int index;

    private void Awake()
    {
        // Auto-find a TMP text under this GameObject if none assigned
        if (textComponent == null)
        {
            textComponent = GetComponentInChildren<TextMeshProUGUI>(true);
        }
    }

    private void Start()
    {
        if (textComponent == null)
        {
            Debug.LogError("Dialog: No TextMeshProUGUI assigned or found.");
            enabled = false;
            return;
        }

    ShowDialog(0); // plays Commander/Shadow Wolf

    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (textComponent.text == lines[index])
            {
                NextLine();
            }
            else
            {
                StopAllCoroutines();
                textComponent.text = lines[index];
            }
        }
    }

    // Call this from other scripts
    public void ShowDialog(int id)
    {
        if (id < 0 || id >= dialogs.Length)
        {
            Debug.LogWarning("Dialog index out of range!");
            return;
        }

        lines = dialogs[id];
        index = 0;

        // make sure this object is active so StartCoroutine works
        if (!gameObject.activeInHierarchy) gameObject.SetActive(true);
        enabled = true;                           // ensure the component is enabled
        if (textComponent) textComponent.enabled = true;

        GameLock.Lock();

        if (healthBarUI) healthBarUI.SetActive(false);
        if (coinImageUI) coinImageUI.SetActive(false);
        if (coinTextUI)  coinTextUI.SetActive(false);

        textComponent.text = string.Empty;
        StopAllCoroutines();
        StartCoroutine(TypeLine());
    }

    private IEnumerator TypeLine()
    {
        foreach (char c in lines[index])
        {
            textComponent.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    private void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textComponent.text = string.Empty;
            StartCoroutine(TypeLine());
        }
        else
        {
            GameLock.Unlock();    // give control back

            // Show UI again
            if (healthBarUI) healthBarUI.SetActive(true);
            if (coinImageUI) coinImageUI.SetActive(true);
            if (coinTextUI) coinTextUI.SetActive(true);

            gameObject.SetActive(false);
        }
    }
}