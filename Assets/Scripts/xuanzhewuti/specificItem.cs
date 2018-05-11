using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class specificItem : MonoBehaviour {
    /// <summary>
    /// 触发物体
    /// </summary>
    public GameObject SpecificItem;
    /// <summary>
    /// 正常的
    /// </summary>
    public GameObject RightItem;
    /// <summary>
    /// 过了保质期
    /// </summary>
    public GameObject OutShelfLife;
    /// <summary>
    /// 型号错误的
    /// </summary>
    public GameObject ModelError;
    /// <summary>
    /// 包装破损的
    /// </summary>
    public GameObject PackingDamagee;
    public GameObject YesImage;
    public GameObject NoImage;
    public bool useMe = false;
    public static bool waiting;
    public bool isRandom = true;
    public bool isRandomEveryTime = true;
	// Use this for initialization
    /// <summary>
    /// 选择的模型 0 未选择 1 正确 2 过期 3 信号错误 4 包装破损
    /// </summary>
    private int selectItem;
    private List<Vector3> postions;
    private List<GameObject> objects;
    /// <summary>
    /// 选择的模型
    /// </summary>
    /// <returns>0 未选择 1 正确 2 过期 3 信号错误 4 包装破损</returns>
    public int getState()
    {
        return selectItem;
    }
    public void ReSet()
    {
        selectItem = 0;
        YesImage.SetActive(false);
        NoImage.SetActive(false);
    }
	void Start () {
        postions = new List<Vector3>();
        objects = new List<GameObject>();
        selectItem = 0;
        var button = RightItem.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(onRightItem);
            postions.Add(RightItem.transform.localPosition);
            objects.Add(RightItem);
        }
        button = OutShelfLife.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(onOutShelfLife);
            postions.Add(OutShelfLife.transform.localPosition);
            objects.Add(OutShelfLife);
        }
        button = ModelError.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(onModelError);
            postions.Add(ModelError.transform.localPosition);
            objects.Add(ModelError);
        }
        button = PackingDamagee.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(onPackingDamagee);
            postions.Add(PackingDamagee.transform.localPosition);
            objects.Add(PackingDamagee);
        }
        if (SpecificItem == null)
            SpecificItem = gameObject;
        if (SpecificItem.GetComponent<Collider>() == null)
            SpecificItem.AddComponent<BoxCollider>();
        waiting = false;
        ShowThis(false);
        YesImage.SetActive(false);
        NoImage.SetActive(false);
        RandPostion();
	}
	
	// Update is called once per frame
	void Update () {
        if (!useMe)
            return;
        if (waiting)
            return;
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("MouseButtonUp");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.name == SpecificItem.transform.name)
                {
                    ShowThis(true);
                    waiting = true;
                }
            }
        }
	}
    void onRightItem()
    {
        GlobalClass.currentEnemaBag = enemaBag.ok;
        //UIModelCtrl.getInstance().ChangEnemaBag(GlobalClass.currentEnemaBag);
        //GlobalClass.bagWaterMax = 1000;
        //if (DB.getInstance().getArticle("专用物品", GlobalClass.g_CurrentRecord.ID) == "1L")
        selectItem = 1;
        //if (selectItem == 1 || GlobalClass.g_OperatorSchema == OperatorSchema.examModel)
        //{
        YesImage.SetActive(true);
        NoImage.SetActive(false);
        //}
        //else
        //{
        //    YesImage.SetActive(false);
        //    NoImage.SetActive(true);
        //}
        ShowThis(false);
        waiting = false;
    }
    void RandPostion()
    {
        if (isRandom)
        {
            //System.Random rn = new System.Random();
            //List<int> Ran = new List<int>();
            int[] rand = MyCommon.RandomRepeat(postions.Count);
            for (int i = 0; i < postions.Count; i++)
            {
                objects[i].transform.localPosition = postions[rand[i]];
            }
        }
    }
    void onOutShelfLife()
    {
        selectItem = 2;
        GlobalClass.currentEnemaBag = enemaBag.timeError;
       // UIModelCtrl.getInstance().ChangEnemaBag(GlobalClass.currentEnemaBag);
        //if (GlobalClass.g_OperatorSchema == OperatorSchema.examModel)
        //{
        YesImage.SetActive(true);
        NoImage.SetActive(false);
        //}
        //else
        //{
        //    YesImage.SetActive(false);
        //    NoImage.SetActive(true);
        //}
        ShowThis(false);
        waiting = false;
    }
    void onModelError()
    {
        //GlobalClass.bagWaterMax = 2000;
        selectItem = 3;
        GlobalClass.currentEnemaBag = enemaBag.typeError;
       // UIModelCtrl.getInstance().ChangEnemaBag(GlobalClass.currentEnemaBag);
        //if (selectItem == 1 || GlobalClass.g_OperatorSchema == OperatorSchema.examModel)
        //{
        YesImage.SetActive(true);
        NoImage.SetActive(false);
        //}
        //else
        //{
        //    YesImage.SetActive(false);
        //    NoImage.SetActive(true);
        //}
        ShowThis(false);
        waiting = false;
    }
    void onPackingDamagee()
    {
        selectItem = 4;
        GlobalClass.currentEnemaBag = enemaBag.error;
       // UIModelCtrl.getInstance().ChangEnemaBag(GlobalClass.currentEnemaBag);
        YesImage.SetActive(true);
        NoImage.SetActive(false);
        ShowThis(false);
        waiting = false;
    }
    void ShowThis(bool bl)
    {
        if (isRandomEveryTime && bl)
            RandPostion();
        if (RightItem != null)
            RightItem.SetActive(bl);
        if (OutShelfLife != null)
            OutShelfLife.SetActive(bl);
        if (ModelError != null)
            ModelError.SetActive(bl);
        if (PackingDamagee != null)
            PackingDamagee.SetActive(bl);
        if (bl&&YesImage != null)
            YesImage.SetActive(false);
        if (bl&&NoImage != null)
            NoImage.SetActive(false);
    }
}
