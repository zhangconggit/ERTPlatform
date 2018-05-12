using UnityEngine;
using System.Collections;
using CFramework;
//using UIRootNS;
//using Puncture;
using System.Collections.Generic;

/// <summary>
/// UI搭载脚本，挂在场景中
/// </summary>
public class UIRoot :
#if UI
    MonoBehaviour
#else
    UIManager
#endif
{
#if UI
    static UIRoot _instace = null;

    public static UIRoot Instance
    {
        get
        {
            if(_instace == null)
            {
                UIRoot obj = GameObject.FindObjectOfType<UIRoot>();
                if (obj != null)
                    _instace = obj;
                else
                {
                    GameObject root = new GameObject();
                    root.name = "UIRoot";
                    obj = root.AddComponent<UIRoot>();
                    _instace = obj;
                }

            }
            return _instace;
        }
    }
#else
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
#endif
    public List<CustomObject> CustomList;
    public static Dictionary<string, Object> OnlineResources = new Dictionary<string, Object>();
    public Dictionary<string, Sprite> SpriteList;
    // Use this for initialization
    void Awake()
    {
        //MainUIState.Instance.ShowPage(Page.start);

    }
    public bool isOnline = true;
    /// <summary>
    /// UI使用的字体
    /// </summary>
    public Font font;
    void Start()
    {

        //LoadResources();
        foreach (var item in CustomList)
        {
            if (item.game.transform.parent != null)
                item.game.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
#if !UI
    public override void ProcessMsg(CMsg msg)
    {

    }
#endif
    public GameObject InstantiateObject(string path)
    {
        return Instantiate(Resources.Load(path)) as GameObject;
    }
    public GameObject InstantiateUI(string path)
    {
        return Instantiate(Resources.Load(path)) as GameObject;
    }
    public GameObject InstantiatePrefab(GameObject re)
    {
        if (re.transform.parent == null)
        {
            re = Instantiate(re);
            if (re.GetComponent<RectTransform>() != null)
            {
                Vector3 pos = re.GetComponent<RectTransform>().localPosition;
                Vector2 scale = re.GetComponent<RectTransform>().localScale;
                Canvas can = GameObject.FindObjectOfType<Canvas>();
                re.transform.SetParent(can.transform);
                re.GetComponent<RectTransform>().localPosition = new Vector3(pos.x * UPageBase.fAdapterWidth, pos.y * UPageBase.fAdapterHeight, pos.z);
                re.GetComponent<RectTransform>().localScale = new Vector3(scale.x * UPageBase.fAdapterWidth, scale.y * UPageBase.fAdapterHeight);
            }
        }
        return re;
    }
    public GameObject InstantiateCustom(string name, bool isOnline = true)
    {
        GameObject re = GetCustomObject(name);
        if (re.transform.parent == null)
        {
            re = Instantiate(GetCustomObject(name));
            if (re.GetComponent<RectTransform>() != null)
            {
                Vector3 pos = re.GetComponent<RectTransform>().localPosition;
                Vector2 scale = re.GetComponent<RectTransform>().localScale;
                Canvas can = GameObject.FindObjectOfType<Canvas>();
                re.transform.SetParent(can.transform);
                re.GetComponent<RectTransform>().localPosition = new Vector3(pos.x * UPageBase.fAdapterWidth, pos.y * UPageBase.fAdapterHeight, pos.z);
                re.GetComponent<RectTransform>().localScale = new Vector3(scale.x * UPageBase.fAdapterWidth, scale.y * UPageBase.fAdapterHeight);
            }
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
        if (isOnline)
        {
            if (OnlineResources.ContainsKey(_uName))
            {
                return OnlineResources[_uName] as GameObject;
            }
            return null;
        }
        else
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
            if (obj != null)
                return obj.game;
            return null;
        }
    }
    public Sprite GetCustomSprite(string _uName)
    {
        if (isOnline)
        {
            if (OnlineResources.ContainsKey(_uName))
            {
                return OnlineResources[_uName] as Sprite;
            }
            return null;
        }
        else
        {
            return Resources.Load<Sprite>(_uName);
        }
    }
    public Texture2D GetCustomTexture(string _uName)
    {
        if (isOnline)
        {
            if (OnlineResources.ContainsKey(_uName))
            {
                return OnlineResources[_uName] as Texture2D;
            }
            return null;
        }
        else
        {
            return Resources.Load<Texture2D>(_uName);
        }
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
        if (GetCustomObject(_uName) != null)
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

