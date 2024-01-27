using System;
using Microsoft.AspNetCore.SignalR.Client;
using UnityEngine;

namespace Backend
{
    public abstract class SubscriberMono : MonoBehaviour, ISubscriber
    {
        protected IDisposable DisposableListener { get; set; }
        
        public abstract void Subscribe(HubConnection connection);

        public virtual void Dispose() => DisposableListener?.Dispose();
    }
}