using System;
using UnityEngine;

namespace Utility.Unity.Common
{
    public class DelayInvoker<T>
    {
        public bool IsCompleted => _targetCount > 0 && _currentCount >= _targetCount;

        private readonly Action<T> _callbackAction;
        private readonly float _delayTime;
        private readonly int _targetCount;
        private readonly T _genericValue;

        private float _lastInvokeTime;
        private int _currentCount;
        private bool _paused;


        /// <summary>
        /// DelayInvoker 생성자
        /// </summary>
        /// <param name="action">실행될 action</param>
        /// <param name="value">action의 값 Type</param>
        /// <param name="delayTime">지연 시간</param>
        /// <param name="repeatCount">반복 횟수 (기본값:무한반복)</param>
        /// <exception cref="ArgumentNullException">action이 null일 경우 발생</exception>
        public DelayInvoker(Action<T> action, T value, float delayTime, int repeatCount = -1)
        {
            _callbackAction = action ?? throw new ArgumentNullException(nameof(action));
            _delayTime = delayTime;
            _targetCount = repeatCount;
            _genericValue = value;

            _lastInvokeTime = Time.time;
            _currentCount = 0;
            _paused = false;
        }

        /// <summary>
        /// DelayInvoker의 틱. Update에서 호출
        /// </summary>
        public void Tick()
        {
            if (_paused || IsCompleted)
                return;

            if (Time.time - _lastInvokeTime >= _delayTime)
            {
                _lastInvokeTime = Time.time;
                _callbackAction?.Invoke(_genericValue);
                _currentCount++;
            }
        }

        public void Pause() => _paused = true;
        public void Resume() => _paused = false;
    }
}
