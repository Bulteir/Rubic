using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenu_Continue_Btn : MonoBehaviour
{
    public void onClick()
    {
        GlobalVariable.gameState = GlobalVariable.gameState_inGame;
    }
}
