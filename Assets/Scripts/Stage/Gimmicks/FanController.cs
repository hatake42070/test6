using UnityEngine;
using VContainer;

public class FanController : MonoBehaviour
{
    private BoxCollider2D fanCollider;
    private StageManager stageManager;

    private bool isPressed = false;

    [SerializeField] Animator fanAnimator;
    [SerializeField] Animator windAnimator;
    [SerializeField] WindArea windArea;

    [SerializeField] private float windForce = 60f;

    private AudioManager audioManager;

    private void Awake()
    {
        AnimationReset();
        fanCollider = GetComponent<BoxCollider2D>();
        Debug.Log(fanCollider);
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

    private void AnimationReset()
    {
        windAnimator.SetBool("mouse_up", false);
        windAnimator.SetBool("mouse_down", false);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && stageManager != null && stageManager.CanOperateGimmicks)
        {
            Vector2 position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            isPressed = fanCollider.OverlapPoint(position);

            if (isPressed)
            {
                Debug.Log("Fan Clicked!");
                if (fanAnimator != null)
                {
                    fanAnimator.SetBool("mouse_up", false);
                    fanAnimator.SetBool("mouse_down", true);
                }
                if (windAnimator != null)
                {
                    windAnimator.SetBool("mouse_down", true);
                    windAnimator.SetBool("mouse_up", false);
                }
                audioManager.PlaySELoop("se_gim_fan");
            }
        }

        if (Input.GetMouseButtonUp(0) && isPressed)
        {
            isPressed = false;
            if (fanAnimator != null)
            {
                fanAnimator.SetBool("mouse_down", false);
                fanAnimator.SetBool("mouse_up", true);
            }
            if (windAnimator != null)
            {
                windAnimator.SetBool("mouse_down", false);
                windAnimator.SetBool("mouse_up", true);
            }
            audioManager.StopSELoop();
            Debug.Log("Fan Released!");
        }
    }

    private void FixedUpdate()
    {
        if (!isPressed || stageManager == null || !stageManager.CanOperateGimmicks)
            return;

        Rigidbody2D playerRb = windArea.PlayerRB;
        if (playerRb == null)
            return;

        Vector2 direction = Vector2.right;
        playerRb.AddForce(direction * windForce, ForceMode2D.Force);
    }
}
