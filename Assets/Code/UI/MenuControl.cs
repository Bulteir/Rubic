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

    public Camera mainCamera;
    public Camera menuCamera;
    public Canvas canvas;
    public Light menuCubeLight;
    public GameObject menuCubePrefab;
    public GameObject menuCube;
    public GameObject counter;

    public AudioSource menuMusic;
    public AudioSource buttonClick;
    public AudioSource cubeSnap;
    public AudioSource normalModeMusic;
    public AudioSource appleuse;
    public AudioSource challengeModeMusic;
    public AudioSource timesUp;

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

        string veryEasyJokerQuantity = PlayerPrefs.GetString("VeryEasyJoker");
        if (veryEasyJokerQuantity == "")
        {
            veryEasyJokerQuantity = "1";
            PlayerPrefs.SetString("VeryEasyJoker", veryEasyJokerQuantity);
            PlayerPrefs.Save();
        }

        string easyJokerQuantity = PlayerPrefs.GetString("EasyJoker");
        if (easyJokerQuantity == "")
        {
            easyJokerQuantity = "3";
            PlayerPrefs.SetString("EasyJoker", easyJokerQuantity);
            PlayerPrefs.Save();
        }

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
            SetMenuCameraActive();
            //CreateMenuCube();
            menuMusic.Play();
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
            SetMainCameraActive();
            DestroyMenuCube();
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
            SetMainCameraActive();
            DestroyMenuCube();
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
            SetMainCameraActive();
            DestroyMenuCube();
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
            SetMenuCameraActive();
            DestroyMenuCube();
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
            SetMenuCameraActive();
            DestroyMenuCube();
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
            SetMenuCameraActive();
            DestroyMenuCube();
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
            SetMainCameraActive();
            DestroyMenuCube();
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
            SetMenuCameraActive();
            CreateMenuCube();
            //sesler ve müzikler
            if (!menuMusic.isPlaying)
                menuMusic.Play();
            if (normalModeMusic.isPlaying)
                normalModeMusic.Stop();
            if (challengeModeMusic.isPlaying)
                challengeModeMusic.Stop();
            if (appleuse.isPlaying)
                appleuse.Stop();
            if (timesUp.isPlaying)
                timesUp.Stop();
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
            SetMainCameraActive();
            DestroyMenuCube();
            if (menuMusic.isPlaying)
                menuMusic.Stop();
            if(counter.GetComponent<Counter>().isChallengeModeActive)
            {
                if (!challengeModeMusic.isPlaying)
                    challengeModeMusic.Play();
            }
            else
            {
                if (!normalModeMusic.isPlaying)
                    normalModeMusic.Play();
            }
            if (appleuse.isPlaying)
                appleuse.Stop();
            if (timesUp.isPlaying)
                timesUp.Stop();
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
            SetMainCameraActive();
            DestroyMenuCube();
            if (!menuMusic.isPlaying)
                menuMusic.Play();
            if (normalModeMusic.isPlaying)
                normalModeMusic.Pause();
            if (challengeModeMusic.isPlaying)
                challengeModeMusic.Pause();
            if (appleuse.isPlaying)
                appleuse.Stop();
            if (timesUp.isPlaying)
                timesUp.Stop();
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
            SetMainCameraActive();
            DestroyMenuCube();
            if (menuMusic.isPlaying)
                menuMusic.Stop();
            if (normalModeMusic.isPlaying)
                normalModeMusic.Stop();
            if (challengeModeMusic.isPlaying)
                challengeModeMusic.Stop();
            if (!appleuse.isPlaying)
                appleuse.Play();
            if (timesUp.isPlaying)
                timesUp.Stop();
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
            SetMenuCameraActive();
            DestroyMenuCube();
            if (!menuMusic.isPlaying)
                menuMusic.Play();
            if (normalModeMusic.isPlaying)
                normalModeMusic.Stop();
            if (challengeModeMusic.isPlaying)
                challengeModeMusic.Stop();
            if (appleuse.isPlaying)
                appleuse.Stop();
            if (timesUp.isPlaying)
                timesUp.Stop();
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
            SetMenuCameraActive();
            DestroyMenuCube();
            if (!menuMusic.isPlaying)
                menuMusic.Play();
            if (normalModeMusic.isPlaying)
                normalModeMusic.Stop();
            if (challengeModeMusic.isPlaying)
                challengeModeMusic.Stop();
            if (appleuse.isPlaying)
                appleuse.Stop();
            if (timesUp.isPlaying)
                timesUp.Stop();
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
            SetMenuCameraActive();
            DestroyMenuCube();
            if (!menuMusic.isPlaying)
                menuMusic.Play();
            if (normalModeMusic.isPlaying)
                normalModeMusic.Stop();
            if (challengeModeMusic.isPlaying)
                challengeModeMusic.Stop();
            if (appleuse.isPlaying)
                appleuse.Stop();
            if (timesUp.isPlaying)
                timesUp.Stop();
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
            SetMainCameraActive();
            DestroyMenuCube();
            if (menuMusic.isPlaying)
                menuMusic.Stop();
            if (normalModeMusic.isPlaying)
                normalModeMusic.Stop();
            if (challengeModeMusic.isPlaying)
                challengeModeMusic.Stop();
            if (appleuse.isPlaying)
                appleuse.Stop();
            if (!timesUp.isPlaying)
                timesUp.Play();
        }
    }

    void SetMenuCameraActive()
    {
        menuCamera.targetDisplay = 0;
        mainCamera.targetDisplay = 1;
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = menuCamera;
        menuCubeLight.gameObject.SetActive(true);
    }
    void SetMainCameraActive()
    {
        menuCamera.targetDisplay = 1;
        mainCamera.targetDisplay = 0;
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.worldCamera = mainCamera;
        menuCubeLight.gameObject.SetActive(false);
    }

    public void PlayButtonClickSound()
    {
        buttonClick.Play();
    }

    public void PlayCubeSnapSound()
    {
        cubeSnap.Play();
    }

    void CreateMenuCube()
    {
        menuCube = Instantiate(menuCubePrefab, menuCubePrefab.transform.position, menuCubePrefab.transform.rotation);
    }

    void DestroyMenuCube()
    {
        if (menuCube)
        {
            Destroy(menuCube);
        }
    }
}
