using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public static PlayerAttack Instance { get; private set; }

    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 12f;
    public float fireRate = 0.12f; // Z ²Ú ¡æ ÀÚµ¿¿¬»ç
    [HideInInspector] public float damageMultiplier = 1f;

    float _nextFire;

    void Awake() => Instance = this;

    void Update()
    {
        if (Input.GetKey(KeyCode.Z) && Time.time >= _nextFire)
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
        b.Init(isPlayerBullet: true, direction: Vector2.up, speed: bulletSpeed, damage: 1f * damageMultiplier);
    }
}
