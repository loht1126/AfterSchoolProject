using UnityEngine;

public class PlayerInputReader : MonoBehaviour
{
    public static PlayerInputReader Instance { get; private set; }

    public Vector2 MoveDir { get; private set; }
    public bool SlowMoveHeld { get; private set; }
    public bool FireHeld { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
    }

    void Update()
    {
        // 이동 입력 (방향키 + WASD)
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        MoveDir = new Vector2(x, y).normalized;

        // 슬로우 모드 (Shift)
        SlowMoveHeld = Input.GetKey(KeyCode.LeftShift);

        // 발사 (Z)
        FireHeld = Input.GetKey(KeyCode.Z);
    }
}
