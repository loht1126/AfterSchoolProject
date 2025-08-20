using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    [System.Serializable]
    public class SpawnData
    {
        public GameObject enemyPrefab;
        public Vector2 position;
        public float time; // ���� �� �� �ʿ� ��ȯ
    }

    [Tooltip("�������� ���� ���� (�ð�, ��ġ, ������). �ð� �������� ��õ")]
    public List<SpawnData> pattern = new List<SpawnData>();

    float _timer;

    void Update()
    {
        if (GameManager.Instance.IsGameOver) return;
        if (!GameManager.Instance.SlowActive) _timer += Time.deltaTime; // ���ο� �߿� ����

        // �ڿ��� ������ ����
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
