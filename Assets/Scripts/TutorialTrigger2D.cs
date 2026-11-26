using System;
using UnityEngine;

public class TutorialTrigger2D : MonoBehaviour
{
    public event Action PlayerEntered;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerEntered?.Invoke();
        }
    }
}
