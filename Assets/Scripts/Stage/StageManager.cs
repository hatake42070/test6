using UnityEngine;
using UnityEngine.UI;
using VContainer;
using System.Threading.Tasks;
using System;

public class StageManager : MonoBehaviour
{
    private int myStageIndex;
    private StageProgressManager stageProgressManager;
    private SequenceManager sequenceManager;
    private PauseManager pauseManager;
    private TutorialManager tutorialManager;

    [SerializeField] private GameObject[] stagePrefabs;
    private GameObject stagePrefab;

    [Header("ゲーム画面UI")]
    [SerializeField] private GameObject gameUIPanel;
    [SerializeField] private Button retryButton;
    [SerializeField] private Button startButton;
    [SerializeField] private Button menuButton;

    [Header("メニュー画面UI")]
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private Button continueButton;
    [SerializeField] private Button menuStageSelectButton;
    [SerializeField] private Button backToTitleButton;

    [Header("ゲームクリア画面UI")]
    [SerializeField] private GameObject gameClearPanel;
    [SerializeField] private Button nextStageButton;
    [SerializeField] private Button clearSelectStageButton;
    [SerializeField] private Button cleardRetryButton;
    [SerializeField] private Image[] notClearNeji; // グレーのねじをセットしてクリア時非表示にする
    [SerializeField] private Image[] clearComments;
    [SerializeField] private PerfectAnimation perfectAnimation;

    [Header("チュートリアル")]
    [SerializeField] private Button tutorialButton;
    [SerializeField] private GameObject fadePanel;

    [SerializeField] private Image nejiUI;
    [SerializeField] private Sprite[] nejiImages;

    private int getNejiCount = 0;

    private DoorsController doorsController;
    public bool CanOperateGimmicks => !fadePanel.activeInHierarchy;

    private PlayerController player;
    private GirlfriendController girl;
    private bool isStageFinished;

    private NejiController[] nejis;


    [Inject]
    public void Construct(StageProgressManager stageProgressManager, SequenceManager sequenceManager, PauseManager pauseManager, TutorialManager tutorialManager)
    {
        this.stageProgressManager = stageProgressManager;
        this.sequenceManager = sequenceManager;
        this.pauseManager = pauseManager;
        this.tutorialManager = tutorialManager;
    }

    private void Awake()
    {
        tutorialButton.onClick.AddListener(RemoveTutorial);
        tutorialButton.gameObject.SetActive(false);
        retryButton.onClick.AddListener(Retry);
        startButton.onClick.AddListener(GameStart);
        menuButton.onClick.AddListener(OpenMenu);

        cleardRetryButton.onClick.AddListener(Retry);
        nextStageButton.onClick.AddListener(GoToNextStage);
        clearSelectStageButton.onClick.AddListener(() => sequenceManager.ChangeScene("StageSelectScene"));

        continueButton.onClick.AddListener(GameContinue);
        menuStageSelectButton.onClick.AddListener(() => 
        {
            sequenceManager.ChangeScene("StageSelectScene");
            pauseManager.GameResume();
        });
        backToTitleButton.onClick.AddListener(() => sequenceManager.ChangeScene("TitleScene"));

        tutorialManager.OnTutorial += StartTutorial;
    }

    private void Start()
    {
        nejiUI.sprite = nejiImages[getNejiCount];
        PlayStageSequense();
    }

    private void SetStage(int stageIndex)
    {
        if (stagePrefabs[stageIndex] != null)
        {
            stagePrefab = Instantiate(stagePrefabs[stageIndex]);
        }

        IStageResultNotice[] notices = stagePrefab.GetComponentsInChildren<IStageResultNotice>(true);
        nejis = stagePrefab.GetComponentsInChildren<NejiController>(true);
        doorsController = stagePrefab.GetComponentInChildren<DoorsController>(true);
        TutorialObject[] tutorials = stagePrefab.GetComponentsInChildren<TutorialObject>(true);
        tutorialManager.Initialize(tutorials);

        player = stagePrefab.GetComponentInChildren<PlayerController>(true);
        var playerCollider = player != null ? player.GetComponent<CircleCollider2D>() : null;
        girl = stagePrefab.GetComponentInChildren<GirlfriendController>(true);

        foreach (var notice in notices)
        {
            // 呼ぶ処理を購読
            notice.StageFinished += GameSet;
        }
        foreach(var neji in nejis)
        {
            neji.TriggerNeji += GetNeji;
        }
        foreach (var spring in stagePrefab.GetComponentsInChildren<SpringController>(true))
        {
            spring.Initialize(this, playerCollider);
        }
        foreach (var magnet in stagePrefab.GetComponentsInChildren<MagnetController>(true))
        {
            magnet.Initialize(this);
        }
        foreach (var fan in stagePrefab.GetComponentsInChildren<FanController>(true))
        {
            fan.Initialize(this);
        }
    }

