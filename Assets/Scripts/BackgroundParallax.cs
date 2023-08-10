using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundParallax : MonoBehaviour
{
    public float parallaxStrength = 0.1f; // Określa siłę efektu paralaksy.

    private Vector3 lastMousePosition;
    private Vector3 initialBackgroundPosition;

    void Start()
    {
        lastMousePosition = Input.mousePosition;
        initialBackgroundPosition = transform.position;
    }

    void Update()
    {
        Vector3 mouseDelta = Input.mousePosition - lastMousePosition;
        Vector3 parallaxMovement = new Vector3(mouseDelta.x, mouseDelta.y, 0) * parallaxStrength;

        // Przesuń obiekt tła zgodnie z wektorem parallaxMovement
        transform.position += parallaxMovement;

        lastMousePosition = Input.mousePosition;
    }
}
