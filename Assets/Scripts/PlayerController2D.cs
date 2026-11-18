using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Điều khiển nhân vật 2D đi ngang (Trái/Phải) trên sàn nhà, tương tác ngồi vào ghế Sofa
/// và mở giao diện Cuốn Sách Menu (BookMenuUI).
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController2D : MonoBehaviour
{
    [Header("Di chuyển (Movement)")]
    [Tooltip("Tốc độ đi bộ ngang trên sàn nhà (đơn vị Unity / giây).")]
    public float moveSpeed = 3.5f;

    [Tooltip("Giới hạn đi lại bên trái và bên phải của căn phòng.")]
    public float minX = -7.5f;
    public float maxX = 7.5f;

    [Header("Hình ảnh nhân vật (Sprites)")]
    [Tooltip("Sprite khi nhân vật đứng hoặc đi lại trên sàn.")]
    public Sprite standingSprite;

    [Tooltip("Sprite khi nhân vật ngồi trên ghế Sofa.")]
    public Sprite sittingSprite;

    [Header("Tương tác ghế Sofa & Cuốn Sách Menu")]
    [Tooltip("Tham chiếu đến chiếc ghế Sofa trong phòng.")]
    public SofaInteractable targetSofa;

    [Tooltip("Tham chiếu đến giao diện Cuốn Sách mở ra khi ngồi xuống ghế.")]
    public BookMenuUI bookMenu;

    private SpriteRenderer spriteRenderer;
    private bool isSitting;
    private Vector3 initialSpawnPosition;
    private Vector3 standingReturnPosition;
    private GUIStyle promptStyle;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        initialSpawnPosition = transform.position;

        if (standingSprite != null)
        {
            spriteRenderer.sprite = standingSprite;
        }

        if (bookMenu == null)
        {
            bookMenu = FindFirstObjectByType<BookMenuUI>();
        }
    }

    private void Update()
    {
        var keyboard = Keyboard.current;
        if (keyboard == null)
        {
            return;
        }

        // Nếu cuốn sách đang mở trên màn hình -> nhường quyền điều khiển cho giao diện sách
        if (bookMenu != null && bookMenu.IsBookOpen)
        {
            return;
        }

        bool interactPressed = keyboard.eKey.wasPressedThisFrame || keyboard.spaceKey.wasPressedThisFrame;

        // Đọc phím di chuyển ngang (A/D hoặc Mũi tên Trái/Phải)
        float moveInput = 0f;
        if (keyboard.aKey.isPressed || keyboard.leftArrowKey.isPressed)
        {
            moveInput -= 1f;
        }
        if (keyboard.dKey.isPressed || keyboard.rightArrowKey.isPressed)
        {
            moveInput += 1f;
        }

        // Nếu đang ngồi trên ghế (và đã đóng sách bằng nút "Chơi Tiếp")
        if (isSitting)
        {
            if (interactPressed)
            {
                StandUp();
            }
            return;
        }

        // Nếu đang đứng trên sàn và bấm E / Space gần ghế Sofa -> Ngồi vào ghế & mở cuốn sách
        if (interactPressed && targetSofa != null && targetSofa.IsPlayerInRange(transform.position))
        {
            SitDown();
            return;
        }

        // Di chuyển ngang Trái/Phải trên sàn
        if (Mathf.Abs(moveInput) > 0.01f)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x + moveInput * moveSpeed * Time.deltaTime, minX, maxX);
            transform.position = pos;

            // Lật hướng mặt nhân vật: mặc định Sprite nhìn sang phải (flipX = false),
            // khi đi sang trái thì đặt flipX = true.
            spriteRenderer.flipX = moveInput < 0f;
        }
    }

    /// <summary>
    /// Đưa nhân vật vào tư thế ngồi trên đệm ghế (nằm giữa lớp Sofa_Back và Sofa_FrontArm)
    /// và kích hoạt mở giao diện Cuốn Sách.
    /// </summary>
    private void SitDown()
    {
        isSitting = true;
        standingReturnPosition = transform.position;

        // Dịch chuyển nhân vật vào đúng điểm neo trên đệm ghế
        transform.position = targetSofa.GetSitPosition();

        // Đổi sang Sprite tư thế ngồi và quay mặt về phía lò sưởi (bên trái)
        if (sittingSprite != null)
        {
            spriteRenderer.sprite = sittingSprite;
        }
        spriteRenderer.flipX = targetSofa.faceLeftWhenSitting;

        // Mở giao diện cuốn sách sau một nhịp ngắn
        if (bookMenu != null)
        {
            bookMenu.OpenBookWithDelay();
        }
    }

    /// <summary>
    /// Đưa nhân vật đứng dậy trở lại mặt sàn trước ghế.
    /// </summary>
    private void StandUp()
    {
        isSitting = false;
        if (bookMenu != null)
        {
            bookMenu.CloseBook();
        }

        float standX = targetSofa != null ? targetSofa.transform.position.x - 1.15f : standingReturnPosition.x;
        transform.position = new Vector3(standX, standingReturnPosition.y, standingReturnPosition.z);

        if (standingSprite != null)
        {
            spriteRenderer.sprite = standingSprite;
        }
        spriteRenderer.flipX = true;
    }

    /// <summary>
    /// Được gọi từ nút "Gấp Sách" trong BookMenuUI.
    /// </summary>
    public void StandUpFromBook()
    {
        if (isSitting)
        {
            StandUp();
        }
    }

    /// <summary>
    /// Được gọi từ nút "Chơi Mới" trong BookMenuUI: đưa nhân vật đứng dậy và quay về vị trí xuất phát đầu phòng.
    /// </summary>
    public void StartNewGameFromBook()
    {
        isSitting = false;
        transform.position = initialSpawnPosition;

        if (standingSprite != null)
        {
            spriteRenderer.sprite = standingSprite;
        }
        spriteRenderer.flipX = false;
    }

    private void OnGUI()
    {
        if (targetSofa == null)
        {
            return;
        }

        // Ẩn dòng nhắc dưới đáy màn hình khi cuốn sách đang mở
        if (bookMenu != null && bookMenu.IsBookOpen)
        {
            return;
        }

        bool canInteract = isSitting || targetSofa.IsPlayerInRange(transform.position);
        if (!canInteract)
        {
            return;
        }

        if (promptStyle == null)
        {
            promptStyle = new GUIStyle(GUI.skin.box)
            {
                fontSize = 16,
                alignment = TextAnchor.MiddleCenter,
                normal = { textColor = Color.white }
            };
        }

        string message = isSitting
            ? "Đang ngồi sưởi ấm - Nhấn [E] hoặc [Space] để đứng dậy"
            : "Nhấn [E] hoặc [Space] để ngồi vào ghế Sofa & mở Sách";

        float width = 460f;
        float height = 36f;
        Rect rect = new Rect((Screen.width - width) * 0.5f, Screen.height - 60f, width, height);
        GUI.Box(rect, message, promptStyle);
    }
}
