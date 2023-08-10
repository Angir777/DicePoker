using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QuestionModalPanel : MonoBehaviour
{
    private GameObject modalPanel;
    private TextMeshProUGUI modalInfoText;

    private PlayerSettings playerSettings;
    private LevelManager levelManager;

    void Start ()
    {
        // Ustawienie referencji do powiązanych skryptów
        playerSettings = FindObjectOfType<PlayerSettings>().GetComponent<PlayerSettings>();
        levelManager = FindObjectOfType<LevelManager>().GetComponent<LevelManager>();

        // Wszystkie części okna są ustawialne
        modalPanel = GameObject.Find("QuestionModalPanel");
        modalInfoText = GetComponentInChildren<TextMeshProUGUI>();
	}

    /*
    * Pokaż okno modalne
    */
    public void ShowModalPanel(string infoText)
    {
        this.ShowModal(true);

        modalInfoText.SetText(infoText);
    }

    /*
    * Akceptacja
    */
    public void akcept()
    {
        this.ShowModal(false);
        
        // Czyszczenie pliku konfiguracyjnego
        playerSettings.CreateDefaultPlayerSettings();

        levelManager.ChangeScene("NewGame");
    }

    /*
    * Odrzucenie
    */
    public void cancel()
    {
        this.ShowModal(false);
    }

    /*
    * Ustawia aktywność okna w UI Unity
    */
    public void ShowModal(bool value)
    {
        // Jak pokazujemy modal to wyłaczamy interakcję z przyciakmi i całą grupą przycisków
        if (!value)
        {
            GameObject.Find("Menu").GetComponent<CanvasGroup>().interactable = true;
        }
        else
        {
            GameObject.Find("Menu").GetComponent<CanvasGroup>().interactable = false;
        }

        modalPanel.SetActive(value);
    }
}
