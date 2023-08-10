using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Combinations : MonoBehaviour
{
    public bool thrown;
    public string combinationName;
    public string combinationName2;
    public int combinationValue;
    public int combinationValue2;
    
    private List<Dice> dices = new List<Dice>();
    private TextMeshProUGUI combinationText;
    private int scoreCombination = 0;
    private int scoreDots;
    private int oneDotDices;
    private int twoDotDices;
    private int threeDotDices;
    private int fourDotDices;
    private int fiveDotDices;
    private int sixDotDices;

    void Start ()
    {
        for (int i = 0; i < 5; i++)
        {
            dices.Add(GameObject.Find("Dice" + (i + 1).ToString()).GetComponent<Dice>());
        }

        combinationText = GameObject.Find("Combination").GetComponent<TextMeshProUGUI>();
    }
	
	void Update ()
    {
		if (thrown)
        {
            // Jeśli rzut to określ kombinację
            Check();
        }
        else
        {
            // W przeciwnym razie wszystkie oczka są resetowane
            oneDotDices = 0;
            twoDotDices = 0;
            threeDotDices = 0;
            fourDotDices = 0;
            fiveDotDices = 0;
            sixDotDices = 0;
        }

        combinationText.text = combinationName;
    }

    /*
    * Sprawdzenie jaka kombinacja została wyrzucona
    */
    void Check()
    {
        // Resetownie sumy liczby oczek
        scoreDots = 0;
        for (int i = 0; i < 5; i++)
        {
            // Sumowanie liczby oczek
            scoreDots += dices[i].dotCount;

            // Przypisywanie ilości kości o danej liczbie oczek
            switch (dices[i].dotCount)
            {
                case 1:
                    oneDotDices++;
                    break;
                case 2:
                    twoDotDices++;
                    break;
                case 3:
                    threeDotDices++;
                    break;
                case 4:
                    fourDotDices++;
                    break;
                case 5:
                    fiveDotDices++;
                    break;
                case 6:
                    sixDotDices++;
                    break;
            }
        }

        // Pair, Triple, Four of a kind, Poker
        FirstCombination(oneDotDices, 1);
        FirstCombination(twoDotDices, 2);
        FirstCombination(threeDotDices, 3);
        FirstCombination(fourDotDices, 4);
        FirstCombination(fiveDotDices, 5);
        FirstCombination(sixDotDices, 6); 

        // Szukanie drugiej Pary
        if (combinationName != "")
        {
            if (combinationValue != 1)
                SecondCombination(oneDotDices, 1);

            if (combinationValue != 2)
                SecondCombination(twoDotDices, 2);

            if (combinationValue != 3)
                SecondCombination(threeDotDices, 3);

            if (combinationValue != 4)
                SecondCombination(fourDotDices, 4);

            if (combinationValue != 5)
                SecondCombination(fiveDotDices, 5);

            if (combinationValue != 6)
                SecondCombination(sixDotDices, 6);
        }

        // Double, Full
        if (combinationName == "Triple" && combinationName2 == "Pair" || combinationName == "Pair" && combinationName2 == "Triple")
        {
            combinationName = "Full";
            scoreCombination = 6;
            combinationName2 = "";
        }
        else if (combinationName == "Pair" && combinationName2 == "Pair")
        {
            combinationName = "Double";
            scoreCombination = 2;
            combinationName2 = "";
        }

        // Baby straight, Big straight
        if (twoDotDices == 1 && threeDotDices == 1 && fourDotDices == 1 && fiveDotDices == 1 && sixDotDices == 1)
        {
            combinationName = "Big straight";
            scoreCombination = 5;
            combinationName2 = "";
        }
        else if (oneDotDices == 1 && twoDotDices == 1 && threeDotDices == 1 && fourDotDices == 1 && fiveDotDices == 1)
        {
            combinationName = "Baby straight";
            scoreCombination = 4;
            combinationName2 = "";
        }

        // None
        if(combinationName == "")
        {
            combinationName = "None";
            scoreCombination = 0;
            combinationName2 = "";
        }

        thrown = false;
    }

    /*
    * Kombinacja na pierwszym rzucie
    */
    void FirstCombination(int dicesCount, int dots)
    {
        switch (dicesCount)
        {
            case 2:
                combinationName = "Pair";
                scoreCombination = 1;
                combinationValue = dots;
                break;
            case 3:
                combinationName = "Triple";
                scoreCombination = 3;
                combinationValue = dots;
                break;
            case 4:
                combinationName = "Four of a kind";
                scoreCombination = 7;
                combinationValue = dots;
                break;
            case 5:
                combinationName = "Poker";
                scoreCombination = 8;
                combinationValue = dots;
                break;
        }
    }

    /*
    * Kombinacja na drugim rzucie
    */
    void SecondCombination(int dicesCount, int dots)
    {
        switch (dicesCount)
        {
            case 2:
                combinationName2 = "Pair";
                combinationValue2 = dots;
                break;
            case 3:
                combinationName2 = "Triple";
                combinationValue2 = dots;
                break;
        }
    }

    /*
    * Zwraca wynik dla danej kombinacji kości
    */
    public int GetCombinationValue()
    {
        // Wagi
        // (8) Poker - Wszystkie pięć kości ma tę samą wartość (np. pięć sześci).
        // (7) Kareta - Cztery kości mają tę samą wartość (np. cztery ósemki).
        // (6) Full - Trzy kości mają tę samą wartość, a pozostałe dwie kości mają inną tę samą wartość (np. trzy piątki i dwie dziewiątki).
        // (5) Duzy strit - Pięć kolejnych kości (np. 2, 3, 4, 5, 6).
        // (4) Mały strit - Cztery kolejne kości (np. 1, 2, 3, 4 lub 2, 3, 4, 5).
        // (3) Trójka - Trzy kości mają tę samą wartość (np. trzy dziesiątki).
        // (2) Dwie pary - Dwie pary kości o tej samej wartości (np. dwie trójki i dwie piątki).
        // (1) Para - Dwie kości o tej samej wartości (np. dwie siódemki).
        // (0) Nic
        return scoreCombination;
    }

    /*
    * Zwraca sumę oczek kosci - potrzebne gdy remis na kombinacji
    */
    public int GetDotsValue()
    {
        // Suma oczek
        return scoreDots;
    }
}
