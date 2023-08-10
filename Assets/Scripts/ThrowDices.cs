using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ThrowDices : MonoBehaviour
{
    public bool firstThrow;
    public bool secondThrow;

    private bool anyDicePicked;

    private List<Dice> dices = new List<Dice>();
    private Combinations combinationsInspector;
    private StrifeManager strifeManager;
    private Instruction instruction;
    private AiThrowDices AiThrowDices;

    private GameObject akceptButton;
    private GameObject passButton;
    private GameObject backButton;
    private GameObject throwButton;

    private void Start() {
        for (int i = 0; i < 5; i++)
        {
            dices.Add(GameObject.Find("Dice" + (i + 1).ToString()).GetComponent<Dice>());
        }

        // Ustawienie referencji do powiązanych skryptów
        instruction = FindObjectOfType<Instruction>().GetComponent<Instruction>();
        combinationsInspector = FindObjectOfType<Combinations>();
        strifeManager = FindObjectOfType<StrifeManager>().GetComponent<StrifeManager>();
        AiThrowDices = FindObjectOfType<AiThrowDices>().GetComponent<AiThrowDices>();

        // Ustawiamy dodatkowe przyciski jako nie aktywne
        akceptButton = GameObject.Find("Akcept");
        akceptButton.SetActive(false);
        passButton = GameObject.Find("Pass");
        passButton.SetActive(false);

        throwButton = GameObject.Find("Throw");
        backButton = GameObject.Find("Back");

        anyDicePicked = CheckIfAnyDicePicked();
    }

    private void Update()
    {
        // Jeśli przechodzimy do drugiego rzutu koścmi
        if (secondThrow)
        {
            // Informacja zwracana graczowi
            instruction.UpdateInstructionText("Choose dice to reroll and roll a second time.");

            GetComponentInChildren<TextMeshProUGUI>().text = "Reroll";

            GetComponent<Button>().onClick.RemoveAllListeners();
            GetComponent<Button>().onClick.AddListener(SecondThrow);

            secondThrow = false;

            // Pokazujemy dodatkowe przyciski, ustawiając je jako aktywne
            passButton.SetActive(true); // Pass - automatycznie punkt dostaje przeciwnik
            akceptButton.SetActive(true); // Zaaakceptuj wynik - wywołana zostaje funkcja `AISecondThrow`

            GameObject.Find("Back").GetComponent<Button>().interactable = false;
        }
    }

    /*
    * Pierwszy rzut kośćmi
    */
    public void FirstThrow()
    {
        combinationsInspector.combinationName = "";
        combinationsInspector.combinationName2 = "";
        combinationsInspector.combinationValue = 0;
        combinationsInspector.combinationValue2 = 0;

        StartCoroutine(SetDiceValuesCoroutine(dices));

        // Wyłączenie przycisków
        throwButton.GetComponent<Button>().interactable = false;
        passButton.GetComponent<Button>().interactable = false;
        akceptButton.GetComponent<Button>().interactable = false;

        // Wywołanie rzutu Ai równo z rzutami gracza
        AiThrowDices.AiFirstThrow = true;

        // Przejście do drugiego rzutu
        secondThrow = true;
    }
    private IEnumerator SetDiceValuesCoroutine(List<Dice> diceList)
    {
        foreach (Dice dice in diceList)
        {
            dice.dotCount = Random.Range(1, 7);
            dice.secondThrow = true;

            yield return new WaitForSeconds(1f); // Opóźnienie między ustawianiem wartości oczek dla kolejnych kości
        }

        combinationsInspector.thrown = true;

        // Włączenie przycisków po losowaniu
        passButton.GetComponent<Button>().interactable = true;
        akceptButton.GetComponent<Button>().interactable = true;
    }

    /*
    * Drugi rzut kośćmi
    */
    public void SecondThrow()
    {
        combinationsInspector.combinationName = "";
        combinationsInspector.combinationName2 = "";
        combinationsInspector.combinationValue = 0;
        combinationsInspector.combinationValue2 = 0; 

        for (int i = 0; i < 5; i++)
        {
            if (dices[i].picked)
            {
                dices[i].dotCount = Random.Range(1, 7);
            }

            dices[i].picked = false;
            dices[i].secondThrow = false;
        }

        combinationsInspector.thrown = true;

        // Wywołanie drugiego rzutu Ai
        AiThrowDices.AiSecondThrow = true;
        // Podsumowanie rundy następuje po ostatnim ruchu Ai
    }

    /*
    * Ustalenie zakładu
    */
    public void NewBet()
    {
        for (int i = 0; i < 5; i++)
        {
            // Kości są puste
            dices[i].blank = true;
        }
        
        // Informacja zwracana graczowi
        instruction.UpdateInstructionText("Set the bet amount.");
        
        // Wynik rzutu jest pusty
        GameObject.Find("Combination").GetComponent<TextMeshProUGUI>().text = "";
        FindObjectOfType<Combinations>().GetComponent<Combinations>().combinationName = "";
        
        // Pierwszy rzut - firstThrow
        GetComponentInChildren<TextMeshProUGUI>().text = "Roll the dice";

        GetComponent<Button>().onClick.RemoveAllListeners();
        GetComponent<Button>().onClick.AddListener(FirstThrow);

        FindObjectOfType<GoldManager>().isBet = true;

        throwButton.SetActive(true); 
        backButton.SetActive(true);
    }

    /*
    * Nasłuchujemy, czy wybrano kości do ponownego rzutu
    */
    public void dicePicked()
    {
        // Czy jakaś kostka została wybrana?
        anyDicePicked = CheckIfAnyDicePicked();
        // Aktualizuj stan przycisku na podstawie zmiennej anyDicePicked
        if (GameObject.Find("Throw") == true)
        {
            GameObject.Find("Throw").GetComponent<Button>().interactable = anyDicePicked;
        }
    }
    private bool CheckIfAnyDicePicked()
    {
        foreach (var dice in dices)
        {
            if (dice.picked)
            {
                return true; // Jeśli znajdziemy choć jedną wybraną kostkę, zwracamy true
            }
        }
        return false; // Jeśli nie znaleziono żadnej wybranej kostki, zwracamy false
    }
}
