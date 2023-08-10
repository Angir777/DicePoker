using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using UnityEngine.SceneManagement;
using TMPro;

public class NewGame : MonoBehaviour
{
    private PlayerSettings playerSettings;
    private QuestionModalPanel questionModalPanel;
    private GameObject continueButton;
    private string filePath;

    private void Start()
    {
        // Ustawienie referencji do powiązanych skryptów
        playerSettings = FindObjectOfType<PlayerSettings>().GetComponent<PlayerSettings>();
        questionModalPanel = FindObjectOfType<QuestionModalPanel>().GetComponent<QuestionModalPanel>();
        questionModalPanel.ShowModal(false);

        if (this.CheckPlayerSettings() == false)
        {
            continueButton = GameObject.Find("Continue");
            continueButton.GetComponent<Button>().interactable = false;

            // TextMeshProUGUI textComponent = continueButton.GetComponentInChildren<TextMeshProUGUI>();
            // Color color;
            // ColorUtility.TryParseHtmlString("#000000", out color);
            // textComponent.color = color;
        }
    }

    public void ShowModalNewGame()
    {
        if (this.CheckPlayerSettings() == true)
        {
            // Modal czy na pewno gracz chce rozpocząć nową grę?
            questionModalPanel.ShowModalPanel("Are you sure you want to start a new game? Previous progress will be lost.");
        }
        else
        {
            questionModalPanel.akcept();
        }
    }

    /*
    * Sprawdza czy jest plik z ustawieniami
    */
    private bool CheckPlayerSettings()
    {
        filePath = Path.Combine(Application.persistentDataPath, "game_settings.json");
        if (File.Exists(filePath))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
