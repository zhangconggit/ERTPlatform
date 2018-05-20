using UnityEngine;
using System.Collections;
using CFramework;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;

/// <summary>
/// 叩诊_手
/// </summary>
public class UKouzhenHand : UPageBase
{
    private GameObject Hand_L;
    private GameObject Hand_R;
    private bool isExcuting = false;

    //叩诊手来回叩诊次数
    int maxKouzhenCnt = 4;

    //固定
    int nowKouzhenCnt = 0;
    //首次动作时保存初始手位置
    Vector3 oldHand_RVector3;

    public UKouzhenHand()
    {
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
        //Hand_L.transform.localPosition = new Vector3(-0.05370078f, 0.7597989f, 0.1519342f);
        //Hand_L.transform.localRotation = Quaternion.Euler(324.3471f, 95.9215f, 169.9006f);
        Hand_L.SetActive(false);
        //右手(子节点)
        Hand_R = GameObject.Find("Models").transform.Find("chest_body-new/chest_shou_L/chest_shou_R").gameObject;
        //Hand_R.transform.localPosition = new Vector3(-0.03f, 0.004f, -0.03f);
        //Hand_R.transform.localRotation = Quaternion.Euler(347.3566f, 327.664f, 8.0002f);
        Hand_R.SetActive(true);
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
        isExcuting = true;
    }
    /// <summary>
    /// 隐藏叩诊手
    /// </summary>
    public void Hide()
    {
        Hand_L.SetActive(false);
        isExcuting = false;
    }
    /// <summary>
    /// 移动叩诊手
    /// </summary>
    /// <param name="Position">鼠标点</param>
    public void MoveHands(Vector3 position, Vector3 normal)
    {
        if (isExcuting)
        {
            return;
        }
        // 显示叩诊手
        this.Show();
        //叩诊左手(主手)显示位置设定
        //Hand_L.transform.position = position;
        //ModelCtrol.Instance.setModelsOnNormalline(Hand_L, normal, new Vector3(0,0,1), 0);//指定方向
        //Quaternion qNormal = Quaternion.Euler(normal);
        Hand_L.transform.rotation = Quaternion(Hand_L, normal);//指定方向
        Hand_L.transform.position = normal * 0.02f + position; ;//指定位置

        //执行叩诊动作
        ExcutingKouHand();

        //this.setModelsOnNormal(Hand_L, normal, new Vector3(1, 0, 0), 0);//指定方向
        //Hand_L.transform.position = normal * 0.01f + position; ;//指定位置
    }

    /// <Summary>
    /// 指定模型状态方向与点击点的法线偏移角度
    /// </Summary>   
    public Quaternion Quaternion(GameObject obj, Vector3 normal)
    {
        float angle = Vector3.Angle(obj.transform.rotation.eulerAngles, normal);
        float radians = angle * Mathf.PI * 2;
        //float radians = (angle + 180f) / 360f * Mathf.PI * 2;
        float w = Mathf.Cos(radians / 2);
        float s = Mathf.Sin(radians / 2);

        Vector3 f = new Vector3(0, 0, 0);

        float x = normal.x * s;
        float y = normal.y * s;
        float z = normal.z * s;

        return new Quaternion(x, y, z, w);

        //float angleoffset = 0;

        //float angle = Vector3.Angle(normal, f);
        //Vector3 cross = -Vector3.Normalize(Vector3.Cross(normal, f));
        //Quaternion q;
        //q.w = Mathf.Cos(((angle - angleoffset) / 2) * Mathf.PI / 180);
        //q.x = cross.x * Mathf.Sin(((angle - angleoffset) / 2) * Mathf.PI / 180);
        //q.y = cross.y * Mathf.Sin(((angle - angleoffset) / 2) * Mathf.PI / 180);
        //q.z = cross.z * Mathf.Sin(((angle - angleoffset) / 2) * Mathf.PI / 180);

        //return q;
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
}
