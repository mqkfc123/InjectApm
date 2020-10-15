
using System;

namespace SkyApm.Logging
{
    /// <summary>
    /// 日志等级。
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// 追踪。
        /// </summary>
        Trace,

        /// <summary>
        /// 调试。
        /// </summary>
        Debug,

        /// <summary>
        /// 信息。
        /// </summary>
        Information,

        /// <summary>
        /// 警告。
        /// </summary>
        Warning,

        /// <summary>
        /// 错误。
        /// </summary>
        Error,

        /// <summary>
        /// 致命错误。
        /// </summary>
        Fatal
    }


    public interface ILogger
    {
        void Debug(string message);

        void Information(string message);

        void Warning(string message);

        void Error(string message, Exception exception);

        void Trace(string message);
    }
}