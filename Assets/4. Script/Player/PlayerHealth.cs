using UnityEngine;
using System;

public class PlayerHealth : MonoBehaviour
{
    public static PlayerHealth Instance { get; private set; }

    public int maxHealth = 3;
    public int currentHealth;
    public bool isInvincible;

    public event Action<int, int> OnHealthChanged; // (현재, 최대)

    void Awake()
    {
        Instance = this;
        currentHealth = maxHealth;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }

    public void TakeDamage(int dmg)
    {
        if (isInvincible || GameManager.Instance.IsGameOver) return;
        currentHealth -= dmg;
        if (currentHealth < 0) currentHealth = 0;

        OnHealthChanged?.Invoke(currentHealth, maxHealth);

        if (currentHealth <= 0)
            GameManager.Instance.GameOver();
    }

    // 하드코어 모드 → HP를 1로 고정
    public void SetHardcoreMode()
    {
        maxHealth = 1;
        currentHealth = 1;
        OnHealthChanged?.Invoke(currentHealth, maxHealth);
    }
}
