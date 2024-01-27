using System;
using Microsoft.AspNetCore.SignalR.Client;
using NativeWebSocket;
using UniRx.Async;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitUsersController : MonoBehaviour
{
    private string url2 = @"http://localhost:5232/game";

    HubConnection hubConnection;

    public void RunGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    async void Start()
    {
        hubConnection = await ConnectToHubAsync();
        await hubConnection.InvokeAsync("gameState", 1);
        await hubConnection.InvokeAsync("gameState", 2);
    }
    public async UniTask<HubConnection> ConnectToHubAsync()
    {
        Debug.Log("ConnectToHubAsync start");

        //Создаем соединение с нашим написанным тестовым хабом
        var connection = new HubConnectionBuilder()
            .WithUrl(url2)
            .WithAutomaticReconnect()
            .Build();
  
        Debug.Log("connection handle created");
  
        //подписываемся на сообщение от хаба, чтобы проверить подключение
        connection.On<int>("gameState",
            (id) => Debug.Log($"Res gameState: {id}"));
  
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
            }
        }
        return connection;
    }
    
    private async void OnApplicationQuit()
    {
        if(hubConnection != null)
        {
            await hubConnection.DisposeAsync();
        }
        hubConnection = null;
    }

    private async void OnDestroy()
    {
        if(hubConnection != null)
        {
            await hubConnection.DisposeAsync();
        }
        hubConnection = null;
    }
}
