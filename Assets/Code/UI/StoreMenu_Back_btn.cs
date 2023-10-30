using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoreMenu_Back_btn : MonoBehaviour
{
    public void onClick()
    {
        GlobalVariable.gameState = GlobalVariable.gameState_MainMenu;
    }
}
