using System;
using UnityEngine;

public class GirlfriendController : MonoBehaviour, IStageResultNotice
{
    public event Action<StageResult, Vector2> StageFinished;
    private Animator girlAnimator;
    private bool isFacingDirectionLeft;
    private SpriteRenderer girlSpriteRenderer;

    private void Awake()
    {
        girlSpriteRenderer = GetComponent<SpriteRenderer>();
        girlAnimator = GetComponent<Animator>();
        AnimationReset();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            StageFinished?.Invoke(StageResult.Clear, Vector2.zero);

            var differenceX = collision.transform.position.x - transform.position.x;
            FaceDirection(differenceX);
            girlAnimator.SetBool("goal", true);
        }
    }

    private void AnimationReset()
    {
        girlAnimator.SetBool("goal", false);
        girlAnimator.SetBool("game_over", false);
    }

    public void GameOverAnimation()
    {
        // 現状ゲームオーバーのアニメーションは顔の向きを変えていない（変えないといけないかも）
        girlAnimator.SetBool("game_over", true);
    }

    private void FaceDirection(float differenceX)
    {
        // プレイヤーの当たってきた方向によって顔の向きを変える
        if (differenceX <= 0)
        {
            isFacingDirectionLeft = true;
            SetFacing(isFacingDirectionLeft);
        }
        else
        {
            isFacingDirectionLeft = false;
            SetFacing(isFacingDirectionLeft);
        }
    }

    // デフォルト右見てる
    private void SetFacing(bool faceDirection)
    {
        girlSpriteRenderer.flipX = faceDirection;
    }


    // --- 以下、アニメーションイベント用 ---
    public void CloseShutter()
    {

    }

    public void GameOverAnimationCallBack()
    {

    }

    public void PlayGameOverSound()
    {

    }
}
