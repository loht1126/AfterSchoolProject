using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool IsGameOver { get; private set; }
    public float ElapsedTime { get; private set; }

    // ���ο�(����) ��ų ����: �÷��̾ ����, ������ ����
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
