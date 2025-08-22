// Assets/4. Script/Wave/WaveManager.cs
using UnityEngine;
using System.Collections;


public class WaveManager : MonoBehaviour
{
    public static WaveManager Instance { get; private set; }

    [Header("Mode")]
    public GameMode gameMode = GameMode.Normal;
    public int bossClearToWin = 3;      // Normal 모드에서 게임 클리어 조건

    [Header("Random Wave")]
    public GameObject[] enemyPrefabs;   // 일반 적 프리팹들(타입별 프리팹을 넣어둠)
    public GameObject bossPrefab;       // 보스 프리팹
    public int bossWaveInterval = 5;    // n웨이브마다 보스
    public float nextWaveDelay = 3f;    // 웨이브 종료 후 대기

    [Header("Spawn Area")]
    public float spawnX = 10f;          // 화면 오른쪽 바깥 X
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
            Debug.Log("튜토리얼 모드 시작 (추후 구현)");

        StartCoroutine(StartNextWave());
    }


    IEnumerator StartNextWave()
    {
        _spawning = true;
        _currentWaveIndex++;
        Debug.Log($"[WaveManager] Wave {_currentWaveIndex} 시작");

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
            Debug.LogWarning("[WaveManager] enemyPrefabs 비어있음");
            return;
        }

        // 보스 클리어 횟수에 따른 적 수 범위
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
            Debug.Log($"[WaveManager] 보스 처치! 누적: {_bossClearCount}");
        }

        // 웨이브 종료 판단
        if (_aliveEnemies <= 0 && !_spawning)
            StartCoroutine(WaitAndStartNext());
    }

    IEnumerator WaitAndStartNext()
    {
        Debug.Log("[WaveManager] Wave Cleared");
        yield return new WaitForSeconds(nextWaveDelay);

        // Normal 모드 클리어 조건
        if (gameMode == GameMode.Normal && _bossClearCount >= bossClearToWin)
        {
            GameManager.Instance.GameClear();
            yield break;
        }

        // Endless/Hardcore(=하드코어+일반 플로우) 계속 진행
        yield return StartNextWave();
    }
}
