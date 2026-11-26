using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;


public enum TutorialMode
{
    OnGameStart,    // 初回のスタート画面で表示
    OnFirstContact  // 対象に初めて触れたときに表示
}

public class TutorialManager : MonoBehaviour
{
    private PauseManager pauseManager;

    private List<TutorialObject> tutorials = new();         // ステージ上の全チュートリアルオブジェクトの参照
    private List<TutorialObject> startTutorials = new();    // チュートリアルモードがOnGameStartのオブジェクトの参照

    private TutorialObject currentTutorialObject;

    public event Action OnTutorial = delegate { };  // チュートリアルの発生をStageManagerに知らせる

    [Inject]
    public void Construct(PauseManager pauseManager)
    {
        this.pauseManager = pauseManager;
        pauseManager.OnPauseReasonChanged += OnPauseReasonChanged;
    }

    public void Initialize(TutorialObject[] tutorialObjects)
    {
        foreach(var tutorial in tutorialObjects)
        {
            tutorial.TutorialEnable -= ShowTutorial;
        }
        tutorials.Clear();
        startTutorials.Clear();
        currentTutorialObject = null;

        foreach (var tutorial in tutorialObjects)
        {
            if (tutorial == null)
            {
                continue;
            }

            tutorials.Add(tutorial);

            if (tutorial.GetTutorialMode() == TutorialMode.OnGameStart)
            {
                startTutorials.Add(tutorial);
            }
            else if (tutorial.GetTutorialMode() == TutorialMode.OnFirstContact)
            {
                tutorial.TutorialEnable += ShowTutorial;
            }
        }
        ApplyAnimationState();
    }

    /// <summary>
    /// OnFirstContactチュートリアルからの通知で実行される
    /// </summary>
    /// <param name="tutorialObject">チュートリアルを発生させるターゲット</param>
    private void ShowTutorial(TutorialObject tutorialObject)
    {
        OnTutorial.Invoke();
        currentTutorialObject = tutorialObject;
        pauseManager.AddPauseReason(PauseReason.TutorialPause);
        tutorialObject.DisplayHukidasi();
    }

    // OnFirstContactチュートリアル解除時に実行
    public void RemoveTutorial()
    {
        currentTutorialObject.HideHukidasi();
        currentTutorialObject = null;
        pauseManager.RemovePauseReason(PauseReason.TutorialPause);
    }

    private void StartTutorial(List<TutorialObject> startTutorialObjects)
    {
        StopAllAnimation();
        foreach(var tutorialObject in startTutorialObjects)
        {
            tutorialObject.DisplayHukidasi();
            tutorialObject.StartAnimation();
        }
    }

    public void HideAllTutorials()
    {
        foreach(var tutorialObject in tutorials)
        {
            if(tutorialObject != null)
            {
                tutorialObject.HideHukidasi();
            }
        }
    }

    public void StopAllAnimation()
    {
        foreach (var tutorial in tutorials)
        {
            if (tutorial != null)
            {
                tutorial.StopAnimation();
            }
        }
    }

    public void StartAllAnimation()
    {
        foreach (var tutorial in tutorials)
        {
            if (tutorial != null)
            {
                tutorial.StartAnimation();
            }
        }
    }

    private void OnPauseReasonChanged()
    {
        ApplyAnimationState();
    }

    private void ApplyAnimationState()
    {
        // メニューが開いている間は，全アニメーション停止
        if (pauseManager.HasPauseReason(PauseReason.MenuPause))
        {
            StopAllAnimation();
            return;
        }

        // スタートポーズ時はTutorialModeがOnGameStartのオブジェクトだけアニメーション再生
        if (pauseManager.HasPauseReason(PauseReason.StartPause))
        {
            StartTutorial(startTutorials);
            return;
        }

        // チュートリアル中は，チュートリアル対象のみアニメーション再生
        if (pauseManager.HasPauseReason(PauseReason.TutorialPause))
        {
            StopAllAnimation();
            currentTutorialObject?.StartAnimation();
            return;
        }

        // すべてのPauseReasonがない場合，アニメーション再開
        if (!pauseManager.HasAnyPauseReason())
        {
            StartAllAnimation();
        }
    }

    private void OnDestroy()
    {
        foreach (var tutorial in tutorials)
        {
            if (tutorial != null)
            {
                tutorial.TutorialEnable -= ShowTutorial;
            }
        }
        if (pauseManager != null)
        {
            pauseManager.OnPauseReasonChanged -= OnPauseReasonChanged;
        }
    }
}
