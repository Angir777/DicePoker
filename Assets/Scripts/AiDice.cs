using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AiDice : MonoBehaviour
{
    public int AiDotCount;
    public bool AiSecondThrow;
    public bool AiPicked;
    public bool AiBlank;

    public Sprite AiDot0;
    public Sprite AiDot1;
    public Sprite AiDot2;
    public Sprite AiDot3;
    public Sprite AiDot4;
    public Sprite AiDot5;
    public Sprite AiDot6;

    public Sprite AiPickedDot1;
    public Sprite AiPickedDot2;
    public Sprite AiPickedDot3;
    public Sprite AiPickedDot4;
    public Sprite AiPickedDot5;
    public Sprite AiPickedDot6;

    private Image AiDiceImage;
    private AiThrowDices AiThrowDices;

    void Start ()
    {
        AiThrowDices = FindObjectOfType<AiThrowDices>().GetComponent<AiThrowDices>();
        AiDotCount = 0;
        AiDiceImage = GetComponent<Image>();
	}

    private void Update()
    {
        if (AiBlank)
        {
            AiDiceImage.sprite = AiDot0;
            AiDotCount = 0;
            AiBlank = false;
        }

        if (!AiPicked)
        {
            switch (AiDotCount)
            {
                case 1:
                    AiDiceImage.sprite = AiDot1;
                    break;
                case 2:
                    AiDiceImage.sprite = AiDot2;
                    break;
                case 3:
                    AiDiceImage.sprite = AiDot3;
                    break;
                case 4:
                    AiDiceImage.sprite = AiDot4;
                    break;
                case 5:
                    AiDiceImage.sprite = AiDot5;
                    break;
                case 6:
                    AiDiceImage.sprite = AiDot6;
                    break;
            }
        }
        else
        {
            switch (AiDotCount)
            {
                case 1:
                    AiDiceImage.sprite = AiPickedDot1;
                    break;
                case 2:
                    AiDiceImage.sprite = AiPickedDot2;
                    break;
                case 3:
                    AiDiceImage.sprite = AiPickedDot3;
                    break;
                case 4:
                    AiDiceImage.sprite = AiPickedDot4;
                    break;
                case 5:
                    AiDiceImage.sprite = AiPickedDot5;
                    break;
                case 6:
                    AiDiceImage.sprite = AiPickedDot6;
                    break;
            }
        }
    }
}