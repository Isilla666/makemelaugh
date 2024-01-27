using Microsoft.AspNetCore.SignalR.Client;
using UniRx.Async;

namespace Backend.Invoker
{
    internal sealed class SignalInvoke : ISignalInvoke
    {
        private readonly HubConnection _connection;

        public SignalInvoke(HubConnection connection) =>
            _connection = connection;


        public bool WithConnection => _connection.State == HubConnectionState.Connected;

        public async UniTask SendCommandToChangeState(int state)
        {
            if (_connection.State != HubConnectionState.Connected)
                return;

            await _connection.SendAsync("gameState", state);
        }
    }
}