using UnityEngine;

public class BulletBase : MonoBehaviour
{
    [Header("Runtime")]
    public bool isPlayerBullet;
    public Vector2 direction;
    public float speed;
    public float damage;

    [Header("Common")]
    public float lifeTime = 4f;

    private float _lifeTimer;
    private float _movedWhileSlow;

    public void Init(bool isPlayerBullet, Vector2 direction, float speed, float damage)
    {
        this.isPlayerBullet = isPlayerBullet;

        // 방향 강제 (플레이어 → 오른쪽, 적 → 왼쪽)
        this.direction = isPlayerBullet ? Vector2.right : Vector2.left;

        // 초기값 복사
        this.speed = speed;
        this.damage = damage;
    }

    private void Update()
    {
        // 수명 체크
        _lifeTimer += Time.unscaledDeltaTime;
        if (_lifeTimer >= lifeTime)
        {
            Destroy(gameObject);
            return;
        }

        // 슬로우 상태 처리
        if (GameManager.Instance.SlowActive)
        {
            if (isPlayerBullet)
            {
                if (_movedWhileSlow < 0.5f)
                {
                    float dt = Time.unscaledDeltaTime;
                    transform.Translate(direction * speed * dt, Space.World);
                    _movedWhileSlow += dt;
                }
            }
            return; // 적 탄은 정지
        }

        // 일반 이동
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isPlayerBullet && other.CompareTag("Enemy"))
        {
            DamageManager.Instance.ApplyDamage(other.gameObject, damage, gameObject);
            Destroy(gameObject);
        }
        else if (!isPlayerBullet && other.CompareTag("Player"))
        {
            DamageManager.Instance.ApplyDamage(other.gameObject, damage, gameObject);
            Destroy(gameObject);
        }
    }

}
