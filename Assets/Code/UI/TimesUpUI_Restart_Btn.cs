using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TimesUpUI_Restart_Btn : MonoBehaviour
{
    public GameObject rubicCube;
    public GameObject counter;
    public Button solvingQuantity_Btn;
    public GameObject GeneralControls;

    public void onClick()
    {
        rubicCube.GetComponentInChildren<CubeControl>().resetRubicCube(gameObject.GetComponent<Button>());
        counter.GetComponent<Counter>().resetCounter();
        GlobalVariable.gameState = GlobalVariable.gameState_inGame;
        PlayerPrefs.SetInt("SolvingQuantity", GlobalVariable.defaultSolvingQuantity);
        PlayerPrefs.Save();
        solvingQuantity_Btn.GetComponentInChildren<TMP_Text>().text = GlobalVariable.defaultSolvingQuantity.ToString();
        GeneralControls.GetComponent<AdMobController>().ShowInterstitialAd();
    }
}
