using System.Collections;
using System.Collections.Generic;
using Unity.Services.Core;
using UnityEngine;

public class StoreMenu_Btn : MonoBehaviour
{
    public void onClick()
    {
        GlobalVariable.gameState = GlobalVariable.gameState_StoreMenu;
    }
}
