using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlexibleScaler : MonoBehaviour
{
    public Image backgroundImage;
    public CanvasScaler canvasScaler;

    private void Start()
    {
        // Ustawienie początkowej skali tła
        backgroundImage.rectTransform.localScale = Vector3.one;
    }

    private void Update()
    {
        // Pobranie aktualnej rozdzielczości ekranu
        Vector2 currentResolution = new Vector2(Screen.width, Screen.height);

        // Obliczenie skali na podstawie aktualnej rozdzielczości i reference resolution
        float scale = Mathf.Min(currentResolution.x / canvasScaler.referenceResolution.x, currentResolution.y / canvasScaler.referenceResolution.y);

        // Zmiana skali tła
        backgroundImage.rectTransform.localScale = new Vector3(scale, scale, 1f);
    }
}
