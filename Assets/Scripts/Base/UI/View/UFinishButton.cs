using UnityEngine;
using UnityEngine.UI;

namespace CFramework
{
    /// <summary>
    /// 按钮
    /// </summary>
    public class UFinishButton : UPageBase
    {
        public UPageButton baseButton;
        public UFinishButton()
        {
            InitDate();
        }
        public UFinishButton(string Text)
        {
            InitDate();
            baseButton.text = Text;

        }
        public UFinishButton(string Text, Rect rect, AnchoredPosition anchored)
        {
            InitDate();
            baseButton.text = Text;
            SetAnchored(anchored);
            this.rect = rect;
            baseButton.rect = new Rect(0, 0, rect.width, rect.height);
        }
        void InitDate()
        {
            SetAnchored(AnchoredPosition.bottom_right);
            rect = new Rect(-130, -100, 200, 70);
            baseButton = new UPageButton();
            baseButton.SetParent(this);
            baseButton.rect = new Rect(0, 0, 200, 70);
            baseButton.button.transition = ButtonTransition.SpriteSwap;
            baseButton.LoadSprite("anniu-160");
            baseButton.LoadPressSprite("anniu-160h");
            baseButton.text = "选择完成";
            baseButton.button.text.color = Color.white;
        }
    }
}
