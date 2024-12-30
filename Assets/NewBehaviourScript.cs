using UnityEngine;
using TMPro; // Добавляем пространство имен для TextMeshPro
using UnityEngine.UI;
using System;

public class ClickerGame : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI scoreText; // Заменяем на TextMeshProUGUI
    [SerializeField] private TextMeshProUGUI clickPowerText;
    [SerializeField] private TextMeshProUGUI autoClickerText;
    [SerializeField] private Button clickButton;
    [SerializeField] private Button upgradeClickButton;
    [SerializeField] private Button buyAutoClickerButton;

    [Header("Game Settings")]
    [SerializeField] private float baseClickPower = 1f;
    [SerializeField] private float clickPowerMultiplier = 1.5f;
    [SerializeField] private float autoClickerBaseCost = 10f;
    [SerializeField] private float upgradeClickCost = 5f;

    private double score;
    private float clickPower;
    private int autoClickers;
    private float nextAutoClickTime;
    private float autoClickInterval = 1f;

    private void Start()
    {
        InitializeGame();
        UpdateUI();
    }

    private void Update()
    {
        HandleAutoClickers();
    }

    private void InitializeGame()
    {
        score = 0;
        clickPower = baseClickPower;
        autoClickers = 0;

        // Настройка кнопок
        clickButton.onClick.AddListener(OnClick);
        upgradeClickButton.onClick.AddListener(UpgradeClickPower);
        buyAutoClickerButton.onClick.AddListener(BuyAutoClicker);
    }

    private void OnClick()
    {
        score += clickPower;
        UpdateUI();
    }

    private void UpgradeClickPower()
    {
        if (score >= upgradeClickCost)
        {
            score -= upgradeClickCost;
            clickPower *= clickPowerMultiplier;
            upgradeClickCost *= 2;
            UpdateUI();
        }
    }

    private void BuyAutoClicker()
    {
        float currentAutoclickerCost = autoClickerBaseCost * Mathf.Pow(1.5f, autoClickers);
        
        if (score >= currentAutoclickerCost)
        {
            score -= currentAutoclickerCost;
            autoClickers++;
            UpdateUI();
        }
    }

    private void HandleAutoClickers()
    {
        if (Time.time >= nextAutoClickTime && autoClickers > 0)
        {
            score += baseClickPower * autoClickers;
            nextAutoClickTime = Time.time + autoClickInterval;
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        // Можно использовать rich text для стилизации
        scoreText.text = $"<color=#FFD700>Score:</color> {Math.Floor(score)}";
        clickPowerText.text = $"<b>Click Power:</b> {clickPower:F1}\n<color=#00FF00>Upgrade Cost:</color> {upgradeClickCost}";
        autoClickerText.text = $"<b>Auto Clickers:</b> {autoClickers}\n<color=#00FF00>Cost:</color> {autoClickerBaseCost * Mathf.Pow(1.5f, autoClickers):F0}";
    }

    private void SaveGame()
    {
        PlayerPrefs.SetString("Score", score.ToString());
        PlayerPrefs.SetFloat("ClickPower", clickPower);
        PlayerPrefs.SetInt("AutoClickers", autoClickers);
        PlayerPrefs.Save();
    }

    private void LoadGame()
    {
        if (PlayerPrefs.HasKey("Score"))
        {
            score = double.Parse(PlayerPrefs.GetString("Score"));
            clickPower = PlayerPrefs.GetFloat("ClickPower");
            autoClickers = PlayerPrefs.GetInt("AutoClickers");
            UpdateUI();
        }
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }
}
