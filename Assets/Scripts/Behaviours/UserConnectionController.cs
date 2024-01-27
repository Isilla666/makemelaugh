using Backend.Events;
using Backend.Registration;
using UnityEngine;
using UnityEngine.UI;

namespace Behaviours
{
    public class UserConnectionController : MonoBehaviour
    {
        public Text userCountText;

        private ISignalListener<int> _userCountEvent;

        private void Start()
        {
            _userCountEvent = SignalRegistration<UserEvent>.Resolve();
            _userCountEvent.OnValueChanged += OnUserChanged;
            OnUserChanged(0);
        }

        private void OnDestroy() =>
            _userCountEvent.OnValueChanged -= OnUserChanged;

        private void OnUserChanged(int count) =>
            userCountText.text = $"{count}";
    }
}