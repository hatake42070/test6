using UnityEngine;
using UnityEngine.UI;

public class PerfectAnimation : MonoBehaviour
{
    private Image circleImage;
    private bool animationStart;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(600f, 200f);
        circleImage = GetComponentInChildren<Image>();
        animationStart = false;
        // AnimationStart();
    }

    public void AnimationStart()
    {
        RectTransform circleTransform = circleImage.rectTransform;
        circleTransform.rotation = Quaternion.identity;
        animationStart = true;
    }
    public void AnimationStop()
    {
        animationStart = false;
    }

    private void Update()
    {
        if (!animationStart) return;

        RectTransform circleTransform = circleImage.rectTransform;
        circleTransform.Rotate(0f, 0f, -90f * Time.deltaTime, Space.Self);
    }
}