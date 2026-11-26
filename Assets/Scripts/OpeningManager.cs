using UnityEngine;
using VContainer;


public class OpeningManager : MonoBehaviour
{
    private Animator animator;
    private SequenceManager sequenceManager;
    [SerializeField] private GameObject doorsPrefab;
    private Animator doorsAnimator;

    [Inject]
    public void Construct(SequenceManager sequenceManager)
    {
        this.sequenceManager = sequenceManager;
    }

    private void Awake()
    {
        animator = GetComponent<Animator>();
        doorsAnimator = doorsPrefab.GetComponent<Animator>();
    }

    private void Start()
    {
        StartOpening();
    }

    private void StartOpening()
    {
        animator.SetBool("start", true);
    }

    // オープニングアニメーションの最後に呼ばれる（アニメーションイベント）
    private void GoNextScene()
    {
        sequenceManager.ChangeScene("StageScene");
    }

    private void CloseDoors()
    {
        doorsAnimator.SetTrigger("DoorsClose");
    }

}
