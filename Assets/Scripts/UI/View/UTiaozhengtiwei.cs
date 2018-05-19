using UnityEngine;
using System.Collections;
using CFramework;
using UnityEngine.UI;
using System.Collections.Generic;

public delegate void ClickEventHandler(UPageButton imageBtn);
/// <summary>
/// 调整体位_人物
/// </summary>
public class UTiaozhengtiwei : UPageBase
{
    public GameObject StandPeople;
    public GameObject SitPeople;
    public bool isShow;
    public event ClickEventHandler clickEvent;

    public UTiaozhengtiwei()
    {
        StandPeople = GameObject.Find("Models").transform.Find("chest_body-new/chest_body_s").gameObject;
        StandPeople.transform.localPosition = new Vector3(-0.5139443f, -0.002361169f, -0.3850695f);
        StandPeople.transform.localRotation = Quaternion.Euler(-90, 92.6531f, 0);
        StandPeople.SetActive(false);

        SitPeople = GameObject.Find("Models").transform.Find("chest_body-new/chest_body").gameObject;
        SitPeople.transform.localPosition = new Vector3(1.187028f, -0.002361169f, 0.2012178f);
        SitPeople.transform.localRotation = Quaternion.Euler(270, 0, 0);
        SitPeople.SetActive(false);

        isShow = false;
    }
    /// <summary>
    /// 创建体位图
    /// </summary>
    /// <param name="lstTiweiImagePath"></param>
    public void SetTiweiImages(Dictionary<string, string> lstTiweiImagePath)
    {
        //分辨率：1920 x 1080
        //图片初期位置和大小
        Rect pos = new Rect(0, 0, 400, 600);
        //图片间的间距
        int diff = 50;
        int i_imagesCnt = lstTiweiImagePath.Count;
        //随机数
        int[] ran = MyCommon.RandomRepeat(i_imagesCnt);

        int index = 0;
        foreach (KeyValuePair<string, string> keyValue in lstTiweiImagePath)
        {
            pos.x = ran[index] * (pos.width + diff) - (pos.width + diff) * i_imagesCnt / 2 + pos.width / 2 + diff / 2;

            CreatImageBtn(keyValue.Key, keyValue.Value, pos);
            index++;
        }
        isShow = true;
    }

    /// <summary>
    /// 创建体位图
    /// </summary>
    /// <param name="defPath">初期图片</param>
    /// <param name="path">图片</param>
    /// <param name="pos">图片位置</param>
    private void CreatImageBtn(string defPath, string path, Rect pos)
    {
        UPageButton btn = new UPageButton();
        btn.SetParent(this);
        btn.name = path;
        btn.SetAnchored(AnchoredPosition.center);
        btn.rect = pos;
        btn.LoadSprite(defPath);
        btn.LoadPressSprite(path);

        btn.onClick.AddListener(() => { clickEvent(btn); });
    }
}
