﻿using System.Diagnostics;
using UnityEngine;

namespace ConsoleLog
{
    public enum LogType
    {
        Debug,
        Info,
        Warning,
        Error
    }

    public static class Log
    {
        private static string GetLogPrefix(LogType logType)
        {
            return "[" + logType + "]";
        }

        private static Color GetLogColor(LogType logType)
        {
            switch (logType)
            {
                case LogType.Warning:
                    return Color.yellow;
                case LogType.Error:
                    return Color.red;
                default:
                    return Color.white;
            }
        }

        public static void ConsoleMessage(string message)
        {
            CommonOutput(null, LogType.Info, message);
        }

        public static void Info(string message)
        {
            CommonOutput(new StackTrace(true).GetFrame(1), LogType.Info, message);
        }

        public static void Debug(string message)
        {
            CommonOutput(new StackTrace(true).GetFrame(1), LogType.Debug, message);
        }

        public static void Warning(string message)
        {
            CommonOutput(new StackTrace(true).GetFrame(1), LogType.Warning, message);
        }

        public static void Error(string message)
        {
            CommonOutput(new StackTrace(true).GetFrame(1), LogType.Error, message);
        }

        private static void CommonOutput(StackFrame frame, LogType logType, string message)
        {
            ConsoleOutput(frame, GetLogPrefix(logType), GetLogColor(logType), message);
            UnityLogOutput(logType, GetLogString(frame, GetLogPrefix(logType), message));
        }

        private static void UnityLogOutput(LogType logType, string message)
        {
            switch (logType)
            {
                case LogType.Warning:
                    UnityEngine.Debug.LogWarning(message);
                    break;
                case LogType.Error:
                    UnityEngine.Debug.LogError(message);
                    break;
                default:
                    UnityEngine.Debug.Log(message);
                    break;
            }
        }

        private static void ConsoleOutput(StackFrame frame, string prefix, Color color, string message)
        {
            if (ConsoleController.Instance != null)
            {
                string s = "<color=#" + ColorUtility.ToHtmlStringRGBA(color) + ">" +
                           GetLogString(frame, prefix, message) + "</color>";
                ConsoleController.Instance.LogMessage(s);
            }
            else
            {
                UnityEngine.Debug.LogError("Console controller is NULL");
            }
        }

        private static string GetLogString(StackFrame frame, string prefix, string message)
        {
            if (frame != null)
            {
                string fullFileName = frame.GetFileName();
                if (!string.IsNullOrEmpty(fullFileName))
                {
                    string[] s = fullFileName.Split('/');
                    string fileName = s[s.Length - 1];
                    fileName = fileName.Remove(fileName.Length - 3, 3);

                    return "[" + Time.frameCount + "] " + prefix + " " +
                           fileName + "." + frame.GetMethod().Name + " (" + frame.GetFileLineNumber() + "): " + message;
                }

                return "[" + Time.frameCount + "] " + prefix + " " + frame.GetMethod().Name + " (" +
                       frame.GetFileLineNumber() + "): " + message;
            }
            return prefix + ": " + message;
        }
    }
}