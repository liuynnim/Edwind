using UnityEngine;

/// <summary>
/// Điều khiển Camera 2D bám theo nhân vật mượt mà bằng Vector3.SmoothDamp.
/// Chạy trong LateUpdate() để triệt tiêu hiện tượng rung/giật khung hình (jitter).
/// Tích hợp khóa biên giới hạn (bounds) để camera không bị lọt ra ngoài khoảng đen của căn phòng.
/// </summary>
public class CameraFollow2D : MonoBehaviour
{
    [Header("Mục tiêu theo dõi (Target)")]
    [Tooltip("Kéo GameObject Player vào đây.")]
    public Transform target;

    [Header("Độ mượt mà (Smoothing)")]
    [Tooltip("Thời gian trễ lướt theo mục tiêu (giây). Giá trị khoảng 0.15 - 0.3 là đẹp nhất.")]
    public float smoothTime = 0.22f;

    [Tooltip("Độ lệch tâm nhìn so với vị trí nhân vật (trục Z luôn giữ cố định ở -10).")]
    public Vector3 offset = new Vector3(0f, 0.65f, -10f);

    [Header("Giới hạn trong phòng (Room Bounds)")]
    [Tooltip("Bật để khóa camera không trôi ra ngoài viền căn phòng.")]
    public bool useBounds = true;

    [Tooltip("Giới hạn góc máy bên trái.")]
    public float minX = -4.2f;

    [Tooltip("Giới hạn góc máy bên phải.")]
    public float maxX = 4.2f;

    [Tooltip("Giới hạn góc máy bên dưới (mặt sàn).")]
    public float minY = -0.7f;

    [Tooltip("Giới hạn góc máy bên trên (trần nhà).")]
    public float maxY = 0.4f;

    private Vector3 currentVelocity = Vector3.zero;

    private void LateUpdate()
    {
        if (target == null)
        {
            return;
        }

        // 1. Tính toán tọa độ mong muốn dựa trên vị trí nhân vật + độ lệch offset
        Vector3 desiredPosition = target.position + offset;

        // 2. Khóa vị trí trong phạm vi căn phòng nếu bật useBounds
        if (useBounds)
        {
            desiredPosition.x = Mathf.Clamp(desiredPosition.x, minX, maxX);
            desiredPosition.y = Mathf.Clamp(desiredPosition.y, minY, maxY);
        }

        // Luôn đảm bảo trục Z của camera không đổi để nhìn rõ các mặt phẳng 2D
        desiredPosition.z = offset.z;

        // 3. Di chuyển lướt mượt mà tới vị trí mục tiêu
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref currentVelocity, smoothTime);
    }

    private void OnDrawGizmosSelected()
    {
        if (!useBounds) return;

        // Vẽ khung viền giới hạn của camera trong Scene View bằng đường kẻ vàng
        Gizmos.color = new Color(1f, 0.9f, 0.2f, 0.75f);
        Vector3 center = new Vector3((minX + maxX) * 0.5f, (minY + maxY) * 0.5f, 0f);
        Vector3 size = new Vector3(maxX - minX, maxY - minY, 0.1f);
        Gizmos.DrawWireCube(center, size);
    }
}
