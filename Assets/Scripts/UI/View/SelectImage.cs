using UnityEngine;
using System.Collections;
using CFramework;
using UnityEngine.UI;
using System.Collections.Generic;
using System;

/// <summary>
/// 剖面辅助图
/// </summary>
public class SelectImage : UPageBase
{

    UImage backgraundImage;
    List<UToogleItem> images=new List<UToogleItem>();
    UPageButton okButton;
    UPageButton currentButton;
    public CEvent<string> OnOkbutton = new CEvent<string>();

    public SelectImage()
    {
        name = "SelectImage";
        SetAnchored(AnchoredPosition.full);
        SetBorderSpace(0, 0, 0, 0);

        backgraundImage = new UImage(AnchoredPosition.full);
        backgraundImage.SetParent(this);
        backgraundImage.color = new  Color(33/255,33/255,33/255,33/255);
        backgraundImage.gameObejct.AddComponent<ToggleGroup>();

        okButton = new UPageButton(AnchoredPosition.bottom);
        okButton.SetParent(this);
        okButton.rect = new Rect(0, -80, 160, 48);
        okButton.name = "okButton";
        okButton.LoadSprite("keys_030");
        okButton.LoadPressSprite("keys_031");
        okButton.text = "确定";
        okButton.onClick.AddListener(ButtenEven);
    }
    public void SetButtomImage(List<string> imagePath, List<string> afterPath)
    {
        Rect pos = new Rect(0, 0, 350, 600);
        int diff = 50;
        int[] ran = MyCommon.RandomRepeat(imagePath.Count);
        for (int i = 0; i < imagePath.Count; i++)
        {

            pos.x = i * (pos.width + diff) - (pos.width + diff) * imagePath.Count / 2 + pos.width/2 + diff/2;
            CreatImage(imagePath[ran[i]], afterPath[ran[i]], pos);
        }
    }
    public void CreatImage(string path,string aftrer, Rect pos)
    {
        UToogleItem image = new UToogleItem(path,new Rect(0,0, pos.width,pos.height));
        image.SetParent(backgraundImage);
        image.rect = pos;
        image.name = path;
        image.SetAnchored(AnchoredPosition.center);
        image.LoadSelectedImage(aftrer);
        image.SetGroup(backgraundImage.gameObejct.GetComponent<ToggleGroup>());
        images.Add(image);
    }
    public void SetButton(List<string> imagePathDefault, List<string> imagePath)
    {
        Rect pos = new Rect(0, 0, 400, 400);
        int diff = 80;
        int[] ran = MyCommon.RandomRepeat(imagePath.Count);
        for (int i = 0; i < imagePath.Count; i++)
        {
            //pos.x = i * (pos.width + diff) - (pos.width + diff) * imagePath.Count / 2 + pos.width / 2 + diff / 2;
            pos.x = (pos.width/2 + diff) * (i % 2 == 0 ? -1 : 1);
            pos.y = (pos.height/2 + diff/2 ) * (i / 2 < 1 ? -1 : 1) - 40;
            CreatButton(imagePathDefault[ran[i]], imagePath[ran[i]], pos);
        }
    }

    public void CreatButton(string pathDefault, string path, Rect pos)
    {
        UPageButton btn = new UPageButton();
        btn.SetParent(backgraundImage);
        btn.name = path;
        btn.SetAnchored(AnchoredPosition.center);
        btn.rect = pos;
        btn.LoadSprite(pathDefault);
        btn.LoadPressSprite(path);
        btn.onClick.AddListener(()=> { OnImageButtonClick(btn); });
    }

    public void OnImageButtonClick(UPageButton btn)
    {
        if (null != currentButton)
            currentButton.LoadSprite(currentButton.sprite.name.Replace("_h", ""));
        currentButton = btn;
        currentButton.LoadSprite(btn.sprite.name+"_h");
    }
    void ButtenEven()
    {
        string str = "";
        if(currentButton != null)
            str = currentButton.name;
        foreach (var item in images)
        {
            if (item.selected)
            {
                str = item.gameObejct.name;
                break;
            }
        }
        OnOkbutton.Invoke(str);
    }
}

