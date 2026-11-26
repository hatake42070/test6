using UnityEngine;
using VContainer;
using UnityEngine.UI;

public class ButtonPressedSound : MonoBehaviour
{
    private AudioManager audioManager;
    private Button button;
    [Inject]
    public void Construct(AudioManager audioManager)
    {
        this.audioManager = audioManager;
    }

    private void Awake()
    {
        button = GetComponent<Button>();
        if (button) button.onClick.AddListener(PlayButtonPressedSE);
    }

    private void PlayButtonPressedSE()
    {
        audioManager.PlaySE("button_2");
    }
}
