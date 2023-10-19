using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardMenu_Back_Btn : MonoBehaviour
{
    public void onClick()
    {
        GlobalVariable.gameState = GlobalVariable.gameState_MainMenu;
    }
}
