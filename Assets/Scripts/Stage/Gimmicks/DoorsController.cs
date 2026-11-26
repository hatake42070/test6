using UnityEngine;

public class DoorsController : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        OpenDoors();
    }

    public void OpenDoors()
    {
        animator.SetTrigger("DoorsOpen");
    }

    public void CloseDoors()
    {
        animator.SetTrigger("DoorsClose");
    }
}
