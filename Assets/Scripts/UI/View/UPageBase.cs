
using System.Collections.Generic;
using UnityEngine;

namespace CFramework
{
    /// <summary>
    /// 中心点位置
    /// </summary>
    public enum AnchoredPosition
    {
        full = 0,
        center,
        bottom,
        top,
        right,
        left,
        bottom_right,
        bottom_left,
        top_right,
        top_left
    }
    /// <summary>
    /// UI主脚本
    /// </summary>
    public class UPageBase
    {
        /// <summary>
        /// 根节点
        /// </summary>
        static UPageBase root = null;
        /// <summary>
        /// 查找页面对象
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns></returns>
        public static UPageBase FindPage(string name)
        {
            return root.GetChild(name);
        }
        /// <summary>
        /// 查找页面对象
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns></returns>
        public static T FindPage<T>(string name) where T : UPageBase
        {
            return root.GetChild(name) as T;
        }
        /// <summary>
        /// 场景对象
        /// </summary>
        public GameObject gameObejct { get; set; }
        /// <summary>
        /// 页面，节点名字
        /// </summary>
        public string name { get { return gameObejct.name; } set { gameObejct.name = value; } }
        public Transform transform { get { return gameObejct.transform; } }
        public RectTransform rectTransform { get { return transform.GetComponent<RectTransform>(); } }

        List<UPageBase> children = new List<UPageBase>();
        /// <summary>
        /// 子节点个数
        /// </summary>
        public int childCount { get { return children.Count; } }
        protected UPageBase parent = null;

        /// <summary>
        /// 0,0点在左上角，
        /// </summary>
        public Rect rect
        {
            get
            {
                return rectTransform.rect;
            }
            set
            {
                Rect pRect = transform.parent.GetComponent<RectTransform>().rect;
                rectTransform.localPosition = new Vector2(value.position.x, - value.position.y) + new Vector2((rectTransform.pivot.x - 0.5f) * pRect.width, (rectTransform.pivot.y - 0.5f) * pRect.height);
                rectTransform.sizeDelta = new Vector2(value.width - parent.rect.width * (rectTransform.anchorMax.x - rectTransform.anchorMin.x), value.height - parent.rect.height * (rectTransform.anchorMax.y - rectTransform.anchorMin.y));
            }
        }
        private UPageBase(bool isRoot)
        {

            Canvas can = MonoBehaviour.FindObjectOfType<Canvas>();
            //Debug.Log("can=" + can.name);
            gameObejct = can.gameObject;
        }
        /// <summary>
        /// 父节点
        /// </summary>
        /// <returns></returns>
        public UPageBase GetParent()
        {
            return parent;
        }
        protected UPageBase()
        {
            if (root == null)
                root = new UPageBase(true);
            gameObejct = new GameObject();
            gameObejct.AddComponent<RectTransform>();
            Canvas can = Object.FindObjectOfType<Canvas>();
            //SetParent(root);
            gameObejct.transform.SetParent(root.transform, false);
            parent = root;
            root.AddChild(this);
            //rootPage.Add(this);
            SetAnchored(AnchoredPosition.center);
            rect = new Rect(0, 0, 0, 0);
        }
        /// <summary>
        /// 设置父节点
        /// </summary>
        /// <param name="pg"></param>
        public void SetParent(UPageBase pg)
        {
            parent.RemoveChild(this);
            parent = pg;
            transform.SetParent(parent.transform);
            parent.AddChild(this);
        }
        protected void AddChild(UPageBase child)
        {
            children.Add(child);
            //child.SetParent(this);
        }
        protected void RemoveChild(UPageBase child)
        {
            children.Remove(child);
        }
        /// <summary>
        /// 取得子节点
        /// </summary>
        /// <param name="index">节点序号</param>
        /// <returns></returns>
        public UPageBase GetChild(int index)
        {
            if (index < 0 || index >= children.Count)
            {
                throw new System.ArgumentOutOfRangeException();
            }
            return children[index];
        }
        // <summary>
        /// 取得子节点
        /// </summary>
        /// <param name="name">节点名字</param>
        /// <returns></returns>
        public UPageBase GetChild(string name)
        {
            int index = name.IndexOf('/');
            if (index < 0)
                index = name.Length;
            string childName = name.Substring(0, index);
            UPageBase result = children.Find(temp =>
            {
                if (temp.name == childName)
                    return true;
                else
                    return false;
            });
            if (index < name.Length)
            {
                result = result.GetChild(name.Substring(index + 1));
            }
            return result;
        }
        /// <summary>
        /// 取得子节点列表 
        /// </summary>
        /// <returns></returns>
        public List<UPageBase> GetChildren()
        {
            return children;
        }

