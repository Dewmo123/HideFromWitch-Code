using System;
using System.Collections.Generic;

namespace DewmoLib.Utiles
{
    [Serializable]
    public class NotifyValue<T>
    {
        public delegate void ValueChangedHandler(T previousValue, T nextValue);
        public event ValueChangedHandler OnValueChanged;

        public IEqualityComparer<T> equalityComparer;

        private readonly bool _forceChange;

        private T _previousValue;
        private T _value;

        public T Value
        {
            get => _value;
            set
            {
                if (_forceChange || !equalityComparer.Equals(_value, value))
                {
                    _previousValue = _value;
                    _value = value;
                    OnValueChanged?.Invoke(_previousValue, _value);
                }
            }
        }

        public NotifyValue(T value = default(T), bool force = false, IEqualityComparer<T> equalityComparer = null)
        {
            this.equalityComparer = equalityComparer ?? EqualityComparer<T>.Default;

            _forceChange = force;
            _previousValue = default(T);
            _value = value;
        }
    }
}
