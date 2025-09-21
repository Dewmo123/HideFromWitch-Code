using System;

namespace Server.Utiles
{
    public class CountTimeSync
    {
        private int _endTime;
        private float _currentTime;
        private int _timeInterval;
        private Action<double> _elapsed;
        private Action _callback;
        public bool IsRunning { get; private set; }
        public CountTimeSync(Action<double> elapsed, Action callback, int endTime, int interval)
        {
            _callback = callback;
            _elapsed = elapsed;
            _timeInterval = interval;
            _endTime = endTime;
            IsRunning = false;
        }
        public CountTimeSync(Action<double> elapsed, Action callback, int interval)
        {
            _callback = callback;
            _elapsed = elapsed;
            _timeInterval = interval;
            IsRunning = false;
        }
        public void StartCount(int endTime)
        {
            _endTime = endTime;
            _currentTime = 0;
            IsRunning = true;
        }
        public void UpdateDeltaTime()
        {
            if (IsRunning)
            {
                _currentTime += Time.deltaTime;
                if (_currentTime * 1000%_timeInterval <= 30)
                    HandleTimerElapsed();
            }
        }
        private void HandleTimerElapsed()
        {
            _elapsed?.Invoke(_currentTime);
            if (_currentTime > _endTime)
            {
                _currentTime = 0;
                IsRunning = false;
                _callback?.Invoke();
            }
        }
        public void Abort(bool invokeCallback = false)
        {
            IsRunning = false;
            _currentTime = 0;
            if (invokeCallback)
                _callback?.Invoke();
        }
    }
}
