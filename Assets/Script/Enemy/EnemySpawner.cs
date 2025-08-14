using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnData
    {
        public GameObject enemyPrefab;
        public Vector2 position;
        public float time; // 시작 후 몇 초에 소환
    }

    [Tooltip("스테이지 고정 패턴 (시간, 위치, 프리팹). 시간 오름차순 추천")]
    public List<SpawnData> pattern = new List<SpawnData>();

    float _timer;

    void Update()
    {
        if (GameManager.Instance.IsGameOver) return;
        if (!GameManager.Instance.SlowActive) _timer += Time.deltaTime; // 슬로우 중엔 정지

        // 뒤에서 앞으로 제거
        for (int i = pattern.Count - 1; i >= 0; i--)
        {
            if (_timer >= pattern[i].time)
            {
                Instantiate(pattern[i].enemyPrefab, pattern[i].position, Quaternion.identity);
                pattern.RemoveAt(i);
            }
        }
    }
}
