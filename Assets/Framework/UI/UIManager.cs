using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    // ���㼶�ĸ��ڵ�
    private Dictionary<UILayer, Transform> layerRoots = new Dictionary<UILayer, Transform>();
    // ��ʵ����UI
    private Dictionary<string, UIView> uiDict = new Dictionary<string, UIView>();
    // �Ѽ��ص�AssetBundle����
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
    /// ��UI���첽��AssetBundle���أ�
    /// </summary>
    public async Task<T> OpenAsync<T>(object param = null) where T : UIView
    {
        string uiName = typeof(T).Name;
        if (!uiDict.TryGetValue(uiName, out UIView view) || view == null)
        {
            // ����AssetBundle
            string bundlePath = GetBundlePath(uiName);
            AssetBundle bundle = await LoadBundleAsync(bundlePath);

            if (bundle == null)
            {
                Debug.LogError($"UI AssetBundle����ʧ��: {bundlePath}");
                return null;
            }
            // ����Prefab
            GameObject prefab = bundle.LoadAsset<GameObject>(uiName);
            if (prefab == null)
            {
                Debug.LogError($"UI Prefabδ�ҵ�: {uiName} in {bundlePath}");
                return null;
            }
            GameObject go = Instantiate(prefab, layerRoots[view.Layer]); // ����ʱ����Common
            view = go.GetComponent<T>();
            if (view == null)
            {
                Debug.LogError($"UI�ű�δ������Prefab: {uiName}");
                return null;
            }
            // �ҵ�ָ���㼶��
            go.transform.SetParent(layerRoots[view.Layer], false);
            uiDict[uiName] = view;
        }
        // �������ò㼶����ֹPrefabδָ����ȷ�㣩
        view.transform.SetParent(layerRoots[view.Layer], false);
        await view.Open(param);
        return view as T;
    }

    /// <summary>
    /// �ر�UI
    /// </summary>
    public async Task CloseAsync<T>() where T : UIView
    {
        string uiName = typeof(T).Name;
        if (uiDict.TryGetValue(uiName, out UIView view) && view != null)
        {
            await view.Close();
            // ��ѡ�����ٶ���ж��AssetBundle���ɸ������������
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
    /// �첽����AssetBundle�������棩
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
    /// ��ȡUI��ӦAssetBundle·�����ɸ�����Ŀʵ�ʵ�����
    /// </summary>
    private string GetBundlePath(string uiName)
    {
        // ��������UI Bundle������Application.streamingAssetsPath + /ui/ �£�Bundle��=������Сд
        return $"{Application.streamingAssetsPath}/ui/{uiName.ToLower()}.bundle";
    }
}