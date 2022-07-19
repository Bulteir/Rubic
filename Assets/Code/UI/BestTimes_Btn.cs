using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BestTimes_Btn : MonoBehaviour
{
    public TMP_Text bestTimesText;
    public GameObject rubicCube;
    public void onClick()
    {
        GlobalVariable.gameState = GlobalVariable.gameState_BestTimesMenu;
        rubicCube.GetComponent<CubeControl>().getBestTimes(bestTimesText);
    }
}
