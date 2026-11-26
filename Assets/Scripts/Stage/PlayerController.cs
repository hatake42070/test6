using UnityEngine;
using VContainer;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private bool startFacingRight;
    [SerializeField] private float knockbackImpulse = 3f;
    private Rigidbody2D playerRb;
    private Animator playerAnimator;
    private Transform playerTransform;
    private AudioManager audioManager;
    private bool isSEPlayed;

    private void Awake()
    {
        playerRb = GetComponent<Rigidbody2D>();
        playerRb.simulated = false;
        playerAnimator = GetComponent<Animator>();
        playerTransform = GetComponent<Transform>();
        AnimationReset();
        SetFacing(startFacingRight);
        isSEPlayed = false;
    }

    [Inject]
    public void Construct(AudioManager audioManager)
    {
        this.audioManager = audioManager;
    }

    private void SetFacing(bool startFacingRight)
    {
        spriteRenderer.flipX = startFacingRight;
    }

    public void RigidOn()
    {
        playerRb.simulated = true;
    }
    public void RigidOFF()
    {
        playerRb.simulated = false;
    }

    private void AnimationReset()
    {
        if (playerAnimator == null) return;
        playerAnimator.SetBool("right_goal_flag", false);
        playerAnimator.SetBool("goal_flag", false);
        playerAnimator.SetBool("death_flag", false);
    }

    public void GoalAnimation(Transform girlTransform)
    {
        // girlfriendの場所で左右反転する
        FaceDirection(girlTransform);

        if (playerAnimator == null) return;
        // 体の向きを正して物理をOFFにする
        playerRb.simulated = false;
        playerTransform.rotation = Quaternion.identity;

        playerAnimator.SetBool("goal_flag", true);
    }

    public void DeathAnimation(Vector2 direction)
    {
        // 移動と回転の勢いをリセット
        playerRb.linearVelocity = Vector2.zero;
        playerRb.angularVelocity = 0f;

        playerRb.AddForce(direction * knockbackImpulse, ForceMode2D.Impulse);
        if (playerAnimator == null) return;

        playerAnimator.SetBool("death_flag", true);
    }

    private void FaceDirection(Transform girlTransform)
    {
        // 彼女の場所によってゴール時の顔の向きを変える
        float differenceX = girlTransform.position.x - transform.position.x;
        if (differenceX <= 0)
        {
            startFacingRight = false;
            SetFacing(startFacingRight);
        }
        else
        {
            startFacingRight = true;
            SetFacing(startFacingRight);
        }
    }

    public void PlayerDeathSE()
    {
        if (isSEPlayed) return;
        isSEPlayed = true;
        audioManager.PlaySE("goal_sad");
    }
}
