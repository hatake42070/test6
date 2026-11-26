using UnityEngine;
using UnityEngine.UI;
using VContainer;

public class StageSelectionUI : MonoBehaviour
{
    private SequenceManager sequenceManager;
    private StageProgressManager stageProgressManager;

    [SerializeField] private Button goToTitleButton;
    [SerializeField] private StageButtonUI stageButtonPrefab;
    [SerializeField] private Transform[] rows;
    [SerializeField] private Sprite[] stageButtonImages; // 20枚のステージ画像
    [SerializeField] private Sprite[] pressedStageButtonImages;
    [SerializeField] private Sprite[] stageClearRankImages;
    [SerializeField] private Sprite lockedStageImage;
    [SerializeField] private GameObject buttonPanel;


    [Inject]
    public void Construct(SequenceManager sequenceManager, StageProgressManager stageProgressManager)
    {
        this.sequenceManager = sequenceManager;
        this.stageProgressManager = stageProgressManager;
    }

    private void Awake()
    {
        if (goToTitleButton != null)
        {
            goToTitleButton.onClick.AddListener(() => sequenceManager.ChangeScene("TitleScene"));
        }
    }

    private void Start()
    {
        GenerateStageButtons();
    }

    private void GenerateStageButtons()
    {
        for (int stageIndex = 0; stageIndex < stageButtonImages.Length; stageIndex++)
        {
            int rowIndex = stageIndex / 5;  // 1行の５つずつボタンを配置する
            StageButtonUI buttonUI = Instantiate(stageButtonPrefab, rows[rowIndex]);

            int rank = stageProgressManager.GetClearRank(stageIndex);
            bool isUnlocked = stageProgressManager.IsStageUnlocked(stageIndex);

            if (isUnlocked)
            {
                buttonUI.Setup(stageButtonImages[stageIndex], pressedStageButtonImages[stageIndex], stageClearRankImages[rank], isUnlocked, stageIndex, StageButtonClicked);
            }
            else
            {
                buttonUI.Setup(lockedStageImage, pressedStageButtonImages[stageIndex], stageClearRankImages[rank], isUnlocked, stageIndex, StageButtonClicked);
            }
        }
    }

    private void StageButtonClicked(int stageIndex)
    {
        Debug.Log(stageIndex);
        stageProgressManager.SetNextStageIndex(stageIndex);

        if(stageIndex == 0)
        {
            sequenceManager.ChangeScene("OpeningScene");
        }
        else
        {
            sequenceManager.ChangeScene("StageScene");
        }
    }

    // --- デバック用 ---
#if UNITY_EDITOR
    private void Update()
    {
        bool controlHeld = Input.GetKey(KeyCode.LeftControl)
            || Input.GetKey(KeyCode.RightControl);

        if (controlHeld && Input.GetKeyDown(KeyCode.R))
        {
            stageProgressManager.ResetSaveData();
            Debug.Log("セーブデータを初期化しました");
        }
    }
#endif
}
