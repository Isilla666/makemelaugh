using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

namespace Backend
{
    public abstract class SubscriberMono : MonoBehaviour, ISubscriber
    {
        public abstract void Subscribe(HubConnection connection);

        public abstract void Dispose();
    }
}