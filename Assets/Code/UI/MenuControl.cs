using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
using Kociemba;
using System.Threading.Tasks;
using System;
#if UNITY_IOS
using Unity.Advertisement.IosSupport;
#endif

public class MenuControl : MonoBehaviour
{
    public GameObject mainMenuPanel;
    public GameObject pauseMenuPanel;
    public GameObject bestTimePanel;
    public GameObject leaderboardPanel;
    public GameObject storePanel;
    public GameObject inGameUIPanel;
    public GameObject settingsPanel;
    public GameObject counterPanel;
    public GameObject victoryUIPanel;
    public GameObject newGameMenuPanel;
    public GameObject timesUpUI;

    public Button solve_Btn;
    public Button easyJoker_Btn_Quantity;
    public Button veryEasyJoker_Btn_Quantity;
    public Button solve_Btn_Quantity;
    public Button mainMenuNoAds_Btn;

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
        string music = PlayerPrefs.GetString("Music");
        if (music == "")
        {
            music = "1";
            PlayerPrefs.SetString("Music", music);
            PlayerPrefs.Save();
        }
        string soundEffect = PlayerPrefs.GetString("SoundEffect");
        if (soundEffect == "")
        {
            soundEffect = "1";
            PlayerPrefs.SetString("SoundEffect", soundEffect);
            PlayerPrefs.Save();
        }
        string increaseHintPowerupActive = PlayerPrefs.GetString("increaseHintPowerupActive");
        if (increaseHintPowerupActive == "1")
        {
            GlobalVariable.defaultSolvingQuantity = 15;
        }
    }

    //Kociemba tablolar�n� oyun ba�lad��� anda paralele bir thread kullanarak olu�turuyoruz. Bu sayede vakit hem kazann�l�yor hem de oyun tak�lm�yor. 
    private async void LoadKociembaTables()
    {
        await Task.Delay(0);
        isSolveButtonActiveFirstTime = false;
        string info = "";
        string searchString = "UUUUUULLLURRURRURRFFFFFFFFFRRRDDDDDDLLDLLDLLDBBBBBBBBB";
        //bir kere pcde table'lalr� olu�turup apk i�inde telefona att�m. Bu sayede s�reden a��r� tasarruf sa�lad�m. Telefonda kullan�labilir hale geldi.
        //�uan builTable:True olsa da tablo olu�turmuyor. Yorum sat�r�na ald�m.
        string solution = SearchRunTime.solution(searchString, out info, buildTables: true);
        Debug.Log("Kociemba tables is ready");
        isLoadedKociembaTables = true;
    }

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;

        //oyun a��l�rken store initial yapaca��z. ��nk� restore buton i�in loadcatalog'un �al��m�� olmas� ve OnInitialized fonksiyonuna girmi� olmas� gerekiyor
        storePanel.GetComponent<StoreController>().UnityServicesInitial();

        #region iosta reklam g�sterebilmek i�in gerekli olan izin kontrol�
#if UNITY_IOS
        // check with iOS to see if the user has accepted or declined tracking
        //var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

        //if (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        //{
        //    ATTrackingStatusBinding.RequestAuthorizationTracking();
        //}
