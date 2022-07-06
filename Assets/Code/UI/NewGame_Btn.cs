using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewGame_Btn : MonoBehaviour
{
    public Transform RubicCube;
    public void onClick()
    {
        GlobalVariable.gameState = GlobalVariable.gameState_inGame;
        Button newGame_Btn = gameObject.GetComponent<Button>();
        RubicCube.GetComponent<CubeControl>().shuffleCube(newGame_Btn);

    }
}
