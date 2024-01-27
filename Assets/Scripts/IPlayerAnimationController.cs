public interface IPlayerAnimationController
{
    public PlayerTypeAnimation LastLooped { get; set; }
    void ChangeType(PlayerTypeAnimation typeAnimation);
}