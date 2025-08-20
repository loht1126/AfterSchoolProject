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
        // �̵� �Է� (����Ű + WASD)
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        MoveDir = new Vector2(x, y).normalized;

        // ���ο� ��� (Shift)
        SlowMoveHeld = Input.GetKey(KeyCode.LeftShift);

        // �߻� (Z)
        FireHeld = Input.GetKey(KeyCode.Z);
    }
}
