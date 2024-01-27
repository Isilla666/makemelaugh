using Backend.Registration;
using UnityEngine;

namespace Backend.Events
{
    public class ActionEvent : AbstractEvent<object, ActionEvent.ActionData>
    {
        [System.Serializable]
        public struct ActionData
        {
            public int id;

            public int damage;

            public int team;
        }

        protected override string MethodName => "actionTeam";

        protected override ActionData ConvertToOutput(object actionData)
        {
            var json = actionData.ToString();
            var data = JsonUtility.FromJson<ActionData>(json);
            Debug.Log($"[ACTION EVENT]: Damage : {data.damage} from team {data.team} with id {data.id}");
            return data;
        }

        private void Start() =>
            SignalRegistration<ActionEvent>.Register(this);

        private void OnDestroy() =>
            SignalRegistration<ActionEvent>.Unregister();
    }

    [System.Serializable]
    public class JsonPayload
    {
        public string message;
    }
}