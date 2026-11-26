using System;
using UnityEngine;

public enum StageResult{
    Clear,
    GameOver
}

// ƒQ[ƒ€‚ÌŒ‹‰Ê‚É’¼Ú‰e‹¿‚ğ—^‚¦‚éˆ—
public interface IStageResultNotice
{
    event Action<StageResult, Vector2> StageFinished;
}
