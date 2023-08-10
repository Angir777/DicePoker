using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.UI;

public class ButtonColorHoverText : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI textComponent;

    private Color originalColor;

    private Button button;

    void Start()
    {
        originalColor = textComponent.color;
        button = GetComponent<Button>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        bool isInteractable = button.interactable;

        if (IsParentInteractable() && isInteractable)
        {
            textComponent.color = new Color(255, 255, 255, 255);
        }
        else
        {
            textComponent.color = new Color(0, 0, 0, 255);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        textComponent.color = originalColor;
    }

    private bool IsParentInteractable()
    {
        CanvasGroup parentCanvasGroup = GetComponentInParent<CanvasGroup>();
        if (parentCanvasGroup != null)
        {
            return parentCanvasGroup.interactable;
        }
        return true; // Jeśli nie znaleziono CanvasGroup, zwróć true jako domyślną wartość
    }
}
