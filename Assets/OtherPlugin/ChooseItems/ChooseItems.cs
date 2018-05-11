using UnityEngine;
using System.Collections;
using CFramework;
using System.Collections.Generic;
using UnityEngine.UI;

public class ChooseItems : UIManager
{
    public delegate void OnItemStateChangeEvent();
    public OnItemStateChangeEvent OnItemStateChange;
    public static new ChooseItems Instance
    {
        get
        {
            return CMonoSingleton<ChooseItems>.Instance();
        }
    }

    public new void OnDestroy()
    {
        CMonoSingleton<ChooseItems>.OnDestroy();
    }
    public new Camera camera;
    public Vector3 cameraPath;
    public Vector3 cameraAngle;

    public GameObject NamePrefab;
    public GameObject objectParent;
    [Header("Param")]
    public List<Text> text;
    public Sprite okIcon;
    public Sprite errorIcon;
    public ChooseMode mode;
    //ChooseItemsNS.ChooseMode.MoveItem
    public Vector3 moveParam;
    [Range(0,1)]
    public float moveSpeed = 1;
    public GameObject startObject =null;
    public GameObject endObject = null;
    //end
    
    [SerializeField]
    public List<ItemObject> items;

    List<soloItem> soloItems=new List<soloItem>();
    bool isStart = false;
    // Use this for initialization
    void Awake()
    {
        Init();
        EndChooseItems();
    }
    public void Init()
    {
        soloItems.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<soloItem>() != null)
                soloItems.Add(transform.GetChild(i).GetComponent<soloItem>());
        }
    }
    public void StartChooseItems()
    {
        isStart = true;
        foreach (var item in soloItems)
        {
            if(item.target.activeSelf)
                item.gameObject.SetActive(true);
        }
    }
    public void EndChooseItems()
    {
        Debug.Log("EndChooseItems");
        isStart = false;
        foreach (var item in soloItems)
        {
            item.gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// 更新需要选择的物品
    /// </summary>
    /// <param name="Name">物品显示名字</param>
    /// <param name="isOK">是否需要</param>
    public void UpdateOkItem(string[] _names,bool[] _isOK)
    {
        for (int i = 0; i < _names.Length && i< _isOK.Length; i++)
        {
            soloItem so = soloItems.Find(name =>
            {
                if (name.textName == _names[i])
                    return true;
                else
                    return false;
            });
            if (so != null)
                so.bIsOk = _isOK[i];
        }
    }
    /// <summary>
    /// 设置需要选择的物品
    /// </summary>
    /// <param name="Name">物品显示名字</param>
    public void SetOkItem(string[] _names)
    {
        foreach (var so in soloItems)
        {
            so.bIsOk = false;
            for (int i = 0; i < _names.Length ; i++)
            {
                if(_names[i] == so.textName)
                {
                    so.bIsOk = true;
                    break;
                }
            }
        }
    }
    /// <summary>
    /// 设置需要选择的物品 以外的将被隐藏
    /// </summary>
    /// <param name="Name">物品显示名字</param>
    public void SetSelectItem(string[] _names, bool[] _isOK)
    {
        foreach (var so in soloItems)
        {
            bool find = false;
            for (int i = 0; i < _names.Length; i++)
            {
                if (_names[i] == so.textName)
                {
                    so.bIsOk = _isOK[i];
                    find = true;
                    break;
                }
            }
            if (find == false)
            {
                so.bIsOk = false;
                so.target.SetActive(false);
                so.gameObject.SetActive(false);
            }
        }
    }
    /// <summary>
    /// TODO: 待补充 
    /// </summary>
    /// <param name="_paths"></param>
    /// <param name="_names"></param>
    public void UpdateItemName(string[] _paths,string[] _names)
    {
        for (int i = 0; i < _paths.Length; i++)
        {

        }
    }
    /// <summary>
    /// 取得需要选择的总数量
    /// </summary>
    /// <returns></returns>
    public int GetNeedSelectNumber()
    {
        int number = 0;
        foreach (var item in soloItems)
        {
            if (item.bIsOk)
                number++;
        }
        return number;
    }
    /// <summary>
    /// 取得已选择的总数量
    /// </summary>
    /// <returns></returns>
    public int GetSelectedNumber()
    {
        int number = 0;
        foreach (var item in soloItems)
        {
            if (item.bSelected)
                number++;
        }
        return number;
    }
    /// <summary>
    /// 取得More的总数量
    /// </summary>
    /// <returns></returns>
    public int GetMoreSelectNumber()
    {
        int number = 0;
        foreach (var item in soloItems)
        {
            if (item.IsMoreSelect())
                number++;
        }
        return number;
    }
    /// <summary>
    /// 取得Lost的总数量
    /// </summary>
    /// <returns></returns>
    public int GetLostSelectNumber()
    {
        int number = 0;
        foreach (var item in soloItems)
        {
            if (item.IsLostSelect())
                number++;
        }
        return number;
    }
    // Update is called once per frame
    void Update()
    {
        if (isStart && Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                SelectItem(hit.collider.name);
            }
        }

    }
    void SelectItem(string modeName)
    {
        foreach (var item in soloItems)
        {
            if (modeName == item.target.name)
            {
                Debug.Log("select =" + modeName);
                if (mode == ChooseMode.MoveItem)
                    item.ChangeState(moveParam, moveSpeed);
                else
                    item.ChangeState();
                if (OnItemStateChange != null)
                    OnItemStateChange();
            }
        }
    }
    soloItem GetSoloItem(string modeName)
    {
        return soloItems.Find(name =>
        {
            if (name.target.name == modeName)
            {
                return true;
            }
            else
            {
                return false;
            }
        });
    }
    public void SetItemShowName(string nodeName,string showName)
    {
        soloItem so = soloItems.Find(name =>
        {
            if (name.target.name == nodeName)
                return true;
            else
                return false;
        });
        if (so != null)
            so.textName  = showName;
    }
}

public enum ChooseMode
{
    MoveItem,
    Choose
}
[System.Serializable]
public class ItemObject
{
    public string Name = "";
    public GameObject obj;
    [Tooltip("正确物品")]
    public bool isOk;
}
