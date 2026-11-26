using System;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

// スキップできない
public class OpeningSkip : MonoBehaviour
{
    private Button skipButton;
    private float doubleClickInterval = 0.3f;
    private float lastClickTime = -1f;

    private Camera mainCamera;

    private SequenceManager sequenceManager;

    [Inject]
    public void Construct(SequenceManager sequenceManager)
    {
        this.sequenceManager = sequenceManager;
    }

    void Start()
    {
        mainCamera = Camera.main;
    }

    private void Update()
    {
        if(Input.GetMouseButtonDown(0)) // 左クリックを感知 -> 画面タップに変える
        {
            Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            RaycastHit2D hit = Physics2D.Raycast(worldPoint, Input.mousePosition);

            if(hit.collider != null)
            {
                onSkipButtonCkicked();
            }
        }
    }

    private void onSkipButtonCkicked()
    {
        float timeSinceLastClick = Time.time - lastClickTime;
        Debug.Log("click");

        if (timeSinceLastClick <= doubleClickInterval)
        {
            // ダブルクリック時の処理
            OnDoubleClick();
            lastClickTime = -1f; // リセット
        }
        else
        {
            lastClickTime = Time.time;
        }
    }

    private void OnDoubleClick()
    {
        sequenceManager.ChangeScene("StageScene");
    }
}
