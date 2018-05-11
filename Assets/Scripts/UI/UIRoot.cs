using UnityEngine;
using System.Collections;
using CFramework;
//using UIRootNS;
//using Puncture;
using System.Collections.Generic;

/// <summary>
/// UI搭载脚本，挂在场景中
/// </summary>
public class UIRoot : UIManager {
   
    public static new UIRoot Instance
    {
        get
        {
            return CMonoSingleton<UIRoot>.Instance();
        }
    }

    public new void OnDestroy()
    {
        CMonoSingleton<UIRoot>.OnDestroy();
    }
    public List<CustomObject> CustomList;
    // Use this for initialization
    void Awake()
    {
        //MainUIState.Instance.ShowPage(Page.start);

    }
    /// <summary>
    /// UI使用的字体
    /// </summary>
    public Font font;
    void Start () {
        foreach (var item in CustomList)
        {
            item.game.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public override void ProcessMsg(CMsg msg)
    {

    }

    public GameObject InstantiateObject(string path)
    {
        return Instantiate(Resources.Load(path)) as GameObject;
    }
    public GameObject InstantiateUI(string path)
    {
        return Instantiate(Resources.Load(path)) as GameObject;
    }
    public GameObject InstantiatePrefab(GameObject prefab)
    {
        return Instantiate(prefab);
    }
    public GameObject InstantiateCustom(string name)
    {
        GameObject re = GetCustomObject(name);
        if( re.transform.parent == null)
        {
            re = Instantiate(GetCustomObject(name));
        }
        return re;
    }
    public void DestroyImmediate(GameObject obj)
    {
        DestroyImmediate(obj);
    }
    /// <summary>
    /// 返回自定义对象
    /// </summary>
    /// <param name="_uName"></param>
    /// <returns></returns>
    public GameObject GetCustomObject(string _uName)
    {
        CustomObject obj = CustomList.Find(temp =>
        {
            
            if (temp.uName == _uName)
            {
                return true;
            }
            else
                return false;
        });
        if(obj != null)
            return obj.game;
        return null;
    }
    /// <summary>
    /// 返回自定义对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="_uName"></param>
    /// <returns></returns>
    public T GetCustomObject<T>(string _uName)
    {
        T t = default(T);
        CustomObject obj = CustomList.Find(temp =>
        {
            if (temp.uName == _uName)
                return true;
            else
                return false;
        });
        if (obj != null)
        {
            t = obj.game.GetComponent<T>();
        }
        return t;
    }
    /// <summary>
    /// 设置自定义对象的隐藏显示
    /// </summary>
    /// <param name="_uName"></param>
    /// <param name="isShow"></param>
    public void SetCustomObjectShow(string _uName, bool isShow)
    {
        if(GetCustomObject(_uName) != null)
            GetCustomObject(_uName).SetActive(isShow);
    }
    /// <summary>
    /// 隐藏所有自定义对象
    /// </summary>
    public void HideAllCustomObject()
    {
        foreach (var item in CustomList)
        {
            item.game.SetActive(false);
        }
    }
}
[System.Serializable]
public class CustomObject
{
    public string uName;
    public GameObject game;
}
