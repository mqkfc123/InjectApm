using System;
using System.Collections.Generic;
using System.Text;

namespace SkyApm.Abstractions.Context.Utils
{

    public static class ExceptionExtensions
    {
        public static string ConvertToString(Exception exception, int maxLength)
        {
            var message = new StringBuilder();

            while (exception != null)
            {
                bool overMaxLength;
                message.Append(exception.Message);

                PrintStackFrame(message, exception.StackTrace, maxLength, out overMaxLength);

                if (overMaxLength)
                {
                    break;
                }

                exception = exception.InnerException;
            }

            return message.ToString();
        }

        private static void PrintStackFrame(StringBuilder message, string stackTrace,
            int maxLength, out bool overMaxLength)
        {
            message.AppendLine(stackTrace);
            overMaxLength = message.Length > maxLength;
        }
    }
}
