using Backend.Registration;

namespace Backend.Events
{
    public class UserEvent : AbstractEvent<int, int>
    {
        protected override string MethodName => "users";

        protected override int ConvertToOutput(int input) => input;
        
        private void Start() =>
            SignalRegistration<UserEvent>.Register(this);

        private void OnDestroy() =>
            SignalRegistration<UserEvent>.Unregister();
    }
}