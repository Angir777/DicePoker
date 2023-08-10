using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelButton : MonoBehaviour
{
    public string enemyName;

    private ModalPanelAboutEnemy modalPanelAboutEnemy;

    private void Start() {
        modalPanelAboutEnemy = FindObjectOfType<ModalPanelAboutEnemy>();
    }

    public void ShowModalAboutEnemy()
    {
        // Modal czy na pewno gracz chce rozpocząć nową grę?
        if (modalPanelAboutEnemy != null)
        {
            modalPanelAboutEnemy.ShowModalPanel(enemyName);
        }
    }
}
