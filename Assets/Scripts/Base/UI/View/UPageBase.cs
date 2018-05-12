
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        static Canvas uiCanvas = null;
        /// <summary>
        /// 查找页面对象
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns></returns>
        public static UPageBase FindPage(string name)
        {
            return root.GetChild(name);
        }
        #region 设置分辨率
        const float constUIWidth = 1920f;
        const float constUIHeight = 1080f;
        public static float fAdapterWidth = 1f;
        public static float fAdapterHeight = 1f;

        const string constAdapter5X4 = "1.2500";
        const string constAdapter4X3 = "1.3333";
        const string constAdapter3X2 = "1.5000";
        const string constAdapter16X9 = "1.7878";
        const string constAdapter16X10 = "1.6000";
        // const string constAdapter1366X768 = "1.7786";
        #endregion

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

        Rect m_rect;


        /// <summary>
        /// 0,0点在左上角，
        /// </summary>
        public Rect rect
        {
            get
            {
                return m_rect;
            }
            set
            {
                m_rect = value;
                if (parent == null)
                {
                    Rect pRect = transform.parent.GetComponent<RectTransform>().rect;
                    pRect.x /= fAdapterWidth;
                    pRect.y /= fAdapterHeight;
                    pRect.width /= fAdapterWidth;
                    pRect.height /= fAdapterHeight;
                    Vector2 v2 = new Vector2(value.position.x, -value.position.y) + new Vector2((rectTransform.pivot.x - 0.5f) * pRect.width, (rectTransform.pivot.y - 0.5f) * pRect.height);
                    rectTransform.localPosition = new Vector3(v2.x * fAdapterWidth, v2.y * fAdapterHeight, 0);
                    Vector2 size2 = new Vector2(value.width - transform.parent.GetComponent<RectTransform>().rect.width * (rectTransform.anchorMax.x - rectTransform.anchorMin.x), value.height - transform.parent.GetComponent<RectTransform>().rect.height * (rectTransform.anchorMax.y - rectTransform.anchorMin.y));
                    rectTransform.sizeDelta = new Vector2(size2.x * fAdapterWidth, size2.y * fAdapterHeight);
                }
                else
                {
                    Rect pRect = transform.parent.GetComponent<RectTransform>().rect;
                    pRect.x /= fAdapterWidth;
                    pRect.y /= fAdapterHeight;
                    pRect.width /= fAdapterWidth;
                    pRect.height /= fAdapterHeight;
                    Vector2 v2 = new Vector2(value.position.x, -value.position.y) + new Vector2((rectTransform.pivot.x - 0.5f) * pRect.width, (rectTransform.pivot.y - 0.5f) * pRect.height);
                    rectTransform.localPosition = new Vector3(v2.x * fAdapterWidth, v2.y * fAdapterHeight, 0);

                    Vector2 size2 = new Vector2(value.width - parent.rect.width * (rectTransform.anchorMax.x - rectTransform.anchorMin.x), value.height - parent.rect.height * (rectTransform.anchorMax.y - rectTransform.anchorMin.y));
                    rectTransform.sizeDelta = new Vector2(size2.x * fAdapterWidth, size2.y * fAdapterHeight);
                }
            }
        }
        public Vector2 Position
        {
            get
            {
                return m_rect.position;
            }
            set
            {
                m_rect.position = value;
                rect = m_rect;
            }
        }
        public Vector2 Size
        {
            get
            {
                return m_rect.size;
            }
            set
            {
                m_rect.size = value;
                rect = m_rect;
            }
        }


        private UPageBase(bool isRoot)
        {
            //float adapter3 = Screen.width * 1f / Screen.height;
            float currentWidth = 1920;
            float currentHeight = 1080;
            string sAdpter = GetCurrentAdapter();
            //Debug.Log(sAdpter);
            switch (sAdpter)
            {
                case constAdapter5X4:
                    currentWidth = 1000;
                    currentHeight = 800;

                    break;
                case constAdapter4X3:
                    currentWidth = 800;
                    currentHeight = 600;
                    break;
                case constAdapter3X2:
                    currentWidth = 1500;
                    currentHeight = 1000;
                    break;
                case constAdapter16X9:
                    currentWidth = 1920;
                    currentHeight = 1080;
                    break;
                case constAdapter16X10:
                    currentWidth = 1600;
                    currentHeight = 1000;
                    break;
                //case constAdapter1366X768:
                //    currentWidth =  1366;
                //    currentHeight = 768;
                //    break;
                default:
                    break;
            }
            fAdapterWidth = currentWidth / constUIWidth;
            fAdapterHeight = currentHeight / constUIHeight;
            CanvasScaler canvas = Object.FindObjectOfType<CanvasScaler>();
            canvas.referenceResolution = new Vector2(currentWidth, currentHeight);
            //Debug.Log("can=" + can.name);
            Canvas can = MonoBehaviour.FindObjectOfType<Canvas>();
            gameObejct = can.gameObject;
        }
        
        public UPageBase()
        {
            if (root == null)
                root = new UPageBase(true);
            gameObejct = new GameObject();
            gameObejct.AddComponent<RectTransform>();
            gameObejct.transform.SetParent(root.transform, false);
            parent = root;
            root.AddChild(this);
            //rootPage.Add(this);
            SetAnchored(AnchoredPosition.center);
            rect = new Rect(0, 0, 0, 0);
        }

        /// <summary>
        /// 取得当前适配分辨率
        /// </summary>
        /// <returns></returns>
        string GetCurrentAdapter()
        {

            float tmp = 5f / 4;
            float current = Screen.width * 1f / Screen.height;
            string result = constAdapter5X4;
            if (Mathf.Abs(current - 4f / 3) < Mathf.Abs(current - tmp))
            {
                tmp = 4f / 3;
                result = constAdapter4X3;
            }
            if (Mathf.Abs(current - 3f / 2) < Mathf.Abs(current - tmp))
            {
                tmp = 3f / 2;
                result = constAdapter3X2;
            }
            if (Mathf.Abs(current - 16f / 9) < Mathf.Abs(current - tmp))
            {
                tmp = 16f / 9;
                result = constAdapter16X9;
            }
            if (Mathf.Abs(current - 16f / 10) < Mathf.Abs(current - tmp))
            {
                tmp = 16f / 10;
                result = constAdapter16X10;
            }

            return result;
        }
        /// <summary>
        /// 父节点
        /// </summary>
        /// <returns></returns>
        public UPageBase GetParent()
        {
            return parent;
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
        /// <summary>
        /// 设置父节点
        /// </summary>
        /// <param name="pg"></param>
        public void SetParent(Transform pg)
        {
            parent = null;
            //parent.RemoveChild(this);
            //parent = pg;
            transform.SetParent(pg);
            //parent.AddChild(this);
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
        /// <summary>
        /// 取得子节点
        ///</summary>
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

        public void SetActive(bool value)
        {
            gameObejct.SetActive(value);
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
            rect = new Rect(x, y, width, height);
        }
    }
}
