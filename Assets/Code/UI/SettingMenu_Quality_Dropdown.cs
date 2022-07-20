using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Localization.Settings;

public class SettingMenu_Quality_Dropdown : MonoBehaviour
{
    public TMP_Dropdown qualityDropdownList;

    public void ChangeSelected()
    {
        int selectedItemVal = qualityDropdownList.value;
        if (selectedItemVal == 0)//low
        {
            QualitySettings.SetQualityLevel(2);
            PlayerPrefs.SetString("SelectedQuality", "2");
        }
        else if (selectedItemVal == 1)//medium
        {
            QualitySettings.SetQualityLevel(3);
            PlayerPrefs.SetString("SelectedQuality", "3");
        }
        else if (selectedItemVal == 2)//high
        {
            QualitySettings.SetQualityLevel(5);
            PlayerPrefs.SetString("SelectedQuality", "5");
        }
    }

    void OnEnable()
    {
        UpdateQualityOptions();  
    }
    
    public void UpdateQualityOptions()
    {
        qualityDropdownList.options[0].text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Low_Q");
        qualityDropdownList.options[1].text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "Medium_Q");
        qualityDropdownList.options[2].text = LocalizationSettings.StringDatabase.GetLocalizedString("GeneralTexts", "High_Q");
    }

}
