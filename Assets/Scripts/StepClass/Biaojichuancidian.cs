using System;
using CFramework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// 标记穿刺点
/// </summary>
public class Biaojichuancidian : StepBase
{
    //标记穿刺点结束按钮
    UPageButton btnBiaojiOk;
    GameObject biaojibi;
   
    public Biaojichuancidian()
    {
        //设置镜头
        cameraEnterPosition = new Vector3(-0.321f, 0.765f, 0.366f);
        cameraEnterEuler = new Vector3(11.3619f, 146.9361f, 3.7071f);

        //标记笔隐藏
        biaojibi = GameObject.Find("Models").transform.Find("tools/biaojibi").gameObject;
        biaojibi.transform.localRotation = Quaternion.Euler(-87.1696f, 309.8034f, -89.6495f);
        biaojibi.SetActive(false);
    }

    /// <summary>
    /// 镜头移动结束
    /// </summary>
    public override void CameraMoveFinished()
    {
        //标记笔显示
        biaojibi.SetActive(true);

        //标记穿刺点结束按钮
        btnBiaojiOk = new UPageButton(AnchoredPosition.bottom);
        btnBiaojiOk.rect = new UnityEngine.Rect(0, -20, 200, 70);
        btnBiaojiOk.text = "标记穿刺点结束";
        btnBiaojiOk.LoadSprite("anniu-160");
        btnBiaojiOk.LoadPressSprite("anniu-160h");
        btnBiaojiOk.button.text.color = UnityEngine.Color.white;
        btnBiaojiOk.onClick.AddListener(onClickBiaojiOk);

        base.CameraMoveFinished();
    }

    public override void StepUpdate()
    {
        //标记笔随鼠标移动
        biaojibi.transform.localPosition = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.1919f));

        //按下鼠标左键
        if (Input.GetMouseButtonDown(0))
        {
            //Gizmos.color = Color.green;
            //Gizmos.DrawLine(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.1919f)), Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x + 0.5f, Input.mousePosition.y, 0.1919f)));
        }
        //松开鼠标左键
        else if (Input.GetMouseButtonUp(0))
        {
            
        }
        base.StepUpdate();
    }

    /// <summary>
    /// 步骤结束
    /// </summary>
    public override void StepFinish()
    {
        //标记笔隐藏
        biaojibi.SetActive(false);
        //摧毁标记穿刺点结束按钮
        btnBiaojiOk.Destroy();

        base.StepFinish();
    }

    /// <summary>
    /// 点击标记穿刺点结束按钮
    /// </summary>
    private void onClickBiaojiOk()
    {
        //当前步骤结束
        State = StepStatus.did;
    }
}
