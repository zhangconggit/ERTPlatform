
using System.Collections.Generic;
using UnityEngine;

namespace CFramework
{
    class UMessageBox:UPageBase
    {
        public UPlane back;
       // public UImage top;
        public UText tilte;
        public UText context;
        public UPageButton okButton;
        VoidDelegate _buttonCallback;
        static UMessageBox staticMessageBox = null;
        List<UPageButton> buttomList = new List<UPageButton>();
        public UMessageBox()
        {
            rect = new UnityEngine.Rect(0, 0, 750, 400);
            back = new UPlane();
            back.SetParent(this);
            back.SetBorderSpace(0, 0, 0, 0);
            back.LoadImage("Images/Common/tanchu-tishi");
            //top = new UImage(AnchoredPosition.top_right);
            //top.SetParent(this);
            //top.rect = new UnityEngine.Rect(0, 0, 50, 50);
            //top.LoadImage("Images/Common/dl-zhanghao");

            tilte = new UText(AnchoredPosition.top);
            tilte.SetParent(this);
            tilte.rect = new UnityEngine.Rect(0, 0, 500, 70);
            tilte.baseText.alignment = UnityEngine.TextAnchor.MiddleCenter;
            tilte.text = "标题";
            context = new UText();
            context.SetParent(this);
            context.rect = new UnityEngine.Rect(0, 0, 500, 260);
            context.baseText.alignment = UnityEngine.TextAnchor.MiddleCenter;
            context.text = "内容";
            context.baseText.color = UnityEngine.Color.black;

            okButton = new UPageButton(AnchoredPosition.bottom);
            okButton.SetParent(this);
            okButton.rect = new UnityEngine.Rect(0, -20, 200, 70);
            okButton.text = "确认";
            okButton.LoadSprite("Images/Common/anniu-160");
            okButton.LoadPressSprite("Images/Common/anniu-160h");
            okButton.onClick.AddListener(OnOkButton);
            okButton.button.text.color = UnityEngine.Color.white;
            buttomList.Add(okButton);
            if (staticMessageBox == null)
                staticMessageBox = this;
        }


        public void SetPageRect(UnityEngine.Rect newRect)
        {
            this.rect = new UnityEngine.Rect();
        }
        public void AddButtonLine(string showText,Vector2 buttonsSize,VoidDelegate OnCallback=null)
        {
            UPageButton add1;// = new UPageButton();
            add1 = new UPageButton(AnchoredPosition.bottom);
            add1.SetParent(this);
            add1.rect = new UnityEngine.Rect(0, -20, 200, 70);
            add1.text = showText;
            add1.LoadSprite("Images/Common/anniu-160");
            add1.LoadPressSprite("Images/Common/anniu-160h");
            add1.onClick.AddListener(OnCallback);
            add1.button.text.color = UnityEngine.Color.white;
            //add1.button.GetComponent<UButton>().pressColor = Color.cyan;
            buttomList.Add(add1);
            for (int i = 0; i < buttomList.Count; i++)
            {
                buttomList[i].rect = new UnityEngine.Rect( new Vector2((200 + 30) * (i - (buttomList.Count-1) / 2f), -40), buttonsSize);
            }
            
        }
        public void AddButtonSide(string showText, Vector2 buttonsSize,string spritePath, VoidDelegate OnCallback = null)
        {
            UPageButton add1;// = new UPageButton();
            add1 = new UPageButton(AnchoredPosition.bottom);
            add1.SetParent(this);
            add1.rect = new UnityEngine.Rect(0, -20, 250, 70);
            add1.text = showText;
            add1.LoadSprite(spritePath);
            add1.LoadPressSprite(spritePath);
            add1.onClick.AddListener(OnCallback);
            add1.button.text.color = UnityEngine.Color.white;
            //add1.button.GetComponent<UButton>().pressColor = Color.cyan;
            //add1.button.GetComponent<UButton>().transition = ButtonTransition.ColorTint;
            buttomList.Add(add1);
            for (int i = 0; i < buttomList.Count; i++)
            {
                buttomList[i].rect = new UnityEngine.Rect(new Vector2(0,(buttonsSize.y+20) * (i - (buttomList.Count - 1))-30), buttonsSize);
                buttomList[i].button.text.alignment = TextAnchor.LowerCenter;
               // buttomList[i].button.GetComponent<UButton>().transition = ButtonTransition.ColorTint;
                buttomList[i].button.text.color = new Color32(219,124,21,255);
               // buttomList[i].button.GetComponent<UButton>().pressColor =Color.cyan;
            }
        }

        public void SetMessage(string title,string context,string buttonText,VoidDelegate buttonCallback)
        {
            tilte.text = title;
            this.context.text = context;
            okButton.text = buttonText;
            _buttonCallback = buttonCallback;
        }
        public void OnOkButton()
        {
            if(_buttonCallback != null)
                _buttonCallback.Invoke();
            gameObejct.SetActive(false);
        }
        public static void Show(string title, string context, string buttonText, VoidDelegate buttonCallback)
        {
            if (staticMessageBox == null)
                staticMessageBox = new UMessageBox();
            else
                staticMessageBox.gameObejct.SetActive(true);
            staticMessageBox.tilte.text = title;
            staticMessageBox.context.text = context;
            staticMessageBox.okButton.text = buttonText;
            staticMessageBox._buttonCallback = buttonCallback;
        }
    }

}
