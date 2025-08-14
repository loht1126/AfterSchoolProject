using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("Texts (TMP)")]
    public TMP_Text healthText;
    public TMP_Text timeText;

    [Header("X Skill UI")]
    public TMP_Text skillXNameText;
    public Image skillXIcon;
    public Image skillXCooldownImage;
    public TMP_Text skillXCooldownText;

    [Header("C Skill UI")]
    public TMP_Text skillCNameText;
    public Image skillCIcon;
    public Image skillCCooldownImage;
    public TMP_Text skillCCooldownText;

    [Header("UI Elements")]
    public Slider healthSlider;

    private PlayerHealth _hp;
    private PlayerSkillHandler _skills;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        _hp = FindObjectOfType<PlayerHealth>();
        _skills = FindObjectOfType<PlayerSkillHandler>();

        if (_hp != null)
        {
            _hp.OnHealthChanged += UpdateHealthUI;
            InitHealthUI(_hp.currentHealth, _hp.maxHealth);
        }

        if (_skills != null)
            InitSkillUI();
    }

    void Update()
    {
        if (_hp != null)
            healthText.text = $"HP: {_hp.currentHealth}/{_hp.maxHealth}";

        timeText.text = $"Time: {Format(GameManager.Instance.ElapsedTime)}";

        if (_skills != null)
        {
            UpdateSkillUI(_skills.skillX, skillXCooldownImage, skillXCooldownText, _skills.CooldownRemainingX, _skills.CooldownX);
            UpdateSkillUI(_skills.skillC, skillCCooldownImage, skillCCooldownText, _skills.CooldownRemainingC, _skills.CooldownC);
        }
    }

    #region Health UI
    void InitHealthUI(int current, int max)
    {
        if (healthSlider != null)
        {
            healthSlider.maxValue = max;
            healthSlider.minValue = 0;
            healthSlider.wholeNumbers = true;
            healthSlider.value = current;
        }
    }

    void UpdateHealthUI(int current, int max)
    {
        if (healthSlider != null)
            healthSlider.value = current;
    }
    #endregion

    #region Skill UI
    void InitSkillUI()
    {
        SetupSkillUI(_skills.skillX, skillXNameText, skillXIcon);
        SetupSkillUI(_skills.skillC, skillCNameText, skillCIcon);
    }

    void SetupSkillUI(SkillData skill, TMP_Text nameText, Image icon)
    {
        if (skill != null)
        {
            nameText.text = skill.skillName;
            icon.sprite = skill.icon;
        }
    }

    void UpdateSkillUI(SkillData skill, Image cooldownImage, TMP_Text cooldownText, float remain, float total)
    {
        bool isOnCooldown = (skill != null && remain > 0f && total > 0f);

        cooldownImage.gameObject.SetActive(isOnCooldown);
        cooldownText.gameObject.SetActive(isOnCooldown);

        if (isOnCooldown)
        {
            cooldownImage.fillAmount = remain / total;
            cooldownText.text = $"{remain:0.0}s";
        }
    }
    #endregion

    string Format(float t)
    {
        int m = (int)(t / 60f);
        int s = (int)(t % 60f);
        return $"{m:00}:{s:00}";
    }
}
