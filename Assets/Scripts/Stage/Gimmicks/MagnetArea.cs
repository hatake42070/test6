using UnityEngine;

public class MagnetArea : MonoBehaviour
{
    public Rigidbody2D PlayerRB { get; private set; }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerRB = collision.attachedRigidbody;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            PlayerRB = null;
        }
    }
}
