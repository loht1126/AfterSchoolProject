using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float moveSpeed = 2f;
    public float health = 3f;

    [Header("Fire")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireInterval = 1.6f;
    public float bulletSpeed = 7f;

    float _fireTimer;

    void Update()
    {
        if (GameManager.Instance.SlowActive) return; // 슬로우 동안 정지

        transform.Translate(Vector2.down * moveSpeed * Time.deltaTime);

        _fireTimer += Time.deltaTime;
        if (_fireTimer >= fireInterval)
        {
            _fireTimer = 0f;
            if (bulletPrefab && firePoint)
            {
                var go = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                var b = go.GetComponent<BulletBase>();
                b.Init(isPlayerBullet: false, direction: Vector2.down, speed: bulletSpeed, damage: 1f);
            }
        }
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0f) Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 플레이어와 몸통 충돌
        if (other.CompareTag("Player"))
        {
            PlayerHealth.Instance.TakeDamage(1);
            Destroy(gameObject);
        }
    }
}
