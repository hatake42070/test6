using UnityEngine;

/// <summary>
/// ステージの進捗（クリア状況）を管理する
/// </summary>
public class StageProgressManager
{
    private int nextStageIndex = 0;

    private readonly SaveManager saveManager;
    private StageSaveData StageData => saveManager.Data.stage;

    // テスト時のみステージを1,2,4,6の順に開放する
    private int[] testStageIndex = { 0, 1, 3, 5 };


    public StageProgressManager(SaveManager saveManager)
    {
        this.saveManager = saveManager;
    }

    public void RecordClear(int stageIndex, int score)
    {
        StageData.stageClearRanks[stageIndex] = Mathf.Max(StageData.stageClearRanks[stageIndex], score);

        //if (StageData.isStageUnlocked[stageIndex + 1] == false)
        //{
        //    StageData.isStageUnlocked[stageIndex + 1] = true;
        //}
        if (StageData.isStageUnlocked[testStageIndex[stageIndex + 1]] == false)
        {
            StageData.isStageUnlocked[testStageIndex[stageIndex + 1]] = true;
        }

        saveManager.Save();
    }

    public int GetClearRank(int stageIndex)
    {
        return StageData.stageClearRanks[stageIndex];
    }

    public bool IsStageUnlocked(int stageIndex)
    {
        return StageData.isStageUnlocked[stageIndex];
    }

    public void SetNextStageIndex(int stageIndex)
    {
        nextStageIndex = stageIndex;
    }

    public int GetNextStageIndex()
    {
        return nextStageIndex;
    }

    public void ResetSaveData()
    {
        saveManager.ResetSaveData();
    }
}
