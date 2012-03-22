// A variation of http://dhickey.ie/post/2011/09/01/SystemClock.aspx

using System;

namespace Tests.NCron
{
    public class SystemClock
    {
        private DateTime? _systemUtcTime;
        private readonly DateTime _start;
        private int _lastTicks = -1;
        private DateTime _lastUtcDateTime = DateTime.MinValue;

        public SystemClock(DateTime systemUtcTimeStartTime)
        {
            _start = DateTime.UtcNow;
            _systemUtcTime = systemUtcTimeStartTime;
            _lastTicks = -1;
        }

        public DateTime UtcNow
        {
            get
            {
                int tickCount = Environment.TickCount;
                if (tickCount == _lastTicks)
                {
                    return _lastUtcDateTime;
                }
                if (_systemUtcTime == null)
                {
                    _lastUtcDateTime = DateTime.UtcNow;
                }
                else
                {
                    var progressed = (DateTime.UtcNow - _start);
                    _lastUtcDateTime = _systemUtcTime.Value + progressed;
                }
                _lastTicks = tickCount;
                return _lastUtcDateTime;
            }
        }
    }
}