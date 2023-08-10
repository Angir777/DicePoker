using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ModalPanel : MonoBehaviour
{
    private string whatToRun;

    private StrifeManager strifeManager;
    private GameObject modalPanel;
    private TextMeshProUGUI modalInfoText;
    private Button modalAkceptButton;

    void Start ()
    {
        // Ustawienie referencji do powiązanych skryptów
        strifeManager = FindObjectOfType<StrifeManager>().GetComponent<StrifeManager>();
        // Wszystkie części okna są ustawialne
        modalPanel = GameObject.Find("ModalPanel");
        modalInfoText = GetComponentInChildren<TextMeshProUGUI>();
        modalAkceptButton = GetComponentInChildren<Button>();
	}

    /*
    * Pokaż okno modalne
    */
    public void ShowModalPanel(string infoText, string action)
    {
        this.ShowModal(true);

        modalInfoText.SetText(infoText);

        whatToRun = action; // Przechwytujemy nazwę funkcji która ma się wykonć po zamnięciu okna modalnego

        modalAkceptButton.onClick.AddListener(CloseModalPanel);
    }

    /*
    * Zamknij okno modalne
    */
    private void CloseModalPanel()
    {
        this.ShowModal(false);

        modalAkceptButton.onClick.RemoveListener(CloseModalPanel);
        
        // Okresla jaka metoda ma się wykonac w "StrifeManager" po zamknięciu okna
        // Jeśli modal ma gdzie indziej prowadzić po zamknieciu, to tutaj określamy gdzie
        if (whatToRun == "StartNewRound")
        {
            strifeManager.StartNewRound();
        }
        else if (whatToRun == "EndGame")
        {
           SceneManager.LoadScene("Continue");
        }
    }

    /*
    * Ustawia aktywność okna w UI Unity
    */
    public void ShowModal(bool value)
    {
        modalPanel.SetActive(value);
    }
}
