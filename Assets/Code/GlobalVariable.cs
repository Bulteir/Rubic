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

}