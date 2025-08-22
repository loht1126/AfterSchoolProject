using UnityEngine;

public class Spawner : MonoBehaviour
{
    public static Spawner Instance { get; private set; }

    void Awake()
    {
        Instance = this;
    }

    public GameObject SpawnEnemy(GameObject enemyPrefab, Transform spawnPoint)
    {
        return Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
    }
}
