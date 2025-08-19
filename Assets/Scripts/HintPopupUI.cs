using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class HintPopupUI : MonoBehaviour
{
    [SerializeField] private CanvasGroup group;
    [SerializeField] private TextMeshProUGUI hintText;

    void Reset()
    {
        group = GetComponent<CanvasGroup>();
        hintText = GetComponentInChildren<TextMeshProUGUI>(true);
    }

    // Do NOT hide in Awake/Start. The object should start inactive in the Hierarchy.

    public void Show(string message)
    {
        if (!gameObject.activeSelf) gameObject.SetActive(true); // safe: won't trigger a hide
        hintText.text = message;
        group.alpha = 1f;
        group.interactable = false;
        group.blocksRaycasts = false;
    }

    public void Hide()
    {
        // Keep it active; just make it invisible and non-blocking
        group.alpha = 0f;
        group.interactable = false;
        group.blocksRaycasts = false;
    }
}

