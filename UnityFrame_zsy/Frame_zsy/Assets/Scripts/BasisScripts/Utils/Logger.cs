using System;
using System.Diagnostics;
using System.Reflection;
using UnityEngine;

public class Logger
{

    public static bool EnableLogger = true;//是否激活

    /// <summary>
    /// 弃用
    /// </summary>
    private static string InitStr(string message)
    {
        StackTrace trace = new StackTrace();

        //获取是哪个类来调用的
        Type type = trace.GetFrame(1).GetMethod().DeclaringType;

        //获取是类中的那个方法调用的
        string method = trace.GetFrame(1).GetMethod().ToString();

        return string.Format("Type:[{0}]->  Method:[{1}]->  Message:[{2}]", type.ToString(), method, message);
    }

    private static string ToString(string typeStr, Type type, string method, object message)
    {
        return string.Format("   {0}   Type:[ {1} ]--->  Method:[ {2} ]--->  Message:[ {3} ]", typeStr, type.ToString(), method, message.ToString());
    }

    public static void Log(object message)
    {
        if (EnableLogger)
        {

            StackTrace trace = new StackTrace();

            Type type = trace.GetFrame(1).GetMethod().DeclaringType;

            string method = trace.GetFrame(1).GetMethod().ToString();

            UnityEngine.Debug.Log(ToString("Log:---->", type, method, message));
        }
    }

    public static void Log(object message, UnityEngine.Object context)
    {
        if (EnableLogger)
        {

            StackTrace trace = new StackTrace();

            Type type = trace.GetFrame(1).GetMethod().DeclaringType;

            string method = trace.GetFrame(1).GetMethod().ToString();

            UnityEngine.Debug.Log(ToString("Log:---->", type, method, message), context);
        }
    }

    public static void LogError(object message)
    {
        if (EnableLogger)
        {

            StackTrace trace = new StackTrace();

            Type type = trace.GetFrame(1).GetMethod().DeclaringType;

            string method = trace.GetFrame(1).GetMethod().ToString();

            UnityEngine.Debug.LogError(ToString("Error:---->", type, method, message));
        }
    }

    public static void LogError(object message, UnityEngine.Object context)
    {
        if (EnableLogger)
        {

            StackTrace trace = new StackTrace();

            Type type = trace.GetFrame(1).GetMethod().DeclaringType;

            string method = trace.GetFrame(1).GetMethod().ToString();

            UnityEngine.Debug.LogError(ToString("Error:---->", type, method, message), context);
        }
    }

    private static void LogErrorFormat(string format, params object[] args)
    {
        if (EnableLogger) UnityEngine.Debug.LogErrorFormat(format, args);
    }

    private static void LogErrorFormat(UnityEngine.Object context,
                        string format, params object[] args)
    {
        if (EnableLogger) UnityEngine.Debug.LogErrorFormat(context, format, args);
    }

    private static void LogException(Exception exception)
    {
        if (EnableLogger) UnityEngine.Debug.LogException(exception);
    }

    private static void LogException(Exception exception, UnityEngine.Object context)
    {
        if (EnableLogger) UnityEngine.Debug.LogException(exception, context);
    }

    private static void LogFormat(string format, params object[] args)
    {
        if (EnableLogger) UnityEngine.Debug.LogFormat(format, args);
    }

    private static void LogFormat(UnityEngine.Object context, string format, params object[] args)
    {
        if (EnableLogger) UnityEngine.Debug.LogFormat(context, format, args);
    }

    public static void LogWarning(object message)
    {
        if (EnableLogger)
        {
            StackTrace trace = new StackTrace();

            Type type = trace.GetFrame(1).GetMethod().DeclaringType;

            string method = trace.GetFrame(1).GetMethod().ToString();

            UnityEngine.Debug.LogWarning(ToString("Warning:---->", type, method, message));
        }
    }

    public static void LogWarning(object message, UnityEngine.Object context)
    {
        if (EnableLogger)
        {

            StackTrace trace = new StackTrace();

            Type type = trace.GetFrame(1).GetMethod().DeclaringType;

            string method = trace.GetFrame(1).GetMethod().ToString();

            UnityEngine.Debug.LogWarning(ToString("Warning:---->", type, method, message), context);
        }
    }

    private static void LogWarningFormat(string format, params object[] args)
    {
        if (EnableLogger) UnityEngine.Debug.LogWarningFormat(format, args);
    }

    private static void LogWarningFormat(UnityEngine.Object context, string format, params object[] args)
    {
        if (EnableLogger) UnityEngine.Debug.LogWarningFormat(context, format, args);
    }

}



