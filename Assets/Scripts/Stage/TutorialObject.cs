using System;
using UnityEngine;

public class TutorialObject : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private TutorialMode tutorialMode = TutorialMode.OnGameStart;
    [SerializeField] private SpriteRenderer hukidasi;
    [SerializeField] private SpriteRenderer highrightRenderer;

    // チュートリアルを発生させるコライダーを設置
    [SerializeField] private TutorialTrigger2D contactTrigger;

    public event Action<TutorialObject> TutorialEnable = delegate { };
    private bool notified = false;

    private void Awake()
    {
        if (hukidasi)
        {
            hukidasi.enabled = false;
        }
        if (highrightRenderer)
        {
            highrightRenderer.enabled = false;
        }
    }

    private void OnEnable()
    {
        if (contactTrigger != null)
        {
            contactTrigger.PlayerEntered += NotifyFirstContact;
        }
    }

    private void OnDisable()
    {
        if (contactTrigger != null)
        {
            contactTrigger.PlayerEntered -= NotifyFirstContact;
        }
    }

    public TutorialMode GetTutorialMode()
    {
        return tutorialMode;
    }

    public void DisplayHukidasi()
    {
        if(hukidasi)
        {
            hukidasi.enabled = true;
        }
        if (highrightRenderer)
        {
            highrightRenderer.enabled = true;
        }
    }

    public void HideHukidasi()
    {
        if(hukidasi)
        {
            hukidasi.enabled = false;
        }
        if (highrightRenderer)
        {
            highrightRenderer.enabled = false;
        }
    }

    public void StartAnimation()
    {
        if(animator)
        {
            animator.speed = 1;
        }
    }

    public void StopAnimation()
    {
        if(animator)
        {
            animator.speed = 0;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (contactTrigger == null && collision.gameObject.CompareTag("Player"))
        {
            NotifyFirstContact();
        }
    }

    private void NotifyFirstContact()
    {
        if (tutorialMode != TutorialMode.OnFirstContact || notified) return;
        notified = true;
        TutorialEnable.Invoke(this);
    }
}
