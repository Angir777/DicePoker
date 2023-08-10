using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelection : MonoBehaviour
{
    public Button[] lvlButtons;

    public Sprite lockedSprite;
    public Sprite unlockedSprite;
    public Sprite winSprite;

    private PlayerSettings playerSettings;

    public TextMeshProUGUI playerLevelText;
    public TextMeshProUGUI playerGoldText;

    void Start()
    {
        // Wczytaj ustawienia
        playerSettings = FindObjectOfType<PlayerSettings>().GetComponent<PlayerSettings>();
        playerSettings.LoadPlayerSettings();

        // Poziom na jakim jest gracz
        int playerLevel = playerSettings.playerData.diceGame.level;

        // Zaznaczamy jakie levely sa aktywne, a jakie sa jeszcze zablokowane
        for (int i = 0; i < lvlButtons.Length; i++)
        {
            if (i > playerLevel)
            {
                // Level zablokowany
                lvlButtons[i].interactable = false;
                Image buttonImage = lvlButtons[i].GetComponent<Image>();
                buttonImage.sprite = lockedSprite;
            }
            else
            {
                // Level odblokowany
                Image buttonImage = lvlButtons[i].GetComponent<Image>();
                buttonImage.sprite = unlockedSprite;

                // Jeśli wygrał to level wygrany
                PlayerSettings.EnemyBetConfigurations enemyConfigurations = playerSettings.playerData.betConfigurations.enemyConfigurations.Find(config => config.id == i);
                if (enemyConfigurations != null && enemyConfigurations.defeated)
                {
                    buttonImage.sprite = winSprite;
                }
            }
        }
    }

    private void Update() {
        playerLevelText.text = playerSettings.playerData.diceGame.status;
        playerGoldText.text = playerSettings.playerData.money.ToString();
    }
}
