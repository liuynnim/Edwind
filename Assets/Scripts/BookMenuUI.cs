using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

/// <summary>
/// Quản lý giao diện Cuốn Sách Menu hiển thị khi nhân vật ngồi vào ghế Sofa.
/// - Nửa bên trái của cuốn sách: Mặt bìa trong (Inside Cover).
/// - Nửa bên phải của cuốn sách: Mặt trang giấy (Paper Page) chứa các nút "Chơi Tiếp", "Chơi Mới", "Gấp Sách".
/// </summary>
public class BookMenuUI : MonoBehaviour
{
    [Header("Tài nguyên & Tham chiếu")]
    [Tooltip("Sprite hình cuốn sách mở (book_menu.png).")]
    public Sprite bookSprite;

    [Tooltip("Tham chiếu đến script điều khiển nhân vật.")]
    public PlayerController2D playerController;

    [Header("Hiệu ứng mở sách")]
    [Tooltip("Độ trễ ngắn (giây) sau khi nhân vật ngồi xuống ghế trước khi mở sách.")]
    public float openDelay = 0.25f;

    [Tooltip("Tốc độ hiệu ứng mờ dần / phóng to khi mở sách.")]
    public float fadeSpeed = 6.0f;

    private Canvas menuCanvas;
    private CanvasGroup canvasGroup;
    private RectTransform bookRect;
    private Text statusToastText;
    private float statusToastTimer;

    private bool isTargetOpen;
    private float openDelayTimer;

    /// <summary>
    /// Trạng thái cuốn sách có đang hiển thị trên màn hình hay không.
    /// </summary>
    public bool IsBookOpen => isTargetOpen;

    private void Awake()
    {
        BuildBookCanvasUI();
        SetBookImmediate(false);
    }

    private void Update()
    {
        // Đếm ngược độ trễ mở sách sau khi ngồi
        if (openDelayTimer > 0f)
        {
            openDelayTimer -= Time.deltaTime;
            if (openDelayTimer <= 0f)
            {
                isTargetOpen = true;
                menuCanvas.enabled = true;
            }
        }

        // Hiệu ứng chuyển động mở / gấp sách mượt mà
        if (menuCanvas != null && menuCanvas.enabled)
        {
            float targetAlpha = isTargetOpen ? 1f : 0f;
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, fadeSpeed * Time.deltaTime);

            float scale = Mathf.Lerp(0.88f, 1.0f, canvasGroup.alpha);
            bookRect.localScale = new Vector3(scale, scale, 1f);

            canvasGroup.interactable = isTargetOpen && canvasGroup.alpha > 0.8f;
            canvasGroup.blocksRaycasts = isTargetOpen;

            if (!isTargetOpen && canvasGroup.alpha <= 0.01f)
            {
                menuCanvas.enabled = false;
            }
        }

        // Cho phép bấm phím Escape để gấp sách và đứng dậy
        if (isTargetOpen && Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            OnClickFoldBookAndStandUp();
        }

