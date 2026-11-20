using UnityEngine;
using UnityEngine.Rendering.Universal;

/// <summary>
/// Điều khiển hoạt ảnh ngọn lửa cháy lập lòe của Lò Sưởi (4 frame)
/// đồng thời tạo hiệu ứng ánh sáng cam ấm áp (Light 2D) bập bùng tự nhiên theo ngọn lửa.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))]
public class FireplaceAnimator2D : MonoBehaviour
{
    [Header("Chuỗi Frame Ngọn Lửa (4 Frames)")]
    [Tooltip("Danh sách 4 Sprite frame lửa cháy cắt từ fireplace_sheet.png.")]
    public Sprite[] flameFrames;

    [Tooltip("Tốc độ chuyển frame lửa (số frame / giây).")]
    public float framesPerSecond = 8f;

    [Header("Hiệu ứng Ánh sáng Lửa lập lòe (Light 2D Flicker)")]
    [Tooltip("Đèn Point Light 2D của lò sưởi.")]
    public Light2D fireplaceLight;

    [Tooltip("Cường độ sáng trung bình của ngọn lửa.")]
    public float baseIntensity = 1.35f;

    [Tooltip("Biên độ dao động ánh sáng khi lửa bập bùng.")]
    public float flickerIntensityRange = 0.22f;

    [Tooltip("Tốc độ lập lòe của ánh sáng.")]
    public float flickerSpeed = 6.5f;

    private SpriteRenderer spriteRenderer;
    private Animator animator;
    private float frameTimer;
    private int currentFrame;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();

        if (fireplaceLight == null)
        {
            fireplaceLight = GetComponentInChildren<Light2D>();
        }
    }

    private void Update()
    {
        // 1. Nếu không dùng Animator hoặc cần đảm bảo đổi frame Sprite mượt mà
        if ((animator == null || !animator.enabled || animator.runtimeAnimatorController == null)
            && flameFrames != null && flameFrames.Length > 0)
        {
            frameTimer += Time.deltaTime;
            float interval = 1f / Mathf.Max(1f, framesPerSecond);
            if (frameTimer >= interval)
            {
                frameTimer -= interval;
                currentFrame = (currentFrame + 1) % flameFrames.Length;
                if (flameFrames[currentFrame] != null)
                {
                    spriteRenderer.sprite = flameFrames[currentFrame];
                }
            }
        }

        // 2. Hiệu ứng ánh sáng Point Light 2D bập bùng bằng hàm Perlin Noise mượt mà
        if (fireplaceLight != null)
        {
            float noise = Mathf.PerlinNoise(Time.time * flickerSpeed, 0.37f); // 0..1
            float offset = (noise - 0.5f) * 2f * flickerIntensityRange;
            fireplaceLight.intensity = baseIntensity + offset;
            fireplaceLight.pointLightOuterRadius = 4.5f + offset * 0.6f;
        }
    }
}
