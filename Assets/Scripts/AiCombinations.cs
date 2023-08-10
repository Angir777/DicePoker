using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AiCombinations : MonoBehaviour
{
    public bool AiThrown;
    public string AiCombinationName;
    public string AiCombinationName2;
    public int AiCombinationValue;
    public int AiCombinationValue2;
    
    private List<AiDice> AiDices = new List<AiDice>();
    private TextMeshProUGUI AiCombinationText;
    private int AiScoreCombination = 0;
    private int AiScoreDots;
    private int AiOneDotDices;
    private int AiTwoDotDices;
    private int AiThreeDotDices;
    private int AiFourDotDices;
    private int AiFiveDotDices;
    private int AiSixDotDices;

    void Start ()
    {
        for (int i = 0; i < 5; i++)
        {
            AiDices.Add(GameObject.Find("AiDice" + (i + 1).ToString()).GetComponent<AiDice>());
        }

        AiCombinationText = GameObject.Find("AiCombination").GetComponent<TextMeshProUGUI>();
    }
	
	void Update ()
    {
		if (AiThrown)
        {
            // Jeśli rzut to określ kombinację
            AiCheck();
        }
        else
        {
            // W przeciwnym razie wszystkie oczka są resetowane
            AiOneDotDices = 0;
            AiTwoDotDices = 0;
            AiThreeDotDices = 0;
            AiFourDotDices = 0;
            AiFiveDotDices = 0;
            AiSixDotDices = 0;
        }

        AiCombinationText.text = AiCombinationName;
    }

    /*
    * Sprawdzenie jaka kombinacja została wyrzucona
    */
    void AiCheck()
    {
        // Resetownie sumy liczby oczek
        AiScoreDots = 0;
        for (int i = 0; i < 5; i++)
        {
            // Sumowanie liczby oczek
            AiScoreDots += AiDices[i].AiDotCount;

            // Przypisywanie ilości kości o danej liczbie oczek
            switch (AiDices[i].AiDotCount)
            {
                case 1:
                    AiOneDotDices++;
                    break;
                case 2:
                    AiTwoDotDices++;
                    break;
                case 3:
                    AiThreeDotDices++;
                    break;
                case 4:
                    AiFourDotDices++;
                    break;
                case 5:
                    AiFiveDotDices++;
                    break;
                case 6:
                    AiSixDotDices++;
                    break;
            }
        }

        // Pair, Triple, Four of a kind, Poker
        AiFirstCombination(AiOneDotDices, 1);
        AiFirstCombination(AiTwoDotDices, 2);
        AiFirstCombination(AiThreeDotDices, 3);
        AiFirstCombination(AiFourDotDices, 4);
        AiFirstCombination(AiFiveDotDices, 5);
        AiFirstCombination(AiSixDotDices, 6); 

        // Szukanie drugiej Pary
        if (AiCombinationName != "")
        {
            if (AiCombinationValue != 1)
                AiSecondCombination(AiOneDotDices, 1);

            if (AiCombinationValue != 2)
                AiSecondCombination(AiTwoDotDices, 2);

            if (AiCombinationValue != 3)
                AiSecondCombination(AiThreeDotDices, 3);

            if (AiCombinationValue != 4)
                AiSecondCombination(AiFourDotDices, 4);

            if (AiCombinationValue != 5)
                AiSecondCombination(AiFiveDotDices, 5);

            if (AiCombinationValue != 6)
                AiSecondCombination(AiSixDotDices, 6);
        }

        // Double, Full
        if (AiCombinationName == "Triple" && AiCombinationName2 == "Pair" || AiCombinationName == "Pair" && AiCombinationName2 == "Triple")
        {
            AiCombinationName = "Full";
            AiScoreCombination = 6;
            AiCombinationName2 = "";
        }
        else if (AiCombinationName == "Pair" && AiCombinationName2 == "Pair")
        {
            AiCombinationName = "Double";
            AiScoreCombination = 2;
            AiCombinationName2 = "";
        }

        // Baby straight, Big straight
        if (AiTwoDotDices == 1 && AiThreeDotDices == 1 && AiFourDotDices == 1 && AiFiveDotDices == 1 && AiSixDotDices == 1)
        {
            AiCombinationName = "Big straight";
            AiScoreCombination = 5;
            AiCombinationName2 = "";
        }
        else if (AiOneDotDices == 1 && AiTwoDotDices == 1 && AiThreeDotDices == 1 && AiFourDotDices == 1 && AiFiveDotDices == 1)
        {
            AiCombinationName = "Baby straight";
            AiScoreCombination = 4;
            AiCombinationName2 = "";
        }

        // None
        if(AiCombinationName == "")
        {
            AiCombinationName = "None";
            AiScoreCombination = 0;
            AiCombinationName2 = "";
        }

        AiThrown = false;
    }

    /*
    * Kombinacja na pierwszym rzucie
    */
    void AiFirstCombination(int AiDicesCount, int AiDots)
    {
        switch (AiDicesCount)
        {
            case 2:
                AiCombinationName = "Pair";
                AiScoreCombination = 1;
                AiCombinationValue = AiDots;
                break;
            case 3:
                AiCombinationName = "Triple";
                AiScoreCombination = 3;
                AiCombinationValue = AiDots;
                break;
            case 4:
                AiCombinationName = "Four of a kind";
                AiScoreCombination = 7;
                AiCombinationValue = AiDots;
                break;
            case 5:
                AiCombinationName = "Poker";
                AiScoreCombination = 8;
                AiCombinationValue = AiDots;
                break;
        }
    }

    /*
    * Kombinacja na drugim rzucie
    */
    void AiSecondCombination(int AiDicesCount, int AiDots)
    {
        switch (AiDicesCount)
        {
            case 2:
                AiCombinationName2 = "Pair";
                AiCombinationValue2 = AiDots;
                break;
            case 3:
                AiCombinationName2 = "Triple";
                AiCombinationValue2 = AiDots;
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
        return AiScoreCombination;
    }

    /*
    * Zwraca sumę oczek kosci - potrzebne gdy remis na kombinacji
    */
    public int GetDotsValue()
    {
        // Suma oczek
        return AiScoreDots;
    }
}