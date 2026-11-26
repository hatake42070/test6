using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ボタンを押したときに小さくなるボタンにアタッチする
/// </summary>
public class ButtonPressedAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private Vector2 originalScale;
    private Vector2 pressedScale = new Vector2(0.9f, 0.9f);
    private bool isButtnEnable;
    private Button button;

    private void Start()
    {
        button = GetComponent<Button>();
        originalScale = transform.localScale;
    }

    // ボタンが押されたとき
    public void OnPointerDown(PointerEventData eventData)
    {
        if (button.isActiveAndEnabled == false || button.interactable == false) return;
        transform.localScale = Vector2.Scale(originalScale, pressedScale);
    }

    // ボタンから指やマウスが離れたとき
    public void OnPointerUp(PointerEventData eventData)
    {
        transform.localScale = originalScale;
    }
}
