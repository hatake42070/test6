using System;
using UnityEngine;
using VContainer;

public class CircularSaw : MonoBehaviour, IStageResultNotice
{
    public event Action<StageResult, Vector2> StageFinished;
    private AudioManager audioManager;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            var direction = (collision.transform.position - transform.position).normalized;
            StageFinished?.Invoke(StageResult.GameOver, direction);
            audioManager.PlaySE("marunoko");
        }
    }

    [Inject]
    public void Construct(AudioManager audioManager)
    {
        this.audioManager = audioManager;
    }
}
