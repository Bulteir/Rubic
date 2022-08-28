using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class NewGameMenu_ChallengeMode_Btn : MonoBehaviour
{
    public Transform RubicCube;
    public Transform Counter;
    public GameObject GeneralControls;
    public Button challangeModeBtn;

    public void onClick()
    {
        GlobalVariable.gameState = GlobalVariable.gameState_inGame;
        Counter.GetComponent<Counter>().isChallengeModeActive = true;

        Button challengeMode_Btn = gameObject.GetComponent<Button>();
        RubicCube.GetComponent<CubeControl>().resolveMoves = 0;        
        RubicCube.GetComponentInChildren<CubeControl>().Moves.text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Moves:") +
            RubicCube.GetComponentInChildren<CubeControl>().resolveMoves;

        string json = PlayerPrefs.GetString("Bests");
        List<CubeControl.BestTimesStruct> bestTimesList = new List<CubeControl.BestTimesStruct>();
        bestTimesList = JsonUtility.FromJson<CubeControl.JsonableListWrapper<CubeControl.BestTimesStruct>>(json).list;
        Counter.GetComponent<Counter>().text.text = bestTimesList[0].time;
        
        RubicCube.GetComponent<CubeControl>().shuffleCube(challengeMode_Btn);
        GeneralControls.GetComponent<AdMobController>().RequestAndLoadInterstitialAd();
    }

    public void SetChallangeModeBtnInteractable(bool interactable)
    {
        if (interactable)
        {
            challangeModeBtn.GetComponentInChildren<TMPro.TMP_Text>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 255f / 255f);
        }
        else
        {
            challangeModeBtn.GetComponentInChildren<TMPro.TMP_Text>().color = new Color(255f / 255f, 255f / 255f, 255f / 255f, 128f / 255f);
        }
    }
}
