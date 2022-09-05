using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu_Restart_Btn : MonoBehaviour
{
    public GameObject rubicCube;
    public GameObject counter;
    public Button solvingQuantity_Btn;
    public GameObject GeneralControls;
    public Button restart_Btn;
    bool adIsReady = false;

    public void onClick()
    {
        if (rubicCube.GetComponentInChildren<CubeControl>().isRotateStarted == false && rubicCube.GetComponentInChildren<CubeControl>().isShuffleRotation == false)
        {
            rubicCube.GetComponentInChildren<CubeControl>().resetRubicCube(restart_Btn);
            counter.GetComponent<Counter>().resetCounter();
            GlobalVariable.gameState = GlobalVariable.gameState_inGame;
            PlayerPrefs.SetInt("SolvingQuantity", GlobalVariable.defaultSolvingQuantity);
            PlayerPrefs.Save();
            solvingQuantity_Btn.GetComponentInChildren<TMP_Text>().text = GlobalVariable.defaultSolvingQuantity.ToString();
            if (adIsReady)
                GeneralControls.GetComponent<AdMobController>().ShowInterstitialAd();
        }
    }

    void Update()
    {
        if ((rubicCube.GetComponentInChildren<CubeControl>().isRotateStarted == true || rubicCube.GetComponentInChildren<CubeControl>().isShuffleRotation == true) && restart_Btn.interactable == true)
        {
            restart_Btn.interactable = false;
        }
        else if ((rubicCube.GetComponentInChildren<CubeControl>().isRotateStarted == false && rubicCube.GetComponentInChildren<CubeControl>().isShuffleRotation == false) && restart_Btn.interactable == false)
        {
            restart_Btn.interactable = true;
        }
    }

    public void adLoaded()
    {
        adIsReady = true;
    }
}
