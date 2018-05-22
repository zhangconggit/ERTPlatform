using System;
using CFramework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

/// <summary>
/// 叩诊
/// </summary>
public class Kouzhen : StepBase
{
    GameObject Hand_L;
    GameObject Hand_R;
    bool isKouzhen = false;

    //叩诊手来回叩诊次数
    int maxKouzhenCnt = 4;
    //固定
    int nowKouzhenCnt = 0;
    //首次动作时保存初始手位置
    Vector3 oldHand_RVector3;
    //叩诊结束按钮
    UPageButton btnKouzhenOk;
    public Kouzhen()
    {
        //设置镜头
        cameraEnterPosition = new Vector3(-0.321f, 0.765f, 0.366f);
        cameraEnterEuler = new Vector3(11.3619f, 146.9361f, 3.7071f);

        List<string> lstGutou = new List<string>() {
            "chest_body-new/chest_body/chest_rib7"      //第七肋
            ,"chest_body-new/chest_body/chest_rib8"     //第八肋
            ,"chest_body-new/chest_body/chest_rib9"     //第九肋
            ,"chest_body-new/chest_body/chest_rib10"    //第十肋
            ,"chest_body-new/chest_body/chest_rib11"    //第十一肋
            ,"chest_body-new/chest_body/chest_gap6"     //第七肋前
            ,"chest_body-new/chest_body/chest_gap7"     //第七肋 - 第八肋之间
            ,"chest_body-new/chest_body/chest_gap8"     //第八肋 - 第九肋之间
            ,"chest_body-new/chest_body/chest_gap9"     //第九肋 - 第十肋之间
            ,"chest_body-new/chest_body/chest_gap10"    //第十肋 - 第十一肋之间
            ,"chest_body-new/chest_body/chest_blade"    //"肩胛骨"
        };
        for (int i = 0; i < lstGutou.Count; i++)
        {
            GameObject Gutou = GameObject.Find("Models").transform.Find(lstGutou[i]).gameObject;
            Gutou.SetActive(true);
        }

        //左手
        Hand_L = GameObject.Find("Models").transform.Find("chest_body-new/chest_shou_L").gameObject;
        Hand_L.SetActive(false);
        //右手(子节点)
        Hand_R = GameObject.Find("Models").transform.Find("chest_body-new/chest_shou_L/chest_shou_R").gameObject;
        Hand_R.SetActive(true);
    }
    /// <summary>
    /// 镜头移动结束
    /// </summary>
    public override void CameraMoveFinished()
    {
        //叩诊结束按钮
        btnKouzhenOk = new UPageButton(AnchoredPosition.bottom);
        btnKouzhenOk.rect = new UnityEngine.Rect(0, -20, 200, 70);
        btnKouzhenOk.text = "叩诊结束";
        btnKouzhenOk.LoadSprite("anniu-160");
        btnKouzhenOk.LoadPressSprite("anniu-160h");
        btnKouzhenOk.button.text.color = UnityEngine.Color.white;
        btnKouzhenOk.onClick.AddListener(onClickKouzhenOk);

        base.CameraMoveFinished();
    }
    /// <summary>
    /// 步骤结束
    /// </summary>
    public override void StepFinish()
    {
        //摧毁叩诊结束按钮
        btnKouzhenOk.Destroy();

        base.StepFinish();
    }
    /// <summary>
    /// 鼠标点击坐着人物触发事件
    /// </summary>
    /// <param name="obj"></param>
    public override void MouseClickModel(RaycastHit obj)
    {
        switch (obj.transform.name)
        {
            case "chest_gap6"://"第六肋间隙"
            case "chest_rib7"://"第七肋"
                VoiceControlScript.Instance.AudioPlay(AudioStyle.Environment, "knock_hollow", false);//播放清音
                MoveHands(obj.point, obj.normal);
                break;
            case "chest_blade":// "肩胛骨"
            case "chest_rib8": //第八肋
            case "chest_rib9": //第九肋
            case "chest_rib10": //第十肋
            case "chest_rib11": //第十一肋
            case "chest_gap7": //第七肋间隙
            case "chest_gap8": //第八肋间隙
            case "chest_gap9": //第九肋间隙
            case "chest_gap10": //第十肋间隙
                VoiceControlScript.Instance.AudioPlay(AudioStyle.Environment, "knock_solid", false);//播放实音
                MoveHands(obj.point, obj.normal);
                break;
        }
        
        base.MouseClickModel(obj);
    }

    /// <summary>
    /// 移动叩诊手
    /// </summary>
    /// <param name="Position">鼠标点</param>
    private void MoveHands(Vector3 position, Vector3 normal)
    {
        if (isKouzhen)
        {
            return;
        }

        // 显示叩诊手
        this.Show();
        //叩诊左手(主手)显示位置设定
        ModelCtrol.Instance.setModelsOnNormalline(Hand_L, normal, new Vector3(0, 0, 1), 270);
        Hand_L.transform.position = normal * 0.02f + position; ;//指定位置

        //执行叩诊动作
        ExcutingKouHand();
    }

    /// <summary>
    /// 显示叩诊手
    /// </summary>
    private void Show()
    {
        Hand_L.SetActive(true);

        // 首次动作时保存初始手位置
        oldHand_RVector3 = Hand_R.transform.localPosition;
        nowKouzhenCnt = 0;
        isKouzhen = true;
    }

    /// <summary>
    /// 隐藏叩诊手
    /// </summary>
    public void Hide()
    {
        Hand_L.SetActive(false);
        isKouzhen = false;
    }
    /// <summary>
    /// 执行叩诊动作
    /// </summary>
    private void ExcutingKouHand()
    {
        nowKouzhenCnt++;
        if (nowKouzhenCnt % 2 == 0)
        {
            if (nowKouzhenCnt == maxKouzhenCnt)
            {
                //叩诊结束时隐藏叩诊手
                ModelCtrol.Instance.MoveModel(Hand_R, oldHand_RVector3, 0.2f, Hide);
            }
            else
            {
                ModelCtrol.Instance.MoveModel(Hand_R, oldHand_RVector3, 0.2f, ExcutingKouHand);
            }
        }
        else
        {
            if (nowKouzhenCnt == maxKouzhenCnt)
            {
                maxKouzhenCnt = maxKouzhenCnt + 1;
            }

            ModelCtrol.Instance.MoveModel(Hand_R, new Vector3(oldHand_RVector3.x, oldHand_RVector3.y + 0.01f, oldHand_RVector3.z), 0.2f, ExcutingKouHand);
        }
    }

    /// <summary>
    /// 点击叩诊结束按钮
    /// </summary>
    private void onClickKouzhenOk()
    {
        //当前步骤结束
        State = StepStatus.did;
    }
}
