using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// 资源管理器
/// </summary>
public static class ResMangaer
{
    // AssetBundle缓存
    private static readonly Dictionary<string, AssetBundle> bundleCache = new();

    public static T loadAssets<T>(string path, string bundleName) where T : UnityEngine.Object
    {
        // 检查参数是否有效
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        // 检查缓存中是否存在指定的AssetBundle
        if (bundleCache.TryGetValue(bundleName, out AssetBundle bundle))
        {
            // 从AssetBundle中加载资源
            T asset = bundle.LoadAsset<T>(path);
            if (asset != null)
            {
                return asset;
            }
        }

        //加载bundle
        string bundlePath = getBundlePath(bundleName);
        if (!string.IsNullOrEmpty(bundlePath))
        {
#if UNITY_EDITOR
                
            // 在编辑器中，直接从指定路径加载AssetBundle
            AssetBundle newBundle = AssetDatabase.LoadAssetAtPath<AssetBundle>(bundlePath);
            if (newBundle != null)
            {
                bundleCache[bundleName] = newBundle;
                // 从新加载的AssetBundle中获取资源
                T asset = newBundle.LoadAsset<T>(path);
                return asset;
            }
#else
            AssetBundle newBundle = LoadBundle(bundlePath);
            if (newBundle != null)
            {
                bundleCache[bundleName] = newBundle;
                T asset = newBundle.LoadAsset<T>(path);
                return asset;
            }
#endif

        }

        return null;
    }

     public static string getBundlePath(string bundleName)
    {
        //根据bundleName构建路径，这里假设所有AssetBundle都在Assets/AssetBundles目录下
        #if UNITY_EDITOR
                return $"{Application.dataPath}/AssetBundles/{bundleName}";
        #else
                //先去热更新目录下面找，再到StreamingAssets目录下面找
                string path = $"{Application.persistentDataPath}/AssetBundles/{bundleName}";
                if (!File.Exists(path))
                { 
                    path = $"{Application.streamingAssetsPath}/AssetBundles/{bundleName}";
                }
                return path;

        #endif
    }

    /// <summary>
    /// 加载AssetBundle
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static AssetBundle LoadBundle(string path)
    {
        return AssetBundle.LoadFromFile(path);

    }
    /// <summary>
    /// 卸载单个bundle
    /// </summary>
    /// <param name="bundleName"></param>
    public static void unloadBundle(string bundleName)
    {
        if (bundleCache.TryGetValue(bundleName, out AssetBundle bundle))
        {
            bundle.Unload(true);
            bundleCache.Remove(bundleName);
        }
    }
    /// <summary>
    /// 卸载所有bundle
    /// </summary>
    public static void unloadAllBundles()
    {
        foreach (var bundle in bundleCache.Values)
        {
            bundle.Unload(true);
        }
        bundleCache.Clear();
    }
}