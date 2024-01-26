using System;
using NativeWebSocket;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitUsersController : MonoBehaviour
{
    private string url = @"wss://remote-api.playstand.ru/pubsub";

    WebSocket websocket;

    public void RunGame()
    {
        SceneManager.LoadScene("MainScene");
    }
    async void Start()
    {
        //websocket = new WebSocket("wss://echo.websocket.org");
        websocket = new WebSocket(url);

        websocket.OnOpen += () =>
        {
            Debug.Log("Connection open!");
        };

        websocket.OnError += (e) =>
        {
            Debug.Log("Error! " + e);
        };

        websocket.OnClose += (e) =>
        {
            Debug.Log("Connection closed!");
        };

        websocket.OnMessage += (bytes) =>
        {
            // Reading a plain text message
            var message = System.Text.Encoding.UTF8.GetString(bytes);
            Debug.Log("Received OnMessage! (" + bytes.Length + " bytes) " + message);
        };

        // Keep sending messages at every 0.3s
        //InvokeRepeating(nameof(SendWebSocketMessage), 0.0f, 0.3f);
        await websocket.Connect();
    }

    void Update()
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if(websocket != null)
        {
            websocket.DispatchMessageQueue();
        }
#endif
    }

    async void SendWebSocketMessage()
    {
        if (websocket.State == WebSocketState.Open)
        {
            // Sending bytes
            await websocket.Send(new byte[] { 10, 20, 30 });

            // Sending plain text
            await websocket.SendText("plain text message");
        }
    }

    private async void OnApplicationQuit()
    {
        if(websocket != null)
        {
            await websocket.Close();
        }
        websocket = null;
    }

    private async void OnDestroy()
    {
        if(websocket != null)
        {
            await websocket.Close();
        }
        websocket = null;
    }
}
