using System;
using Backend.Events;
using JetBrains.Annotations;

namespace Backend.Registration
{
    public static class SignalRegistration<T>
    {
        [CanBeNull] private static T _implementation;

        internal static void Register(T impl) => _implementation = impl;

        internal static void Unregister() => _implementation = default;
        
        public static T Resolve() =>
            _implementation ?? throw new NotImplementedException($"No implementation for {typeof(T)}");
    }
}