using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class NewGameMenu_NormalMode_Btn : MonoBehaviour
{
    public Transform RubicCube;
    public Transform counter;
    public GameObject GeneralControls;

    public void onClick()
    {
        GlobalVariable.gameState = GlobalVariable.gameState_inGame;
        Button normalMode_Btn = gameObject.GetComponent<Button>();
        RubicCube.GetComponent<CubeControl>().resolveMoves = 0;
        RubicCube.GetComponentInChildren<CubeControl>().Moves.text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Moves:") +
            RubicCube.GetComponentInChildren<CubeControl>().resolveMoves;
        RubicCube.GetComponent<CubeControl>().shuffleCube(normalMode_Btn);
        counter.GetComponent<Counter>().isChallengeModeActive = false;
        counter.GetComponent<Counter>().resetCounter();
        if (PlayerPrefs.GetString("NoAdsActive") != "1")
        {
            GeneralControls.GetComponent<AdMobInterstitialAdController>().LoadAd();
        }
    }
}
