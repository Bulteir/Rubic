using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class Settings_Btn : MonoBehaviour
{
    public TMP_Dropdown languageDropdownList;
    public TMP_Dropdown qaulityDropdownList;
    public void onClick()
    {
        GlobalVariable.gameState = GlobalVariable.gameState_SettingsMenu;
        languageDropdownList.value = -1;
        qaulityDropdownList.value = -1;

    }
}
