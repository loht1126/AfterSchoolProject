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
    public GameObject settingPanel;   // Ÿ��Ʋ/���� ���� ���� �г�

    [Header("UI Elements")]
    public Toggle hardcoreToggle;
    public TMP_InputField nameInputField;

    private bool hardcoreEnabled = false;
    private bool isPaused = false;

    // �г��� ��ȿ�� �˻� (�ѱ�/����/����/Ư�� ��ȣ, 1~12��)
    private readonly Regex nameRegex = new Regex(@"^[��-�Ra-zA-Z0-9_\-\.!\?@#$%&*]{1,12}$");

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
        // �⺻ �г� ����
        if (mainPanel != null) mainPanel.SetActive(true);
        if (modePanel != null) modePanel.SetActive(false);
        if (namePanel != null) namePanel.SetActive(false);
        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingPanel != null) settingPanel.SetActive(false);

        // �̺�Ʈ ���
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

    // === ���� ��ư ===
    public void OnClickStart() { mainPanel.SetActive(false); modePanel.SetActive(true); }
    public void OnClickRanking() { Debug.Log("��ŷ ���� (���� ����)"); }
    public void OnClickQuitGame() { Application.Quit(); }   // Exit ��� QuitGame�� ����

    // === ��� ���� ��ư ===
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
            Debug.LogWarning("�г����� 1~12���� �ѱ�, ����, ����, �Ϻ� ��ȣ�� �����մϴ�.");
            return;
        }

        GameSettings.PlayerName = inputName;
        namePanel.SetActive(false);

        SceneManager.LoadScene("GameScene");
    }

    // === ���� ���� ===
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;

        if (pausePanel != null) pausePanel.SetActive(true);
        if (settingPanel != null) settingPanel.SetActive(false); // ���� �� ������ �ݱ�
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingPanel != null) settingPanel.SetActive(false); // �簳 �� ������ �ݱ�
    }

    public void OnClickReturnToTitle()
    {
        Time.timeScale = 1f;
        isPaused = false;

        if (pausePanel != null) pausePanel.SetActive(false);
        if (settingPanel != null) settingPanel.SetActive(false);

        SceneManager.LoadScene("TitleScene");
    }

    // === ���� ���� ===
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
