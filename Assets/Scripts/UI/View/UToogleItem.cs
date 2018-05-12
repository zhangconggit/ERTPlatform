using UnityEngine;
using System.Collections;
using CFramework;
using UnityEngine.UI;

public class UToogleItem : UPageBase
{
    UImage background;
    UImage checkImage;//选中

    string strimage;
    public UToogleItem(string imagesrc, Rect rect)
    {
        strimage = imagesrc;
        background = new UImage();
        background.SetParent(this);
        background.name = "image";
        background.SetAnchored(AnchoredPosition.top_left);
        background.rect = rect;
        background.LoadImage(imagesrc);

        checkImage = new UImage();
        checkImage.name = "选中标志";
        checkImage.LoadImage("boder");
        checkImage.SetParent(this);
        checkImage.SetAnchored(AnchoredPosition.top_left);

        checkImage.rect = rect;

        gameObejct.AddComponent<Toggle>();
        gameObejct.GetComponent<Toggle>().graphic = checkImage.gameObejct.GetComponent<Image>();
    }

    public string getimage(){
        return strimage;
    }
}
