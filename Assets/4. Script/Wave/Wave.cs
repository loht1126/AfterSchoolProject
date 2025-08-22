// Assets/4. Script/Wave/Wave.cs
using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Wave
{
    [System.Serializable]
    public class SpawnEntry
    {
        public GameObject enemyPrefab;
        public Vector2 position;
        public float delay; // 웨이브 시작 후 몇 초 뒤 스폰
    }

    public bool isBossWave;
    public List<SpawnEntry> entries = new List<SpawnEntry>();
}
