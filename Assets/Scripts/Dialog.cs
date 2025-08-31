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
        new string[] {
            "Commander: Shadow Wolf, respond if you hear me.",
            "Shadow Wolf: Copy that, Commander. I am in position."
        },
        new string[] {
            "Villager: Thank you, hero!",
            "Hero: Stay safe, the danger isn’t over yet."
        },
        new string[] {
            "Boss: You dare challenge me?",
            "Player: I will stop you no matter what!"
        }
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