using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewGame_Btn : MonoBehaviour
{
    public Button challenge_Btn;
    public Button solvingQuantity_Btn;
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
        PlayerPrefs.SetInt("SolvingQuantity", GlobalVariable.defaultSolvingQuantity);
        PlayerPrefs.Save();
        solvingQuantity_Btn.GetComponentInChildren<TMP_Text>().text = GlobalVariable.defaultSolvingQuantity.ToString();
    }
}
