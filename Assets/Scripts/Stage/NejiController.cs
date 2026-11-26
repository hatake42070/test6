using System;
using UnityEngine;
using System.Threading.Tasks;

public class NejiController : MonoBehaviour
{
    public event Action TriggerNeji;
    private Animator boltAnimator;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            TriggerNeji?.Invoke();
            boltAnimator.SetBool("destroy", true);
        }
    }
    private void Awake()
    {
        boltAnimator = GetComponent<Animator>();
    }

    public async void NejiUpDownAnimationStart()
    {
        float rand = UnityEngine.Random.Range(0f, 2.0f);
        await Task.Delay(TimeSpan.FromSeconds(rand));
        boltAnimator.SetBool("switch", true);
    }

    public void DestroyNeji()
    {
        Destroy(gameObject); // é©êgÇîjâÛ
    }
}
