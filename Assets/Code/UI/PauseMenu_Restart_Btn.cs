using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu_Restart_Btn : MonoBehaviour
{
    public GameObject rubicCube;
    public GameObject counter;

    public void onClick()
    {
        rubicCube.GetComponentInChildren<CubeControl>().resetRubicCube(rubicCube.transform,true, gameObject.GetComponent<Button>());
        counter.GetComponent<Counter>().resetCounter();
        GlobalVariable.gameState = GlobalVariable.gameState_inGame;
    }
}
