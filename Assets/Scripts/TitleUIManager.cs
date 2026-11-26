using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class TitleUIManager : MonoBehaviour
{
    private SequenceManager sequenceManager;
    [SerializeField] private Button startButton;
    [SerializeField] private Button optionButton;
    [SerializeField] private Button closeOptionPanel;
    [SerializeField] private GameObject optionPanel;
    [SerializeField] private Button exitButton;
    [SerializeField] private GameObject exitPanel;
    [SerializeField] private Button exitConfirmButton;
    [SerializeField] private Button exitCancelButton;
    [SerializeField] private Button muteButton;
    [SerializeField] private Sprite soundOn;
    [SerializeField] private Sprite soundOff;
    private AudioManager audioManager;
    private bool isMuted;

    [Inject]
    public void Construct(SequenceManager sequenceManager, AudioManager audioManager)
    {
        this.sequenceManager = sequenceManager;
        this.audioManager = audioManager;
    }

    private void Start()
    {
        startButton.onClick.AddListener(() => sequenceManager.ChangeScene("StageSelectScene"));
        optionButton.onClick.AddListener(OpenOptionPanel);
        closeOptionPanel.onClick.AddListener(CloseSettingPanel);
        optionPanel.SetActive(false);
        exitButton.onClick.AddListener(OpenExitPanel);
        exitPanel.SetActive(false);
        exitConfirmButton.onClick.AddListener(GameExit);
        exitCancelButton.onClick.AddListener(CloseExitPanel);
        muteButton.onClick.AddListener(OnClickMuteButton);
        RefreshMuteButton();
    }

    private void OpenOptionPanel()
    {
        optionPanel.SetActive(true);
        exitButton.enabled = false;
    }

    private void CloseSettingPanel()
    {
        optionPanel.SetActive(false);
        exitButton.enabled = true;
    }

    private void OpenExitPanel()
    {
        exitPanel.SetActive(true);
        optionButton.enabled = false;
    }

    private void CloseExitPanel()
    {
        exitPanel.SetActive(false);
        optionButton.enabled = true;
    }

    private void OnClickMuteButton()
    {
        audioManager.SetMuted(!audioManager.IsMuted);
        RefreshMuteButton();
    }

    private void RefreshMuteButton()
    {
        muteButton.image.sprite = audioManager.IsMuted ? soundOff : soundOn;
    }

    // ゲーム終了処理（Unityエディタとビルド後で処理を変える）
    private void GameExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
