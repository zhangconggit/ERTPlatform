using UnityEngine;
using UnityEngine.UI;

namespace CFramework
{
    /// <summary>
    /// 面板
    /// </summary>
    public class UPlane :UPageBase
    {
        

        Image baseImage;
        //PlanePosition pos = PlanePosition.center;
        public Sprite sprite {
            get { return baseImage.sprite; }
            set { baseImage.sprite = value; }
        }
        public Color color {
            get { return baseImage.color; }
            set { baseImage.color = value; }
        }
        public UPlane()
        {
            name = "UPlane";
            baseImage = gameObejct.AddComponent<Image>();
           // rect = new Rect(0, 0, parent.rect.width, parent.rect.height);
            SetAnchored(AnchoredPosition.full);
        }

        public UPlane(Color color)
        {
            name = "UPlane";
            baseImage = gameObejct.AddComponent<Image>();
            rect = new Rect(0, 0, parent.rect.width, parent.rect.height);
            SetAnchored(AnchoredPosition.full);

            this.color = color;
        }
        public UPlane(string path)
        {
            name = "UPlane";
            baseImage = gameObejct.AddComponent<Image>();
            rect = new Rect(0, 0, parent.rect.width, parent.rect.height);
            SetAnchored(AnchoredPosition.full);

            LoadImage(path);
        }
        public void LoadImage( string path)
        {
            sprite = Resources.Load<Sprite>(path);
        }

    }
}
