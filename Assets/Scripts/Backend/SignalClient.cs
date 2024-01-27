using System;
using Microsoft.AspNetCore.SignalR.Client;
using UniRx.Async;
using UnityEngine;

namespace Backend
{
    public class SignalClient : MonoBehaviour
    {
#if DEBUG
        private const string BackendUrl = @"http://localhost:5555/game";
#else
        private const string BackendUrl = @"http://158.160.131.200:5555/game";
#endif

        public SubscriberMono[] subscribers;

        private HubConnection _hubConnection;

        private async void Start() =>
            _hubConnection = await CreateConnectionAsync();

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

            Debug.Log("connection handle created");

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

                    Debug.Log("start connection");
                    await connection.StartAsync();
                    Debug.Log("connection finished");
                }
                catch (Exception e)
                {
                    Debug.LogException(e);
                    await DisposeHub();
                    return null;
                }
            }

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