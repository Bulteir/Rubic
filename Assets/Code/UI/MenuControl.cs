using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using Kociemba;
using System.Threading.Tasks;

public class MenuControl : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject pauseMenuPanel;
    public GameObject bestTimePanel;
    public GameObject inGameUIPanel;
    public GameObject settingsPanel;
    public GameObject counterPanel;
    public GameObject victoryUIPanel;
    public GameObject newGameMenuPanel;
    public GameObject timesUpUI;

    public Button solve_Btn;
    public bool isLoadedKociembaTables = false;
    bool isSolveButtonActiveFirstTime = false;
    public GameObject rubicCube;

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

        Task.Factory.StartNew(() => LoadKociembaTables());
    }

    //Kociemba tablolarýný oyun baþladýðý anda paralele bir thread kullanarak oluþturuyoruz. Bu sayede vakit hem kazannýlýyor hem de oyun takýlmýyor. 
    private async void LoadKociembaTables()
    {
        await Task.Delay(0);
        isSolveButtonActiveFirstTime = false;
        string info = "";
        string searchString = "UUUUUULLLURRURRURRFFFFFFFFFRRRDDDDDDLLDLLDLLDBBBBBBBBB";
        //bir kere pcde table'lalrý oluþturup apk içinde telefona attým. Bu sayede süreden aþýrý tasarruf saðladým. Telefonda kullanýlabilir hale geldi.
        //Þuan builTable:True olsa da tablo oluþturmuyor. Yorum satýrýna aldým.
        string solution = SearchRunTime.solution(searchString, out info,buildTables:true);
        Debug.Log("Kociemba tables is ready");
        isLoadedKociembaTables = true;
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
            newGameMenuPanel.SetActive(false);
            timesUpUI.SetActive(false);
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
            newGameMenuPanel.SetActive(false);
            timesUpUI.SetActive(false);
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
            newGameMenuPanel.SetActive(false);
            timesUpUI.SetActive(false);
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
            newGameMenuPanel.SetActive(false);
            timesUpUI.SetActive(false);
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
            newGameMenuPanel.SetActive(false);
            timesUpUI.SetActive(false);
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
            newGameMenuPanel.SetActive(false);
            timesUpUI.SetActive(false);
        }
        else if (GlobalVariable.gameState == GlobalVariable.gameState_NewGameMenu)
        {
            inGameUIPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(false);
            counterPanel.SetActive(false);
            victoryUIPanel.SetActive(false);
            newGameMenuPanel.SetActive(true);
            timesUpUI.SetActive(false);
        }
        else if (GlobalVariable.gameState == GlobalVariable.gameState_TimesUp)
        {
            inGameUIPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(false);
            counterPanel.SetActive(false);
            victoryUIPanel.SetActive(false);
            newGameMenuPanel.SetActive(false);
            timesUpUI.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isLoadedKociembaTables == true && isSolveButtonActiveFirstTime == false && rubicCube.GetComponent<CubeControl>().isShuffleRotation == false)
        {
            solve_Btn.interactable = true;
            isSolveButtonActiveFirstTime = true;
            solve_Btn.GetComponent<Solve_Btn>().SetSolvingQuantityBtnInteractable(true);
        }

        if (GlobalVariable.gameState == GlobalVariable.gameState_MainMenu && mainMenuPanel.activeSelf == false)
        {
            inGameUIPanel.SetActive(false);
            mainMenuPanel.SetActive(true);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(false);
            counterPanel.SetActive(false);
            victoryUIPanel.SetActive(false);
            newGameMenuPanel.SetActive(false);
            timesUpUI.SetActive(false);
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
            newGameMenuPanel.SetActive(false);
            timesUpUI.SetActive(false);
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
            newGameMenuPanel.SetActive(false);
            timesUpUI.SetActive(false);
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
            newGameMenuPanel.SetActive(false);
            timesUpUI.SetActive(false);
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
            newGameMenuPanel.SetActive(false);
            timesUpUI.SetActive(false);
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
            newGameMenuPanel.SetActive(false);
            timesUpUI.SetActive(false);
        }
        else if (GlobalVariable.gameState == GlobalVariable.gameState_NewGameMenu && newGameMenuPanel.activeSelf == false)
        {
            inGameUIPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(false);
            counterPanel.SetActive(false);
            victoryUIPanel.SetActive(false);
            newGameMenuPanel.SetActive(true);
            timesUpUI.SetActive(false);
        }
        else if (GlobalVariable.gameState == GlobalVariable.gameState_TimesUp && timesUpUI.activeSelf == false)
        {
            inGameUIPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(false);
            counterPanel.SetActive(false);
            victoryUIPanel.SetActive(false);
            newGameMenuPanel.SetActive(false);
            timesUpUI.SetActive(true);
        }
    }

}
