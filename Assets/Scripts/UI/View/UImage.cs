using UnityEngine;
using UnityEngine.UI;

namespace CFramework
{
    /// <summary>
    /// 图片
    /// </summary>
    class UImage : UPageBase
    {
        Image baseImage;
        //PlanePosition pos = PlanePosition.center;
        public Sprite sprite
        {
            get { return baseImage.sprite; }
            set { baseImage.sprite = value; }
        }
        public Color color
        {
            get { return baseImage.color; }
            set { baseImage.color = value; }
        }
        public UImage()
        {
            name = "UImage";
            baseImage = gameObejct.AddComponent<Image>();
            rect = new Rect(0, 0, 200, 200);
        }
        public UImage(AnchoredPosition _pos)
        {
            name = "UImage";
            baseImage = gameObejct.AddComponent<Image>();
            SetAnchored(_pos);
            rect = new Rect(0, 0, 200, 200);
        }
        public UImage(string path)
        {
            name = "UImage";
            baseImage = gameObejct.AddComponent<Image>();
            rect = new Rect(0, 0, 200, 200);
            LoadImage(path);
        }
        public void LoadImage(string path)
        {
            sprite = Resources.Load<Sprite>(path);
        }
    }
}