        // Ẩn dòng thông báo trạng thái sau vài giây
        if (statusToastTimer > 0f)
        {
            statusToastTimer -= Time.deltaTime;
            if (statusToastTimer <= 0f && statusToastText != null)
            {
                statusToastText.text = "";
            }
        }
    }

    /// <summary>
    /// Gọi khi nhân vật vừa ngồi xuống ghế Sofa để mở cuốn sách.
    /// </summary>
    public void OpenBookWithDelay()
    {
        openDelayTimer = openDelay;
    }

    /// <summary>
    /// Đóng cuốn sách ngay lập tức.
    /// </summary>
    public void CloseBook()
    {
        openDelayTimer = 0f;
        isTargetOpen = false;
    }

    private void SetBookImmediate(bool open)
    {
        openDelayTimer = 0f;
        isTargetOpen = open;
        if (canvasGroup != null)
        {
            canvasGroup.alpha = open ? 1f : 0f;
            canvasGroup.interactable = open;
            canvasGroup.blocksRaycasts = open;
        }
        if (menuCanvas != null)
        {
            menuCanvas.enabled = open;
        }
    }

    // =========================================================================
    // SỰ KIỆN CÁC NÚT TRÊN TRANG GIẤY BÊN PHẢI
    // =========================================================================

    private void OnClickContinueGame()
    {
        ShowToast("Đang tiếp tục hành trình... (Nhấn [E] hoặc [Space] khi muốn đứng dậy)");
        CloseBook();
    }

    private void OnClickNewGame()
    {
        ShowToast("Bắt đầu cuộc hành trình mới!");
        CloseBook();
        if (playerController != null)
        {
            playerController.StartNewGameFromBook();
        }
    }

    private void OnClickFoldBookAndStandUp()
    {
        CloseBook();
        if (playerController != null)
        {
            playerController.StandUpFromBook();
        }
    }

    private void ShowToast(string message)
    {
        if (statusToastText != null)
        {
            statusToastText.text = message;
            statusToastTimer = 3.0f;
        }
    }

    // =========================================================================
    // KHỞI TẠO GIAO DIỆN UGUI CANVAS TỰ ĐỘNG
    // =========================================================================

    private void BuildBookCanvasUI()
    {
        // Đảm bảo Scene có EventSystem tương thích với New Input System để click chuột vào nút
        if (FindFirstObjectByType<EventSystem>() == null)
        {
            GameObject esObj = new GameObject("EventSystem");
            esObj.AddComponent<EventSystem>();
            esObj.AddComponent<InputSystemUIInputModule>();
        }

        Font defaultFont = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        if (defaultFont == null)
        {
            defaultFont = Resources.GetBuiltinResource<Font>("Arial.ttf");
        }

        // 1. Tạo Canvas chính
        GameObject canvasObj = new GameObject("BookMenuCanvas");
        canvasObj.transform.SetParent(transform, false);
        menuCanvas = canvasObj.AddComponent<Canvas>();
        menuCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
        menuCanvas.sortingOrder = 100;

        CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1280f, 720f);
        scaler.matchWidthOrHeight = 0.5f;

        canvasObj.AddComponent<GraphicRaycaster>();
        canvasGroup = canvasObj.AddComponent<CanvasGroup>();

        // 2. Lớp nền tối mờ phía sau cuốn sách (Dimmed Backdrop)
        GameObject backdropObj = new GameObject("DimmedBackdrop");
        backdropObj.transform.SetParent(canvasObj.transform, false);
        RectTransform backdropRect = backdropObj.AddComponent<RectTransform>();
        backdropRect.anchorMin = Vector2.zero;
        backdropRect.anchorMax = Vector2.one;
        backdropRect.offsetMin = Vector2.zero;
        backdropRect.offsetMax = Vector2.zero;

        Image backdropImg = backdropObj.AddComponent<Image>();
        backdropImg.color = new Color(0.05f, 0.03f, 0.02f, 0.62f);

        // 3. Khung Cuốn Sách ở chính giữa màn hình (560x360 phóng lên 720x462 trên chuẩn 720p)
        GameObject bookObj = new GameObject("BookContainer");
        bookObj.transform.SetParent(backdropObj.transform, false);
        bookRect = bookObj.AddComponent<RectTransform>();
        bookRect.anchorMin = new Vector2(0.5f, 0.5f);
        bookRect.anchorMax = new Vector2(0.5f, 0.5f);
        bookRect.sizeDelta = new Vector2(720f, 462f);
        bookRect.anchoredPosition = new Vector2(0f, 10f);

        Image bookImg = bookObj.AddComponent<Image>();
        if (bookSprite != null)
        {
            bookImg.sprite = bookSprite;
            bookImg.preserveAspect = true;
        }
        else
        {
            bookImg.color = new Color(0.95f, 0.92f, 0.84f, 1f);
        }

        // 4. Vùng Trang Giấy Bên Phải (Right Page Menu Area)
        // Tâm của trang giấy bên phải nằm ở x = +160 (tương ứng nửa bên phải cuốn sách)
        GameObject rightPageObj = new GameObject("RightPage_MenuButtons");
        rightPageObj.transform.SetParent(bookObj.transform, false);
        RectTransform rightPageRect = rightPageObj.AddComponent<RectTransform>();
        rightPageRect.anchorMin = new Vector2(0.5f, 0.5f);
        rightPageRect.anchorMax = new Vector2(0.5f, 0.5f);
        rightPageRect.sizeDelta = new Vector2(280f, 340f);
        rightPageRect.anchoredPosition = new Vector2(160f, 0f);

        // Tạo 3 nút bấm trên mặt giấy bên phải
        CreatePageButton(
            rightPageRect,
            defaultFont,
            "Btn_Continue",
            "CHƠI TIẾP",
            new Vector2(0f, 22f),
            new Color(0.22f, 0.43f, 0.24f, 1f),
            OnClickContinueGame
        );

        CreatePageButton(
            rightPageRect,
            defaultFont,
            "Btn_NewGame",
            "CHƠI MỚI",
            new Vector2(0f, -46f),
            new Color(0.20f, 0.28f, 0.44f, 1f),
            OnClickNewGame
        );

        CreatePageButton(
            rightPageRect,
            defaultFont,
            "Btn_CloseBook",
            "GẤP SÁCH",
            new Vector2(0f, -114f),
            new Color(0.54f, 0.23f, 0.19f, 1f),
            OnClickFoldBookAndStandUp
        );

        // 5. Dòng hướng dẫn nhỏ dưới chân màn hình khi mở sách
        GameObject hintObj = new GameObject("BookHintText");
        hintObj.transform.SetParent(backdropObj.transform, false);
        RectTransform hintRect = hintObj.AddComponent<RectTransform>();
        hintRect.anchorMin = new Vector2(0.5f, 0f);
        hintRect.anchorMax = new Vector2(0.5f, 0f);
        hintRect.sizeDelta = new Vector2(600f, 36f);
        hintRect.anchoredPosition = new Vector2(0f, 36f);

        Text hintText = hintObj.AddComponent<Text>();
        hintText.font = defaultFont;
        hintText.fontSize = 16;
        hintText.alignment = TextAnchor.MiddleCenter;
        hintText.color = new Color(0.92f, 0.86f, 0.75f, 0.9f);
        hintText.text = "Dùng chuột chọn trên trang giấy phải hoặc nhấn [ESC] để gấp sách";

        // 6. Dòng thông báo trạng thái (Toast) ở góc trên màn hình
        GameObject toastObj = new GameObject("StatusToastText");
        toastObj.transform.SetParent(canvasObj.transform, false);
        RectTransform toastRect = toastObj.AddComponent<RectTransform>();
        toastRect.anchorMin = new Vector2(0.5f, 1f);
        toastRect.anchorMax = new Vector2(0.5f, 1f);
        toastRect.sizeDelta = new Vector2(700f, 40f);
        toastRect.anchoredPosition = new Vector2(0f, -32f);

        statusToastText = toastObj.AddComponent<Text>();
        statusToastText.font = defaultFont;
        statusToastText.fontSize = 18;
        statusToastText.fontStyle = FontStyle.Bold;
        statusToastText.alignment = TextAnchor.MiddleCenter;
        statusToastText.color = new Color(1f, 0.92f, 0.65f, 1f);
        statusToastText.text = "";
    }

    private void CreatePageButton(
        RectTransform parent,
        Font font,
        string objectName,
        string label,
        Vector2 anchoredPos,
        Color inkColor,
        UnityEngine.Events.UnityAction onClickAction)
    {
        // Viền ngoài của nút trên trang giấy
        GameObject btnObj = new GameObject(objectName);
        btnObj.transform.SetParent(parent, false);
        RectTransform btnRect = btnObj.AddComponent<RectTransform>();
        btnRect.anchorMin = new Vector2(0.5f, 0.5f);
        btnRect.anchorMax = new Vector2(0.5f, 0.5f);
        btnRect.sizeDelta = new Vector2(220f, 48f);
        btnRect.anchoredPosition = anchoredPos;

        Image borderImg = btnObj.AddComponent<Image>();
        borderImg.color = inkColor;

        // Nền giấy sáng bên trong nút (tạo khung viền dày 2px)
        GameObject innerObj = new GameObject("PaperFill");
        innerObj.transform.SetParent(btnObj.transform, false);
        RectTransform innerRect = innerObj.AddComponent<RectTransform>();
        innerRect.anchorMin = Vector2.zero;
        innerRect.anchorMax = Vector2.one;
        innerRect.offsetMin = new Vector2(2f, 2f);
        innerRect.offsetMax = new Vector2(-2f, -2f);

        Image innerImg = innerObj.AddComponent<Image>();
        innerImg.color = new Color(1f, 0.98f, 0.93f, 1f);

        Button btn = btnObj.AddComponent<Button>();
        btn.targetGraphic = innerImg;

        ColorBlock colors = btn.colors;
        colors.normalColor = new Color(1f, 0.98f, 0.93f, 1f);
        colors.highlightedColor = new Color(0.96f, 0.90f, 0.76f, 1f);
        colors.pressedColor = new Color(0.88f, 0.80f, 0.64f, 1f);
        colors.selectedColor = colors.highlightedColor;
        btn.colors = colors;

        btn.onClick.AddListener(onClickAction);

        // Chữ hiển thị trên nút
        GameObject txtObj = new GameObject("Label");
        txtObj.transform.SetParent(btnObj.transform, false);
        RectTransform txtRect = txtObj.AddComponent<RectTransform>();
        txtRect.anchorMin = Vector2.zero;
        txtRect.anchorMax = Vector2.one;
        txtRect.offsetMin = Vector2.zero;
        txtRect.offsetMax = Vector2.zero;

        Text txt = txtObj.AddComponent<Text>();
        txt.font = font;
        txt.text = label;
        txt.fontSize = 18;
        txt.fontStyle = FontStyle.Bold;
        txt.alignment = TextAnchor.MiddleCenter;
        txt.color = inkColor;
    }
}
