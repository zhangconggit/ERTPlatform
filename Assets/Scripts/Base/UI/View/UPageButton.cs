using UnityEngine;
using UnityEngine.UI;

namespace CFramework
{
    /// <summary>
    /// 按钮
    /// </summary>
    public class UPageButton : UPageBase
    {
        /// <summary>
        /// 点击事件
        /// </summary>
        public CEvent onClick = new CEvent();
        /// <summary>
        /// 
        /// </summary>
        UButton baseButton;
        Image baseImage;
        UText baseText;
        /// <summary>
        /// 按钮控件 设置键盘按键和长按事件
        /// </summary>
        public UButton button
        {
            get
            {
                return baseButton;
            }
        }
        //PlanePosition pos = PlanePosition.center;
        public Sprite sprite
        {
            get { return baseButton.normalImage; }
            set
            {
                baseButton.normalImage = value;
                baseImage.sprite = value;
            }
        }
        public Sprite pressSprite
        {
            get { return baseButton.pressImage; }
            set { baseButton.pressImage = value; }
        }
        public string text
        {
            get { return baseButton.text.text; }
            set { baseButton.text.text = value; }
        }
        public UPageButton()
        {
            init();
        }
        public UPageButton(AnchoredPosition _pos)
        {
            init();
            SetAnchored(_pos);
        }
        /// <summary>
        /// 按钮上的字
        /// </summary>
        /// <param name="name"></param>
        public UPageButton(string text)
        {
            init();
            this.text = text;
        }
        void init()
        {
            name = "UPageButton";
            baseButton = gameObejct.AddComponent<UButton>();
            baseButton.magnification = 1;
            baseImage = gameObejct.AddComponent<Image>();
            rect = new Rect(0, 0, 200, 200);
            baseButton.AddListionEvent(OnPress);
            baseText = new UText(AnchoredPosition.full);
            baseText.SetParent(this);
            baseText.SetBorderSpace(0, 0, 0, 0);
            baseText.baseText.color = Color.black;
            baseButton.text = baseText.baseText;
            baseButton.text.alignment = TextAnchor.MiddleCenter;
            baseButton.transition = ButtonTransition.ColorTint;
        }
        public new Rect rect
        {
            get
            {
                return base.rect;
            }
            set
            {
                base.rect = value;
                //if(baseText != null)
                //    baseText.rect = new Rect(0, 0, rect.width, rect.height);
            }
        }
        /// <summary>
        /// 加载默认图片
        /// </summary>
        /// <param name="path"></param>
        public void LoadSprite(string path)
        {
            if(UIRoot.Instance.GetCustomSprite(path)!=null)
            {
                baseButton.transition = ButtonTransition.SpriteSwap;
                sprite = UIRoot.Instance.GetCustomSprite(path);//Resources.Load<Sprite>(path);
            }         
        }
        /// <summary>
        /// 加载按下的图片
        /// </summary>
        /// <param name="path"></param>
        public void LoadPressSprite(string path)
        {
            if(UIRoot.Instance.GetCustomSprite(path)!=null)
            {
                baseButton.transition = ButtonTransition.SpriteSwap;
                pressSprite = UIRoot.Instance.GetCustomSprite(path);//Resources.Load<Sprite>(path);
            }
        }
        bool _enable = true;
        public bool enable
        {
            get
            {
                return _enable;
            }
            set
            {
                _enable = value;
                if (_enable)
                {
                    baseImage.color = new Color(0.5f, 0.5f, 0.5f);
                }
                else
                {
                    baseImage.color = new Color(1f, 1f, 1f);
                }
            }
        }
        void OnPress()
        {
            if (_enable)
                onClick.Invoke();
        }
    }
}
