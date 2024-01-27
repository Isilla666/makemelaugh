using UnityEngine;
using UnityEngine.SceneManagement;

public class Boot : MonoBehaviour
{
    public void LoadNextScene() => SceneManager.LoadScene("WaitUsersScene");
}