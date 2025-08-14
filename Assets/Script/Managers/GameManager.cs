using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameOver { get; private set; }
    public float ElapsedTime { get; private set; }

    // 슬로우(정지) 스킬 상태: 플레이어만 정상, 나머지 정지
    public bool SlowActive { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (IsGameOver) return;
        ElapsedTime += Time.deltaTime;
    }

    public void GameOver()
    {
        if (IsGameOver) return;
        IsGameOver = true;
        Debug.Log("[GameManager] Game Over");
    }

    public void StartSlow(float duration)
    {
        if (SlowActive) return;
        StartCoroutine(SlowRoutine(duration));
    }

    IEnumerator SlowRoutine(float duration)
    {
        SlowActive = true;
        yield return new WaitForSecondsRealtime(duration);
        SlowActive = false;
    }
}
