using UnityEngine;

public class DamageManager : MonoBehaviour
{
    public static DamageManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// 데미지를 적용한다.
    /// </summary>
    /// <param name="target">대상 오브젝트</param>
    /// <param name="damage">데미지 값</param>
    /// <param name="source">공격자 (플레이어, 적, 스킬 등)</param>
    public void ApplyDamage(GameObject target, float damage, GameObject source = null)
    {
        if (target == null) return;

        // Player
        if (target.CompareTag("Player"))
        {
            PlayerHealth.Instance.TakeDamage(Mathf.RoundToInt(damage));
            Debug.Log($"[DamageManager] Player가 {damage} 피해를 받음 (by {source?.name ?? "Unknown"})");
            return;
        }

        // Enemy
        EnemyController enemy = target.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log($"[DamageManager] {enemy.name}이 {damage} 피해를 받음 (by {source?.name ?? "Unknown"})");
            return;
        }
    }
}
