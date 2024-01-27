using Behaviours;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WaitUsersController : MonoBehaviour
{
    public GameStateController gameStateController;

    private async void Start() =>
        await gameStateController.ChangeStateTo(GameStateController.GameState.WaitUser);


    public async void RunGame()
    {
        bool res = await gameStateController.ChangeStateTo(GameStateController.GameState.Start);

        if (res)
            SceneManager.LoadScene("MainScene");
    }
}