using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 6f;
    [Range(0.1f, 1f)] public float slowFactor = 0.4f;

    [Header("이동 제한")]
    public float maxY = 7.1f; // 위쪽 최대 y좌표
    public float boundaryOffset = 0.5f; // 카메라 범위 보정값
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

        // 경계 제한
        Vector3 p = transform.position;

        // 카메라 뷰포트 → 월드 좌표 변환
        Vector3 minWorld = mainCam.ViewportToWorldPoint(new Vector3(0, 0, mainCam.nearClipPlane));
        Vector3 maxWorld = mainCam.ViewportToWorldPoint(new Vector3(1, 1, mainCam.nearClipPlane));

        // offset 적용
        float minX = minWorld.x + boundaryOffset;
        float maxX = maxWorld.x - boundaryOffset;
        float minY = minWorld.y + boundaryOffset;

        // 좌우, 아래는 카메라 범위 (+offset)
        p.x = Mathf.Clamp(p.x, minX, maxX);
        p.y = Mathf.Clamp(p.y, minY, maxY); // 위쪽은 카메라 대신 고정값

        transform.position = p;
    }
}
