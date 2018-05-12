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

    Slider progressSlider;

    Text downText;
    [HideInInspector]
    internal int assetbundleversionid = 0;

    Dictionary<string, Dictionary<string, Object>> ModelData;
    Dictionary<string, Dictionary<string, Object>> VoiceData;
    Dictionary<string, Dictionary<string, Object>> UIData;
    Dictionary<string, _CLASS_StaticModelInfo> _ModelInfotmp;

    internal VoidDelegate _callback;

    bool isReset = false;

    private LoadAssets()
    {

    }

    public static LoadAssets Instance
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
    IEnumerator DownLoadAsset(List<_CLASS_ASSETBUNDLE> _assetInfoTmp)
    {
        Dictionary<_CLASS_ASSETBUNDLE, AssetBundle> tmp_load = new Dictionary<_CLASS_ASSETBUNDLE, AssetBundle>();
        foreach (_CLASS_ASSETBUNDLE info in _assetInfoTmp)
        {
            isReset = true;
            progressSlider.value = 0;
            downText.text = "正在获取" + info.packagedesc + ",请您稍候...";
            www = WWW.LoadFromCacheOrDownload(BundleURL + (info.packagepath == "" ? info.assetpackagename : info.packagepath + "/" + info.assetpackagename), assetbundleversionid);
            yield return www;
            isReset = false;
            AssetBundle bundle = www.assetBundle;
            
            tmp_load.Add(info, bundle);
            www.Dispose();
        }
        foreach (var item in tmp_load)
        {
            Object[] tmp;
            yield return new WaitForEndOfFrame();
            tmp = item.Value.LoadAllAssets();
            //tmp = item.Value.LoadAllAssetsAsync().allAssets;
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
        Debug.Log("-----CFrame-->>>获取模型配置....");
        StartCoroutine(dealModelAsset());

    }

    /// <summary>
    /// 下载资源版本文件
    /// </summary>
    /// <param name="callback"></param>
    /// <returns></returns>
    IEnumerator DownVersionTxt(VoidDelegate callback)
    {
        WWW www = new WWW(BundleURL + "version.txt");
        yield return www;
        fileHelper.setConfigText("versionWWW", www.text.Split('\n'));
        callback();
    }
    /// <summary>
    /// 加载场景模型
    /// </summary>
    /// <returns></returns>
    IEnumerator dealModelAsset()
    {
        int count = 0;
        foreach (KeyValuePair<string, _CLASS_StaticModelInfo> info in _ModelInfotmp)
        {
            downText.text = "正在加载" + info.Value.modelDesc + ",请您稍候...";
            Debug.Log("-----CFrame-->>>加载场景资源： " + info.Value.modelDesc);
            int separatorNo = info.Value.modelPath.LastIndexOf("/");
            string _ModelName = info.Value.modelPath.Substring(separatorNo + 1, info.Value.modelPath.Length - separatorNo - 1);
            count++;
            try
            {
                if (ModelData.ContainsKey(info.Value.assetbundlename))
                {
                    if (ModelData[info.Value.assetbundlename].ContainsKey(_ModelName))
                    {
                        GameObject obj = Instantiate(ModelData[info.Value.assetbundlename][_ModelName]) as GameObject;
                        obj.name = _ModelName;
                        string _ModelPos = info.Value.modelPath.Substring(0, separatorNo);
                        creatParentTrans(_ModelPos);
                        obj.transform.SetParent(GameObject.Find("Models").transform.Find(_ModelPos));
                        float[] f = new float[3];
                        if (float.TryParse(info.Value.modelPos[0], out (f[0])) && float.TryParse(info.Value.modelPos[1], out (f[1])) && float.TryParse(info.Value.modelPos[2], out (f[2])))
                        {
                            obj.transform.localPosition = new Vector3(f[0], f[1], f[2]);
                        }
                        if (float.TryParse(info.Value.modelEuler[0], out (f[0])) && float.TryParse(info.Value.modelEuler[1], out (f[1])) && float.TryParse(info.Value.modelEuler[2], out (f[2])))
                        {
                            obj.transform.eulerAngles = new Vector3(f[0], f[1], f[2]);
                        }
                        if (info.Value.defaultStatus == "0")
                        {
                            obj.SetActive(false);
                        }
                        else
                        {
                            obj.SetActive(true);
                        }
                        obj = null;
                    }
                    else
                    {
                        Debug.Log("-----CFrame-->>>丢失资源，资源名称：" + _ModelName+ "模型加载");
                    }
                }
                else
                {
                    Debug.Log("-----CFrame-->>>丢失资源包，资源包名称：" + info.Value.assetbundlename+ "模型加载");
                }
            }
            catch
            {
                Debug.Log("-----CFrame-->>>加载资源失败，资源名称：" + _ModelName+ "模型加载");
            }
            //资源加载完毕后Unload
            yield return 0;
            if (count == _ModelInfotmp.Count)
            {
                ModelData.Clear();
                Resources.UnloadUnusedAssets();
                Debug.Log("-----CFrame-->>>获取声音数据配置....");
                StartCoroutine(LoadVoice());
                //Load();
            }
        }
    }

    void creatParentTrans(string parentPath)
    {
        //Debug.Log("parentPath is : : : : " + parentPath);
        if (parentPath == "")
            return;

        string Objname;
        Transform parentNow = GameObject.Find("Models").transform;

        parentPath = parentPath + "/";
        int i = parentPath.IndexOf("/");

        do
        {
            Objname = parentPath.Substring(0, i);

            if (parentNow.Find(Objname) == null)
            {
                GameObject obj = new GameObject(Objname);
                obj.transform.SetParent(parentNow);
                obj.transform.localPosition = new Vector3(0f, 0f, 0f);
                obj.transform.localEulerAngles = new Vector3(0f, 0f, 0f);
                //Debug.Log("创建节点：：：：：：：：：：：：：" + Objname + "，，，，，父节点为：" + parentNow);
                parentNow = obj.transform;
            }
            else
            {
                parentNow = parentNow.Find(Objname);
            }
            parentPath = parentPath.Substring(i + 1, parentPath.Length - i - 1);
            //Debug.Log("after parentPath is " + parentPath);
            i = parentPath.IndexOf("/");
        }
        while (i > 0);

    }

    void Load()
    {
        foreach (KeyValuePair<string, Dictionary<string, Object>> info in VoiceData)
        {
            VoiceControlScript.Instance.setVoiceAsset(info.Value);
        }

        foreach (KeyValuePair<string, Dictionary<string, Object>> info in UIData)
        {
            foreach (KeyValuePair<string, Object> _info in info.Value)
            {
                UIRoot.OnlineResources.Add(_info.Key, _info.Value);
                //Debug.Log("-----CFrame-->>>存储UI资源：" + _info.Key, "");
            }
        }
        _callback();
    }

    /// <summary>
    /// 加载声音提示
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadVoice()
    {
        foreach (KeyValuePair<string, Dictionary<string, Object>> info in VoiceData)
        {
            downText.text = "正在加载语音库,请您稍候...";
            VoiceControlScript.Instance.setVoiceAsset(info.Value);
        }
        yield return 0;
        VoiceData.Clear();
        Debug.Log("-----CFrame-->>>获取UI数据配置....");
        StartCoroutine(LoadUIImages());
    }

    /// <summary>
    /// 加载UI资源
    /// </summary>
    /// <returns></returns>
    IEnumerator LoadUIImages()
    {
        foreach (KeyValuePair<string, Dictionary<string, Object>> info in UIData)
        {
            foreach (KeyValuePair<string, Object> _info in info.Value)
            {
                if (!UIRoot.OnlineResources.ContainsKey(_info.Key))
                {
                    UIRoot.OnlineResources.Add(_info.Key, _info.Value);
                    downText.text = "正在加载图集,请您稍候...";
                }
                    
            }
        }
        UIData.Clear();
        yield return 0;
        downText.text = "加载完成，即将启动！";
        _callback();
    }

    void DownLoadAssetAndScene(KeyValuePair<string, _CLASS_StaticModelInfo> info)
    {
        //电脑端加载使用
        //Debug.Log("加载资源： " + info.Value.modelDesc);
        int separatorNo = info.Value.modelPath.LastIndexOf("/");
        string _ModelName = info.Value.modelPath.Substring(separatorNo + 1, info.Value.modelPath.Length - separatorNo - 1);
        WWW asset = new WWW(BundleURL + _ModelName);
        AssetBundle bundle = asset.assetBundle;

        //try
        //{

        GameObject obj = Instantiate(bundle.LoadAssetAsync(_ModelName + ".prefab").asset) as GameObject;
        obj.name = _ModelName;
        string _ModelPos = info.Value.modelPath.Substring(0, separatorNo);
        obj.transform.SetParent(GameObject.Find("Models").transform.Find(_ModelPos));
        //}
        //catch
        //{
        //    Debug.Log("Load Asset Error !! Asset name is " + _ModelName);           
        //}
        //资源加载完毕后Unload
        bundle.Unload(false);
    }

    void Update()
    {
        if (isReset)
        {
            progressSlider.value = www.progress;
        }
    }
}