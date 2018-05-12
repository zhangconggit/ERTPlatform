
using System.Collections.Generic;
using UnityEngine;

namespace CFramework
{
    public class UMessageBox : UPageBase
    {
        public UPlane back;
        // public UImage top;
        public UText tilte;
        public UText context;
        public UPageButton okButton;
        VoidDelegate _buttonCallback;
        /// <summary>
        /// 单按钮
        /// </summary>
        static UMessageBox staticMessageBox = null;
        /// <summary>
        /// 双按钮
        /// </summary>
        static UMessageBox staticMessageBoxTwo = null;

        private static UPlane bg;

        List<UPageButton> buttomList = new List<UPageButton>();
        public UMessageBox()
        {
            rect = new UnityEngine.Rect(0, 0, 750, 400);

            GameObject gameObject = GameObject.Find("UMessageBg");
            if (gameObject == null)
            {
                if (UMessageBox.bg == null)
                {
                    UMessageBox.bg = new UPlane();
                    UMessageBox.bg.name = "UMessageBg";
                    UMessageBox.bg.SetBorderSpace(0f, 0f, 0f, 0f);
                    UMessageBox.bg.color = new Color(0f, 0f, 0f, 0.09803922f);
                    UMessageBox.bg.gameObejct.SetActive(false);
                }
            }
            else
            {
                UMessageBox.bg.gameObejct = gameObject;
            }

            back = new UPlane();
            back.SetParent(this);
            back.SetBorderSpace(0, 0, 0, 0);
            back.LoadImage("tanchu-tishi");
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
            okButton.LoadSprite("anniu-160");
            okButton.LoadPressSprite("anniu-160h");
            okButton.onClick.AddListener(OnOkButton);
            okButton.button.text.color = UnityEngine.Color.white;
            buttomList.Add(okButton);

        }


        public void SetPageRect(UnityEngine.Rect newRect)
        {
            this.rect = new UnityEngine.Rect();
        }
        public void AddButtonLine(string showText, Vector2 buttonsSize, VoidDelegate OnCallback = null)
        {
            UPageButton add1;// = new UPageButton();
            add1 = new UPageButton(AnchoredPosition.bottom);
            add1.SetParent(this);
            add1.rect = new UnityEngine.Rect(0, -20, 200, 70);
            add1.text = showText;
            add1.LoadSprite("anniu-160");
            add1.LoadPressSprite("anniu-160h");
            add1.onClick.AddListener(OnCallback);
            add1.button.text.color = UnityEngine.Color.white;
            add1.button.GetComponent<UButton>().pressColor = Color.cyan;
            buttomList.Add(add1);
            for (int i = 0; i < buttomList.Count; i++)
            {
                buttomList[i].rect = new UnityEngine.Rect(new Vector2((200 + 30) * (i - (buttomList.Count - 1) / 2f), -40), buttonsSize);
            }

        }
        public void AddButtonSide(string showText, Vector2 buttonsSize, string spritePath, VoidDelegate OnCallback = null)
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
                buttomList[i].rect = new UnityEngine.Rect(new Vector2(0, (buttonsSize.y + 20) * (i - (buttomList.Count - 1)) - 30), buttonsSize);
                buttomList[i].button.text.alignment = TextAnchor.LowerCenter;
                buttomList[i].button.GetComponent<UButton>().transition = ButtonTransition.ColorTint;
                buttomList[i].button.text.color = new Color32(219, 124, 21, 255);
                buttomList[i].button.GetComponent<UButton>().pressColor = Color.cyan;
            }
        }

        public void SetMessage(string title, string context, string buttonText, VoidDelegate buttonCallback)
        {
            tilte.text = title;
            this.context.text = context;
            okButton.text = buttonText;
            _buttonCallback = buttonCallback;
        }
        public void OnOkButton()
        {
            if (_buttonCallback != null)
                _buttonCallback.Invoke();
            gameObejct.SetActive(false);
            UMessageBox.bg.gameObejct.SetActive(false);
        }

        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="context"></param>
        /// <param name="buttonText"></param>
        /// <param name="buttonCallback"></param>
        public static void Show(string title, string context, string buttonText, VoidDelegate buttonCallback)
        {
            if (staticMessageBox == null)
                staticMessageBox = new UMessageBox();
            else
                staticMessageBox.gameObejct.SetActive(true);

            UMessageBox.bg.gameObejct.SetActive(true);
            staticMessageBox.tilte.text = title;
            staticMessageBox.context.text = context;
            if (buttonText == "")
            {
                UMessageBox.staticMessageBox.okButton.gameObejct.SetActive(false);
            }
            else
            {
                UMessageBox.staticMessageBox.okButton.gameObejct.SetActive(true);
            }
            staticMessageBox.okButton.text = buttonText;
            staticMessageBox._buttonCallback = buttonCallback;
            UMessageBox.bg.transform.SetAsLastSibling();
            staticMessageBox.transform.SetAsLastSibling();
        }
        /// <summary>
        /// 显示消息框
        /// </summary>
        /// <param name="title"></param>
        /// <param name="context"></param>
        /// <param name="buttonText"></param>
        /// <param name="buttonCallback"></param>
        /// <param name="NobuttonText"></param>
        /// <param name="NobuttonCallback"></param>
        public static void Show(string title, string context, string buttonText, VoidDelegate buttonCallback, string NobuttonText, VoidDelegate NobuttonCallback)
        {
            if (staticMessageBoxTwo == null)
            {
                staticMessageBoxTwo = new UMessageBox();
                staticMessageBoxTwo.AddButtonLine("NO", new Vector2(200, 70), null);
            }
            else
                staticMessageBoxTwo.gameObejct.SetActive(true);

            UMessageBox.bg.gameObejct.SetActive(true);
            staticMessageBoxTwo.tilte.text = title;
            staticMessageBoxTwo.context.text = context;
            staticMessageBoxTwo.okButton.text = buttonText;
            staticMessageBoxTwo._buttonCallback = buttonCallback;
            staticMessageBoxTwo.buttomList[1].text = NobuttonText;
            staticMessageBoxTwo.buttomList[1].onClick.RemoveAllListener();
            staticMessageBoxTwo.buttomList[1].onClick.AddListener(NobuttonCallback);
            staticMessageBoxTwo.buttomList[1].onClick.AddListener(() => { staticMessageBoxTwo.gameObejct.SetActive(false); UMessageBox.bg.gameObejct.SetActive(false); });
            UMessageBox.bg.transform.SetAsLastSibling();
            staticMessageBoxTwo.transform.SetAsLastSibling();
        }
    }

}
