using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldManager : MonoBehaviour
{
    public GameObject windowPrefab;
    public float goldAmount;
    public float betAmount;
    public float totalBetAmount = 0;
    public bool isBet;

    private PlayerSettings playerSettings;
    private Instruction instruction;
    private ThrowDices throwDices;
    private StrifeManager strifeManager;
    private TextMeshProUGUI goldText;
    private TextMeshProUGUI betText;
    private TextMeshProUGUI enemyText;

    public string currentEnemyName;

    void Start()
    {
        // Ustawienie referencji do powiązanych skryptów
        instruction = FindObjectOfType<Instruction>().GetComponent<Instruction>();
        throwDices = FindObjectOfType<ThrowDices>().GetComponent<ThrowDices>();
        strifeManager = FindObjectOfType<StrifeManager>().GetComponent<StrifeManager>();

        // Ustawienie referencji do obiektów wyświetlajacych ilość pieniędzy gracza i kwotę zakładu
        goldText = GameObject.Find("Gold").GetComponent<TextMeshProUGUI>();
        betText = GameObject.Find("Bet").GetComponent<TextMeshProUGUI>();
        enemyText = GameObject.Find("Enemy").GetComponent<TextMeshProUGUI>();

        // Wczytaj ustawienia
        playerSettings = FindObjectOfType<PlayerSettings>().GetComponent<PlayerSettings>();
        playerSettings.LoadPlayerSettings();
        // I ustaw ile pieniędzy ma gracz
        goldAmount = playerSettings.playerData.money;

        // Nazwa przeciwnika z którym gra gracz
        currentEnemyName = PlayerPrefs.GetString("enemyName");
        enemyText.text = currentEnemyName;

        // Kwota zakładu na start
        betText.text = totalBetAmount.ToString();
    }

    void Update()
    {
        if (goldAmount == 0 && betText.text == "0")
        {
            instruction.UpdateInstructionText("You're broke!");

            isBet = false;

            throwDices.GetComponent<Button>().interactable = false;
            GameObject.Find("Back").GetComponent<Button>().interactable = true;
        }

        goldText.text = goldAmount.ToString();

        // Pokaż moduł ustalania zakładu
        if (isBet)
        {
            Bet();

            throwDices.GetComponent<Button>().interactable = false;

            isBet = false;
        }
    }

    /*
    * Pokaż modół ustalania zakładu
    */
    void Bet()
    {
        Instantiate(windowPrefab, FindObjectOfType<Canvas>().transform);

        GameObject.Find("MoreButton").GetComponent<Button>().onClick.AddListener(More);
        GameObject.Find("LessButton").GetComponent<Button>().onClick.AddListener(Less);
        GameObject.Find("BetButton").GetComponent<Button>().onClick.AddListener(Accept);

        if (strifeManager.isFirstRound == true)
        {
            Text amountText = GameObject.Find("Amount").GetComponent<Text>();
            amountText.enabled = false;
        }
    }

    /*
    * Akceptacja kwoty zakładu
    */
    public void Accept()
    {
        // Referencja do obiektu wyświetlającego tekst
        Text amountText = GameObject.Find("Amount").GetComponent<Text>();
        betAmount = float.Parse(amountText.text);

        if (betAmount == 0 && strifeManager.isFirstRound == true)
        {
            instruction.UpdateInstructionText("Choose your bet amount!");
            return; // Zwróć z metody, aby uniknąć kontynuowania po błędzie
        }

        if (goldAmount >= betAmount)
        {
            throwDices.GetComponent<Button>().interactable = true;

            goldAmount -= betAmount;

            totalBetAmount += betAmount;
            betText.text = totalBetAmount.ToString();

            Destroy(GameObject.Find("Bet(Clone)").gameObject);

            instruction.UpdateInstructionText("Roll the dice!");
        }
        else
        {
            instruction.UpdateInstructionText("You don't have that kind of cash!");
        }
    }

    /*
    * Zwiększenie kwoty zakładu
    */
    public void More()
    {
        // Referencja do obiektu wyświetlającego tekst
        Text amountText = GameObject.Find("Amount").GetComponent<Text>();
        amountText.enabled = true;
        int currentAmount = int.Parse(amountText.text);

        // Pobierz konfiguracje zakładów dla danego przeciwnika
        PlayerSettings.EnemyBetConfigurations enemyConfigurations = playerSettings.playerData.betConfigurations.enemyConfigurations.Find(config => config.enemyName == currentEnemyName);

        if (strifeManager.isFirstRound)
        {
            int maxIndex = enemyConfigurations.firstRound.Count - 1;
            int currentIndex = enemyConfigurations.firstRound.IndexOf(currentAmount);

            // Zwiększ wartość zakładu o 10, jeśli nie jest ostatnim elementem
            if (currentIndex < maxIndex)
            {
                amountText.text = enemyConfigurations.firstRound[currentIndex + 1].ToString();
            }
        }
        else
        {
            int maxIndex = enemyConfigurations.subsequentRounds.Count - 1;
            int currentIndex = enemyConfigurations.subsequentRounds.IndexOf(currentAmount);

            // Zwiększ wartość zakładu o 50, jeśli nie jest ostatnim elementem
            if (currentIndex < maxIndex)
            {
                amountText.text = enemyConfigurations.subsequentRounds[currentIndex + 1].ToString();
            }
        }
    }

    /*
    * Zmniejszenie kwoty zakładu
    */
    public void Less()
    {
        // Referencja do obiektu wyświetlającego tekst
        Text amountText = GameObject.Find("Amount").GetComponent<Text>();
        amountText.enabled = true;
        int currentAmount = int.Parse(amountText.text);

        // Pobierz konfiguracje zakładów dla danego przeciwnika
        PlayerSettings.EnemyBetConfigurations enemyConfigurations = playerSettings.playerData.betConfigurations.enemyConfigurations.Find(config => config.enemyName == currentEnemyName);

        if (strifeManager.isFirstRound)
        {
            int currentIndex = enemyConfigurations.firstRound.IndexOf(currentAmount);

            // Zmniejsz wartość zakładu o 10, jeśli nie jest pierwszym elementem
            if (currentIndex > 0)
            {
                amountText.text = enemyConfigurations.firstRound[currentIndex - 1].ToString();
            }
            else
            {
                // Jeśli wartość wynosi zero, ustaw najniższą dostępną wartość
                amountText.text = enemyConfigurations.firstRound[0].ToString();
            }
        }
        else
        {
            int currentIndex = enemyConfigurations.subsequentRounds.IndexOf(currentAmount);

            // Zmniejsz wartość zakładu o 50, jeśli nie jest pierwszym elementem
            if (currentIndex > 0)
            {
                amountText.text = enemyConfigurations.subsequentRounds[currentIndex - 1].ToString();
            }
        }
    }
}
