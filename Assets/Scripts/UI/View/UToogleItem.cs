using UnityEngine;
using System.Collections;
using CFramework;
using UnityEngine.UI;

public class UToogleItem : UPageBase
{
    UImage background;
    UImage checkImage;//选中
    public CEvent<bool> OnChange = new CEvent<bool>();

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

        Toggle tog = gameObejct.AddComponent<Toggle>();
        gameObejct.GetComponent<Toggle>().graphic = checkImage.gameObejct.GetComponent<Image>();
        tog.onValueChanged.AddListener((bl) => { OnChange.Invoke(bl); });
    }
    public void LoadSelectedImage(string path)
    {
        checkImage.LoadImage(path);
    }
    public bool selected
    {
        get
        {
            return gameObejct.GetComponent<Toggle>().isOn;
        }
        set
        {
            gameObejct.GetComponent<Toggle>().isOn = value;
        }

    }
    public string getimage()
    {
        return strimage;
    }
    public void SetGroup(ToggleGroup group )
    {
        gameObejct.GetComponent<Toggle>().group = group;
    }
}
