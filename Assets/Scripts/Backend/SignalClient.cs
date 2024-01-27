using System;
using Backend.Invoker;
using Backend.Registration;
using Microsoft.AspNetCore.SignalR.Client;
using UniRx.Async;
using UnityEngine;

namespace Backend
{
    public class SignalClient : MonoBehaviour
    {
#if !DEBUG
        private const string BackendUrl = @"http://localhost:5555/game";
#else
        private const string BackendUrl = @"https://gorbulka.ru/game";
#endif

        private void Awake() => DontDestroyOnLoad(gameObject);

        public SubscriberMono[] subscribers;

        private HubConnection _hubConnection;

        private ISignalInvoke _signalInvoke;

        public async UniTask<bool> Connect()
        {
            if (_hubConnection != null)
                await DisposeHub();

            _hubConnection = await CreateConnectionAsync();
            SignalRegistration<ISignalInvoke>.Register(_signalInvoke);
            return _hubConnection != null && _hubConnection.State == HubConnectionState.Connected;
        }

        private async void OnApplicationQuit() =>
            await DisposeHub();

        private async void OnDestroy() =>
            await DisposeHub();

        private async UniTask<HubConnection> CreateConnectionAsync()
        {
            //Создаем соединение с нашим написанным тестовым хабом
            var connection = new HubConnectionBuilder()
                .WithUrl(BackendUrl)
                .WithAutomaticReconnect()
                .Build();

            Debug.Log("[Signal Client]: connection handle created");

            foreach (var subscriber in subscribers)
                subscriber.Subscribe(connection);

            while (connection.State != HubConnectionState.Connected)
            {
                try
                {
                    if (connection.State == HubConnectionState.Connecting)
                    {
                        await UniTask.Delay(TimeSpan.FromSeconds(1));
                        continue;
                    }

                    Debug.Log("[Signal Client]: start connection");
                    await connection.StartAsync();
                    Debug.Log("[Signal Client]: connection finished");
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    await DisposeHub();
                    _signalInvoke = new NoConnectedInvoke();
                    return null;
                }
            }

            _signalInvoke = new SignalInvoke(connection);
            return connection;
        }

        private async UniTask DisposeHub()
        {
            foreach (var subscriber in subscribers)
                subscriber.Dispose();

            if (_hubConnection == null)
                return;

            await _hubConnection.DisposeAsync();
            _hubConnection = null;
        }
    }
}