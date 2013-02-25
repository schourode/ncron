using System;
using NCron.Logging;
using NLog;

namespace NCron.Integration.NLog
{
    /// <summary>
    /// Implements the <see cref="Logging.ILogFactory"/> interface using "NLog" as log provider.
    /// </summary>
    public class NLogFactory : ILogFactory
    {
        public ILog GetLogForJob(ICronJob job)
        {
            var nlogger = LogManager.GetLogger(job.GetType().FullName);
            return new LogAdapter(nlogger);
        }

        internal class LogAdapter : ILog
        {
            private readonly Logger _log;

            public LogAdapter(Logger log)
            {
                _log = log;
            }

            public void Debug(Func<string> msgCallback)
            {
                if (_log.IsDebugEnabled)
                    _log.Debug(msgCallback());
            }

            public void Debug(Func<string> msgCallback, Func<Exception> exCallback)
            {
                if (_log.IsDebugEnabled)
                    _log.Debug(msgCallback(), exCallback());
            }

            public void Info(Func<string> msgCallback)
            {
                if (_log.IsInfoEnabled)
                    _log.Info(msgCallback());
            }

            public void Info(Func<string> msgCallback, Func<Exception> exCallback)
            {
                if (_log.IsInfoEnabled)
                    _log.Info(msgCallback(), exCallback());
            }

            public void Warn(Func<string> msgCallback)
            {
                if (_log.IsWarnEnabled)
                    _log.Warn(msgCallback());
            }

            public void Warn(Func<string> msgCallback, Func<Exception> exCallback)
            {
                if (_log.IsWarnEnabled)
                    _log.Warn(msgCallback(), exCallback());
            }

            public void Error(Func<string> msgCallback)
            {
                if (_log.IsErrorEnabled)
                    _log.Error(msgCallback());
            }

            public void Error(Func<string> msgCallback, Func<Exception> exCallback)
            {
                if (_log.IsErrorEnabled)
                    _log.Error(msgCallback(), exCallback());
            }

            public void Dispose()
            {
            }
        }
    }
}