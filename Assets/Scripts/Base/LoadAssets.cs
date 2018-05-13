using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using CFramework;
using UnityEngine.UI;
using System.IO;

internal class LoadAssets : CMonoSingleton<LoadAssets>
{
    internal string BundleURL;

    WWW www;

    //Slider progressSlider;

    //Text downText;
    [HideInInspector]
    //internal int assetbundleversionid = 0;

    public Dictionary<string, Dictionary<string, Object>> ModelData;
    public Dictionary<string, Dictionary<string, Object>> VoiceData;
    public Dictionary<string, Dictionary<string, Object>> UIData;
    public Dictionary<string, Dictionary<string, Object>> PrefabData;
    public Dictionary<string, Dictionary<string, Object>> TxtData;
    //Dictionary<string, _CLASS_StaticModelInfo> _ModelInfotmp;

    private LoadAssets()
    {
        ModelData = new Dictionary<string, Dictionary<string, Object>>();
        VoiceData = new Dictionary<string, Dictionary<string, Object>>();
        UIData = new Dictionary<string, Dictionary<string, Object>>();
        PrefabData = new Dictionary<string, Dictionary<string, Object>>();
        TxtData = new Dictionary<string, Dictionary<string, Object>>();
    }

    public static new LoadAssets Instance
    {
        get
        {
            return CMonoSingleton<LoadAssets>.Instance();
        }
    }

    /// <summary>
    /// 下载所有资源，并加载场景模型数据
    /// </summary>
    /// <param name="_assetInfoTmp"></param>
    /// <returns></returns>
    public IEnumerator DownLoadAsset(List<_CLASS_ASSETBUNDLE> _assetInfoTmp)
    {
        Dictionary<_CLASS_ASSETBUNDLE, AssetBundle> tmp_load = new Dictionary<_CLASS_ASSETBUNDLE, AssetBundle>();
        foreach (_CLASS_ASSETBUNDLE info in _assetInfoTmp)
        {
            //p1rogressSlider.value = 0;
            Debug.Log("正在获取" + info.packagedesc + ",请您稍候...");
            www = WWW.LoadFromCacheOrDownload(Config.assetsPath + "/"+ (info.packagepath == "" ? info.assetpackagename : info.packagepath + "/" + info.assetpackagename), info.packageh5);
            yield return www;
            AssetBundle bundle = www.assetBundle;

            tmp_load.Add(info, bundle);
            www.Dispose();
        }
        foreach (var item in tmp_load)
        {
            Object[] tmp;
            yield return new WaitForEndOfFrame();
            tmp = item.Value.LoadAllAssets();
            Dictionary<string, Object> _tmp = new Dictionary<string, Object>();
            for (int i = 0; i < tmp.Length; i++)
            {
                try
                {
                    if (tmp[i].GetType() == typeof(Texture2D))
                        _tmp.Add(tmp[i].name + "_T", tmp[i]);
                    else
                        _tmp.Add(tmp[i].name, tmp[i]);
                }
                catch
                {
                    if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
                        Debug.Log("资源重复" + tmp[i].name);
                    throw;
                }
            }

            switch (item.Key.style)
            {
                case AssetBundleStyleEnum.None:
                    break;
                case AssetBundleStyleEnum.Model:
                    ModelData.Add(item.Key.assetpackagename, _tmp);
                    break;
                case AssetBundleStyleEnum.Audios:
                    VoiceData.Add(item.Key.assetpackagename, _tmp);
                    break;
                case AssetBundleStyleEnum.UI:
                    UIData.Add(item.Key.assetpackagename, _tmp);
                    break;
                case AssetBundleStyleEnum.Prefab:
                    PrefabData.Add(item.Key.assetpackagename, _tmp);
                    break;
                case AssetBundleStyleEnum.Txt:
                    TxtData.Add(item.Key.assetpackagename, _tmp);
                    break;
            }
        }


        foreach (KeyValuePair<_CLASS_ASSETBUNDLE, AssetBundle> item in tmp_load)
        {
            item.Value.Unload(false);
        }
        Resources.UnloadUnusedAssets();
        tmp_load.Clear();
        yield return 0;

        ////下载完成后
        
        yield return StartCoroutine(LoadUIImages());

    }

    /// <summary>
    /// 加载场景模型
    /// </summary>
    /// <returns></returns>
    IEnumerator dealModelAsset()
    {
        Debug.Log("获取模型配置....");
        foreach (var item in ModelData)
        {
            foreach (var item2 in item.Value)
            {
                GameObject obj = Instantiate(item2.Value) as GameObject;
                obj.name = item2.Key;
                obj.transform.SetParent(GameObject.Find("Models").transform);
                obj.SetActive(true);
                obj = null;
            }
            
        }
        //资源加载完毕后Unload
        yield return 0;
        ModelData.Clear();
        Resources.UnloadUnusedAssets();
        yield return StartCoroutine(LoadVoice());
    }

    /// <summary>
    /// 加载声音提示
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadVoice()
    {
        Debug.Log("正在加载语音库,请您稍候...");
        foreach (KeyValuePair<string, Dictionary<string, Object>> info in VoiceData)
        {
            VoiceControlScript.Instance.setVoiceAsset(info.Value);
        }
        VoiceControlScript.Instance.Init();
        yield return 0;
        VoiceData.Clear();
        //StartCoroutine(LoadUIImages());
        Debug.Log("资源加载完成");
    }

    /// <summary>
    /// 加载UI资源
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadUIImages()
    {
        Debug.Log("正在加载图集,请您稍候...");
        foreach (KeyValuePair<string, Dictionary<string, Object>> info in UIData)
        {
            foreach (KeyValuePair<string, Object> _info in info.Value)
            {
                if (!UIRoot.Instance.UIResources.ContainsKey(_info.Key))
                {
                    UIRoot.Instance.UIResources.Add(_info.Key, _info.Value);
                    
                }

            }
        }
        UIData.Clear();
        yield return 0;

        Debug.Log("正在加载文本,请您稍候...");
        foreach (KeyValuePair<string, Dictionary<string, Object>> info in TxtData)
        {
            foreach (KeyValuePair<string, Object> _info in info.Value)
            {
                if (!UIRoot.Instance.TxtResources.ContainsKey(_info.Key))
                {
                    UIRoot.Instance.TxtResources.Add(_info.Key, _info.Value);
                    
                }

            }
        }
        TxtData.Clear();
        yield return 0;

        Debug.Log("正在加载预设体,请您稍候...");
        foreach (KeyValuePair<string, Dictionary<string, Object>> info in PrefabData)
        {
            foreach (KeyValuePair<string, Object> _info in info.Value)
            {
                if (!UIRoot.Instance.PrefabResources.ContainsKey(_info.Key))
                {
                    UIRoot.Instance.PrefabResources.Add(_info.Key, _info.Value);
                    
                }

            }
        }
        PrefabData.Clear();
        yield return StartCoroutine(dealModelAsset());
        
    }

}