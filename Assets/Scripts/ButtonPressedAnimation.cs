using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// ボタンを押したときに小さくなるボタンにアタッチする
/// </summary>
public class ButtonPressedAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 originalScale;
    private Vector2 pressedScale = new Vector2(0.9f, 0.9f);

    private void Start()
    {
        originalScale = transform.localScale;
    }

    // ボタンが押されたとき
    public void OnPointerDown(PointerEventData eventData)
    {
        transform.localScale = Vector2.Scale(originalScale, pressedScale);
    }

    // ボタンから指やマウスが離れたとき
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }
}
