using Kociemba;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;

public class LeaderboardMenu_Btn : MonoBehaviour
{
    public GameObject GeneralController;
    public void onClick()
    {
        if (UnityServices.State == ServicesInitializationState.Initialized)
        {
            GeneralController.GetComponent<LeaderboardController>().FillLeaderboardList();
        }
        GlobalVariable.gameState = GlobalVariable.gameState_LeaderboardMenu;
    }
}
