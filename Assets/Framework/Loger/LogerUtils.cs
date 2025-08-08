using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 日志打印类
/// </summary>
public static class LogerUtils
{
    // 控制是否输出日志
    public static bool EnableLog = true; 
    /// <summary>
    /// 设置日志是否可用
    /// </summary>
    /// <param name="b"></param>
    public static void SetLogEnable(bool b)
    {
        EnableLog = b;
    }

    public static void Log(string message, Object context = null)
    {
        if (!EnableLog) return;
        if (context == null)
            Debug.Log("[LOG] " + message);
        else
            Debug.Log("[LOG] " + message, context);
    }

    public static void LogWarning(string message, Object context = null)
    {
        if (!EnableLog) return;
        if (context == null)
            Debug.LogWarning("[WARN] " + message);
        else
            Debug.LogWarning("[WARN] " + message, context);
    }

    public static void LogError(string message, Object context = null)
    {
        if (!EnableLog) return;
        if (context == null)
            Debug.LogError("[ERROR] " + message);
        else
            Debug.LogError("[ERROR] " + message, context);
    }
}
