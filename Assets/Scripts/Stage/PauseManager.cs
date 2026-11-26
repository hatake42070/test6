using System;
using System.Collections.Generic;
using UnityEngine;

public enum PauseReason
{
    StartPause,
    MenuPause,
    TutorialPause
}

public class PauseManager : MonoBehaviour
{
    // HashSetでポーズの理由を持つ．それらが空の時にResumeする
    private HashSet<PauseReason> pauseReasons = new HashSet<PauseReason>();
    public event Action OnPauseReasonChanged = delegate { };

    public void AddPauseReason(PauseReason reason)
    {
        pauseReasons.Add(reason);
        OnPauseReasonChanged.Invoke();
        GamePause();
    }

    public void RemovePauseReason(PauseReason reason)
    {
        pauseReasons.Remove(reason);
        OnPauseReasonChanged.Invoke();
        if (pauseReasons.Count == 0)
        {
            GameResume();
        }
    }

    public void RemoveAllPauseReson()
    {
        pauseReasons.Clear();
        OnPauseReasonChanged.Invoke();
        GameResume();
    }

    // ゲームポーズ
    public void GamePause()
    {
        Time.timeScale = 0;
    }

    // ポーズ解除
    public void GameResume()
    {
        Time.timeScale = 1;
    }

    public bool HasPauseReason(PauseReason reason)
    {
        return pauseReasons.Contains(reason);
    }

    // 何か一つでも要素があるか
    public bool HasAnyPauseReason()
    {
        return pauseReasons.Count > 0 ? true : false;
    }
}
