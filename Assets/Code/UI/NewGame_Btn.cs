using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NewGame_Btn : MonoBehaviour
{
    public Button challenge_Btn;
    public Button solvingQuantity_Btn;
    public GameObject generalControls;
    public Button veryEasyJokerQuantity_Btn;
    public Button easyJokerQuantity_Btn;
    public Transform rubicCube;

    public void onClick()
    {
        GlobalVariable.gameState = GlobalVariable.gameState_NewGameMenu;

        string json = PlayerPrefs.GetString("Bests");
        if (json == "")
        {
            challenge_Btn.interactable = false;
            challenge_Btn.GetComponent<NewGameMenu_ChallengeMode_Btn>().SetChallangeModeBtnInteractable(false);
        }
        else
        {
            challenge_Btn.interactable = true;
            challenge_Btn.GetComponent<NewGameMenu_ChallengeMode_Btn>().SetChallangeModeBtnInteractable(true);
        }
        PlayerPrefs.SetInt("SolvingQuantity", GlobalVariable.defaultSolvingQuantity);
        PlayerPrefs.Save();
        rubicCube.GetComponent<CubeControl>().shuffleStepCount = GlobalVariable.defaultShuffleStepCount;
        solvingQuantity_Btn.GetComponentInChildren<TMP_Text>().text = GlobalVariable.defaultSolvingQuantity.ToString();
        veryEasyJokerQuantity_Btn.GetComponentInChildren<TMP_Text>().text = PlayerPrefs.GetString("VeryEasyJoker");
        easyJokerQuantity_Btn.GetComponentInChildren<TMP_Text>().text = PlayerPrefs.GetString("EasyJoker");
        GlobalVariable.rewardAdState = GlobalVariable.rewardAdState_idle;
        easyJokerQuantity_Btn.transform.parent.GetComponent<Button>().interactable = true;
        veryEasyJokerQuantity_Btn.transform.parent.GetComponent<Button>().interactable = true;
    }
}
