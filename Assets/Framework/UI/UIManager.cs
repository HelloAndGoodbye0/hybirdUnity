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
    //// �Ѽ��ص�AssetBundle����
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
    /// ��UI���첽��AssetBundle���أ�
    /// </summary>
    public T OpenAsync<T>(UIConfig param = null) where T : UIView
    {
        string prefab = param.Prefab;
        if (!uiDict.TryGetValue(prefab, out UIView view) || view == null)
        {
            // ����AssetBundle
            string bundlePath = param.Bundle;
            AssetBundle bundle = ResMangaer.LoadBundle(bundlePath);

            if (bundle == null)
            {
                Debug.LogError($"UI AssetBundle����ʧ��: {bundlePath}");
                return null;
            }
            // ����Prefab
            GameObject gameObj = bundle.LoadAsset<GameObject>(prefab);
            if (gameObj == null)
            {
                Debug.LogError($"UI Prefabδ�ҵ�: {prefab} in {prefab}");
                return null;
            }
            GameObject go = Instantiate(gameObj, layerRoots[view.Layer]); // ����ʱ����Common
            view = go.GetComponent<T>();
            if (view == null)
            {
                Debug.LogError($"UI�ű�δ������Prefab: {prefab}");
                return null;
            }
            // �ҵ�ָ���㼶��
            go.transform.SetParent(layerRoots[view.Layer], false);
            uiDict[prefab] = view;
        }
        // �������ò㼶����ֹPrefabδָ����ȷ�㣩
        view.transform.SetParent(layerRoots[view.Layer], false);
        view.Open(param);
        return view as T;
    }

    /// <summary>
    /// �ر�UI
    /// </summary>
    public void Close<T>() where T : UIView
    {
        string uiName = typeof(T).Name;
        if (uiDict.TryGetValue(uiName, out UIView view) && view != null)
        {
             view.Close();
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




}