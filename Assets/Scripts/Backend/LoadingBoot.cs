using UnityEngine;
using UnityEngine.Events;

namespace Backend
{
    public class LoadingBoot : MonoBehaviour
    {
        public SignalClient client;

        public UnityEvent onConnectedSuccess;

        public UnityEvent onConnectedFailure;

        private void Start() =>
            StartLoading();

        public async void StartLoading()
        {
            bool connected = await client.Connect();
            OnClientConnected(connected);
        }


        private void OnClientConnected(bool success)
        {
            if (success)
                Connected();
            else
                Failure();
        }

        private void Failure() => onConnectedFailure?.Invoke();

        private void Connected() => onConnectedSuccess?.Invoke();
    }
}