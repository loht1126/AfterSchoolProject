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
        public float delay; // ���̺� ���� �� �� �� �� ����
    }

    public bool isBossWave;
    public List<SpawnEntry> entries = new List<SpawnEntry>();
}
