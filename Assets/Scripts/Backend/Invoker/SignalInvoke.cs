using Backend.Registration;
using Microsoft.AspNetCore.SignalR.Client;
using UniRx.Async;

namespace Backend.Invoker
{
    public class SignalInvoke
    {
        private readonly HubConnection _connection;

        public SignalInvoke(HubConnection connection)
        {
            _connection = connection;
            SignalRegistration<SignalInvoke>.Register(this);
        }


        public async UniTask SendCommandToChangeState(int state)
        {
            if (_connection.State != HubConnectionState.Connected)
                return;

            await _connection.SendAsync("gameState", state);
        }
    }
}