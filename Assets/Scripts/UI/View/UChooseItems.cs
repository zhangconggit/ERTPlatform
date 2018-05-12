using UnityEngine;
using UnityEngine.UI;

namespace CFramework
{
    /// <summary>
    /// 选择物品的页面信息
    /// </summary>
    public class UChooseItems: UPageBase
    {
        /// <summary>
        /// 点击选择完成按钮
        /// </summary>
        public CEvent OnClickFinishButton=new CEvent();
        GameObject baseChooseItems;
        /// <summary>
        /// 选择物品动作相关
        /// </summary>
        public ChooseItems baseItems {
            get
            {
                return baseChooseItems.GetComponent<ChooseItems>();
            }
        }
        public UPageButton FinishButton; 
        public UChooseItems()
        {
            SetAnchored(AnchoredPosition.full);
            SetBorderSpace(0, 0, 0, 0);
            gameObejct.name = "ChooseItems";
            baseChooseItems = UIRoot.Instance.InstantiateCustom("ChooseItemsRoot");
            baseChooseItems.transform.parent = transform;
            baseChooseItems.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            baseChooseItems.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            baseChooseItems.transform.localScale = new Vector3(1, 1, 1);
            baseChooseItems.name = "baseChooseItems";
            baseChooseItems.SetActive(true);

            FinishButton = new UPageButton(AnchoredPosition.bottom_right);
            FinishButton.SetParent(this);
            FinishButton.rect = new Rect(-150, -100, 200, 70);
            FinishButton.button.transition = ButtonTransition.SpriteSwap;
            FinishButton.LoadSprite("anniu-160");
            FinishButton.LoadPressSprite("anniu-160h");
            FinishButton.text = "选择完成";
            FinishButton.button.text.color = Color.white;
            FinishButton.button.AddListionEvent(() => { OnClickFinishButton.Invoke(); });
        }
    }
}
