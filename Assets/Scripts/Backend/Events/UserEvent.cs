using Backend.Registration;
using UnityEngine;

namespace Backend.Events
{
    public class UserEvent : AbstractEvent<int, int>
    {
        protected override string MethodName => "users";

        protected override int ConvertToOutput(int input)
        {
            Debug.Log($"[USER EVENT]: User count now {input}");
            return input;
        }

        private void Start() =>
            SignalRegistration<UserEvent>.Register(this);

        private void OnDestroy() =>
            SignalRegistration<UserEvent>.Unregister();
    }
}