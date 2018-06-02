using UnityEngine;
using UnityEngine.UI;

namespace CFramework
{
    /// <summary>
    /// 文字
    /// </summary>
    public class UText : UPageBase
    {
        /// <summary>
        /// 字体控件
        /// </summary>
        public Text baseText;

        /// <summary>
        /// 文字
        /// </summary>
        public string text
        {
            get { return baseText.text; }
            set { baseText.text = value; }
        }

        public UText()
        {
            name = "UText";
            baseText = gameObejct.AddComponent<Text>();
            rect = new Rect(0, 0, 200, 50);

            Font ft = UIRoot.Instance.font;
            baseText.font = ft;
            baseText.fontSize = 24;
        }
        public UText(AnchoredPosition _pos)
        {
            name = "UText";
            baseText = gameObejct.AddComponent<Text>();
            SetAnchored(_pos);
            rect = new Rect(0, 0, 200, 50);
            Font ft = UIRoot.Instance.font;
            baseText.font = ft;
            baseText.fontSize = 24;
        }
        public UText(string value)
        {
            name = "UText";
            baseText = gameObejct.AddComponent<Text>();
            rect = new Rect(0, 0, 200, 50);
            text = value;
            Font ft = UIRoot.Instance.font;
            baseText.font = ft;
            baseText.fontSize = 24;
        }
        public float GetTxtHight()
        {
            return baseText.preferredHeight;
        }
    }
}
