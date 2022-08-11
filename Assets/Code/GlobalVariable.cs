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

    public static int rewardAdState = rewardAdState_idle;
    public const int rewardAdState_idle = 0;
    public const int rewardAdState_solve = 1;
    public const int rewardAdState_veryEasyJoker = 2;
    public const int rewardAdState_easyJoker = 3;

    public const int defaultShuffleStepCount = 20;

}
