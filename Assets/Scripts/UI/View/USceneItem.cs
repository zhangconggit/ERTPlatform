using UnityEngine;
using System.Collections;
using CFramework;
using UnityEngine.UI;
public class USceneItem : UPageBase
{
    UImage background;
    UText title;
    UText speak;
    UText txt;
    UPageButton button;

    float diff = 10;

    public CEvent OnClick = new CEvent();

    string strimage;

    public USceneItem()
    {
        SetAnchored(AnchoredPosition.center);
        rect = new Rect(-100, 0, 1200, 60);
        name = "item";
        background = new UImage();
        background.SetParent(this);
        background.LoadImage("mxdialog_bk");
        background.SetAnchored(AnchoredPosition.full);
        background.SetBorderSpace(0, 0, 0, 0);
        
        
        title = new UText(AnchoredPosition.top);
        title.SetParent(this);
        title.baseText.fontSize = 33;
        title.rect = new Rect(0, 0, 1000, 60);
        speak = new UText(AnchoredPosition.top);
        speak.SetParent(this);
        speak.rect = new Rect(0, 0, 1000, 60);
        txt = new UText(AnchoredPosition.top);
        txt.SetParent(this);
        txt.rect = new Rect(0, 0, 1000, 60);
        button = new UPageButton(AnchoredPosition.right);
        button.SetParent(this);
        button.rect = new Rect(-30, 0, 160, 48);
        button.text = "进入";
        button.onClick.AddListener(() => { OnClick.Invoke(); });
    }
    public void SetContext(string _title,string _speak, string _txt)
    {
        title.text = _title +"：";
        
        title.Size = new Vector2(title.Size.x, title.baseText.preferredHeight );
        title.Position = new Vector2(0, diff*2);
        speak.text ="    描述："+ _speak;
        speak.Position = new Vector2(0, title.Position.y + + title.Size.y + diff);
        speak.Size = new Vector2(speak.Size.x, speak.baseText.preferredHeight );
        txt.text = "    要求：" + _txt;
        txt.Position = new Vector2(0, speak.Position.y+ speak.Size.y + diff);
        txt.Size = new Vector2(txt.Size.x, txt.baseText.preferredHeight);
        Size = new Vector2(1200, txt.Position.y + txt.Size.y + diff * 2);
    }
}