using UnityEngine;
using System.Collections;
using CFramework;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ChooseItems : UIManager
{
    public delegate void OnItemStateChangeEvent();
    
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

    public OnItemStateChangeEvent OnItemStateChange;

    /// <summary>
    /// 专用物品未选择
    /// </summary>
    public CEvent OnSpecialNone = new CEvent();
    public new Camera camera;
    public Vector3 cameraPath;
    public Vector3 cameraAngle;

    public GameObject NamePrefab;
    public GameObject objectParent;
    [Header("Param")]
    public List<Text> text;
    public Sprite okIcon;
    public Sprite errorIcon;
    public Sprite trueIcon;
    public ChooseMode mode;
    //ChooseItemsNS.ChooseMode.MoveItem
    public Vector3 moveParam;
    [Range(0, 1)]
    public float moveSpeed = 1;
    public GameObject startObject = null;
    public GameObject endObject = null;
    public Button LastshowButton;
    //end
    Vector3 cameraPathTmp = new Vector3(-10.539f, 2.029f, 0.4269999f);//-10.539f, 2.029f, 0.4269999f
    Vector3 cameraAngleTmp = new Vector3(63.46215f, 179.963f, -0.002716064f);//63.46215f, 179.963f, -0.002716064f
    [HideInInspector]
    public bool first = true;
    [SerializeField]
    public List<ItemObject> items;

    List<soloItem> soloItems = new List<soloItem>();
    bool isStart = false;

    UPageButton currentButton;

    public void LoadTarget()
    {

    }
    public void Init(string modelparentpath)
    {
        soloItems.Clear();
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<soloItem>() != null)
            {
                soloItem item = transform.GetChild(i).GetComponent<soloItem>();
                if (item.target == null)
                {
                    var item2 = modelparentpath + "/" + item.gameObject.name; //models/chooseItem/qi_xie_desk/8th_puncture_needle_qxb
                    int index = item2.IndexOf('/');//index=6
                     item.target = GameObject.Find(item2.Substring(0, index)).transform.Find(item2.Substring(index + 1)).gameObject;//models/chooseItem/qi_xie_desk/8th_puncture_needle_qxb

                }
                if (item.target != null)
                    soloItems.Add(item);
                else
                    item.gameObject.SetActive(false);
            }
        }
    }
   ///* public void Init(Dictionary<string, string> modelList)
   // {
   //     //List<_CLASS_SceneModelProperty> modelList
   //     soloItems.Clear();
   //     for (int i = 0; i < transform.childCount; i++)
   //     {
   //         if (transform.GetChild(i).GetComponent<soloItem>() != null)
   //         {
   //             soloItem item = transform.GetChild(i).GetComponent<soloItem>();
   //             if (item.target == null)
   //             {
   //                 foreach (var item2 in modelList)
   //                 {
   //                     try
   //                     {
   //                         if (item.gameObject.name == item2.Key.Substring(item2.Key.LastIndexOf('/') + 1))
   //                         {
   //                             int index = item2.Key.IndexOf('/');
   //                             item.target = GameObject.Find(item2.Key.Substring(0, index)).transform.Find(item2.Key.Substring(index + 1)).gameObject;
   //                             break;
   //                         }
   //                     }
   //                     catch
   //                     {
   //                         Debug.Log(item2.Value + "的路径" + item2.Key + "错误。");
   //                         //CFramework.CLog
   //                     }
   //                 }

   //             }
   //             if (item.target != null)
   //                 soloItems.Add(item);
   //             else
   //                 item.gameObject.SetActive(false);
   //         }
   //     }

   //     EndChooseItems();
   // }*/
    void CalibrationIcons()
    {
        foreach (var item in soloItems)//把UI信息带入到对应的modle上（文字和图片）
        {
            if (item.target.activeSelf)
            {
                //item.gameObject.SetActive(true);
                Vector3 dir = Camera.main.WorldToScreenPoint(item.target.transform.position);// - camera.WorldToScreenPoint(item.target.transform.position);item.transform
                item.gameObject.transform.position = dir; // += dir;
            }
        }
        //Destroy(cameraObject);//摧毁
    }
    public void StartChooseItems(bool ShowText = true)
    {
        if (first)
        {
            CalibrationIcons();
            first = false;//再次进入后不在运行if语句
        }
        isStart = true;
        foreach (var item in soloItems)
        {
            if (item.target.activeSelf)
            {
                if (ShowText)
                    item.gameObject.SetActive(true);
                item.Init();//不显modle上UI中的图片
            }
        }

    }
    public void EndChooseItems()
    {
        //Debug.Log("EndChooseItems");
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
    public void UpdateOkItem(string[] _names, bool[] _isOK)
    {
        for (int i = 0; i < _names.Length && i < _isOK.Length; i++)
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
    public void DisplayTrue(string[] _name,bool[] _isOK)
    {
        for (int i=0;i<_name.Length;i++)
        {

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
            for (int i = 0; i < _names.Length; i++)
            {
                if (_names[i] == so.textName)
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
    /// TODO: 点击确定显示正确物品
    /// </summary>
    public void ShowTrueGoods()
    {
        foreach (var name in soloItems)
        {
            if (name.bIsOk&&name.bSelected)
            {
                name.tureIcon.SetActive(true);
                name.okIcon.SetActive(false);
                name.errorIcon.SetActive(false);
            }
            else if(name.bIsOk==false&&name.bSelected)
            {
                name.tureIcon.SetActive(false);
                name.okIcon.SetActive(false);
                name.errorIcon.SetActive(true);
            }
        }
    }
    /// <summary>
    /// TODO: 待补充 
    /// </summary>
    /// <param name="_paths"></param>
    /// <param name="_names"></param>
    public void UpdateItemName(string[] _paths, string[] _names)
    {
        for (int i = 0; i < _paths.Length; i++)
        {

        }
    }

    public void SetShowMode(bool NoError)
    {
        foreach (var item in soloItems)
        {
            //item.NoErrorMode = NoError;
            item.SetErrorMode(NoError);
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
                if (!EventSystem.current.IsPointerOverGameObject())//IsPointerOverGameObject()返回true就是UI
                    SelectItem(hit.collider.name);//collider碰撞对象

            }
        }

    }

    void SelectItem(string modeName)
    {
        foreach (var item in soloItems)
        {
          

            if (modeName == item.target.name)
            {
                if (modeName == fileHelper.ReadIni("Specialitems","test", "StepConfig"))
                {
                    if (item.bSelected)
                        item.ChangeState();
                    setSpecialObjPlane(item);
                }
                else
                {
                    if (mode == ChooseMode.MoveItem)
                        item.ChangeState(moveParam, moveSpeed);
                    else
                        item.ChangeState();
                }
                if (OnItemStateChange != null)
                    OnItemStateChange();
            }
        }
    }
    //public void showtruegoods(string[] _name)
    //{
    //    foreach (var name in soloItems)
    //    {
    //        for (int i = 0; i <= _name.Length; i++)
    //        {
    //            if (_name[i] == name.textName)
    //                name.tureIcon.SetActive(true);
    //           else  if (_name[i] != name.textName && name.bSelected)
    //                    name.tureIcon.SetActive(false);

    //        }
    //    }
    //}
    string[] Specialitems;
    string wrongTypeImage;
    List<string> btnList =new List<string>();
    UPlane m_uplane;
    void setSpecialObjPlane(soloItem item)
    {
       
        specialItem = item;
        if (m_uplane != null)
            m_uplane.Destroy();

        string str = "right,error,overdate";

        if (btnList.Count == 0)
        {
            foreach (var number in MyCommon.RandomRepeat(3))
            {
                btnList.Add(str.Split(',')[number]);
            }
        }

        m_uplane = new UPlane();//定义UI
        m_uplane.SetAnchored(AnchoredPosition.center);//定义位置远点
        m_uplane.rect = new Rect(65, 0, 1620, 1080 / 6 * 5);//定义显示框的位置的大小
        m_uplane.gameObejct.AddComponent<ToggleGroup>();//副节点
        m_uplane.color = new Color(0.9f, 0.9f, 0.9f);
        m_uplane.LoadImage("");

        UText m_utext = new UText();
        m_utext.SetAnchored(AnchoredPosition.center);
        m_utext.text = "请选择正确物品";
        m_utext.rect = new Rect(100, -375, 500, 100);
        m_utext.baseText.fontSize = 55;
        m_utext.baseText.color = Color.white;
        m_utext.SetParent(m_uplane);

        for (int i = 0; i < btnList.Count; i++)
        {
            UPageButton btn = new UPageButton();
            btn.SetParent(m_uplane);
            btn.name = btnList[i];
            btn.SetAnchored(AnchoredPosition.center);
            btn.rect = new Rect(-540 + i*500 + 50, 9, 500, 300);
            btn.LoadSprite("check_anesthetic_3");
            btn.LoadPressSprite("check_anesthetic_3_h");
            btn.onClick.AddListener(() => { OnImageButtonClick(btn); });

            UText m_datetext_1 = new UText();
            m_datetext_1.SetAnchored(AnchoredPosition.center);
            m_datetext_1.SetParent(m_uplane);
            m_datetext_1.rect = new Rect(-300+i * 500 + 50, 240, 500, 300);
            m_datetext_1.baseText.fontSize = 23;
            m_datetext_1.baseText.color = new Color(0f, 0f, 0f);
            
            m_datetext_1.rectTransform.localEulerAngles = new Vector3(0f, 0f, 5.6f);
            m_datetext_1.baseText.raycastTarget = false;
            UText m_type_1 = new UText();
         
            m_type_1.SetAnchored(AnchoredPosition.center);
            m_type_1.SetParent(m_uplane);
            m_type_1.rect = new Rect( -300+i * 500 + 50, 200, 500, 300);
            m_type_1.baseText.color = new Color(0f, 0f, 0f);
            m_type_1.rectTransform.localEulerAngles = new Vector3(0f, 0f, 5.6f);
            m_type_1.baseText.fontSize = 23;
            m_type_1.baseText.raycastTarget = false;
            if (btnList[i] == "right")
            {
                m_datetext_1.text = "有效日期: " + (DateTime.Now.Year + 2).ToString() + "年" + (DateTime.Now.Month.ToString()) + "月1日";
                m_type_1.text = "规格：16Fr        ";
            }
            else
            {
                if (btnList[i] == "error")
                {
                    m_datetext_1.text = "有效日期: " + (DateTime.Now.Year + 2).ToString() + "年" + (DateTime.Now.Month.ToString()) + "月1日";
                    m_type_1.text = "规格：8Fr\0\0\0\0\0\0\0\0";
                }
                else
                {
                    m_type_1.text = "规格：16Fr\r\r\r\r\r\r   ";
                    m_datetext_1.text = "有效日期: " + (DateTime.Now.Year - 1).ToString() + "年" + (DateTime.Now.Month.ToString()) + "月1日";
                }
            }
        }


        UFinishButton OkButton = new UFinishButton("确定", new Rect(23, 363, 180, 70), AnchoredPosition.center);
        OkButton.SetParent(m_uplane);
        OkButton.baseButton.onClick.AddListener(specialItemChoose);

        m_uplane.transform.SetAsLastSibling();
        List<UPageBase> list = m_uplane.GetChildren();
        foreach (UPageBase upb in list)
        {
            UToogleItem tmp = null;
            try
            {
                tmp = (UToogleItem)upb;
            }
            catch
            {
                continue;
            }

            if (tmp != null)
            {
                upb.gameObejct.GetComponent<Toggle>().isOn = false;

            }
        }
        m_uplane.gameObejct.SetActive(true);
        UPageBase.FindPage("ChooseGoodsUI/ChooseItems/chooseItemFinishButton").gameObejct.SetActive(false);
        //GameObject.Find("Canvas").transform.Find("ChooseItems/UPageButton").gameObject.SetActive(false);
    }

    string btnName=string.Empty;
    public void OnImageButtonClick(UPageButton btn)
    {
        if (null != currentButton)
            currentButton.LoadSprite(currentButton.sprite.name.Replace("_h", ""));
        currentButton = btn;
        currentButton.LoadSprite(btn.sprite.name + "_h");
        btnName = btn.name;
    }

    specialItemChoose chooseResult = global::specialItemChoose.None;
    soloItem specialItem;
    void specialItemChoose()
    {
        currentButton.LoadSprite(currentButton.sprite.name.Replace("_h", ""));
        currentButton = null;
        m_uplane.SetActive(false);
        chooseResult = global::specialItemChoose.None;
        
        List<UPageBase> list = m_uplane.GetChildren();
  
        UPageBase.FindPage("ChooseGoodsUI/ChooseItems/chooseItemFinishButton").gameObejct.SetActive(true);
        if (btnName=="right")
        {
            specialItem.bIsOk = true;
            specialItem.ChangeState();
            chooseResult = global::specialItemChoose.right;
        }
        else
        {
            specialItem.bIsOk = false;
            //specialItem.errorIcon.SetActive(true);
            specialItem.ChangeState();
            if (btnName== "overdate")
            {
                chooseResult = global::specialItemChoose.overDate;
            }
            //else if (tmp.getimage() == breakImage)
            //{
            //    chooseResult = global::specialItemChoose.broken;
            //}
            else if (btnName == "error")
            {
                chooseResult = global::specialItemChoose.wrongType;
            }
        }
        if (string.IsNullOrEmpty(btnName))
        {
            OnSpecialNone.Invoke();
            return;
        }
        
        m_uplane.gameObejct.SetActive(false);
        if (OnItemStateChange != null)
            OnItemStateChange();
    }

    public string specialItemChooseRes()
    {
        switch (chooseResult)
        {
            case global::specialItemChoose.None:
                return null;
            case global::specialItemChoose.right:
                return "chooseBagRight";
            case global::specialItemChoose.overDate:
                return "chooseBagOverDay";
            case global::specialItemChoose.broken:
                return "chooseBagBreaked";
            case global::specialItemChoose.wrongType:
                return "chooseBagTypeWrong";
            default:
                return null;
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
    public void SetItemShowName(string nodeName, string showName)
    {
        soloItem so = soloItems.Find(name =>
        {
            if (name.target.name == nodeName)
                return true;
            else
                return false;
        });
        if (so != null)
            so.textName = showName;
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
public enum specialItemChoose
{
    None = 0,
    right,
    overDate,
    broken,
    wrongType,
}




