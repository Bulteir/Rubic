using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimesUpUI_MainMenu_Btn : MonoBehaviour
{
    public GameObject rubicCube;
    public GameObject GeneralControls;

    public void onClick()
    {
        rubicCube.GetComponent<CubeControl>().resetRubicCube();
        GlobalVariable.gameState = GlobalVariable.gameState_MainMenu;
        GeneralControls.GetComponent<AdMobInterstitialAdController>().ShowAd();
    }
}
