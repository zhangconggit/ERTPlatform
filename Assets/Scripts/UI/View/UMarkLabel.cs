using UnityEngine;
using UnityEngine.UI;

namespace CFramework
{
    /// <summary>
    /// 刻度图片
    /// </summary>
    class UMarkLabel : UPageBase
    {
        public UText baseText;
        public UImage baseImage;
        //PlanePosition pos = PlanePosition.center;
        
        
        public UMarkLabel()
        {
            name = "UMarkLabel";
            SetAnchored(AnchoredPosition.center);
            rect = new Rect(500, -200, 120, 80);
            
            baseImage =new UImage(AnchoredPosition.full);
            baseImage.SetParent(this);
            baseImage.SetBorderSpace(0, 0, 0, 0);
            baseText = new UText(AnchoredPosition.full);
            baseText.SetParent(this);
            baseText.SetBorderSpace(0, 0, 0, 0);
            baseText.baseText.alignment = TextAnchor.MiddleCenter;
            baseText.baseText.color = Color.white;
            baseText.text = "0cm";
            baseImage.LoadImage("Images/Common/kedu");
        }
    }
}
