using System;
using UnityEngine;
using UnityEngine.UI;

public class StageButtonUI : MonoBehaviour
{
    [SerializeField] private Image buttonImage;  // ステージボタン画像の配置参照
    [SerializeField] private Image rankImage;
    [SerializeField] private Button button;
    private int myStageIndex;
    private Action<int> onClickCallback;

    public void Setup(Sprite stageButtonImage, Sprite pressedImage, Sprite stageClearRankImages, bool isUnlocked, int stageIndex, Action<int> onClidk)
    {
        button.enabled = isUnlocked;

        buttonImage.sprite = stageButtonImage;
        rankImage.sprite = stageClearRankImages;
        myStageIndex = stageIndex;
        onClickCallback = onClidk;

        SpriteState state = button.spriteState;
        state.pressedSprite = pressedImage;
        button.spriteState = state;

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() => onClickCallback?.Invoke(myStageIndex));
    }
}
