using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class TitleUIManager : MonoBehaviour
{
    public static TitleUIManager Instance { get; private set; }

    [Header("Panels")]
    public GameObject mainPanel;
    public GameObject modePanel;
    public GameObject namePanel;
    public GameObject pausePanel;
    public GameObject settingPanel;   // Å¸ÀÌÆ²/ÆÛÁî °ø¿ë ¼¼ÆÃ ÆÐ³Î

    [Header("UI Elements")]
    public Toggle hardcoreToggle;
    public TMP_InputField nameInputField;

    private bool hardcoreEnabled = false;
    private bool isPaused = false;

    // ´Ð³×ÀÓ À¯È¿¼º °Ë»ç (ÇÑ±Û/¿µ¹®/¼ýÀÚ/Æ¯Á¤ ±âÈ£, 1~12ÀÚ)
    private readonly Regex nameRegex = new Regex(@"^[°¡-ÆRa-zA-Z0-9_\-\.!\?@#$%&*]{1,12}$");

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // ±âº» ÆÐ³Î »óÅÂ
        if (mainPanel != null) mainPanel.SetActive(true);
        if (modePanel != null) modePanel.SetActive(false);
        if (namePanel != null) namePanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingPanel != null) settingPanel.SetActive(false);

        // ÀÌº¥Æ® µî·Ï
        if (hardcoreToggle != null)
            hardcoreToggle.onValueChanged.AddListener(OnHardcoreToggled);

        if (nameInputField != null)
            nameInputField.characterLimit = 12;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "GameScene")
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButtonDown("Cancel"))
            {
                if (!isPaused) PauseGame();
                else ResumeGame();
            }
        }
    }

    void OnHardcoreToggled(bool value) => hardcoreEnabled = value;

    // === ¸ÞÀÎ ¹öÆ° ===
    public void OnClickStart() { mainPanel.SetActive(false); modePanel.SetActive(true); }
    public void OnClickRanking() { Debug.Log("·©Å· º¸±â (ÃßÈÄ ±¸Çö)"); }
    public void OnClickQuitGame() { Application.Quit(); }   // Exit ´ë½Å QuitGame¸¸ ³²±è

    // === ¸ðµå ¼±ÅÃ ¹öÆ° ===
    public void OnClickTutorial() { PrepareName(GameMode.Normal, true); }
    public void OnClickNormal() { PrepareName(GameMode.Normal); }
    public void OnClickEndless() { PrepareName(GameMode.Endless); }
    public void OnClickBack() { modePanel.SetActive(false); mainPanel.SetActive(true); }

    void PrepareName(GameMode mode, bool tutorial = false)
    {
        GameSettings.SelectedGameMode = mode;
        GameSettings.Hardcore = hardcoreEnabled;
        GameSettings.Tutorial = tutorial;

        modePanel.SetActive(false);
        namePanel.SetActive(true);
    }

    public void OnClickConfirmName()
    {
        string inputName = nameInputField.text.Trim();

        if (string.IsNullOrEmpty(inputName) || !nameRegex.IsMatch(inputName))
        {
            Debug.LogWarning("´Ð³×ÀÓÀº 1~12ÀÚÀÇ ÇÑ±Û, ¿µ¹®, ¼ýÀÚ, ÀÏºÎ ±âÈ£¸¸ °¡´ÉÇÕ´Ï´Ù.");
            return;
        }

        GameSettings.PlayerName = inputName;
        namePanel.SetActive(false);

        SceneManager.LoadScene("GameScene");
    }

    // === ÆÛÁî °ü·Ã ===
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null) pausePanel.SetActive(true);
        if (settingPanel != null) settingPanel.SetActive(false); // ÆÛÁî ½Ã ¼¼ÆÃÀº ´Ý±â
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingPanel != null) settingPanel.SetActive(false); // Àç°³ ½Ã ¼¼ÆÃÀº ´Ý±â
    }

    public void OnClickReturnToTitle()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingPanel != null) settingPanel.SetActive(false);

        SceneManager.LoadScene("TitleScene");
    }

    // === ¼³Á¤ °ü·Ã ===
    public void OpenSettingPanel()
    {
        if (settingPanel != null)
            settingPanel.SetActive(true);
    }

    public void CloseSettingPanel()
    {
        if (settingPanel != null)
            settingPanel.SetActive(false);
    }
}