        /// <summary>
        /// 设置中心点
        /// </summary>
        /// <param name="anchored"></param>
        public void SetAnchored(AnchoredPosition anchored)
        {
            switch (anchored)
            {
                case AnchoredPosition.full:
                    rectTransform.anchorMin = new Vector2(0, 0);
                    rectTransform.anchorMax = new Vector2(1, 1);
                    rectTransform.pivot = new Vector2(0.5f, 0.5f);
                    break;
                case AnchoredPosition.center:
                    rectTransform.anchorMin = new Vector2(0.5f, 0.5f);
                    rectTransform.anchorMax = new Vector2(0.5f, 0.5f);
                    rectTransform.pivot = new Vector2(0.5f, 0.5f);
                    break;
                case AnchoredPosition.bottom:
                    rectTransform.anchorMin = new Vector2(0.5f, 0f);
                    rectTransform.anchorMax = new Vector2(0.5f, 0f);
                    rectTransform.pivot = new Vector2(0.5f, 0f);
                    break;
                case AnchoredPosition.top:
                    rectTransform.anchorMin = new Vector2(0.5f, 1f);
                    rectTransform.anchorMax = new Vector2(0.5f, 1f);
                    rectTransform.pivot = new Vector2(0.5f, 1f);
                    break;
                case AnchoredPosition.right:
                    rectTransform.anchorMin = new Vector2(1f, 0.5f);
                    rectTransform.anchorMax = new Vector2(1f, 0.5f);
                    rectTransform.pivot = new Vector2(1f, 0.5f);
                    break;
                case AnchoredPosition.left:
                    rectTransform.anchorMin = new Vector2(0f, 0.5f);
                    rectTransform.anchorMax = new Vector2(0f, 0.5f);
                    rectTransform.pivot = new Vector2(0f, 0.5f);
                    break;
                case AnchoredPosition.bottom_right:
                    rectTransform.anchorMin = new Vector2(1f, 0f);
                    rectTransform.anchorMax = new Vector2(1f, 0f);
                    rectTransform.pivot = new Vector2(1f, 0f);
                    break;
                case AnchoredPosition.bottom_left:
                    rectTransform.anchorMin = new Vector2(0f, 0f);
                    rectTransform.anchorMax = new Vector2(0f, 0f);
                    rectTransform.pivot = new Vector2(0f, 0f);
                    break;
                case AnchoredPosition.top_right:
                    rectTransform.anchorMin = new Vector2(1f, 1f);
                    rectTransform.anchorMax = new Vector2(1f, 1f);
                    rectTransform.pivot = new Vector2(1f, 1f);
                    break;
                case AnchoredPosition.top_left:
                    rectTransform.anchorMin = new Vector2(0f, 1f);
                    rectTransform.anchorMax = new Vector2(0f, 1f);
                    rectTransform.pivot = new Vector2(0f, 1f);
                    break;
                default:
                    break;
            }

        }
        /// <summary>
        /// 摧毁自己
        /// </summary>
        public void Destroy()
        {
            if (parent != null)
                parent.RemoveChild(this);
            for (int i = 0; i < childCount; i++)
            {
                children[i].Destroy();
            }
            children.Clear();
            GameObject.DestroyImmediate(gameObejct);
        }
        /// <summary>
        /// 设置边框距 当前只适用于AnchoredPosition.full
        /// </summary>
        /// <param name="top">上边距离</param>
        /// <param name="right">右边距离</param>
        /// <param name="bottom">下边距离</param>
        /// <param name="left">左边距离</param>
        public void SetBorderSpace(float top, float right, float bottom, float left)
        {
            float width = parent.rect.width - left - right;
            float height = parent.rect.height - top - bottom;
            float x = right - (parent.rect.width / 2 - width / 2);
            float y = top - (parent.rect.height / 2 - height / 2);
            //Rect _re = new Rect(x, y, width, height);
            rect = new Rect(x, y, width, height);
        }
    }
}
