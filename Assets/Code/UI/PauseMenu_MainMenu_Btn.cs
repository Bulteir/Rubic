using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu_MainMenu_Btn : MonoBehaviour
{
    public GameObject counter;
    public GameObject rubicCube;
    public void onClick()
    {
        rubicCube.GetComponentInChildren<CubeControl>().resetRubicCube(rubicCube.transform);
        counter.GetComponent<Counter>().resetCounter();
        GlobalVariable.gameState = GlobalVariable.gameState_MainMenu;
    }
}
