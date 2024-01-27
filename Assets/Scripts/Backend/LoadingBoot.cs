using System;
using UnityEngine;
using UnityEngine.Events;

namespace Backend
{
    public class LoadingBoot : MonoBehaviour
    {
        public SignalClient client;

        public UnityEvent onConnectedSuccess;

        public UnityEvent onConnectedFailure;

        private void Start()
        {
            if (client.IsConnected)
                Connected();
            else
                client.OnConnected += OnClientConnected;
        }

        private void OnClientConnected(bool success)
        {
            client.OnConnected -= OnClientConnected;
            if (success)
                Connected();
            else
                Failure();
        }

        private void Failure() => onConnectedFailure?.Invoke();

        private void Connected() => onConnectedSuccess?.Invoke();
    }
}