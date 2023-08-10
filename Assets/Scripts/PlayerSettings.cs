using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class PlayerSettings : MonoBehaviour
{
    /*
    * To główny obiek reprezenyujący plik JSON z ustawieniami gry
    */
    [System.Serializable]
    public class PlayerData
    {
        public float money;
        public DiceGameData diceGame;
        public BetConfigurations betConfigurations;
    }

    [System.Serializable]
    public class DiceGameData
    {
        public string lastDefeatedNpcName;
        public string status;
        public int level;
    }

    [System.Serializable]
    public class BetConfigurations
    {
        // Tutaj jako w "betConfigurations" dodajemy "enemyConfigurations", a w nim zapisujemy listę
        public List<EnemyBetConfigurations> enemyConfigurations;
    }
    [System.Serializable]
    public class EnemyBetConfigurations
    {
        public int id;
        public bool defeated;
        public string status;
        public string enemyName;
        public List<int> firstRound;
        public List<int> subsequentRounds;
        public string info;
    }

    public PlayerData playerData;

    private string filePath; // Ściezka do pliku na WIN: C:/Users/ziom_/AppData/LocalLow/DefaultCompany/KoscianyPoker\game_settings.json

    /*
    * Metoda uruchamiana na start - załadowanie ustawień gry
    */
    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "game_settings.json");
        LoadPlayerSettings();
    }

    /*
    * Zapis ustawień
    */
    public void SavePlayerSettings()
    {
        string json = JsonUtility.ToJson(playerData);
        File.WriteAllText(filePath, json);
    }

    /*
    * Załadowanie ustawień
    */
    public void LoadPlayerSettings()
    {
        if (File.Exists(filePath))
        {
            // Jeśli plik istnieje to zostaje wczytany
            string json = File.ReadAllText(filePath);
            playerData = JsonUtility.FromJson<PlayerData>(json);
        }
        else
        {
            // Jeśli nie ma pliku z ustawieniami to kierujemy na główną scenę
            Scene currentScene = SceneManager.GetActiveScene();
            if (currentScene.name != "Menu" && currentScene.name != "Settings")
            {
                // Przekierowanie
                SceneManager.LoadScene("Menu");
            }
        }
    }

    /*
    * Generowanie podstawowych ustawień na początku gry
    */
    public void CreateDefaultPlayerSettings()
    {
        // Usówanie starego PlayerPrefs jesli istnieje
        if (PlayerPrefs.HasKey("enemyName"))
        {
            PlayerPrefs.DeleteKey("enemyName");
            // PlayerPrefs.DeleteAll();
        }

        // Obiekt do zapisu ustawień gry
        playerData = new PlayerData();

        // Podstawoe informacje o graczu
        playerData.money = 50;

        // Ustawienia gry w kości
        playerData.diceGame = new DiceGameData();
        playerData.diceGame.lastDefeatedNpcName = ""; // Z kim gracz rozegrał ostatnio pojedynek
        playerData.diceGame.status = "Novice"; // Nowicjusz, Zawodowiec, Szuler, Mistrz
        playerData.diceGame.level = 0; // Liczba doświadczenia

        // Przeciwnicy w grze w kości
        playerData.betConfigurations = new BetConfigurations();
        playerData.betConfigurations.enemyConfigurations = new List<EnemyBetConfigurations>();

        // Novice ---------------------------------------------------------------------------

        // Enemy 1
        EnemyBetConfigurations e1 = new EnemyBetConfigurations();
        e1.id = 0; // ID przeciwnika
        e1.defeated = false; // Czy gracz pokonał już tego przeciwnika?
        e1.enemyName = "Tom"; // Nazwa przeciwnika
        e1.status = "Novice"; // Poziom przeciwnika - tu nowicjusz
        e1.firstRound = new List<int> { 2, 5, 10 }; // Kwoty pierwszego zakładu na rundę
        e1.subsequentRounds = new List<int> { 0, 5, 10 }; // Kwoty kolejnych zakładów
        playerData.betConfigurations.enemyConfigurations.Add(e1);
        e1.info = "Tom is a young blacksmith with a strong arm and a dream of becoming a dice master. He's the perfect warm-up opponent.";

        // Enemy 2
        EnemyBetConfigurations e2 = new EnemyBetConfigurations();
        e2.id = 1; // ID przeciwnika
        e2.defeated = false; // Czy gracz pokonał już tego przeciwnika?
        e2.enemyName = "Arabella"; // Nazwa przeciwnika
        e2.status = "Novice"; // Poziom przeciwnika - tu nowicjusz
        e2.firstRound = new List<int> { 3, 5, 10 }; // Kwoty pierwszego zakładu na rundę
        e2.subsequentRounds = new List<int> { 0, 5, 15 }; // Kwoty kolejnych zakładów
        playerData.betConfigurations.enemyConfigurations.Add(e2);
        e2.info = "Arabella is an ambitious and cunning lady who can manipulate dice as well as her opponents. Be careful not to let her manipulate you.";

        // Enemy 3
        EnemyBetConfigurations e3 = new EnemyBetConfigurations();
        e3.id = 2; // ID przeciwnika
        e3.defeated = false; // Czy gracz pokonał już tego przeciwnika?
        e3.enemyName = "Billy"; // Nazwa przeciwnika
        e3.status = "Novice"; // Poziom przeciwnika - tu nowicjusz
        e3.firstRound = new List<int> { 5, 10, 15 }; // Kwoty pierwszego zakładu na rundę
        e3.subsequentRounds = new List<int> { 0, 10, 15 }; // Kwoty kolejnych zakładów
        playerData.betConfigurations.enemyConfigurations.Add(e3);
        e3.info = "Billy is a country boy with a fiery temper and good manure-handling skills. You deal with him twice. But for sure?";

        // Enemy 4
        EnemyBetConfigurations e4 = new EnemyBetConfigurations();
        e4.id = 3; // ID przeciwnika
        e4.defeated = false; // Czy gracz pokonał już tego przeciwnika?
        e4.enemyName = "Graszko"; // Nazwa przeciwnika
        e4.status = "Novice"; // Poziom przeciwnika
        e4.firstRound = new List<int> { 15, 20, 25 }; // Kwoty pierwszego zakładu na rundę
        e4.subsequentRounds = new List<int> { 0, 15, 20 }; // Kwoty kolejnych zakładów
        playerData.betConfigurations.enemyConfigurations.Add(e4);
        e4.info = "Graszko is a cunning innkeeper who perfectly understands the psychology of players and can manipulate them before they even roll the dice. Her presence at the gaming table instills unease among opponents, and her cool self-assurance causes many people to doubt their own abilities. Settle the bill before the game.";

        // Enemy 5
        EnemyBetConfigurations e5 = new EnemyBetConfigurations();
        e5.id = 4; // ID przeciwnika
        e5.defeated = false; // Czy gracz pokonał już tego przeciwnika?
        e5.enemyName = "Brother Antoni"; // Nazwa przeciwnika
        e5.status = "Novice"; // Poziom przeciwnika
        e5.firstRound = new List<int> { 20, 25, 30 }; // Kwoty pierwszego zakładu na rundę
        e5.subsequentRounds = new List<int> { 0, 20, 30 }; // Kwoty kolejnych zakładów
        playerData.betConfigurations.enemyConfigurations.Add(e5);
        e5.info = "Brother Antoni is a humble clergyman with a passion for gambling. He claims to have found a way to reach people through dice games and share his wisdom. Will he reach you?";

        // Enemy 6
        EnemyBetConfigurations e6 = new EnemyBetConfigurations();
        e6.id = 5; // ID przeciwnika
        e6.defeated = false; // Czy gracz pokonał już tego przeciwnika?
        e6.enemyName = "Aria"; // Nazwa przeciwnika
        e6.status = "Novice"; // Poziom przeciwnika
        e6.firstRound = new List<int> { 25, 30, 35 }; // Kwoty pierwszego zakładu na rundę
        e6.subsequentRounds = new List<int> { 0, 25, 30 }; // Kwoty kolejnych zakładów
        playerData.betConfigurations.enemyConfigurations.Add(e6);
        e6.info = "Aria is a talented thief who uses her nimble fingers to manipulate the dice and deceive opponents. Be vigilant.";

        // Professional ---------------------------------------------------------------------------

        // Enemy 7
        EnemyBetConfigurations e7 = new EnemyBetConfigurations();
        e7.id = 6; // ID przeciwnika
        e7.defeated = false; // Czy gracz pokonał już tego przeciwnika?
        e7.enemyName = "Neke"; // Nazwa przeciwnika
        e7.status = "Professional"; // Poziom przeciwnika
        e7.firstRound = new List<int> { 30, 35, 40 }; // Kwoty pierwszego zakładu na rundę
        e7.subsequentRounds = new List<int> { 0, 35, 40 }; // Kwoty kolejnych zakładów
        playerData.betConfigurations.enemyConfigurations.Add(e7);
        e7.info = "Neke is a talented chef with an extraordinary sense of taste. Her dishes are famous throughout the land and attract guests from far and wide. Not only does Neke excel at cooking, but she also knows how to use her culinary skills in dice games. She can manipulate dice rolls to gain an advantage over opponents. Grab a bite before sitting down to play with her.";

        // Enemy 8
        EnemyBetConfigurations e8 = new EnemyBetConfigurations();
        e8.id = 7; // ID przeciwnika
        e8.defeated = false; // Czy gracz pokonał już tego przeciwnika?
        e8.enemyName = "Milva"; // Nazwa przeciwnika
        e8.status = "Professional"; // Poziom przeciwnika
        e8.firstRound = new List<int> { 35, 40, 45 }; // Kwoty pierwszego zakładu na rundę
        e8.subsequentRounds = new List<int> { 0, 30, 35 }; // Kwoty kolejnych zakładów
        playerData.betConfigurations.enemyConfigurations.Add(e8);
        e8.info = "Milva is an extraordinary archer who always hits the mark - both with her bow and in dice games. Be careful not to be her target.";

        // Enemy 9
        EnemyBetConfigurations e9 = new EnemyBetConfigurations();
        e9.id = 8; // ID przeciwnika
        e9.defeated = false; // Czy gracz pokonał już tego przeciwnika?
        e9.enemyName = "Eldrin"; // Nazwa przeciwnika
        e9.status = "Professional"; // Poziom przeciwnika
        e9.firstRound = new List<int> { 45, 50, 60 }; // Kwoty pierwszego zakładu na rundę
        e9.subsequentRounds = new List<int> { 0, 30, 40 }; // Kwoty kolejnych zakładów
        playerData.betConfigurations.enemyConfigurations.Add(e9);
        e9.info = "Eldrin is a poet whose words touch hearts and minds. His verses are full of emotions, beauty, and deep reflection. He utilizes his sensitivity and perceptiveness to read people's eyes and intuitively understand their intentions. Can your dice game skills withstand the mystical powers of poetry?";

        // Enemy 10
        EnemyBetConfigurations e10 = new EnemyBetConfigurations();
        e10.id = 9; // ID przeciwnika
        e10.defeated = false; // Czy gracz pokonał już tego przeciwnika?
        e10.enemyName = "Iri"; // Nazwa przeciwnika
        e10.status = "Professional"; // Poziom przeciwnika
        e10.firstRound = new List<int> { 40, 45, 50 }; // Kwoty pierwszego zakładu na rundę
        e10.subsequentRounds = new List<int> { 0, 35, 55 }; // Kwoty kolejnych zakładów
        playerData.betConfigurations.enemyConfigurations.Add(e10);
        e10.info = "Iri is a skillful circus dancer, capable of manipulating not only her body but also the dice to achieve incredible results in the game. Are you ready to face her unpredictable moves?";

        // Enemy 11
        EnemyBetConfigurations e11 = new EnemyBetConfigurations();
        e11.id = 10; // ID przeciwnika
        e11.defeated = false; // Czy gracz pokonał już tego przeciwnika?
        e11.enemyName = "Mortimer"; // Nazwa przeciwnika
        e11.status = "Professional"; // Poziom przeciwnika
        e11.firstRound = new List<int> { 45, 50, 55 }; // Kwoty pierwszego zakładu na rundę
        e11.subsequentRounds = new List<int> { 0, 35, 55 }; // Kwoty kolejnych zakładów
        playerData.betConfigurations.enemyConfigurations.Add(e11);
        e11.info = "Mortimer is a mad alchemist whose masterful skills in concocting ingredients help him achieve incredible results in dice games. Prepare yourself for a magical showdown.";

        // Enemy 12
        EnemyBetConfigurations e12 = new EnemyBetConfigurations();
        e12.id = 11; // ID przeciwnika
        e12.defeated = false; // Czy gracz pokonał już tego przeciwnika?
        e12.enemyName = "Zoltan"; // Nazwa przeciwnika
        e12.status = "Professional"; // Poziom przeciwnika
        e12.firstRound = new List<int> { 60, 65, 70 }; // Kwoty pierwszego zakładu na rundę
        e12.subsequentRounds = new List<int> { 0, 40, 75 }; // Kwoty kolejnych zakładów
        playerData.betConfigurations.enemyConfigurations.Add(e12);
        e12.info = "Zoltan is an experienced warrior who has found his calling in dice games and pickles washed down with spirits. You'll need a strong head to compete with him.";

        // Sharp ---------------------------------------------------------------------------

        // Enemy 13
        EnemyBetConfigurations e13 = new EnemyBetConfigurations();
        e13.id = 12; // ID przeciwnika
        e13.defeated = false; // Czy gracz pokonał już tego przeciwnika?
        e13.enemyName = "Junior"; // Nazwa przeciwnika
        e13.status = "Sharp"; // Poziom przeciwnika
        e13.firstRound = new List<int> { 100, 125, 130 }; // Kwoty pierwszego zakładu na rundę
        e13.subsequentRounds = new List<int> { 0, 50, 75 }; // Kwoty kolejnych zakładów
        playerData.betConfigurations.enemyConfigurations.Add(e13);
        e13.info = "This street rogue is a master of bluffing and camouflage. He can maintain a poker face while the dice in his hands are under his control. The tricks and deceptive moves he employs make opponents lose their vigilance and easily fall for his schemes. Don't let his nickname deceive you.";

        // Enemy 14
        EnemyBetConfigurations e14 = new EnemyBetConfigurations();
        e14.id = 13; // ID przeciwnika
        e14.defeated = false; // Czy gracz pokonał już tego przeciwnika?
        e14.enemyName = "Sophia"; // Nazwa przeciwnika
        e14.status = "Sharp"; // Poziom przeciwnika
        e14.firstRound = new List<int> { 110, 130, 145 }; // Kwoty pierwszego zakładu na rundę
        e14.subsequentRounds = new List<int> { 0, 60, 80 }; // Kwoty kolejnych zakładów
        playerData.betConfigurations.enemyConfigurations.Add(e14);
        e14.info = "She is a mysterious gypsy with penetrating eyes, capable of predicting the outcomes of dice games. She is a master at reading fortunes and can sense which numbers will appear on the dice before they are even rolled. Can you defy fate?";

        // Enemy 15
        EnemyBetConfigurations e15 = new EnemyBetConfigurations();
        e15.id = 14; // ID przeciwnika
        e15.defeated = false; // Czy gracz pokonał już tego przeciwnika?
        e15.enemyName = "Felix"; // Nazwa przeciwnika
        e15.status = "Sharp"; // Poziom przeciwnika
        e15.firstRound = new List<int> { 150, 145, 155 }; // Kwoty pierwszego zakładu na rundę
        e15.subsequentRounds = new List<int> { 0, 100, 110 }; // Kwoty kolejnych zakładów
        playerData.betConfigurations.enemyConfigurations.Add(e15);
        e15.info = "He is a wealthy and influential merchant who excels at using his financial resources in dice games. His skill in investing and strategic thinking makes him a tough opponent. He is always well-prepared and ready to take risks. Can you defeat Feliks and claim his wealth?";

        // Master ---------------------------------------------------------------------------

        // Enemy 16
        EnemyBetConfigurations e16 = new EnemyBetConfigurations();
        e16.id = 2; // ID przeciwnika
        e16.defeated = false; // Czy gracz pokonał już tego przeciwnika?
        e16.enemyName = "Master Olgierd"; // Nazwa przeciwnika
        e16.status = "Master"; // Poziom przeciwnika
        e16.firstRound = new List<int> { 200, 255, 350 }; // Kwoty pierwszego zakładu na rundę
        e16.subsequentRounds = new List<int> { 0, 200, 250 }; // Kwoty kolejnych zakładów
        playerData.betConfigurations.enemyConfigurations.Add(e16);
        e16.info = "Master Olgierd is a legendary strategist and politician who has also become a master in dice games. Olgierd is a charismatic and powerful figure, known for his commanding presence and skill at manipulating situations. Prepare for an exhilarating match with this exceptional master of dice poker, as he might not be easy to defeat.";
    
        // Nowe dane zostają zapisane w pliku
        SavePlayerSettings();
    }
}
