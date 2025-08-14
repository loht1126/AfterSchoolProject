using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Move")]
    public float moveSpeed = 6f;
    [Range(0.1f, 1f)] public float slowFactor = 0.4f;
    [Tooltip("월드 좌표 기준 이동 가능 영역")]
    public Rect moveBounds = new Rect(-3f, -5f, 6f, 9f); // x,y=좌하단, w,h=폭,높이

    void Update()
    {
        float x = (Input.GetKey(KeyCode.LeftArrow) ? -1 : 0) + (Input.GetKey(KeyCode.RightArrow) ? 1 : 0);
        float y = (Input.GetKey(KeyCode.DownArrow) ? -1 : 0) + (Input.GetKey(KeyCode.UpArrow) ? 1 : 0);
        Vector2 dir = new Vector2(x, y).normalized;

        float speed = moveSpeed * (Input.GetKey(KeyCode.LeftShift) ? slowFactor : 1f);
        // 슬로우 중에도 플레이어는 정상 작동
        float dt = Time.deltaTime;
        transform.Translate(dir * speed * dt, Space.World);

        // 경계 클램프
        Vector3 p = transform.position;
        float minX = moveBounds.xMin, maxX = moveBounds.xMax;
        float minY = moveBounds.yMin, maxY = moveBounds.yMax;
        p.x = Mathf.Clamp(p.x, minX, maxX);
        p.y = Mathf.Clamp(p.y, minY, maxY);
        transform.position = p;
    }
}
