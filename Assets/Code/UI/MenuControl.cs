using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class MenuControl : MonoBehaviour
{

    public GameObject mainMenuPanel;
    public GameObject pauseMenuPanel;
    public GameObject bestTimePanel;
    public GameObject inGameUIPanel;
    public GameObject settingsPanel;
    public GameObject counterPanel;
    public GameObject victoryUIPanel;

    void Awake()
    {
        string selectedLangVal = PlayerPrefs.GetString("SelectedLang");
        if (selectedLangVal != "")
        {
            LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[int.Parse(selectedLangVal)];
        }

        string selectedQualityVal = PlayerPrefs.GetString("SelectedQuality");
        if (selectedQualityVal != "")
        {
            QualitySettings.SetQualityLevel(int.Parse(selectedQualityVal));
        }

    }

    void Start()
    {
        
        if (GlobalVariable.gameState == GlobalVariable.gameState_MainMenu)
        {
            inGameUIPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(false);
            counterPanel.SetActive(false);
            victoryUIPanel.SetActive(false);
        }
        else if (GlobalVariable.gameState == GlobalVariable.gameState_inGame)
        {
            inGameUIPanel.SetActive(true);
            mainMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(false);
            counterPanel.SetActive(true);
            victoryUIPanel.SetActive(false);
        }
        else if (GlobalVariable.gameState == GlobalVariable.gameState_PauseMenu)
        {
            inGameUIPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(true);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(false);
            counterPanel.SetActive(true);
            victoryUIPanel.SetActive(false);
        }
        else if (GlobalVariable.gameState == GlobalVariable.gameState_Victory)
        {
            inGameUIPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(false);
            counterPanel.SetActive(false);
            victoryUIPanel.SetActive(true);
        }
        else if (GlobalVariable.gameState == GlobalVariable.gameState_BestTimesMenu)
        {
            inGameUIPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(true);
            counterPanel.SetActive(false);
            victoryUIPanel.SetActive(false);
        }
        else if (GlobalVariable.gameState == GlobalVariable.gameState_SettingsMenu)
        {
            inGameUIPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(true);
            bestTimePanel.SetActive(false);
            counterPanel.SetActive(false);
            victoryUIPanel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalVariable.gameState == GlobalVariable.gameState_MainMenu && mainMenuPanel.activeSelf == false)
        {
            inGameUIPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(false);
            counterPanel.SetActive(false);
            victoryUIPanel.SetActive(false);
        }
        else if (GlobalVariable.gameState == GlobalVariable.gameState_inGame && inGameUIPanel.activeSelf == false)
        {
            inGameUIPanel.SetActive(true);
            mainMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(false);
            counterPanel.SetActive(true);
            victoryUIPanel.SetActive(false);
        }
        else if (GlobalVariable.gameState == GlobalVariable.gameState_PauseMenu && pauseMenuPanel.activeSelf == false)
        {
            inGameUIPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(true);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(false);
            counterPanel.SetActive(true);
            victoryUIPanel.SetActive(false);
        }
        else if (GlobalVariable.gameState == GlobalVariable.gameState_Victory && victoryUIPanel.activeSelf == false)
        {
            inGameUIPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(false);
            counterPanel.SetActive(false);
            victoryUIPanel.SetActive(true);
        }
        else if (GlobalVariable.gameState == GlobalVariable.gameState_BestTimesMenu && bestTimePanel.activeSelf == false)
        {
            inGameUIPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(true);
            counterPanel.SetActive(false);
            victoryUIPanel.SetActive(false);
        }
        else if (GlobalVariable.gameState == GlobalVariable.gameState_SettingsMenu && settingsPanel.activeSelf == false)
        {
            inGameUIPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(true);
            bestTimePanel.SetActive(false);
            counterPanel.SetActive(false);
            victoryUIPanel.SetActive(false);
        }
    }

}
