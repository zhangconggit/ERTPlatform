using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using CFramework;

public class biaobenbiaoji : StepBase
{
    /// <summary>
    /// 病原体检查标签
    /// </summary>
    GameObject byt;

    /// <summary>
    /// 常规检查标签
    /// </summary>
    GameObject cg;

    /// <summary>
    /// 生化检查标签
    /// </summary>
    GameObject sh;

    /// <summary>
    /// 试管一
    /// </summary>
    GameObject tubeOne;

    /// <summary>
    /// 试管二
    /// </summary>
    GameObject tubeTwo;

    /// <summary>
    /// 试管三
    /// </summary>
    GameObject tubeThree;

    /// <summary>
    /// 试管架子
    /// </summary>
    GameObject SGJ;

    /// <summary>
    /// 点击标签时保持当前位置
    /// </summary>
    Vector3 bqMousePos;

    /// <summary>
    /// 被选中的试管的名称
    /// </summary>
    string sgObject;

    /// <summary>
    /// 被鼠标选择的物体
    /// </summary>
    GameObject mouseObject;

    /// <summary>
    /// 止血钳图片按钮
    /// </summary>
    UPageButton end;

    /// <summary>
    /// 瓶子对应的标签
    /// </summary>
    Dictionary<string, string> bqList;

    public biaobenbiaoji()
    {
        //设置摄像机的位置
        cameraEnterPosition = new Vector3(-1.948f, 0.911f, 0.275f);
        //设置摄像机的旋转角度
        cameraEnterEuler = new Vector3(-3.9102f, 269.0612f, 359.82f);
        // 获取试管架子
        SGJ = GameObject.Find("Models").transform.Find("tools/积液收集_试管_02").gameObject;
        //获取病原体检查标签
        byt = GameObject.Find("Models").transform.Find("tools/积液收集_试管_02/byt").gameObject;
        //获取常规检查标签
        cg = GameObject.Find("Models").transform.Find("tools/积液收集_试管_02/cg").gameObject;
        //获取生化检查标签
        sh = GameObject.Find("Models").transform.Find("tools/积液收集_试管_02/sh").gameObject;

        tubeOne = GameObject.Find("Models").transform.Find("tools/积液收集_试管_02/sj1_tube1").gameObject;
        tubeTwo = GameObject.Find("Models").transform.Find("tools/积液收集_试管_02/sj1_tube2").gameObject;
        tubeThree = GameObject.Find("Models").transform.Find("tools/积液收集_试管_02/sj1_tube3").gameObject;

        bqList = new Dictionary<string, string>();
        mouseObject = null;
        sgObject = null;

        UImage backgroup = CreateUI<UImage>();
        backgroup.SetAnchored(AnchoredPosition.right);
        backgroup.rect = new Rect(-10, 0, 100, 100);
        backgroup.LoadImage("rihgtMenuBg");

        end = CreateUI<UPageButton>();
        end.SetAnchored(AnchoredPosition.right);
        end.rect = new Rect(-10, 0, 100, 100);//0
        end.LoadSprite("Inspect_toolbar");
        end.LoadPressSprite("Injector_50ml");

        end.onClick.AddListener(OnEnd);
        //未标记标本管
        AddScoreItem("10018181");

    }

    private void OnEnd()
    {

        if (bqList.Count == 3)
        {

            if (bqList["sj1_tube1"].Equals(byt))
            {
                AddScoreItem("10018180");
                RemoveScoreItem("10018181");
            }
            else
            {
                AddScoreItem("10018182");
                RemoveScoreItem("10018181");
            }
            
            
        }
        State = StepStatus.did;
    }

    public override void EnterStep(float cameraTime = 1)
    {
        base.EnterStep(cameraTime);
        SGJ.SetActive(true);
        byt.SetActive(true);
        cg.SetActive(true);
        sh.SetActive(true);

        tubeOne.SetActive(true);
        tubeTwo.SetActive(true);
        tubeThree.SetActive(true);

        tubeOne.GetComponent<MeshRenderer>().materials[2].mainTexture = null;
        tubeTwo.GetComponent<MeshRenderer>().materials[0].mainTexture = null;
        tubeThree.GetComponent<MeshRenderer>().materials[2].mainTexture = null;
        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "trbq", true, null);
    }

    public override void StepUpdate()
    {
        if (mouseObject != null)
        {
            if (Input.GetMouseButton(0))
            {
                sgObject = null;
                Vector3 mouseVector3 = Camera.main.ScreenToWorldPoint(new Vector3(
                          Input.mousePosition.x
                        , Input.mousePosition.y
                        , bqMousePos.z
                ));
                mouseObject.transform.position = mouseVector3;

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//射线的起点
                RaycastHit hit;//射线的终点
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.name == "sj1_tube1" || hit.transform.name == "sj1_tube2" || hit.transform.name == "sj1_tube3")
                    {
                        sgObject = hit.transform.name;
                        //TODO 将这个试管点亮
                    }
                    if (hit.transform.name != "sj1_tube1" && hit.transform.name != "sj1_tube2" && hit.transform.name != "sj1_tube3")
                    {
                        sgObject = null;
                    }
                }
            }
            if (Input.GetMouseButtonUp(0))
            {
                if (sgObject != null)
                {
                    //隐藏已经贴过的标签
                    mouseObject.SetActive(false);
                    //显示垃圾桶
                    Texture2D tex = UIRoot.Instance.UIResources[string.Format("{0}image" + "_T", mouseObject.transform.name)] as Texture2D;
                    SetSgtext(sgObject, tex);
                    if (!bqList.ContainsKey(sgObject))
                    {
                        bqList.Add(sgObject, mouseObject.transform.name);
                    }
                    mouseObject = null;
                    //步骤结束
                    //if (bqList.Count >= 3)
                    //{
                    //    State = StepStatus.did;
                    //}
                }
                else
                {
                    //mouseObject.transform.localScale = mouseObject.transform.localScale * 2;
                    mouseObject.SetActive(true);
                    //回到原来位置
                    mouseObject.transform.position = bqMousePos;

                    mouseObject = null;
                }
            }
        }
        base.StepUpdate();
    }

    /// <summary>
    /// 给试管设置标签
    /// </summary>
    /// <param name="name"></param>
    /// <param name="txt"></param>
    private void SetSgtext(string name, Texture2D txt)
    {
        switch (name)
        {
            case "sj1_tube1":
                tubeOne.GetComponent<MeshRenderer>().materials[2].mainTexture = txt;
                break;
            case "sj1_tube2":
                tubeTwo.GetComponent<MeshRenderer>().materials[0].mainTexture = txt;
                break;
            case "sj1_tube3":
                tubeThree.GetComponent<MeshRenderer>().materials[2].mainTexture = txt;
                break;
            default:
                break;
        }
    }

    public override void MouseClickModel(RaycastHit obj)
    {
        if (obj.transform.name == "sh")
        {
            //sh.transform.localScale = sh.transform.localScale / 2;
            mouseObject = sh;
            bqMousePos = obj.transform.position;

        }
        if (obj.transform.name == "byt")
        {
            //byt.transform.localScale = byt.transform.localScale / 2;
            mouseObject = byt;
            bqMousePos = obj.transform.position;
        }
        if (obj.transform.name == "cg")
        {
            //cg.transform.localScale = cg.transform.localScale / 2;
            mouseObject = cg;
            bqMousePos = obj.transform.position;
        }
        base.MouseClickModel(obj);
    }


    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
