using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NewWaveData", menuName = "Game/Wave Data")]
public class WaveData : ScriptableObject
{
    [System.Serializable]
    public class SpawnData
    {
        public GameObject enemyPrefab;
        public Transform spawnPoint;
        public float delay; // �� �� �ڿ� �����Ǵ���
    }

    public List<SpawnData> spawns;
    public bool isBossWave; // ���� ���̺� ����
}
