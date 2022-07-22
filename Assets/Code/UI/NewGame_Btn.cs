using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGame_Btn : MonoBehaviour
{
    public Button challenge_Btn;
    public void onClick()
    {
        GlobalVariable.gameState = GlobalVariable.gameState_NewGameMenu;

        string json = PlayerPrefs.GetString("Bests");
        if (json == "")
        {
            challenge_Btn.interactable = false;
        }
        else
        {
            challenge_Btn.interactable = true;
        }

    }
}
