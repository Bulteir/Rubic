using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_NoAds_Btn : MonoBehaviour
{
    public void onClick()
    {
        GlobalVariable.gameState = GlobalVariable.gameState_StoreMenu;
    }
}
