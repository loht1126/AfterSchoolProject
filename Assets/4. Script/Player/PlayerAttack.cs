using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance { get; private set; }

    [Header("Bullet Settings")]
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float baseBulletSpeed = 12f;
    public float baseDamage = 1f;

    [Header("Multipliers (affected by skills)")]
    public float damageMultiplier = 1f;
    public float speedMultiplier = 1f;

    [Header("Fire Control")]
    public float fireRate = 0.12f;

    private float _nextFire;

    void Awake() => Instance = this;

    void Update()
    {
        var input = PlayerInputReader.Instance;
        if (input == null) return;

        if (input.FireHeld && Time.time >= _nextFire)
        {
            _nextFire = Time.time + fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (!bulletPrefab || !firePoint) return;

        var go = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        var b = go.GetComponent<BulletBase>();

        // 현재 스킬 효과가 반영된 값 전달
        float finalSpeed = baseBulletSpeed * speedMultiplier;
        float finalDamage = baseDamage * damageMultiplier;

        b.Init(true, Vector2.right, finalSpeed, finalDamage);
    }
}
