using UnityEngine;
using VContainer;

public class MagnetController : MonoBehaviour
{
    private BoxCollider2D magnetCollider;
    private StageManager stageManager;

    private bool isPressed = false;

    [SerializeField] Animator animator;
    [SerializeField] private MagnetWaveAnimation waveAnim;
    [SerializeField] private MagnetArea magnetArea;

    [SerializeField] private float attractionForce = 10f;
    private AudioManager audioManager;

    private void Awake()
    {
        waveAnim.gameObject.SetActive(false);
        magnetCollider = GetComponent<BoxCollider2D>();
    }

    [Inject]
    public void Construct(AudioManager audioManager)
    {
        this.audioManager = audioManager;
    }

    public void Initialize(StageManager stageManager)
    {
        this.stageManager = stageManager;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && stageManager != null && stageManager.CanOperateGimmicks)
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isPressed = magnetCollider.OverlapPoint(position);
            Debug.Log(isPressed);

            if (isPressed)
            {
                Debug.Log("Magnet Clicked!");
                if (animator != null)
                {
                    animator.SetBool("gim_up_flag", false);
                    animator.SetBool("gim_down_flag", true);
                }
                waveAnim.gameObject.SetActive(true);
                audioManager.PlaySELoop("magnet_on_1");
            }
        }

        if (Input.GetMouseButtonUp(0) && isPressed)
        {
            isPressed = false;
            if (animator != null)
            {
                animator.SetBool("gim_down_flag", false);
                animator.SetBool("gim_up_flag", true);
            }
            waveAnim.gameObject.SetActive(false);
            audioManager.StopSELoop();
            audioManager.PlaySE("magnet_off_1");
            Debug.Log("Magnet Released!");
        }
    }

    // 引きよせアニメーション
    private void AttractAnimation()
    {
        // 引き寄せアニメーションの再生
        waveAnim.gameObject.SetActive(true);
    }

    private void FixedUpdate()
    {
        if (!isPressed || stageManager == null || !stageManager.CanOperateGimmicks)
            return;

        Rigidbody2D playerRb = magnetArea.PlayerRB;
        if (playerRb == null)
            return;

        Vector2 direction = (Vector2)transform.position - playerRb.position;
        playerRb.AddForce(direction.normalized * attractionForce, ForceMode2D.Force);
    }
}