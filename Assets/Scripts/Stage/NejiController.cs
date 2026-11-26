using System;
using System.Threading.Tasks;
using UnityEngine;
using VContainer;

public class NejiController : MonoBehaviour
{
    public event Action TriggerNeji;
    private Animator boltAnimator;
    private AudioManager audioManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            TriggerNeji?.Invoke();
            boltAnimator.SetBool("destroy", true);
            audioManager.PlaySE("neji");
        }
    }
    private void Awake()
    {
        boltAnimator = GetComponent<Animator>();
    }

    [Inject]
    public void Construct(AudioManager audioManager)
    {
        this.audioManager = audioManager;
    }

    public async void NejiUpDownAnimationStart()
    {
        float rand = UnityEngine.Random.Range(0f, 2.0f);
        await Task.Delay(TimeSpan.FromSeconds(rand));

        if (boltAnimator == null) return;   // ねじアニメーション待機中に戻ったときのエラー対策

        boltAnimator.SetBool("switch", true);
    }

    public void DestroyNeji()
    {
        Destroy(gameObject); // 自身を破壊
    }
}
