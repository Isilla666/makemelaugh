using UniRx.Async;

namespace Backend.Invoker
{
    public interface ISignalInvoke
    {
        bool WithConnection { get; }

        UniTask SendCommandToChangeState(int state);
    }
}