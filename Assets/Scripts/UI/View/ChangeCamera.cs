namespace CFramework
{
    public class ChangeCamera : UPageBase
    {
        public UPageButton lookChe;
        public UPageButton lookRen;
        public UPageButton lookWorld;
        public ChangeCamera()
        {
            SetAnchored(AnchoredPosition.full);
            SetBorderSpace(0, 0, 0, 0);
            lookChe = new UPageButton(AnchoredPosition.bottom);
            lookChe.SetParent(this);
            lookChe.LoadSprite("Images/Common/shijiaok");
            lookChe.LoadPressSprite("Images/Common/shijiaok-h");
            lookChe.rect = new UnityEngine.Rect(-100, -70, 80, 80);
            lookChe.text = "车";
            lookRen = new UPageButton(AnchoredPosition.bottom);
            lookRen.SetParent(this);
            lookRen.LoadSprite("Images/Common/shijiaok");
            lookRen.LoadPressSprite("Images/Common/shijiaok-h");
            lookRen.rect = new UnityEngine.Rect(0, -70, 80, 80);
            lookRen.text = "人";
            lookWorld = new UPageButton(AnchoredPosition.bottom);
            lookWorld.SetParent(this);
            lookWorld.LoadSprite("Images/Common/shijiaok");
            lookWorld.LoadPressSprite("Images/Common/shijiaok-h");
            lookWorld.rect = new UnityEngine.Rect(100, -70, 80, 80);
            lookWorld.text = "全景";
        }
    }
}

