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
    //// 已加载的AssetBundle缓存
    //private Dictionary<string, AssetBundle> bundleCache = new Dictionary<string, AssetBundle>();

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
    public T OpenAsync<T>(UIConfig param = null) where T : UIView
    {
        string prefab = param.Prefab;
        if (!uiDict.TryGetValue(prefab, out UIView view) || view == null)
        {
            // 加载AssetBundle
            string bundlePath = param.Bundle;
            AssetBundle bundle = ResMangaer.LoadBundle(bundlePath);

            if (bundle == null)
            {
                Debug.LogError($"UI AssetBundle加载失败: {bundlePath}");
                return null;
            }
            // 加载Prefab
            GameObject gameObj = bundle.LoadAsset<GameObject>(prefab);
            if (gameObj == null)
            {
                Debug.LogError($"UI Prefab未找到: {prefab} in {prefab}");
                return null;
            }
            GameObject go = Instantiate(gameObj, layerRoots[view.Layer]); // 先临时放在Common
            view = go.GetComponent<T>();
            if (view == null)
            {
                Debug.LogError($"UI脚本未挂载在Prefab: {prefab}");
                return null;
            }
            // 挂到指定层级下
            go.transform.SetParent(layerRoots[view.Layer], false);
            uiDict[prefab] = view;
        }
        // 重新设置层级（防止Prefab未指定正确层）
        view.transform.SetParent(layerRoots[view.Layer], false);
        view.Open(param);
        return view as T;
    }

    /// <summary>
    /// 关闭UI
    /// </summary>
    public void Close<T>() where T : UIView
    {
        string uiName = typeof(T).Name;
        if (uiDict.TryGetValue(uiName, out UIView view) && view != null)
        {
             view.Close();
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




}