    // ゲームのシーケンスを管理する
    private void PlayStageSequense()
    {
        isStageFinished = false;
        getNejiCount = 0;
        nejiUI.sprite = nejiImages[getNejiCount];

        nejiUI.enabled = false;
        fadePanel.SetActive(true);
        gameClearPanel.SetActive(false);
        gameUIPanel.SetActive(false);
        menuPanel.SetActive(false);
        startButton.gameObject.SetActive(true);

        myStageIndex = stageProgressManager.GetNextStageIndex();

        pauseManager.AddPauseReason(PauseReason.StartPause);

        // stageを生成する
        SetStage(myStageIndex);
    }

    // リトライ（同じステージシーケンスを再呼び出し）
    private void Retry()
    {
        // 元のプレハブを削除する
        Destroy(stagePrefab);

        PlayStageSequense();
    }

    // スタートボタンから呼ばれる
    private void GameStart()
    {
        fadePanel.SetActive(false);
        pauseManager.RemovePauseReason(PauseReason.StartPause);
        tutorialManager.HideAllTutorials();
        startButton.gameObject.SetActive(false);
        gameUIPanel.SetActive(true);
        nejiUI.enabled = true;

        foreach(var neji in nejis)
        {
            neji.NejiUpDownAnimationStart();
        }
    }

    private async void GameSet(StageResult result, Vector2 direction)
    {
        if (isStageFinished) return;
        isStageFinished = true;
        if (result == StageResult.Clear)
        {
            Debug.Log("Clear");
            nejiUI.enabled = false;
            gameUIPanel.SetActive(false);

            // ころりアニメーション待機
            if (player != null) player.GoalAnimation(girl.transform);
            await Task.Delay(TimeSpan.FromSeconds(2.5));

            fadePanel.SetActive(true);
            gameClearPanel.SetActive(true);
            SetClearRank(getNejiCount);
            stageProgressManager.RecordClear(myStageIndex, getNejiCount);
            getNejiCount = 0;
        }
        else
        {
            Debug.Log("GameOver");
            if (player != null) player.DeathAnimation(direction);
            if (girl != null) girl.GameOverAnimation();
            await Task.Delay(TimeSpan.FromSeconds(1.66));
            doorsController.CloseDoors();
            // ドアの閉まるアニメーションを待つ処理をここに書く
            await Task.Delay(TimeSpan.FromSeconds(1));
            Retry();
        }
    }

    private void GoToNextStage()
    {
        int nextStageIndex = myStageIndex + 1;
        if (stagePrefabs[nextStageIndex])
        {
            stageProgressManager.SetNextStageIndex(nextStageIndex);
            Retry();
        }
        Debug.Log("次のステージはありません");
    }

    private void OpenMenu()
    {
        gameUIPanel.SetActive(false);
        fadePanel.SetActive(true);
        pauseManager.AddPauseReason(PauseReason.MenuPause);
        menuPanel.SetActive(true);
    }

    private void GameContinue()
    {
        gameUIPanel.SetActive(true);
        menuPanel.SetActive(false);
        fadePanel.SetActive(false);
        pauseManager.RemovePauseReason(PauseReason.MenuPause);
    }

    private void GetNeji()
    {
        getNejiCount++;
        nejiUI.sprite = nejiImages[getNejiCount];
    }

    // OnFirstContactチュートリアルが発生時に実行
    private void StartTutorial()
    {
        fadePanel.SetActive(true);
        tutorialButton.gameObject.SetActive(true);
    }

    // OnFirstContactチュートリアルを画面タップで終了時に実行
    private void RemoveTutorial()
    {
        tutorialButton.gameObject.SetActive(false);
        fadePanel.SetActive(false);
        tutorialManager.RemoveTutorial();
    }

    private void SetClearRank(int rank)
    {
        // 最初にリセット
        foreach (var neji in notClearNeji)
        {
            neji.enabled = true;
        }
        for (int i = 0; i < clearComments.Length; i++)
        {
            clearComments[i].enabled = false;
        }

        // 獲得したねじだけ表示
        for (int i = 0; i < rank; i++)
        {
            notClearNeji[i].enabled = false;
        }

        bool isPerfect = rank == 3;
        perfectAnimation.gameObject.SetActive(isPerfect);
        if (isPerfect) perfectAnimation.AnimationStart();
        clearComments[rank].enabled = true;
    }
}
