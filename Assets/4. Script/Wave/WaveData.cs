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
        public float delay; // 몇 초 뒤에 스폰되는지
    }

    public List<SpawnData> spawns;
    public bool isBossWave; // 보스 웨이브 여부
}
