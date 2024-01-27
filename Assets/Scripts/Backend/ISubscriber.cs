using System;
using Microsoft.AspNetCore.SignalR.Client;

namespace Backend
{
    public interface ISubscriber : IDisposable
    {
        void Subscribe(HubConnection connection);
    }
}