using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
/// Điều khiển nhân vật 2D đi ngang (Trái/Phải) trên sàn nhà, chạy hoạt ảnh Walk Cycle / Idle qua Animator,
/// tương tác ngồi vào ghế Sofa và mở giao diện Cuốn Sách Menu (BookMenuUI).
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class PlayerController2D : MonoBehaviour
{
    private static readonly int IsWalkingHash = Animator.StringToHash("IsWalking");
    private static readonly int IsSittingHash = Animator.StringToHash("IsSitting");

    [Header("Di chuyển (Movement)")]
    [Tooltip("Tốc độ đi bộ ngang trên sàn nhà (đơn vị Unity / giây).")]
    public float moveSpeed = 3.5f;

    [Tooltip("Giới hạn đi lại bên trái và bên phải của căn phòng.")]
    public float minX = -7.5f;
    public float maxX = 7.5f;

    [Tooltip("Giới hạn di chuyển lên và xuống theo trục Y.")]
    public float minY = -2.6f;
    public float maxY = -1f;

    [Header("Hình ảnh & Hoạt ảnh (Sprites & Walk Cycle)")]
    [Tooltip("Sprite mặc định khi nhân vật đứng yên.")]
    public Sprite standingSprite;

    [Tooltip("Sprite khi nhân vật ngồi trên ghế Sofa.")]
    public Sprite sittingSprite;

    [Tooltip("Chuỗi 4 frame bước chân (player_walk_sheet_1 .. 4).")]
    public Sprite[] walkFrames;

    [Tooltip("Chuỗi 2 frame thở nhẹ khi đứng yên (player_walk_sheet_0 & 5).")]
    public Sprite[] idleFrames;

    [Tooltip("Tốc độ chuyển frame khi đi bộ (số frame / giây).")]
    public float walkFrameRate = 8f;

    [Tooltip("Tốc độ chuyển frame khi đứng thở nhẹ (số frame / giây).")]
    public float idleFrameRate = 2f;

    [Header("Tương tác ghế Sofa & Cuốn Sách Menu")]
    [Tooltip("Tham chiếu đến chiếc ghế Sofa trong phòng.")]
    public SofaInteractable targetSofa;

    [Tooltip("Tham chiếu đến giao diện Cuốn Sách mở ra khi ngồi xuống ghế.")]
    public BookMenuUI bookMenu;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private bool isSitting;
    private bool isWalking;
    private float animTimer;
    private int currentFrameIndex;
    private Vector3 initialSpawnPosition;
    private Vector3 standingReturnPosition;
    private GUIStyle promptStyle;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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
            SetMovementAnimationState(false);
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

        // Đọc phím di chuyển dọc (W/S hoặc Mũi tên Lên/Xuống)
        float verticalInput = 0f;
        if (keyboard.wKey.isPressed || keyboard.upArrowKey.isPressed)
        {
            verticalInput += 1f;
        }
        if (keyboard.sKey.isPressed || keyboard.downArrowKey.isPressed)
        {
            verticalInput -= 1f;
        }

        // Nếu đang ngồi trên ghế (và đã đóng sách bằng nút "Chơi Tiếp")
        if (isSitting)
        {
            SetMovementAnimationState(false);
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

        // Di chuyển trong giới hạn X/Y
        bool movingNow = Mathf.Abs(moveInput) > 0.01f || Mathf.Abs(verticalInput) > 0.01f;
        if (movingNow)
        {
            Vector3 pos = transform.position;
            pos.x = Mathf.Clamp(pos.x + moveInput * moveSpeed * Time.deltaTime, minX, maxX);
            pos.y = Mathf.Clamp(pos.y + verticalInput * moveSpeed * Time.deltaTime, minY, maxY);
            transform.position = pos;

            // Lật hướng mặt nhân vật: mặc định Sprite nhìn sang phải (flipX = false),
            // khi đi sang trái thì đặt flipX = true.
            if (Mathf.Abs(moveInput) > 0.01f)
            {
                spriteRenderer.flipX = moveInput < 0f;
            }
        }

        SetMovementAnimationState(movingNow);
        UpdateFallbackSpriteAnimation();
    }

    /// <summary>
    /// Cập nhật biến Bool trên Animator ("IsWalking" và "IsSitting").
    /// </summary>
    private void SetMovementAnimationState(bool walking)
    {
        if (isWalking != walking)
        {
            isWalking = walking;
            animTimer = 0f;
            currentFrameIndex = 0;
        }

        if (animator != null && animator.runtimeAnimatorController != null)
        {
            animator.SetBool(IsWalkingHash, isWalking);
            animator.SetBool(IsSittingHash, isSitting);
        }
    }

    /// <summary>
    /// Đảm bảo hoạt ảnh bước chân / đứng yên vẫn chạy mượt ngay cả khi Animator chưa bật.
    /// </summary>
    private void UpdateFallbackSpriteAnimation()
    {
        if (animator != null && animator.enabled && animator.runtimeAnimatorController != null)
        {
            return;
        }

        Sprite[] activeFrames = isWalking ? walkFrames : idleFrames;
        float fps = isWalking ? walkFrameRate : idleFrameRate;

        if (activeFrames == null || activeFrames.Length == 0)
        {
            return;
        }

        animTimer += Time.deltaTime;
        float interval = 1f / Mathf.Max(1f, fps);
        if (animTimer >= interval)
        {
            animTimer -= interval;
            currentFrameIndex = (currentFrameIndex + 1) % activeFrames.Length;
            if (activeFrames[currentFrameIndex] != null)
            {
                spriteRenderer.sprite = activeFrames[currentFrameIndex];
            }
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
        SetMovementAnimationState(false);

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
        SetMovementAnimationState(false);

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
        SetMovementAnimationState(false);
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
