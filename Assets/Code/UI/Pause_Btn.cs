using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause_Btn : MonoBehaviour
{
    public GameObject rubicCube;
    public void onClick()
    {
        rubicCube.GetComponent<CubeCameraControl>().isEnteredPauseButton = true;
        GlobalVariable.gameState = GlobalVariable.gameState_PauseMenu;
    }
}
