using System;
using UnityEngine;

namespace UnifiedNetwork
{
    public class UnifiedLogger
    {
        private static int loggerMode = (int)LoggerMode.WARNING;
#if UNITY_STANDALONE && !UNITY_EDITOR
        private static string dataPath = "./UnifiedNetworkLog.txt";
#endif

        #region Log methods
        [System.Diagnostics.Conditional("UF_LOG_ENABLE")]
        public static void Trace(string title, string message)
        {
            if (IsCanLogWithMode(LoggerMode.ALL))
            {
                Log(title, message, LoggerMode.ALL);
            }
        }

        [System.Diagnostics.Conditional("UF_LOG_ENABLE")]
        public static void Info(string title, string message)
        {
            if (IsCanLogWithMode(LoggerMode.INFO))
            {
                Log(title, message, LoggerMode.INFO);
            }
        }

        [System.Diagnostics.Conditional("UF_LOG_ENABLE")]
        public static void Warn(string title, string message)
        {
            if (IsCanLogWithMode(LoggerMode.WARNING))
            {
                Log(title, message, LoggerMode.WARNING);
            }
        }

        [System.Diagnostics.Conditional("UF_LOG_ENABLE")]
        public static void Error(string title, string message)
        {
            if (IsCanLogWithMode(LoggerMode.ERROR))
            {
                Log(title, message, LoggerMode.ERROR);
            }
        }

        [System.Diagnostics.Conditional("UF_LOG_ENABLE")]
        public static void Exception(string title, Exception e)
        {
            if (IsCanLogWithMode(LoggerMode.ERROR))
            {
                Log(title, e.ToString(), LoggerMode.ERROR);
            }
        }
        #endregion

        #region Utils
        public static void SetMode(LoggerMode mode)
        {
            loggerMode = (int)mode;
        }

        public static LoggerMode GetMode()
        {
            return (LoggerMode)loggerMode;
        }

        private static bool IsCanLogWithMode(LoggerMode mode)
        {
            if (loggerMode >= (int)mode)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static void Log(string title, string message, LoggerMode mode)
        {
#if UNITY_STANDALONE && !UNITY_EDITOR
            try
            {
                DateTime now = DateTime.Now;
                string content = string.Format("[{0}][{1} {2}] {3}{4}{5}",
                    mode, now.ToShortDateString(), now.ToShortTimeString(), title, message, Environment.NewLine);

                System.IO.File.AppendAllText(dataPath, content);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
#else
            switch (mode)
            {
                case LoggerMode.ERROR:
                    Debug.LogError(title + message);
                    break;

                case LoggerMode.WARNING:
                    Debug.LogWarning(title + message);
                    break;

                default:
                    Debug.Log(title + message);
                    break;
            }
#endif
        }
        #endregion
    }
}