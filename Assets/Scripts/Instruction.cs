using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Instruction : MonoBehaviour
{
    private TextMeshProUGUI myText;
    
	void Start ()
    {
        // Obiekt wyświetlanego tekstu
        myText = GetComponent<TextMeshProUGUI>();
	}

    /*
    * Metoda pomocnicza w innych skryptach - pomaga ustawić odpowiedni komunikat
    */
    public void UpdateInstructionText(string text)
    {
        myText.text = text;
    }
}
