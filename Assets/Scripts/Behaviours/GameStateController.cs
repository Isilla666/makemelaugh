using System;
using Backend.Events;
using Backend.Invoker;
using Backend.Registration;
using UniRx.Async;
using UnityEngine;

namespace Behaviours
{
    public class GameStateController : MonoBehaviour
    {
        private ISignalListener<bool> _stateChangeListener;
        private ISignalInvoke _signalInvoke;

        public static GameState CurrentState { get; private set; } = GameState.WaitUser;

        private bool? _nextChangedState;

        private void Awake()
        {
            _signalInvoke = SignalRegistration<ISignalInvoke>.Resolve();
            _stateChangeListener = SignalRegistration<StateEvent>.Resolve();
            _stateChangeListener.OnValueChanged += StateChangeListenerOnOnValueChanged;
        }

        private void OnDestroy()
        {
            _stateChangeListener.OnValueChanged -= StateChangeListenerOnOnValueChanged;
        }


        public async UniTask<bool> ChangeStateTo(GameState state)
        {
            if (!_signalInvoke.WithConnection)
            {
                CurrentState = state;
                return true;
            }

            _nextChangedState = true;
            await _signalInvoke.SendCommandToChangeState((int) state);

            while (!_nextChangedState.HasValue)
                await UniTask.Delay(TimeSpan.FromMilliseconds(3));

            CurrentState = _nextChangedState.Value ? state : CurrentState;
            return _nextChangedState.Value;
        }

        private void StateChangeListenerOnOnValueChanged(bool isChanged) =>
            _nextChangedState = isChanged;

        public enum GameState
        {
            WaitUser = 1,
            Start = 2,
            End = 3,
        }
    }
}