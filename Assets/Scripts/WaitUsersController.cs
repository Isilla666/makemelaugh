using System;
using Backend.Events;
using Backend.Invoker;
using Backend.Registration;
using Behaviours;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WaitUsersController : MonoBehaviour
{
    public TMP_Text activeUserText;
    public Button startGameButton;
    public GameStateController gameStateController;

    private ISignalListener<int> _activeUsers;
    private ISignalInvoke _signalInvoke;

    private async void Start()
    {
        _signalInvoke = SignalRegistration<ISignalInvoke>.Resolve();
        _activeUsers = SignalRegistration<UserEvent>.Resolve();
        _activeUsers.OnValueChanged += OnUserCountChanged;
        startGameButton.interactable = !_signalInvoke.WithConnection;
        OnUserCountChanged(0);
        
        await gameStateController.ChangeStateTo(GameStateController.GameState.WaitUser);
    }

    private void OnDestroy() =>
        _activeUsers.OnValueChanged -= OnUserCountChanged;

    private void OnUserCountChanged(int userCount)
    {
        activeUserText.text = $"{userCount}";
        startGameButton.interactable = !_signalInvoke.WithConnection || userCount >= 2;
    }


    public async void RunGame()
    {
        bool res = await gameStateController.ChangeStateTo(GameStateController.GameState.Start);

        if (res)
            SceneManager.LoadScene("MainScene");
    }
}