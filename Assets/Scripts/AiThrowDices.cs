using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AiThrowDices : MonoBehaviour
{
    public bool AiFirstThrow;
    public bool AiSecondThrow;

    private List<AiDice> AiDices = new List<AiDice>();
    private AiCombinations AiCombinationsInspector;
    
    private StrifeManager strifeManager;

    private void Start() {
        for (int i = 0; i < 5; i++)
        {
            AiDices.Add(GameObject.Find("AiDice" + (i + 1).ToString()).GetComponent<AiDice>());
        }

        // Ustawienie referencji do powiązanych skryptów
        AiCombinationsInspector = FindObjectOfType<AiCombinations>();
        
        strifeManager = FindObjectOfType<StrifeManager>().GetComponent<StrifeManager>();
    }

    private void Update()
    {
        // Jeśli przechodzimy do pierwszego rzutu koścmi
        if (AiFirstThrow)
        {
            this.AIFirstThrow();
        }
        // Jeśli przechodzimy do drugiego rzutu koścmi
        if (AiSecondThrow)
        {
            this.AISecondThrow();
        }
    }

    /*
    * Pierwszy rzut kośćmi
    */
    public void AIFirstThrow()
    {
        AiCombinationsInspector.AiCombinationName = "";
        AiCombinationsInspector.AiCombinationName2 = "";
        AiCombinationsInspector.AiCombinationValue = 0;
        AiCombinationsInspector.AiCombinationValue2 = 0;

        StartCoroutine(SetDiceValuesCoroutine(AiDices));

        AiFirstThrow = false;
    }
    private IEnumerator SetDiceValuesCoroutine(List<AiDice> aiDiceList)
    {
        foreach (AiDice aiDice in aiDiceList)
        {
            aiDice.AiDotCount = Random.Range(1, 7);
            aiDice.AiSecondThrow = true;

            yield return new WaitForSeconds(1f); // Opóźnienie między ustawianiem wartości oczek dla kolejnych kości AI
        }

        AiCombinationsInspector.AiThrown = true;
    }

    /*
    * Drugi rzut kośćmi
    */
    public void AISecondThrow()
    {
        AiCombinationsInspector.AiCombinationName = "";
        AiCombinationsInspector.AiCombinationName2 = "";
        AiCombinationsInspector.AiCombinationValue = 0;
        AiCombinationsInspector.AiCombinationValue2 = 0;

        // Prawdopodobieństwo poddania się (20%)
        float passProbability = UnityEngine.Random.value;
        if (passProbability <= 0.2f)
        {
            // AI pasuje
            this.AiPass();
        }
        else
        {
            // Prawdopodobieństwo przerzucenia kości (60%)
            float rerollProbability = UnityEngine.Random.value;
            if (rerollProbability <= 0.8f)
            {
                // AI chce ponownie rzucić
                this.AiReroll();
            }
            else
            {
                // AI zatwierdza wynik
            }
        }

        // Wybrane kosci są ponownie rzucane
        for (int i = 0; i < 5; i++)
        {
            if (AiDices[i].AiPicked)
            {
                AiDices[i].AiDotCount = Random.Range(1, 7);
            }

            AiDices[i].AiPicked = false;
            AiDices[i].AiSecondThrow = false;
        }

        AiCombinationsInspector.AiThrown = true;

        AiSecondThrow = false;

        // Podsumowanie rundy
        StartCoroutine(ExecuteAfterDelayEvaluateRound(2.0f));
    }

    /*
    * Ustalenie zakładu
    */
    public void NewBet()
    {
        for (int i = 0; i < 5; i++)
        {
            // Kości są puste
            AiDices[i].AiBlank = true;

            // Wynik rzutu jest pusty
            GameObject.Find("AiCombination").GetComponent<TextMeshProUGUI>().text = "";
            FindObjectOfType<AiCombinations>().GetComponent<AiCombinations>().AiCombinationName = "";
        }
    }

    /*
    * Wstrzymanie gry przed podsumowaniem rundy
    */
    IEnumerator ExecuteAfterDelayEvaluateRound(float delay)
    {
        // Gra daje czas na zapoznanie się z wynikiem ostatniego rzutu
        yield return new WaitForSeconds(delay);

        strifeManager.EvaluateRound();
    }

    /*
    * AI pasuje
    */
    private void AiPass()
    {
        // Sprawdzenie jaka jest waga kombinacji kości,
        // jeśli wysoka to ma się nie poddać, bo to nie logiczne
        int computerCombinationValue = AiCombinationsInspector.GetCombinationValue();
        if (computerCombinationValue < 2) {
            // Ai poddaje się
            strifeManager.AiPassRound();
        } else if (computerCombinationValue >= 2) {
            // AI chce ponownie rzucić
            this.AiReroll();
        }
    }

    /*
    * AI chce ponownie rzucić
    */
    private void AiReroll()
    {
        // Tutaj mam wagę kombinacji, czyli wiem co zostało wylosowane w pierwszym rzucie, np. "Para",
        int computerCombinationValue = AiCombinationsInspector.GetCombinationValue();
        // Jeśli kombinacja to "Mały strit, "Duży strit", "Full" lub "Poker", Ai ma nie wybierać kości do ponownego rzutu tylko zaakceptować wynik
        if (!(computerCombinationValue == 4 || computerCombinationValue == 5 || computerCombinationValue == 6 || computerCombinationValue == 8))
        {
            // Jeśli kombinacja to "Nic", Ai rzuca losową ilością kości z puli wszystkich kości
            if (computerCombinationValue == 0)
            {
                // Lista dostępnych kości
                List<int> availableIndices = new List<int>();
                for (int i = 0; i < 5; i++)
                {
                    availableIndices.Add(i);
                }
                // Losuj ilość kostek do ponownego rzutu
                int numberOfDiceToPick = Random.Range(3, 6);
                for (int i = 0; i < numberOfDiceToPick; i++)
                {
                    // Dopóki ma czym rzucać
                    if (availableIndices.Count > 0)
                    {
                        // Losowanie indeksów kości by nie wybierało zawsze od pierwszej z lewej
                        int randomIndex = Random.Range(0, availableIndices.Count);
                        int pickedIndex = availableIndices[randomIndex];
                        AiDices[pickedIndex].AiPicked = true;
                        availableIndices.RemoveAt(randomIndex); // Zapobiega ponownemu rzutowi
                    }
                }
            // Jeśli kombinacja to "Para", "Dwie pary", "Trójka" lub "Kareta",
            // Ai wybiera te kości które sę nie powtarzają, a znich losowo wybiera ilość kości do rzutu
            } else if (computerCombinationValue == 1 || computerCombinationValue == 2 || computerCombinationValue == 3 || computerCombinationValue == 7) {
                // Lista kości, które się nie powtarzają
                List<int> uniqueIndices = new List<int>();
                for (int i = 0; i < AiDices.Count; i++)
                {
                    bool isUnique = true;
                    for (int j = 0; j < AiDices.Count; j++)
                    {
                        if (i != j && AiDices[i].AiDotCount == AiDices[j].AiDotCount)
                        {
                            isUnique = false;
                            break;
                        }
                    }
                    if (isUnique)
                    {
                        uniqueIndices.Add(i);
                    }
                }
                int randomDiceCount = Random.Range(2, uniqueIndices.Count + 1);
                for (int i = 0; i < randomDiceCount; i++)
                {
                    // Dopóki ma czym rzucać
                    if (uniqueIndices.Count > 0)
                    {
                        // Losowanie indeksów kości by nie wybierało zawsze od pierwszej z lewej
                        int randomIndex = Random.Range(0, uniqueIndices.Count);
                        int pickedIndex = uniqueIndices[randomIndex];
                        AiDices[pickedIndex].AiPicked = true;
                        uniqueIndices.RemoveAt(randomIndex); // Zapobiega ponownemu rzutowi
                    }
                }
            }
        }
    }
}