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

        // ���� ���� (�÷��̾� �� ������, �� �� ����)
        this.direction = isPlayerBullet ? Vector2.right : Vector2.left;

        // �ʱⰪ ����
        this.speed = speed;
        this.damage = damage;
    }

    private void Update()
    {
        // ���� üũ
        _lifeTimer += Time.unscaledDeltaTime;
        if (_lifeTimer >= lifeTime)
        {
            Destroy(gameObject);
            return;
        }

        // ���ο� ���� ó��
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
            return; // �� ź�� ����
        }

        // �Ϲ� �̵�
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
