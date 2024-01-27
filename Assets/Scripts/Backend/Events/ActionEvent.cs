using Backend.Registration;
using UnityEngine;

namespace Backend.Events
{
    public class ActionEvent : AbstractEvent<string, ActionEvent.ActionData>
    {
        [System.Serializable]
        public struct ActionData
        {
            public int Id;

            public int Damage;

            public int Team;
        }

        protected override string MethodName => "gameState";

        protected override ActionData ConvertToOutput(string input)
        {
            var actionData = JsonUtility.FromJson<ActionData>(input);
            Debug.Log($"ac {actionData.Id}, {actionData.Team}, {actionData.Damage}");
            return actionData;
        }

        private void Start() =>
            SignalRegistration<ActionEvent>.Register(this);

        private void OnDestroy() =>
            SignalRegistration<ActionEvent>.Unregister();
    }
}