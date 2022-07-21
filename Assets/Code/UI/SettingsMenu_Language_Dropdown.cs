using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class SettingsMenu_Language_Dropdown : MonoBehaviour
{
    public TMP_Dropdown languageDropdownList;
    public TMP_Dropdown qualityDropdownList;

    public void ChangeSelected()
    {
        int selectedItemVal = languageDropdownList.value;
        if (selectedItemVal == 0)//english
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
            PlayerPrefs.SetString("SelectedLang", "0");
        }
        else if (selectedItemVal == 1)//türkçe
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
            PlayerPrefs.SetString("SelectedLang", "1");
        }
        PlayerPrefs.Save();
        qualityDropdownList.GetComponent<SettingMenu_Quality_Dropdown>().UpdateQualityOptions();
    }
}
