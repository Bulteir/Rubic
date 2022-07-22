using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimesUpUI_Restart_Btn : MonoBehaviour
{
    public GameObject rubicCube;
    public GameObject counter;
    public void onClick()
    {
        rubicCube.GetComponentInChildren<CubeControl>().resetRubicCube(gameObject.GetComponent<Button>());
        counter.GetComponent<Counter>().resetCounter();
        GlobalVariable.gameState = GlobalVariable.gameState_inGame;
    }
}