#endif
        #endregion

        int easyJokerQuantity = int.Parse(PlayerPrefs.GetString("EasyJoker"));
        int veryEasyJokerQuantity = int.Parse(PlayerPrefs.GetString("VeryEasyJoker"));
        if (easyJokerQuantity == 0 || veryEasyJokerQuantity == 0)
        {
            if (PlayerPrefs.GetString("NoAdsActive") != "1")
            {
                transform.GetComponent<AdMobRewardedAdController>().LoadAd();
            }
        }

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
            leaderboardPanel.SetActive(false);
            storePanel.SetActive(false);
            SetMenuCameraActive();
            int music = int.Parse(PlayerPrefs.GetString("Music"));
            if (music == 1)
                menuMusic.Play();
            if (PlayerPrefs.GetString("NoAdsActive") == "1")
            {
                mainMenuNoAds_Btn.gameObject.SetActive(false);
            }
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
            leaderboardPanel.SetActive(false);
            storePanel.SetActive(false);
            SetMainCameraActive();
            DestroyMenuCube();

            if (PlayerPrefs.GetString("NoAdsActive") != "1")
            {
                transform.GetComponent<AdMobBannerViewController>().LoadAd();
            }
            else if (PlayerPrefs.GetString("NoAdsActive") == "1")
            {
                solve_Btn_Quantity.gameObject.SetActive(false);
            }
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
            leaderboardPanel.SetActive(false);
            storePanel.SetActive(false);
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
            leaderboardPanel.SetActive(false);
            storePanel.SetActive(false);
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
            leaderboardPanel.SetActive(false);
            storePanel.SetActive(false);
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
            leaderboardPanel.SetActive(false);
            storePanel.SetActive(false);
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
            leaderboardPanel.SetActive(false);
            storePanel.SetActive(false);
            SetMenuCameraActive();
            DestroyMenuCube();
            if (PlayerPrefs.GetString("NoAdsActive") == "1")
            {
                easyJoker_Btn_Quantity.gameObject.SetActive(false);
                veryEasyJoker_Btn_Quantity.gameObject.SetActive(false);
            }
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
            leaderboardPanel.SetActive(false);
            storePanel.SetActive(false);
            SetMainCameraActive();
            DestroyMenuCube();
        }
        else if (GlobalVariable.gameState == GlobalVariable.gameState_LeaderboardMenu)
        {
            inGameUIPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(false);
            counterPanel.SetActive(false);
            victoryUIPanel.SetActive(false);
            newGameMenuPanel.SetActive(false);
            timesUpUI.SetActive(false);
            leaderboardPanel.SetActive(true);
            storePanel.SetActive(false);
            SetMainCameraActive();
            DestroyMenuCube();
        }
        else if (GlobalVariable.gameState == GlobalVariable.gameState_StoreMenu)
        {
            inGameUIPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(false);
            counterPanel.SetActive(false);
            victoryUIPanel.SetActive(false);
            newGameMenuPanel.SetActive(false);
            timesUpUI.SetActive(false);
            leaderboardPanel.SetActive(false);
            storePanel.SetActive(true);
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
            leaderboardPanel.SetActive(false);
            storePanel.SetActive(false);
            SetMenuCameraActive();
            CreateMenuCube();
            transform.GetComponent<AdMobBannerViewController>().DestroyAd();
            //sesler ve m�zikler
            int music = int.Parse(PlayerPrefs.GetString("Music"));
            if (music == 1)
            {
                if (!menuMusic.isPlaying)
                    menuMusic.Play();
            }

            if (normalModeMusic.isPlaying)
                normalModeMusic.Stop();
            if (challengeModeMusic.isPlaying)
                challengeModeMusic.Stop();
            if (appleuse.isPlaying)
                appleuse.Stop();
            if (timesUp.isPlaying)
                timesUp.Stop();

            int easyJokerQuantity = int.Parse(PlayerPrefs.GetString("EasyJoker"));
            int veryEasyJokerQuantity = int.Parse(PlayerPrefs.GetString("VeryEasyJoker"));
            if (easyJokerQuantity == 0 || veryEasyJokerQuantity == 0)
            {
                if (PlayerPrefs.GetString("NoAdsActive") != "1")
                {
                    transform.GetComponent<AdMobRewardedAdController>().LoadAd();
                }
            }

            if (PlayerPrefs.GetString("NoAdsActive") == "1")
            {
                mainMenuNoAds_Btn.gameObject.SetActive(false);
            }
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
            leaderboardPanel.SetActive(false);
            storePanel.SetActive(false);
            SetMainCameraActive();
            DestroyMenuCube();
            if (PlayerPrefs.GetString("NoAdsActive") != "1")
            {
                transform.GetComponent<AdMobBannerViewController>().LoadAd();
            }
            else if (PlayerPrefs.GetString("NoAdsActive") == "1")
            {
                solve_Btn_Quantity.gameObject.SetActive(false);
            }

            if (menuMusic.isPlaying)
                menuMusic.Stop();

            int music = int.Parse(PlayerPrefs.GetString("Music"));
            if (music == 1)
            {
                if (counter.GetComponent<Counter>().isChallengeModeActive)
                {
                    if (!challengeModeMusic.isPlaying)
                        challengeModeMusic.Play();
                }
                else
                {
                    if (!normalModeMusic.isPlaying)
                        normalModeMusic.Play();
                }
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
            leaderboardPanel.SetActive(false);
            storePanel.SetActive(false);
            SetMainCameraActive();
            DestroyMenuCube();
            transform.GetComponent<AdMobBannerViewController>().DestroyAd();

            int music = int.Parse(PlayerPrefs.GetString("Music"));
            if (music == 1)
            {
                if (!menuMusic.isPlaying)
                    menuMusic.Play();
            }

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
            leaderboardPanel.SetActive(false);
            storePanel.SetActive(false);
            SetMainCameraActive();
            DestroyMenuCube();
            if (menuMusic.isPlaying)
                menuMusic.Stop();
            if (normalModeMusic.isPlaying)
                normalModeMusic.Stop();
            if (challengeModeMusic.isPlaying)
                challengeModeMusic.Stop();
            int soundEffect = int.Parse(PlayerPrefs.GetString("SoundEffect"));
            if (soundEffect == 1)
            {
                if (!appleuse.isPlaying)
                    appleuse.Play();
            }
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
            leaderboardPanel.SetActive(false);
            storePanel.SetActive(false);
            SetMenuCameraActive();
            DestroyMenuCube();

            int music = int.Parse(PlayerPrefs.GetString("Music"));
            if (music == 1)
            {
                if (!menuMusic.isPlaying)
                    menuMusic.Play();
            }
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
            leaderboardPanel.SetActive(false);
            storePanel.SetActive(false);
            SetMenuCameraActive();
            DestroyMenuCube();
            int music = int.Parse(PlayerPrefs.GetString("Music"));
            if (music == 1)
            {
                if (!menuMusic.isPlaying)
                    menuMusic.Play();
            }
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
            leaderboardPanel.SetActive(false);
            storePanel.SetActive(false);
            SetMenuCameraActive();
            DestroyMenuCube();
            int music = int.Parse(PlayerPrefs.GetString("Music"));
            if (music == 1)
            {
                if (!menuMusic.isPlaying)
                    menuMusic.Play();
            }
            if (normalModeMusic.isPlaying)
                normalModeMusic.Stop();
            if (challengeModeMusic.isPlaying)
                challengeModeMusic.Stop();
            if (appleuse.isPlaying)
                appleuse.Stop();
            if (timesUp.isPlaying)
                timesUp.Stop();
            if (PlayerPrefs.GetString("NoAdsActive") == "1")
            {
                easyJoker_Btn_Quantity.gameObject.SetActive(false);
                veryEasyJoker_Btn_Quantity.gameObject.SetActive(false);
            }
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
            leaderboardPanel.SetActive(false);
            storePanel.SetActive(false);
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
            int soundEffect = int.Parse(PlayerPrefs.GetString("SoundEffect"));
            if (soundEffect == 1)
            {
                if (!timesUp.isPlaying)
                    timesUp.Play();
            }
        }
        else if (GlobalVariable.gameState == GlobalVariable.gameState_LeaderboardMenu && leaderboardPanel.activeSelf == false)
        {
            inGameUIPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(false);
            counterPanel.SetActive(false);
            victoryUIPanel.SetActive(false);
            newGameMenuPanel.SetActive(false);
            timesUpUI.SetActive(false);
            leaderboardPanel.SetActive(true);
            storePanel.SetActive(false);
            SetMainCameraActive();
            DestroyMenuCube();
            int music = int.Parse(PlayerPrefs.GetString("Music"));
            if (music == 1)
            {
                if (!menuMusic.isPlaying)
                    menuMusic.Play();
            }
            if (normalModeMusic.isPlaying)
                normalModeMusic.Stop();
            if (challengeModeMusic.isPlaying)
                challengeModeMusic.Stop();
            if (appleuse.isPlaying)
                appleuse.Stop();
            if (timesUp.isPlaying)
                timesUp.Stop();

        }
        else if (GlobalVariable.gameState == GlobalVariable.gameState_StoreMenu && storePanel.activeSelf == false)
        {
            inGameUIPanel.SetActive(false);
            mainMenuPanel.SetActive(false);
            pauseMenuPanel.SetActive(false);
            settingsPanel.SetActive(false);
            bestTimePanel.SetActive(false);
            counterPanel.SetActive(false);
            victoryUIPanel.SetActive(false);
            newGameMenuPanel.SetActive(false);
            timesUpUI.SetActive(false);
            leaderboardPanel.SetActive(false);
            storePanel.SetActive(true);
            SetMainCameraActive();
            DestroyMenuCube();
            int music = int.Parse(PlayerPrefs.GetString("Music"));
            if (music == 1)
            {
                if (!menuMusic.isPlaying)
                    menuMusic.Play();
            }
            if (normalModeMusic.isPlaying)
                normalModeMusic.Stop();
            if (challengeModeMusic.isPlaying)
                challengeModeMusic.Stop();
            if (appleuse.isPlaying)
                appleuse.Stop();
            if (timesUp.isPlaying)
                timesUp.Stop();
        }
    }

    void SetMenuCameraActive()
    {
        //menuCamera.targetDisplay = 0;
        //mainCamera.targetDisplay = 1;
        menuCamera.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(false);
        canvas.renderMode = RenderMode.ScreenSpaceCamera;
        canvas.worldCamera = menuCamera;
        menuCubeLight.gameObject.SetActive(true);
    }
    void SetMainCameraActive()
    {
        //menuCamera.targetDisplay = 1;
        //mainCamera.targetDisplay = 0;
        menuCamera.gameObject.SetActive(false);
        mainCamera.gameObject.SetActive(true);
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.worldCamera = mainCamera;
        menuCubeLight.gameObject.SetActive(false);
    }

    public void PlayButtonClickSound()
    {
        int soundEffect = int.Parse(PlayerPrefs.GetString("SoundEffect"));
        if (soundEffect == 1)
        {
            buttonClick.Play();
        }
    }

    public void PlayCubeSnapSound()
    {
        int soundEffect = int.Parse(PlayerPrefs.GetString("SoundEffect"));
        if (soundEffect == 1)
        {
            cubeSnap.Play();
        }
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
