using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [Header("Stats")]
    public EnemyType type = EnemyType.Normal;
    public float moveSpeed = 2f;
    public float health = 3f;

    [Header("Fire")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float fireInterval = 1.6f;
    public float bulletSpeed = 7f;

    float _fireTimer;
    float _time; // 이동 패턴 계산용
    Vector2 _startPos;

    void Start()
    {
        _startPos = transform.position;

        // 타입에 따라 기본값 조정
        switch (type)
        {
            case EnemyType.Fast:
                moveSpeed *= 1.8f;
                break;
            case EnemyType.Tank:
                health *= 3f;
                moveSpeed *= 0.6f;
                break;
            case EnemyType.Boss:
                health *= 10f;
                moveSpeed = 1.5f;
                break;
        }
    }

    void Update()
    {
        if (GameManager.Instance.SlowActive) return;

        _time += Time.deltaTime;

        // 이동 패턴
        switch (type)
        {
            case EnemyType.Normal:
            case EnemyType.Tank:
            case EnemyType.Fast:
                transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
                break;

            case EnemyType.Diagonal:
                transform.Translate(new Vector2(-1f, -0.5f).normalized * moveSpeed * Time.deltaTime);
                break;

            case EnemyType.ZigZag:
                {
                    float y = Mathf.Sin(_time * 4f) * 1.5f;
                    transform.position = _startPos + new Vector2(-moveSpeed * _time, y);
                }
                break;

            case EnemyType.Wave:
                {
                    float y = Mathf.Sin(_time * 2f) * 2f;
                    transform.position = _startPos + new Vector2(-moveSpeed * _time, y);
                }
                break;

            case EnemyType.Boss:
                {
                    // 보스는 느리게 전진하면서 위치 고정 (패턴 발사에 집중)
                    if (transform.position.x > 5f)
                        transform.Translate(Vector2.left * moveSpeed * Time.deltaTime);
                }
                break;
        }

        // 공격 (보스는 나중에 패턴 확장 가능)
        HandleAttack();
    }

    void HandleAttack()
    {
        _fireTimer += Time.deltaTime;
        if (_fireTimer >= fireInterval)
        {
            _fireTimer = 0f;

            if (bulletPrefab && firePoint)
            {
                var go = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
                var b = go.GetComponent<BulletBase>();
                b.Init(isPlayerBullet: false, direction: Vector2.left, speed: bulletSpeed, damage: 1f);
            }
        }
    }

    public void TakeDamage(float dmg)
    {
        health -= dmg;
        if (health <= 0f)
        {
            bool isBoss = (type == EnemyType.Boss);
            Destroy(gameObject);
            WaveManager.Instance.OnEnemyKilled(isBoss);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth.Instance.TakeDamage(1);
            bool isBoss = (type == EnemyType.Boss);
            Destroy(gameObject);
            WaveManager.Instance.OnEnemyKilled(isBoss);
        }
    }

}
