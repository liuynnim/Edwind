using UnityEngine;

/// <summary>
/// Quản lý chiếc ghế Sofa có thể tương tác để nhân vật ngồi vào.
/// Ghế được tách làm 2 lớp Sprite (Sofa_Back ở phía sau nhân vật và Sofa_FrontArm ở phía trước nhân vật)
/// để tạo hiệu ứng chiều sâu 2.5D khi nhân vật ngồi lọt vào lòng ghế.
/// </summary>
public class SofaInteractable : MonoBehaviour
{
    [Header("Điểm neo khi ngồi (Sit Anchor)")]
    [Tooltip("Vị trí trên mặt đệm ghế mà nhân vật sẽ dịch chuyển vào khi ngồi.")]
    public Transform sitAnchor;

    [Header("Thiết lập tương tác")]
    [Tooltip("Khoảng cách tối đa tính từ ghế để nhân vật có thể bấm phím ngồi.")]
    public float interactRange = 1.6f;

    [Tooltip("Hướng mặt của nhân vật khi ngồi (True = quay mặt sang trái về phía lò sưởi).")]
    public bool faceLeftWhenSitting = true;

    /// <summary>
    /// Kiểm tra xem nhân vật có đang đứng đủ gần ghế để tương tác hay không.
    /// </summary>
    public bool IsPlayerInRange(Vector3 playerPosition)
    {
        float horizontalDistance = Mathf.Abs(playerPosition.x - transform.position.x);
        return horizontalDistance <= interactRange;
    }

    /// <summary>
    /// Lấy tọa độ thế giới (World Position) của điểm ngồi trên ghế.
    /// </summary>
    public Vector3 GetSitPosition()
    {
        if (sitAnchor != null)
        {
            return sitAnchor.position;
        }
        return transform.position + new Vector3(0.08f, 0.34f, 0f);
    }

    private void OnDrawGizmosSelected()
    {
        // Vẽ vòng tròn phạm vi tương tác và điểm ngồi trong Scene View để dễ căn chỉnh
        Gizmos.color = new Color(1f, 0.6f, 0.2f, 0.8f);
        Gizmos.DrawWireSphere(transform.position, interactRange);

        Vector3 sitPos = GetSitPosition();
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(sitPos, 0.08f);
    }
}
