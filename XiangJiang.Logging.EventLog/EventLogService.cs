using System;
using System.Diagnostics;
using System.Text;
using System.Threading;
using XiangJiang.Common;
using XiangJiang.Logging.Abstractions;

namespace XiangJiang.Logging.EventLog
{
    public sealed class EventLogService : ILogService
    {
        private readonly string _eventSource;
        private readonly string _logName = "Application";

        public EventLogService(string eventSource)
        {
            _eventSource = eventSource ?? throw new ArgumentNullException(nameof(_eventSource));
            if (!System.Diagnostics.EventLog.SourceExists(eventSource))
                System.Diagnostics.EventLog.CreateEventSource(eventSource, _logName);
        }

        public EventLogService() : this("Application")
        {
        }

        public void Debug(string message)
        {
            CreateEventLog(message, EventLogEntryType.SuccessAudit);
        }

        public void Dispose()
        {
        }

        public void Error(string message)
        {
            CreateEventLog(message, EventLogEntryType.Error);
        }

        public void Error(string message, Exception ex)
        {
            CreateEventLog(message, EventLogEntryType.Error, ex);
        }

        public void Fatal(string message)
        {
            CreateEventLog(message, EventLogEntryType.FailureAudit);
        }

        public void Fatal(string message, Exception ex)
        {
            CreateEventLog(message, EventLogEntryType.FailureAudit, ex);
        }

        public void Info(string message)
        {
            CreateEventLog(message, EventLogEntryType.Information);
        }

        public void Warn(string message)
        {
            CreateEventLog(message, EventLogEntryType.Warning);
        }

        private void CreateEventLog(string message, EventLogEntryType logEntryType, Exception ex = null)
        {
            string logContent;

            if (ex != null)
            {
                var exceptionMsg = ex.Format(message, true);
                var builder = new StringBuilder();
                builder.AppendLine(exceptionMsg.UserMessage);
                builder.AppendLine(exceptionMsg.ActualMessage);
                builder.AppendLine(exceptionMsg.StackTrace);
                logContent = builder.ToString();
            }
            else
            {
                logContent = message;
            }

            using (var log = new System.Diagnostics.EventLog(_logName)
            {
                Source = _eventSource
            })
            {
                log.WriteEntry(logContent, logEntryType, Thread.CurrentThread.ManagedThreadId);
            }
        }
    }
}