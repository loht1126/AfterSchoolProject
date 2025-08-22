// Assets/4. Script/Wave/WaveManager.cs
using UnityEngine;
using System.Collections;


public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [Header("Mode")]
    public GameMode gameMode = GameMode.Normal;
    public int bossClearToWin = 3;      // Normal ��忡�� ���� Ŭ���� ����

    [Header("Random Wave")]
    public GameObject[] enemyPrefabs;   // �Ϲ� �� �����յ�(Ÿ�Ժ� �������� �־��)
    public GameObject bossPrefab;       // ���� ������
    public int bossWaveInterval = 5;    // n���̺긶�� ����
    public float nextWaveDelay = 3f;    // ���̺� ���� �� ���

    [Header("Spawn Area")]
    public float spawnX = 10f;          // ȭ�� ������ �ٱ� X
    public float spawnYMin = -3.5f;
    public float spawnYMax = 3.5f;

    int _currentWaveIndex = 0;
    int _aliveEnemies = 0;
    int _bossClearCount = 0;
    bool _spawning = false;

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Start()
    {
        gameMode = GameSettings.SelectedGameMode;

        if (GameSettings.Hardcore)
            PlayerHealth.Instance.SetHardcoreMode();

        if (GameSettings.Tutorial)
            Debug.Log("Ʃ�丮�� ��� ���� (���� ����)");

        StartCoroutine(StartNextWave());
    }


    IEnumerator StartNextWave()
    {
        _spawning = true;
        _currentWaveIndex++;
        Debug.Log($"[WaveManager] Wave {_currentWaveIndex} ����");

        if (IsBossWave(_currentWaveIndex))
        {
            SpawnEnemy(bossPrefab, new Vector2(spawnX, 0f));
        }
        else
        {
            GenerateRandomWaveByBossClears();
        }

        _spawning = false;
        yield break;
    }

    bool IsBossWave(int waveIndex)
    {
        return bossWaveInterval > 0 && (waveIndex % bossWaveInterval == 0);
    }

    void GenerateRandomWaveByBossClears()
    {
        if (enemyPrefabs == null || enemyPrefabs.Length == 0)
        {
            Debug.LogWarning("[WaveManager] enemyPrefabs �������");
            return;
        }

        // ���� Ŭ���� Ƚ���� ���� �� �� ����
        int minCount, maxCount;
        if (_bossClearCount <= 0) { minCount = 1; maxCount = 10; }
        else if (_bossClearCount == 1) { minCount = 10; maxCount = 15; }
        else { minCount = 15; maxCount = 20; }

        int count = Random.Range(minCount, maxCount + 1);

        for (int i = 0; i < count; i++)
        {
            var prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            var pos = new Vector2(spawnX, Random.Range(spawnYMin, spawnYMax));
            SpawnEnemy(prefab, pos);
        }
    }

    void SpawnEnemy(GameObject prefab, Vector2 position)
    {
        if (!prefab) return;
        Instantiate(prefab, position, Quaternion.identity);
        _aliveEnemies++;
    }

    public void OnEnemyKilled(bool isBoss = false)
    {
        _aliveEnemies--;

        if (isBoss)
        {
            _bossClearCount++;
            Debug.Log($"[WaveManager] ���� óġ! ����: {_bossClearCount}");
        }

        // ���̺� ���� �Ǵ�
        if (_aliveEnemies <= 0 && !_spawning)
            StartCoroutine(WaitAndStartNext());
    }

    IEnumerator WaitAndStartNext()
    {
        Debug.Log("[WaveManager] Wave Cleared");
        yield return new WaitForSeconds(nextWaveDelay);

        // Normal ��� Ŭ���� ����
        if (gameMode == GameMode.Normal && _bossClearCount >= bossClearToWin)
        {
            GameManager.Instance.GameClear();
            yield break;
        }

        // Endless/Hardcore(=�ϵ��ھ�+�Ϲ� �÷ο�) ��� ����
        yield return StartNextWave();
    }
}
