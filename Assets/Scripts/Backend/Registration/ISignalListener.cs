using UnityEngine.Events;

namespace Backend.Registration
{
    public interface ISignalListener<T>
    {
        public event UnityAction<T> OnValueChanged;
    }
}