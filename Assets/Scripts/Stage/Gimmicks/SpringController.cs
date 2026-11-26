using UnityEngine;
using VContainer;

public class SpringController : MonoBehaviour
{
    [SerializeField] private Animator animator;
    private StageManager stageManager;
    private BoxCollider2D springCollider;
    private CircleCollider2D playerCollider;
    [SerializeField] float launchPower = 10f;

    private AudioManager audioManager;

    private void Awake()
    {
        springCollider = GetComponent<BoxCollider2D>();
    }

    [Inject]
    public void Construct(AudioManager audioManager)
    {
        this.audioManager = audioManager;
    }

    public void Initialize(StageManager stageManager, CircleCollider2D playerCollider)
    {
        this.stageManager = stageManager;
        this.playerCollider = playerCollider;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && stageManager != null && stageManager.CanOperateGimmicks) // クリック感知
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (springCollider.OverlapPoint(position))
            {
                OnSpring();
            }
        }
    }

    private void OnSpring()
    {
        if (animator != null)
        {
            animator.SetBool("spring_up_flag", true);
            audioManager.PlaySE("bane_2");
        }
    }

    private void OffSpring()
    {
        if (animator != null)
        {
            animator.SetBool("spring_up_flag", false);
        }
    }

    // アニメーションイベントから呼ばれ、ばねが伸びきった時にプレイヤーを上向きに打ち上げる
    private void LaunchPlayer()
    {
        if (playerCollider == null || !springCollider.IsTouching(playerCollider)) return;

        var playerRb = playerCollider.GetComponent<Rigidbody2D>();
        playerRb.AddForce(transform.up * launchPower, ForceMode2D.Impulse);
    }
}
