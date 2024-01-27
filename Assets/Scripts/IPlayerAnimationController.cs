using System;

public interface IPlayerAnimationController
{
    bool IsBusy { get; }
    public PlayerTypeAnimation LastLooped { get; set; }
    void ChangeType(PlayerTypeAnimation typeAnimation, Action onComplete = null);
}