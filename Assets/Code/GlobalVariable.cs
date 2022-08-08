using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class GlobalVariable
{
    public const string rubicCube = "RubicCube";
    public const string groupJoint = "GroupJoint";
    public const string touchHelper = "TouchHelper";
    public const string resolveHelper = "ResolveHelper";
    public static int gameState = gameState_MainMenu;
    public const int gameState_inGame = 0;
    public const int gameState_MainMenu = 1;
    public const int gameState_PauseMenu = 2;
    public const int gameState_SettingsMenu = 3;
    public const int gameState_BestTimesMenu = 4;
    public const int gameState_Victory = 5;
    public const int gameState_NewGameMenu = 6;
    public const int gameState_TimesUp = 7;
    public const int defaultSolvingQuantity = 5;
    //test idler
    //public const string AndroidAdUnitId = "ca-app-pub-3940256099942544/5224354917";
    //public const string IphoneAdUnitId = "ca-app-pub-3940256099942544/1712485313";
}
