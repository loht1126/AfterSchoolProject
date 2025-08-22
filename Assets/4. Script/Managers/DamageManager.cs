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
    /// �������� �����Ѵ�.
    /// </summary>
    /// <param name="target">��� ������Ʈ</param>
    /// <param name="damage">������ ��</param>
    /// <param name="source">������ (�÷��̾�, ��, ��ų ��)</param>
    public void ApplyDamage(GameObject target, float damage, GameObject source = null)
    {
        if (target == null) return;

        // Player
        if (target.CompareTag("Player"))
        {
            PlayerHealth.Instance.TakeDamage(Mathf.RoundToInt(damage));
            Debug.Log($"[DamageManager] Player�� {damage} ���ظ� ���� (by {source?.name ?? "Unknown"})");
            return;
        }

        // Enemy
        EnemyController enemy = target.GetComponent<EnemyController>();
        if (enemy != null)
        {
            enemy.TakeDamage(damage);
            Debug.Log($"[DamageManager] {enemy.name}�� {damage} ���ظ� ���� (by {source?.name ?? "Unknown"})");
            return;
        }
    }
}
