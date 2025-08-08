using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // 各层级的父节点
    private Dictionary<UILayer, Transform> layerRoots = new Dictionary<UILayer, Transform>();
    // 已实例化UI
    private Dictionary<string, UIView> uiDict = new Dictionary<string, UIView>();
    // 已加载的AssetBundle缓存
    private Dictionary<string, AssetBundle> bundleCache = new Dictionary<string, AssetBundle>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitLayers();
        }
       
    }

    private void InitLayers()
    {
        foreach (UILayer layer in Enum.GetValues(typeof(UILayer)))
        {
            var go = new GameObject(layer.ToString());
            go.transform.SetParent(transform, false);
            layerRoots[layer] = go.transform;
        }
    }

    /// <summary>
    /// 打开UI（异步从AssetBundle加载）
    /// </summary>
    public async Task<T> OpenAsync<T>(object param = null) where T : UIView
    {
        string uiName = typeof(T).Name;
        if (!uiDict.TryGetValue(uiName, out UIView view) || view == null)
        {
            // 加载AssetBundle
            string bundlePath = GetBundlePath(uiName);
            AssetBundle bundle = await LoadBundleAsync(bundlePath);

            if (bundle == null)
            {
                Debug.LogError($"UI AssetBundle加载失败: {bundlePath}");
                return null;
            }
            // 加载Prefab
            GameObject prefab = bundle.LoadAsset<GameObject>(uiName);
            if (prefab == null)
            {
                Debug.LogError($"UI Prefab未找到: {uiName} in {bundlePath}");
                return null;
            }
            GameObject go = Instantiate(prefab, layerRoots[view.Layer]); // 先临时放在Common
            view = go.GetComponent<T>();
            if (view == null)
            {
                Debug.LogError($"UI脚本未挂载在Prefab: {uiName}");
                return null;
            }
            // 挂到指定层级下
            go.transform.SetParent(layerRoots[view.Layer], false);
            uiDict[uiName] = view;
        }
        // 重新设置层级（防止Prefab未指定正确层）
        view.transform.SetParent(layerRoots[view.Layer], false);
        await view.Open(param);
        return view as T;
    }

    /// <summary>
    /// 关闭UI
    /// </summary>
    public async Task CloseAsync<T>() where T : UIView
    {
        string uiName = typeof(T).Name;
        if (uiDict.TryGetValue(uiName, out UIView view) && view != null)
        {
            await view.Close();
            // 可选：销毁对象，卸载AssetBundle（可根据需求调整）
            // Destroy(view.gameObject);
            // uiDict.Remove(uiName);
        }
    }

    public void Refresh<T>(object param = null) where T : UIView
    {
        string uiName = typeof(T).Name;
        if (uiDict.TryGetValue(uiName, out UIView view) && view != null)
        {
            view.Refresh(param);
        }
    }

    /// <summary>
    /// 异步加载AssetBundle（含缓存）
    /// </summary>
    private async Task<AssetBundle> LoadBundleAsync(string bundlePath)
    {
        if (bundleCache.TryGetValue(bundlePath, out AssetBundle bundle) && bundle != null)
            return bundle;

        var req = AssetBundle.LoadFromFileAsync(bundlePath);
        while (!req.isDone) await Task.Yield();
        bundle = req.assetBundle;
        if (bundle != null)
            bundleCache[bundlePath] = bundle;
        return bundle;
    }

    /// <summary>
    /// 获取UI对应AssetBundle路径（可根据项目实际调整）
    /// </summary>
    private string GetBundlePath(string uiName)
    {
        // 假设所有UI Bundle都放在Application.streamingAssetsPath + /ui/ 下，Bundle名=界面名小写
        return $"{Application.streamingAssetsPath}/ui/{uiName.ToLower()}.bundle";
    }
}