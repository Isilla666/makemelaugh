using Backend.Registration;

namespace Backend.Events
{
    public class StateEvent : AbstractEvent<int, bool>
    {
        protected override string MethodName => "gameState";
        protected override bool ConvertToOutput(int input) => input == 1;
        
        private void Start() =>
            SignalRegistration<StateEvent>.Register(this);

        private void OnDestroy() =>
            SignalRegistration<StateEvent>.Unregister();
    }
}