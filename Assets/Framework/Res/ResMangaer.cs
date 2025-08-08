using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ��Դ������
/// </summary>
public static class ResMangaer
{
    // AssetBundle����
    private static readonly Dictionary<string, AssetBundle> bundleCache = new();

    public static T loadAssets<T>(string path, string bundleName) where T : UnityEngine.Object
    {
        // �������Ƿ���Ч
        if (string.IsNullOrEmpty(path))
        {
            return null;
        }

        // ��黺�����Ƿ����ָ����AssetBundle
        if (bundleCache.TryGetValue(bundleName, out AssetBundle bundle))
        {
            // ��AssetBundle�м�����Դ
            T asset = bundle.LoadAsset<T>(path);
            if (asset != null)
            {
                return asset;
            }
        }

        //����bundle
        string bundlePath = getBundlePath(bundleName);
        if (!string.IsNullOrEmpty(bundlePath))
        {
#if UNITY_EDITOR
                
            // �ڱ༭���У�ֱ�Ӵ�ָ��·������AssetBundle
            AssetBundle newBundle = AssetDatabase.LoadAssetAtPath<AssetBundle>(bundlePath);
            if (newBundle != null)
            {
                bundleCache[bundleName] = newBundle;
                // ���¼��ص�AssetBundle�л�ȡ��Դ
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
        //����bundleName����·���������������AssetBundle����Assets/AssetBundlesĿ¼��
        #if UNITY_EDITOR
                return $"{Application.dataPath}/AssetBundles/{bundleName}";
        #else
                //��ȥ�ȸ���Ŀ¼�����ң��ٵ�StreamingAssetsĿ¼������
                string path = $"{Application.persistentDataPath}/AssetBundles/{bundleName}";
                if (!File.Exists(path))
                { 
                    path = $"{Application.streamingAssetsPath}/AssetBundles/{bundleName}";
                }
                return path;

        #endif
    }

    /// <summary>
    /// ����AssetBundle
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static AssetBundle LoadBundle(string path)
    {
        return AssetBundle.LoadFromFile(path);

    }
    /// <summary>
    /// ж�ص���bundle
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
    /// ж������bundle
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