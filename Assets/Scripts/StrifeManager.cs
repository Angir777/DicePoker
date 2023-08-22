using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class StrifeManager : MonoBehaviour
{
    public bool isFirstRound;
    public string whoWinRound;

    private int countRound;
    private int playerWins;
    private int computerWins;

    private PlayerSettings playerSettings;
    private Instruction instruction;
    private ThrowDices throwDices;
    private GoldManager goldManager;
    private ModalPanel modalPanel;
    private AiThrowDices AiThrowDices;

    private TextMeshProUGUI roundNumberText;

    private bool aiPassRound = false;

    public Sprite pointForPlayer;
    public Sprite pointForEnemy;
    public Sprite draw;
    public Sprite defeat;
    public GameObject octObject;
    private Image octImage;

    private void Start()
    {
        octImage = octObject.GetComponent<Image>();


        // Ustawienie referencji do powiązanych skryptów
        instruction = FindObjectOfType<Instruction>().GetComponent<Instruction>();
        throwDices = FindObjectOfType<ThrowDices>().GetComponent<ThrowDices>();
        goldManager = FindObjectOfType<GoldManager>().GetComponent<GoldManager>();
        AiThrowDices = FindObjectOfType<AiThrowDices>().GetComponent<AiThrowDices>();

        // Ustawienie referencji do okna modalnego
        modalPanel = FindObjectOfType<ModalPanel>().GetComponent<ModalPanel>();
        modalPanel.ShowModal(false);

        roundNumberText = GameObject.Find("RoundNumber").GetComponent<TextMeshProUGUI>();

        StartNewGame();
    }

    /*
    * Rozpoczęcie nowej partii gry w kości
    */
    public void StartNewGame()
    {
        countRound = 0;
        playerWins = 0;
        computerWins = 0;

        StartNewRound();
    }

    /*
    * Rozpoczęcie nowej rundy
    */
    public void StartNewRound()
    {
        countRound++;
        roundNumberText.text = countRound.ToString();

        // Jeśli to runda 1 w kwotach zakładu nie będzie zera
        if (countRound == 1)
        {
            isFirstRound = true;
        }
        else
        {
            isFirstRound = false;
        }

        // Wywołanie metody określającej kwotę zakładu i zerującej kości
        throwDices.NewBet();
        AiThrowDices.NewBet();
    }

    /*
    * Ocena wyniku rundy
    */
    public void EvaluateRound()
    {
        // Pobierz wynik gracza
        int playerCombinationValue = FindObjectOfType<Combinations>().GetCombinationValue();
        int playerDotsValue = FindObjectOfType<Combinations>().GetDotsValue();
        
        // Pobierz wynik Ai
        int computerCombinationValue = FindObjectOfType<AiCombinations>().GetCombinationValue();
        int computerDotsValue = FindObjectOfType<AiCombinations>().GetDotsValue();

        // Porównaj wyniki i zwiększ liczbę wygranych
        if (aiPassRound)
        {
            playerWins++;
            whoWinRound = "player";
        }
        else
        {
            if (playerCombinationValue > computerCombinationValue)
            {
                playerWins++;
                whoWinRound = "player";
            }
            else if (computerCombinationValue > playerCombinationValue)
            {
                computerWins++;
                whoWinRound = "computer";
            }
            else if (playerCombinationValue == computerCombinationValue)
            {
                if (playerDotsValue > computerDotsValue)
                {
                    playerWins++;
                    whoWinRound = "player";
                }
                else if (computerDotsValue > playerDotsValue)
                {
                    computerWins++;
                    whoWinRound = "computer";
                }
                else
                {
                    whoWinRound = "none";
                }
            }
        }

        EndRound(whoWinRound);
    }

    /*
    * Gracz wcisnoł przycisk "Pass" - pasuje daną rundę
    */
    public void PassRound()
    {
        computerWins++;

        whoWinRound = "computer";

        EndRound(whoWinRound);
    }

    /*
    * Ai pasuje daną rundę
    */
    public void AiPassRound()
    {
        // Tutaj ustawiamy tylko flage bo równoczesnie akcja dzieje się w skrypcie AiThrowDices
        this.aiPassRound = true;
    }

    /*
    * Zakończenie pojedyńczej rundy
    */
    public void EndRound(string whoWinRound)
    {
        // Zresetowanie pasu AI
        this.aiPassRound = false;

        // Ukrycie niepotrzebnych przycisków
        GameObject akceptObject = GameObject.Find("Akcept");
        if (akceptObject != null)
        {
            akceptObject.SetActive(false);
        }
        GameObject passObject = GameObject.Find("Pass");
        if (passObject != null)
        {
            passObject.SetActive(false);
        }
        GameObject throwObject = GameObject.Find("Throw");
        if (throwObject != null)
        {
            throwObject.SetActive(false);
        }
        GameObject backObject = GameObject.Find("Back");
        if (backObject != null)
        {
            backObject.SetActive(false);
        }

        // Sprawdź, czy któryś z grających wygrał już dwie rundy
        if (playerWins >= 2 || computerWins >= 2)
        {
            // Wstrzymanie wykonywania kodu na 2 sekundy i zakończenie gry
            StartCoroutine(ExecuteAfterDelayEndGame(2.0f));
        }
        else
        {
            // Komunikat dla gracza o wygranej lub przegranej rundzie
            if (whoWinRound == "player")
            {
                octImage.sprite = pointForPlayer;

                // Wstrzymanie wykonywania kodu na 2 sekundy i rozpoczęcie nowej rundy
                StartCoroutine(ExecuteAfterDelayShowModal(2.0f, "You won this round!", "StartNewRound"));
            }
            else if (whoWinRound == "computer")
            {
                octImage.sprite = pointForEnemy;

                // Wstrzymanie wykonywania kodu na 2 sekundy i rozpoczęcie nowej rundy
                StartCoroutine(ExecuteAfterDelayShowModal(2.0f, "You lost this round!", "StartNewRound"));
            }
            else if (whoWinRound == "none")
            {
                // Wstrzymanie wykonywania kodu na 2 sekundy i rozpoczęcie nowej rundy
                StartCoroutine(ExecuteAfterDelayShowModal(2.0f, "Draw!", "StartNewRound"));
            }
        }
    }

    /*
    * Zakończenie gry
    */
    public void EndGame()
    {
        // Pobierz kwotę zakładu
        float totalBetAmount = goldManager.totalBetAmount;

        // Wczytaj ustawienia gry
        playerSettings = FindObjectOfType<PlayerSettings>().GetComponent<PlayerSettings>();
        playerSettings.LoadPlayerSettings();
        float moneyAmount = playerSettings.playerData.money; // Aktualny stan "money"

        // Pobierz dane od danego przeciwnika
        string currentEnemyName = goldManager.currentEnemyName;
        PlayerSettings.EnemyBetConfigurations enemyConfigurations = playerSettings.playerData.betConfigurations.enemyConfigurations.Find(config => config.enemyName == currentEnemyName);
        bool enemyDefeated = enemyConfigurations.defeated;

        // Wyświetl odpowiedni komunikat z wynikiem gry
        if (playerWins > computerWins)
        {
            // Jeśli gracz raz wygrał to już potem nie zmienia to poziomu pieniędzy
            if (!enemyDefeated)
            {
                // Zaktualizuj dane od przeciwnika
                enemyConfigurations.defeated = true;

                // Zwiększ level gracza
                playerSettings.playerData.diceGame.level += 1;

                // Gra wygrana - gracz zarabia pieniądze
                moneyAmount += totalBetAmount;
                modalPanel.ShowModalPanel("You won the game! You earned " + totalBetAmount + " cash!", "EndGame");
            }
            else
            {
                modalPanel.ShowModalPanel("You won the game!", "EndGame");
            }

            octImage.sprite = draw;
        }
        else if (computerWins > playerWins)
        {
            // Jeśli gracz raz wygrał to już potem nie zmienia to poziomu pieniędzy
            if (!enemyDefeated)
            {
                // Gra przegrana - gracz traci pieniądze
                moneyAmount -= totalBetAmount;
                modalPanel.ShowModalPanel("You lost the game! You lost " + totalBetAmount + " cash!", "EndGame");
            }
            else
            {
                modalPanel.ShowModalPanel("You lost the game!", "EndGame");
            }

            octImage.sprite = defeat;
        }

        // Ustaw zaktualizowaną wartość dla "money"
        if (moneyAmount > 999999)
        {
            playerSettings.playerData.money = 999999;
        }
        else
        {
            playerSettings.playerData.money = moneyAmount;
        }

        // Zapisz zmienione ustawienia do pliku JSON
        playerSettings.SavePlayerSettings();
    }

    /*
    * Wstrzymanie gry przed zakończeniem
    */
    IEnumerator ExecuteAfterDelayEndGame(float delay)
    {
        // Gra daje czas na zapoznanie się z wynikiem ostatniego rzutu
        yield return new WaitForSeconds(delay);

        EndGame();
    }

    /*
    * Wstrzymanie gry przed wyświetleniem modala
    */
    IEnumerator ExecuteAfterDelayShowModal(float delay, string text, string functionName)
    {
        // Gra daje czas na zapoznanie się z wynikiem ostatniego rzutu
        yield return new WaitForSeconds(delay);

        modalPanel.ShowModalPanel(text, functionName);
    }
}
