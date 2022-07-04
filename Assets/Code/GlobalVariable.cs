using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class GlobalVariable
{
    public const string rubicCube = "RubicCube";
    public const string groupJoint = "GroupJoint";
    public const string touchHelper = "TouchHelper";
    public static int gameState = gameState_MainMenu;
    public const int gameState_inGame = 0;
    public const int gameState_MainMenu = 1;
    public const int gameState_PauseMenu = 2;
    public const int gameState_SettingsMenu = 3;
    public const int gameState_BestTimesMenu = 3;
    
    //arayüz tasarýmý yapýlan telefon redmi note 7 -- DD(Default Device)
    public const float DDScreenWitdh = 2340;
    public const float DDScreenHeight = 1080;
    public const float DDbuttonWidth = 750;
    public const float DDbuttonHeight = 150;
    public const float DDfontSize = 72;
}