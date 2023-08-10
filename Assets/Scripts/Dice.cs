using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Dice : MonoBehaviour, IPointerDownHandler
{
    public int dotCount;
    public bool secondThrow;
    public bool picked;
    public bool blank;

    public Sprite dot0;
    public Sprite dot1;
    public Sprite dot2;
    public Sprite dot3;
    public Sprite dot4;
    public Sprite dot5;
    public Sprite dot6;

    public Sprite pickedDot1;
    public Sprite pickedDot2;
    public Sprite pickedDot3;
    public Sprite pickedDot4;
    public Sprite pickedDot5;
    public Sprite pickedDot6;

    private Image diceImage;
    private ThrowDices throwDices;

    void Start ()
    {
        throwDices = FindObjectOfType<ThrowDices>().GetComponent<ThrowDices>();
        dotCount = 0;
        diceImage = GetComponent<Image>();
	}

    private void Update()
    {
        if (blank)
        {
            diceImage.sprite = dot0;
            dotCount = 0;
            blank = false;
        }

        if (!picked)
        {
            switch (dotCount)
            {
                case 1:
                    diceImage.sprite = dot1;
                    break;
                case 2:
                    diceImage.sprite = dot2;
                    break;
                case 3:
                    diceImage.sprite = dot3;
                    break;
                case 4:
                    diceImage.sprite = dot4;
                    break;
                case 5:
                    diceImage.sprite = dot5;
                    break;
                case 6:
                    diceImage.sprite = dot6;
                    break;
            }
        }
        else
        {
            switch (dotCount)
            {
                case 1:
                    diceImage.sprite = pickedDot1;
                    break;
                case 2:
                    diceImage.sprite = pickedDot2;
                    break;
                case 3:
                    diceImage.sprite = pickedDot3;
                    break;
                case 4:
                    diceImage.sprite = pickedDot4;
                    break;
                case 5:
                    diceImage.sprite = pickedDot5;
                    break;
                case 6:
                    diceImage.sprite = pickedDot6;
                    break;
            }
        }
    }

    /*
    * Metoda uruchamiana jest podczas wyboru kosci do drugiego rzutu
    */
    public void OnPointerDown(PointerEventData eventData)
    {
        if (secondThrow)
        {
            if (picked)
            {
                picked = false;  
            }
            else
            {
                picked = true;
            }
            // Sygnał, że wybrano kostkę do ponownego rzutu
            throwDices.dicePicked();
        }
    }
}
