using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class NewGameMenu_ChallengeMode_Btn : MonoBehaviour
{
    public Transform RubicCube;
    public Transform Counter;

    public void onClick()
    {
        GlobalVariable.gameState = GlobalVariable.gameState_inGame;
        Counter.GetComponent<Counter>().isChallengeModeActive = true;

        Button challengeMode_Btn = gameObject.GetComponent<Button>();
        RubicCube.GetComponent<CubeControl>().resolveMoves = 0;        
        RubicCube.GetComponentInChildren<CubeControl>().Moves.text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Moves:") +
            RubicCube.GetComponentInChildren<CubeControl>().resolveMoves;
        RubicCube.GetComponent<CubeControl>().shuffleCube(challengeMode_Btn);
    }
}
