using System;
using UnityEngine;

public class SaveData
{
    public int version = 1;
    public StageSaveData stage = new();
    public SoundSaveData sound = new();
}

[Serializable]
public class StageSaveData
{
    public bool[] isStageUnlocked = new bool[20];  // ステージがアンロックされている：true
    public int[] stageClearRanks = new int[20];    // ステージのクリアランク（０～３）
}

[Serializable]
public class SoundSaveData
{
    public bool isMuted;
}
