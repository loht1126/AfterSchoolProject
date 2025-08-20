using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 6f;
    [Range(0.1f, 1f)] public float slowFactor = 0.4f;

    [Header("�̵� ����")]
    public float maxY = 7.1f; // ���� �ִ� y��ǥ
    public float boundaryOffset = 0.5f; // ī�޶� ���� ������
    Camera mainCam;

    void Awake()
    {
        mainCam = Camera.main;
    }

    void Update()
    {
        var input = PlayerInputReader.Instance;
        if (input == null) return;

        Vector2 dir = input.MoveDir;
        float speed = moveSpeed * (input.SlowMoveHeld ? slowFactor : 1f);

        transform.Translate(dir * speed * Time.deltaTime, Space.World);

        // ��� ����
        Vector3 p = transform.position;

        // ī�޶� ����Ʈ �� ���� ��ǥ ��ȯ
        Vector3 minWorld = mainCam.ViewportToWorldPoint(new Vector3(0, 0, mainCam.nearClipPlane));
        Vector3 maxWorld = mainCam.ViewportToWorldPoint(new Vector3(1, 1, mainCam.nearClipPlane));

        // offset ����
        float minX = minWorld.x + boundaryOffset;
        float maxX = maxWorld.x - boundaryOffset;
        float minY = minWorld.y + boundaryOffset;

        // �¿�, �Ʒ��� ī�޶� ���� (+offset)
        p.x = Mathf.Clamp(p.x, minX, maxX);
        p.y = Mathf.Clamp(p.y, minY, maxY); // ������ ī�޶� ��� ������

        transform.position = p;
    }
}
