using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ModalPanelAboutEnemy : MonoBehaviour
{
    public GameObject modalPanel;
    private TextMeshProUGUI modalInfoText;
    private PlayerSettings playerSettings;

    private string enemyName;

    void Start ()
    {
        // Ustawienie referencji do powiązanych skryptów
        playerSettings = FindObjectOfType<PlayerSettings>().GetComponent<PlayerSettings>();

        // Wszystkie części okna są ustawialne
        // modalPanel = GameObject.Find("ModalPanelAboutEnemy");
        modalInfoText = GetComponentInChildren<TextMeshProUGUI>();

        this.ShowModal(false);
	}

    /*
    * Pokaż okno modalne
    */
    public void ShowModalPanel(string enemyName)
    {
        this.ShowModal(true);

        this.enemyName = enemyName;
        PlayerSettings.EnemyBetConfigurations enemyConfigurations = playerSettings.playerData.betConfigurations.enemyConfigurations.Find(config => config.enemyName == this.enemyName);
        modalInfoText.SetText(enemyConfigurations.info);
    }

    /*
    * Akceptacja
    */
    public void akcept()
    {
        this.ShowModal(false);
        
        // Zapis typu KLUCZ - WARTOŚĆ by odczytać ją na innej scenie, a nie ładowac do jsona
        PlayerPrefs.SetString("enemyName", this.enemyName);
        PlayerPrefs.Save();
        // Przekierowanie na scenę gry
        SceneManager.LoadScene("Game");
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
            GameObject.Find("Back").GetComponent<Button>().interactable = true;
            GameObject.Find("Levels").GetComponent<CanvasGroup>().interactable = true;
        }
        else
        {
            GameObject.Find("Back").GetComponent<Button>().interactable = false;
            GameObject.Find("Levels").GetComponent<CanvasGroup>().interactable = false;
        }

        // modalPanel.SetActive(value);
        if (modalPanel != null)
        {
            modalPanel.SetActive(value);
        }
    }
}
