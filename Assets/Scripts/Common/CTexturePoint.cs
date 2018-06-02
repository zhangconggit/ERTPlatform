using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;


namespace MisTexturePoint
{
    /// <summary>
    /// 多边形类
    /// </summary>
    public class Polygon
    {
        #region CalculateClockDirection  
        /// <summary>  
        /// 判断多边形是顺时针还是逆时针.  
        /// </summary>  
        /// <param name="points">所有的点</param>  
        /// <param name="isYAxixToDown">true:Y轴向下为正(屏幕坐标系),false:Y轴向上为正(一般的坐标系)</param>  
        /// <returns></returns>  
        public static ClockDirection CalculateClockDirection(List<Vector2> points, bool isYAxixToDown)
        {
            int i, j, k;
            int count = 0;
            double z;
            int yTrans = isYAxixToDown ? (-1) : (1);
            if (points == null || points.Count < 3)
            {
                return (0);
            }
            int n = points.Count;
            for (i = 0; i < n; i++)
            {
                j = (i + 1) % n;
                k = (i + 2) % n;
                z = (points[j].x - points[i].x) * (points[k].y * yTrans - points[j].y * yTrans);
                z -= (points[j].y * yTrans - points[i].y * yTrans) * (points[k].x - points[j].x);
                if (z < 0)
                {
                    count--;
                }
                else if (z > 0)
                {
                    count++;
                }
            }
            if (count > 0)
            {
                return (ClockDirection.Counterclockwise);
            }
            else if (count < 0)
            {
                return (ClockDirection.Clockwise);
            }
            else
            {
                return (ClockDirection.None);
            }
        }
        #endregion

        #region CalculatePolygonType  
        /// <summary>        
        ///判断多边形是凸多边形还是凹多边形.  
        ///假定该多边形是简单的多边形，即没有横穿也没有洞的多边形。  
        /// </summary>  
        /// <param name="points"></param>  
        /// <param name="isYAxixToDown">true:Y轴向下为正(屏幕坐标系),false:Y轴向上为正(一般的坐标系)</param>  
        /// <returns></returns>  
        public static PolygonType CalculatePolygonType(List<Vector2> points, bool isYAxixToDown)
        {
            int i, j, k;
            int flag = 0;
            double z;

            if (points == null || points.Count < 3)
            {
                return (0);
            }
            int n = points.Count;
            int yTrans = isYAxixToDown ? (-1) : (1);
            for (i = 0; i < n; i++)
            {
                j = (i + 1) % n;
                k = (i + 2) % n;
                z = (points[j].x - points[i].x) * (points[k].y * yTrans - points[j].y * yTrans);
                z -= (points[j].y * yTrans - points[i].y * yTrans) * (points[k].x - points[j].x);
                if (z < 0)
                {
                    flag |= 1;
                }
                else if (z > 0)
                {
                    flag |= 2;
                }
                if (flag == 3)
                {
                    return (PolygonType.Concave);
                }
            }
            if (flag != 0)
            {
                return (PolygonType.Convex);
            }
            else
            {
                return (PolygonType.None);
            }
        }

        //判断点在线的一边  
        static int isLeft(Vector2 P0, Vector2 P1, Vector2 P2)
        {
            int abc = (int)((P1.x - P0.x) * (P2.y - P0.y) - (P2.x - P0.x) * (P1.y - P0.y));
            return abc;

        }

        /// <summary>
        /// 判断一个点是否在多边形内
        /// </summary>
        /// <param name="pnt"></param>
        /// <param name="region"></param>
        /// <returns></returns>
        public static bool isInRegion(Vector2 pnt, List<Vector2> region)
        {

            int wn = 0, j = 0; //wn 计数器 j第二个点指针  
            for (int i = 0; i < region.Count; i++)
            {
                //开始循环  
                if (i == region.Count - 1)
                {
                    j = 0;//如果 循环到最后一点 第二个指针指向第一点  
                }
                else
                {
                    j = j + 1; //如果不是 ，则找下一点  
                }

                if (region[i].y <= pnt.y) // 如果多边形的点y'y 小于等于 选定点的 Y 坐标  
                {
                    if (region[j].y > pnt.y) // 如果多边形的下一点 大于于 选定点的 Y 坐标  
                    {
                        if (isLeft(region[i], region[j], pnt) > 0)
                        {
                            wn++;
                        }
                    }
                }
                else
                {
                    if (region[j].y <= pnt.y)
                    {
                        if (isLeft(region[i], region[j], pnt) < 0)
                        {
                            wn--;
                        }
                    }
                }
            }
            if (wn == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        #endregion
    }

    /// <summary>  
    /// 时钟方向  
    /// </summary>  
    public enum ClockDirection
    {
        /// <summary>  
        /// 无.可能是不可计算的图形，比如多点共线  
        /// </summary>  
        None,

        /// <summary>  
        /// 顺时针方向  
        /// </summary>  
        Clockwise,

        /// <summary>  
        /// 逆时针方向  
        /// </summary>  
        Counterclockwise
    }

    /// <summary>
    /// 多边形类型
    /// </summary>
    public enum PolygonType
    {
        /// <summary>  
        /// 无.不可计算的多边形(比如多点共线)  
        /// </summary>  
        None,

        /// <summary>  
        /// 凸多边形  
        /// </summary>  
        Convex,

        /// <summary>  
        /// 凹多边形  
        /// </summary>  
        Concave

    }

    public enum PointStatus
    {
        None = 0,
        LimitBoard,
        Mark,
        Draw,
    }

    /// <summary>
    /// 绘制类：可用于模拟消毒并检测消毒留白以及方向
    /// 时间：2017/3/20
    /// 作者：Cyrus.Chen
    /// </summary>
    public class CTexturePoint : MonoBehaviour
    {
        /// <summary>
        /// 检测留白委托
        /// </summary>
        /// <param name="re"></param>
        public delegate void CheckDelagate(bool re);

        class LeaveWhiteParam
        {
            public void isCheckLeave()
            {
                while (!mInstance.stopThread)
                {
                    if (mInstance.threadMutex)
                    {
                        //Debug.Log("线程信息：留白线程执行中...");
                        bool re = mInstance.isGetLeaveNoteAreaByThread(mInstance.MColor,mInstance.threadRaio, mInstance.limitList);
                        if (re)
                        {
                            mInstance.isLeaveWhite = true;
                        }
                        else
                        {
                            mInstance.isLeaveWhite = false;
                        }
                    }

                    if (mInstance.threadRadius)
                    {
                        //Debug.Log("线程信息：半径线程执行中...");
                        float re = mInstance.getMaxRadius();
                        if (re > (mInstance.mRangeRadius + mInstance.mRangeRadius * 0.25f))
                        {
                            mInstance.isRadiusError = true;
                        }
                        else
                        {
                            mInstance.isRadiusError = false;
                        }
                    }

                    if(mInstance.threadDistance)
                    {
                        Vector2 p = (mInstance.currentPoint - mInstance.firstCenter).normalized * 80 + mInstance.currentPoint;
                        if (mInstance.pixels[(int)p.x, (int)p.y] == mInstance.MColor)
                        {
                            mInstance.isFromOutToIn = true;
                        }
                        else
                        {
                             mInstance.isFromOutToIn = false;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 控制检测留白状态
        /// </summary>
        bool threadMutex = false;

        //线程控制检测消毒范围
        bool threadRadius = false;

        //线程控制检测从里到外、从外到里
        bool threadDistance = false;

        //鼠标第一次点击的位置
        Vector2 firstCenter = Vector2.zero;

        //鼠标当前点击位置
        Vector2 currentPoint = Vector2.zero;

        

        /// <summary>
        /// 是否留白
        /// </summary>
        bool isLeaveWhite = true;

        ///半径是否在范围内
        bool isRadiusError = false;

        //是从外到里
        bool isFromOutToIn = false;

        /// <summary>
        /// 实例对象
        /// </summary>
        public static CTexturePoint mInstance;

#region 贴图变量
        /// <summary>
        /// 用以保存最开始的贴图
        /// </summary>
        Texture2D textureMap;

        /// <summary>
        /// 拿来修改用的贴图,显示给用户看的
        /// </summary>
        Texture2D originalTexture;

        //实际主贴图
        Texture2D mirrorTexture;
#endregion

#region 绘制点变量

        /// <summary>
        /// 记录实际绘制点
        /// </summary>
        List<Vector2> pointList = new List<Vector2>();

        /// <summary>
        /// 记录边界点
        /// </summary>
        List<Vector2> limitList = new List<Vector2>();

        /// <summary>
        /// 将要绘制的图形边界点入队
        /// </summary>
        public Queue<Vector2> limitPointList = new Queue<Vector2>();

        /// <summary>
        /// 将要绘制的点存入队列
        /// </summary>
        Queue<Vector2> pointQueue = new Queue<Vector2>();
    
#endregion
        /// <summary>
        /// 记录上一鼠标位置
        /// </summary>
        Vector2 forwardMousePosition = new Vector2(-1, -1);

#region 渲染属性变量
        /// <summary>
        /// 待渲染的颜色
        /// </summary>
        [Tooltip("绘图颜色")]
        public Color mColor;

        /// <summary>
        /// 待渲染的颜色
        /// </summary>
        public Color MColor
        {
            get { return mColor; }
            set { mColor = value; }
        }

        /// <summary>
        /// 渲染区域半径
        /// </summary>
        [Tooltip("绘图半径")]
        public int mRadius = 1;
    

        /// <summary>
        /// 是否显示边界区域
        /// </summary>
        bool showBoard = false;

        /// <summary>
        /// 区域范围边界粗细
        /// </summary>
        [Tooltip("边界绘图半径")]
        public int areaRadius = 5;

        /// <summary>
        /// 区域范围边界颜色
        /// </summary>
        [Tooltip("边界绘图颜色")]
        public Color areaColor = Color.blue;
#endregion

        //消毒区域半径
        float mRangeRadius = 0;
        List<Vector2> clockPoint = new List<Vector2>();
#region 检测线程
        /// <summary>
        /// 留白检测线程
        /// </summary>
        Thread leaveWhiteThread;

        /// <summary>
        /// 像素数组
        /// </summary>
        Color[,] pixels;

        /// <summary>
        /// 材质
        /// </summary>
        public Material material;

        /// <summary>
        /// 线程参数
        /// </summary>
        LeaveWhiteParam lp;

        /// <summary>
        /// 控制线程检测状态
        /// </summary>
        bool stopThread = false;
#endregion

        private bool _judge = false;
        private Vector2 _anchor;//偏移的圆心
        private Vector2 _lastPoint;  //上一帧坐标

        //标记圆心
        Vector2 centerPoint = Vector2.zero;

       

        //当前绘制的距圆心的最大距离
        float curretMaxRadius = 0;

        //传入线程的参数
        Color threadColor;
        //传入线程的参数
        float threadRaio;

        CheckDelagate OnClockWise;//是否顺时针
        CheckDelagate OnLeaveWhite;//留白回调
        CheckDelagate OnCheckRadius;//半径回调-检测是否超出范围
        CheckDelagate OnRadiusDistance;//半径回调-检测是否从里向外

        void Awake()
        {
            mInstance = this;

            textureMap = material.mainTexture as Texture2D;
            if (textureMap == null)
            {
                textureMap = new Texture2D(1024, 1024);
            }
            originalTexture = new Texture2D(textureMap.width, textureMap.height,TextureFormat.ARGB32,false);
            //originalTexture.alphaIsTransparency = true;
            mirrorTexture = new Texture2D(textureMap.width, textureMap.height);
            mirrorTexture.LoadRawTextureData(textureMap.GetRawTextureData());
            mirrorTexture.Apply();
           // material.mainTexture = mirrorTexture;
           
            for (int i = 0; i <= textureMap.width; i++)
            {
                for (int j = 0; j <= textureMap.height; j++)
                {
                    originalTexture.SetPixel(i, j, new Color(1, 1, 1, 0));
                }
            }

             originalTexture.Apply();
           
        }

        /// <summary>
        /// This function is called when the object becomes enabled and active.
        /// </summary>
        void OnEnable()
        {
            material.mainTexture = mirrorTexture;
            material.SetTexture("_DetailAlbedoMap", originalTexture);
            material.EnableKeyword("_DETAIL_MULX2");
            if (pixels == null || pixels.Length == 0)
            {
                pixels = new Color[originalTexture.height, originalTexture.width];
                StartCoroutine("fillPixels");
            }
        }
        void OnQuit()
        {
            material.mainTexture = textureMap;
        }
        /// <summary>
        /// 初始化像素数组
        /// </summary>
        /// <returns></returns>
        IEnumerator fillPixels()
        {
            if (pixels == null || pixels.Length == 0)
            {
                pixels = new Color[originalTexture.height, originalTexture.width];
            }
            for (int i = 0; i < originalTexture.width; i++)
            {
                for (int j = 0; j < originalTexture.height; j++)
                {
                    pixels[i, j] = new Color(0, 0, 0, 0);
                }
            }
            yield return new WaitForEndOfFrame();
        }

        /// <summary>
        /// OnGUI is called for rendering and handling GUI events.
        /// This function can be called multiple times per frame (one call per event).
        /// </summary>
        void OnGUI()
        {
            // GUILayout.Label("MColor="+MColor);
            // GUILayout.Label("threadRaio="+threadRaio);
            // GUILayout.Label("threadMutex="+threadMutex);
            // GUILayout.Label("stopThread="+stopThread);
        }
        // Use this for initialization
        void Start()
        {
            curretMaxRadius = 0;
            setRenderParam(mColor, mRadius);
        }

        void LateUpdate()
        {
            if (pointQueue.Count > 0)
            {
                setPoint(pointQueue.Dequeue(), mColor, mRadius);
                // ClockDirection cd = judgeClock();
                // if (cd == ClockDirection.Clockwise)
                //     OnClockWise(true);
                // else if (cd == ClockDirection.Counterclockwise)
                //     OnClockWise(false);
            }

            if (limitPointList.Count > 0 && showBoard == true)
            {
                setPoint(limitPointList.Dequeue(), areaColor, areaRadius, PointStatus.LimitBoard);
                if (limitPointList.Count == 0)
                {
                    showBoard = false;
                }
            }
        }

        public void setMainTextureColor(Color pColor)
        {
            material.SetColor("_Color",pColor);
        }
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (Vector2.Distance(forwardMousePosition, Input.mousePosition) != 0)
                {
                    forwardMousePosition = Input.mousePosition;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;
                    if (Physics.Raycast(ray, out hit))
                    {
                        if (hit.collider.name == gameObject.name)
                        {
                            _lastPoint = Input.mousePosition;
                            _anchor = _lastPoint - new Vector2(0, 50);//鼠标点击 在点击位置下方设置偏移一定数值的圆心（具体偏移量看实际运用
                            _judge = true;

                            Vector2 uvPoint = hit.textureCoord;
                            uvPoint.x *= originalTexture.width;
                            uvPoint.y *= originalTexture.height;
                            pointQueue.Enqueue(uvPoint);
                            if(firstCenter == Vector2.zero)
                                firstCenter = uvPoint;

                            float value = Vector2.Distance(uvPoint, centerPoint);
                            float off = Vector2.Distance(uvPoint,firstCenter);
                            currentPoint = uvPoint;
                            // Vector2 p = (uvPoint - firstCenter).normalized*40+uvPoint;
                            // if(originalTexture.GetPixel((int)p.x,(int)p.y)==MColor)
                            // {
                            //     OnRadiusDistance(false);
                            // }
                            // else
                            // {
                            //     OnRadiusDistance(true);
                            // }
                            if (pointList.Count > 20)
                            {
                                if(OnRadiusDistance != null)
                                    OnRadiusDistance(!isFromOutToIn);
                            }
                            if (value > curretMaxRadius)
                                curretMaxRadius = value;
                            
                        }
                    }
                }

            }

            if (Input.GetMouseButtonUp(0))
            {
                
                clockPoint.Clear();
                if (_judge)
                {
                    _judge = false;
                    if (OnLeaveWhite != null)
                        OnLeaveWhite(mInstance.isLeaveWhite);
                    if (OnCheckRadius != null)
                        OnCheckRadius(!mInstance.isRadiusError);
                }
                firstCenter = Vector2.zero;
            }

            if (_judge)
            {
                
                //float re = TouchJudge(Input.mousePosition, ref _lastPoint, _anchor);
            
                // if (re > 0)
                //     OnClockWise(true);
                // else if (re < 0)
                // {
                //     OnClockWise(false);
                // }

            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        private void OnApplicationQuit()
        {
            closeLeaveWhite();
            if(textureMap!=null)
                material.mainTexture = textureMap;
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public void clearCheckCache()
        {
            curretMaxRadius = 0;
            StartCoroutine("fillPixels");
        }

        /// <summary>
        /// 线程开始启动留白检测
        /// </summary>
        /// <param name="pColor">消毒颜色</param>
        /// <param name="ratio">留白比例</param>
        /// <param name="points">检测区域(以点组成的多边形)</param>
        /// <param name="callFun">检测完成，执行回调</param>
        public void startCheckLeaveWhite(Color pColor, float ratio)
        {
            threadRaio = ratio;
            
            isLeaveWhite = true;
            if(lp==null)
                lp  = new LeaveWhiteParam();
            //resetCheck();
            if (leaveWhiteThread == null)
            {
                leaveWhiteThread = new Thread(lp.isCheckLeave);
                leaveWhiteThread.IsBackground = true;
                leaveWhiteThread.Start();
                //Debug.Log("线程信息：首次启动留白检测！");
            }
            else
            {
                //Debug.Log("线程信息：线程不为空，状态是：" + leaveWhiteThread.ThreadState);
                //Debug.Log("线程信息：IsAlive=" + leaveWhiteThread.IsAlive);
             
            }
            threadMutex = false;
            threadRadius = false;
            threadDistance = false;
            stopThread = false;
        }

        /// <summary>
        /// 停止留白检测
        /// </summary>
        public void closeLeaveWhite()
        {
            //Debug.Log("线程信息：关闭留白检测！");
            threadMutex = false;
            threadRadius = false;
            threadDistance = false;
            stopThread = true;
        }

        public void EnableCheckThread(bool isEnabled=true)
        {
           // startCheckLeaveWhite(pColor,pRadio);
            curretMaxRadius = 0;
            isLeaveWhite = true;
            isRadiusError = false;
            isFromOutToIn = false;
            clockPoint.Clear();
            threadMutex = isEnabled;
            threadRadius = isEnabled;
            threadDistance = isEnabled;
            //stopThread = false;
            
        }

        public void clearMark()
        {
            mirrorTexture.LoadRawTextureData(textureMap.GetRawTextureData());
            mirrorTexture.Apply();
        }

        public void cancleDraw()
        {
            material.SetTexture("_DetailAlbedoMap", null);
            material.EnableKeyword("_DETAIL_MULX2");
        }

        public bool isExistAlbedo2()
        {
            if(material.GetTexture("_DetailAlbedoMap")!=null)
                return true;
            return false;
        }

        /// <summary>
        /// 设置绘制的参数
        /// </summary>
        /// <param name="pColor">绘制色彩</param>
        /// <param name="pRadius">绘制粗细度</param>
        public void setRenderParam(Color pColor, int pRadius)
        {
            MColor = pColor;
            mRadius = pRadius;
        }

        /// <summary>
        /// 注册回调检测
        /// </summary>
        /// <param name="pColor">绘制色彩</param>
        /// <param name="pRadius">绘制粗细度</param>

        public void SetCallBack(CheckDelagate callbackLeaveWhite, CheckDelagate callbackClockwise, CheckDelagate callbackRadius, CheckDelagate callbackDistance)
        {
            OnLeaveWhite = callbackLeaveWhite;
            OnClockWise = callbackClockwise;
            OnCheckRadius = callbackRadius;
            OnRadiusDistance = callbackDistance;
        }
        /// <summary>
        /// 恢复材质,清楚所有内容
        /// </summary>
        public void resetMaterial()
        {
            material.mainTexture = textureMap;
            resetDrawContent();
        }

        /// <summary>
        /// 恢复材质但保留标记
        /// </summary>
        public void resetMaterial(Vector2 markPoint, Color pColor, float length, float radius)
        {
            curretMaxRadius = 0;
            //resetDrawContent();
            mirrorTexture.LoadRawTextureData(textureMap.GetRawTextureData());
            mirrorTexture.Apply();
            //material.mainTexture = mirrorTexture;
            correctionMarkPoint(markPoint, pColor, length, radius);
        }

        /// <summary>
        /// 清除所有已经绘画的内容，除了标记的内容
        /// </summary>
        public void resetDrawContent()
        {
            pointList.Clear();
            curretMaxRadius = 0;
            if (originalTexture == null)
                originalTexture = new Texture2D(textureMap.width, textureMap.height);
            for (int i = 0; i <= textureMap.width; i++)
            {
                for (int j = 0; j <= textureMap.height; j++)
                {
                    originalTexture.SetPixel(i, j, new Color(1, 1, 1, 0));
                }
            }
            originalTexture.Apply();
        }

        /// <summary>
        /// 清除所有已经绘画的内容，除了标记和显示范围的内容
        /// </summary>
        public void resetDrawContentWithoutBoard(Vector2 markPoint, int radius)
        {
            curretMaxRadius = 0;
            if (originalTexture == null)
                originalTexture = new Texture2D(textureMap.width, textureMap.height);
            for (int i = 0; i <= textureMap.width; i++)
            {
                for (int j = 0; j <= textureMap.height; j++)
                {
                    originalTexture.SetPixel(i, j, new Color(1, 1, 1, 0));
                }
            }
            originalTexture.Apply();
            //setAreaByPoints(markPoint, radius);
        }

        /// <summary>
        /// 清空所有数据，如已着色，色彩不会清空
        /// </summary>
        public void clearCache()
        {
            pointList.Clear();
            limitPointList.Clear();
            clockPoint.Clear();
            threadMutex = false;
            threadRadius = false;
            threadDistance = false;
            firstCenter = Vector2.zero;
            currentPoint = Vector2.zero;
            isLeaveWhite = true;
            isRadiusError = false;
            isFromOutToIn = false;
            StartCoroutine("fillPixels");
        }

        /// <summary>
        /// 绘制点
        /// </summary>
        /// <param name="pPoint">点</param>
        /// <param name="pColor">绘制颜色</param>
        /// <param name="pRadius">范围半径</param>
        /// <param name="isLimitBoard">是否显示边框</param>
        void setPoint(Vector2 pPoint, Color pColor, float pRadius, PointStatus status = PointStatus.Draw)
        {
            int maxX = (int)(pPoint.x + pRadius);
            int minX = (int)(pPoint.x - pRadius);
            int maxY = (int)(pPoint.y + pRadius);
            int minY = (int)(pPoint.y - pRadius);

            if (minX < 0)
                minX = 0;
            if (minY < 0)
                minY = 0;
            if (maxX > originalTexture.width)
                maxX = originalTexture.width;
            if (maxY > originalTexture.height)
                maxY = originalTexture.height;
            if (status == PointStatus.Draw)
            {
                pointList.Add(pPoint);
                if(clockPoint.Count<=2)
                    clockPoint.Add(pPoint);
                else
                    {
                        clockPoint.RemoveAt(0);
                        clockPoint.Add(pPoint);
                    }
            }
                
            for (int i = minX; i < maxX; i++)
            {
                for (int j = minY; j < maxY; j++)
                {
                    if (Vector2.Distance(pPoint, new Vector2(i, j)) <= pRadius)
                    {
                        if (status == PointStatus.Draw)
                        {
                            originalTexture.SetPixel(i, j, pColor);
                            pixels[i, j] = pColor;
                        }
                        else
                        {
                            mirrorTexture.SetPixel(i, j, pColor);
                        }


                    }
                }
            }
            if (status == PointStatus.Draw)
            {
                originalTexture.Apply();
            }
            else
            {
                mirrorTexture.Apply();
            }
        }

        /// <summary>
        /// 线程内检测是否留白
        /// </summary>
        /// <param name="pColor"></param>
        /// <param name="ratio"></param>
        /// <param name="points"></param>
        /// <returns></returns>
        bool isGetLeaveNoteAreaByThread(Color pColor, float ratio, List<Vector2> points)
        {
            limitPointList.Clear();
            int count = 0;
            int Total = 0;

            for (int i = 0; i < pixels.GetLength(0); i++)
            {
                for (int j = 0; j < pixels.GetLength(1); j++)
                {
                    if (Vector2.Distance(centerPoint, new Vector2(i, j)) <= mRangeRadius)
                    {
                        Total++;
                    }
                }
            }
           
            for (int i = 0; i < pixels.GetLength(0); i++)
            {
                for (int j = 0; j < pixels.GetLength(1); j++)
                {
                    if (Vector2.Distance(centerPoint, new Vector2(i, j)) <= mRangeRadius)
                    {
                        if (pixels[i, j] != pColor)
                        {
                            count++;
                            pixels[i, j] = Color.blue;
                            if (count > Total * ratio)
                                return true;
                        }
                    }
                }
            }

            // for (int i = 0; i < pixels.GetLength(0); i++)
            // {
            //     for (int j = 0; j < pixels.GetLength(1); j++)
            //     {
            //         if (Polygon.isInRegion(new Vector2(i, j), points))
            //         {
            //             Total++;
            //         }
            //     }
            // }
            // for (int i = 0; i < pixels.GetLength(0); i++)
            // {
            //     for (int j = 0; j < pixels.GetLength(1); j++)
            //     {
            //         if (Polygon.isInRegion(new Vector2(i, j), points))
            //         {
            //             if (pixels[i, j] != pColor)
            //             {
            //                 count++;
            //                 pixels[i, j] = Color.yellow;
            //                 if (count > Total * ratio)
            //                     return true;
            //             }
            //         }
            //     }
            // }

            return false;
        }

        /// <summary>
        /// 判断当前是否是顺时针/逆时针绘制
        /// </summary>
        /// <returns></returns>
        public ClockDirection isClockwise()
        {
            return Polygon.CalculateClockDirection(pointList, false);
        }

        /// <summary>
        /// 设置有效边界区域，并且显示出来，若无需显示，则不必使用
        /// </summary>
        /// <param name="points"></param>
        public void setAreaByPoints(List<Vector2> points)
        {
            showBoard = false;
            limitPointList.Clear();
            for (int i = 0; i < points.Count; i++)
            {
                Vector2 n = new Vector2(points[i].x, points[i].y);
                limitPointList.Enqueue(points[i]);
                limitList.Add(points[i]);
            }
            showBoard = true;
        }

        public void ApplyTexture()
        {
             originalTexture.Apply();
        }

        /// <summary>
        /// 设置有效边界区域，并且显示出来，若无需显示，则不必使用
        /// </summary>
        /// <param name="points"></param>
        public void setAreaByPoints(Vector2 pCenter, int pRadius)
        {
            mRangeRadius = pRadius;
            showBoard = false;
            limitPointList.Clear();

            int minX = (int)pCenter.x - pRadius;
            int maxX = (int)pCenter.x + pRadius;
            int minY = (int)pCenter.y - pRadius;
            int maxY = (int)pCenter.y + pRadius;
            if (minX < 0)
                minX = 0;
            if (minY < 0)
                minY = 0;
            if (maxX > mirrorTexture.width)
                maxX = mirrorTexture.width;
            if (maxY > mirrorTexture.height)
                maxY = mirrorTexture.height;
            Color[] c = new Color[] { areaColor };
            for (int i = minX; i < maxX; i++)
            {
                for (int j = minY; j < maxY; j++)
                {
                    float d = Vector2.Distance(pCenter, new Vector2(i, j));
                    if (d >= pRadius - 2 && d <= pRadius + 2)
                    {
                        Vector2 p = new Vector2(i, j);
                        limitPointList.Enqueue(p);
                        limitList.Add(p);
                        originalTexture.SetPixel(i, j, areaColor);
                    }

                }
            }
            originalTexture.Apply();
            // showBoard = true;
        }

        /// <summary>
        /// 重新校正标记点
        /// </summary>
        /// <param name="points"></param>
        /// <param name="pColor"></param>
        /// <param name="length"></param>
        /// <param name="radius"></param>
        public void correctionMarkPoint(List<Vector2> points, Color pColor, float length, float radius)
        {
            if (points == null || points.Count == 0)
            {
                Debug.LogWarning("点阵为空，无法校正！");
                return;
            }
            Vector2 point = points[points.Count / 2];
            centerPoint = point;
            int minx = 0;
            int maxx = 0;
            int miny = 0;
            int maxy = 0;
            minx = (int)(point.x - length / 2);
            if (minx < 0)
                minx = 0;
            maxx = (int)(point.x + length / 2);
            if (maxx > originalTexture.width)
                maxx = originalTexture.width;

            miny = (int)(point.y - length / 2);
            if (minx < 0)
                minx = 0;
            maxy = (int)(point.y + length / 2);
            if (maxx > originalTexture.height)
                maxx = originalTexture.height;

            for (int i = minx, j = (int)point.y; i <= maxx; i++)
            {
                setPoint(new Vector2(i, j), pColor, radius);
            }

            for (int i = miny, j = (int)point.x; i <= maxy; i++)
            {
                setPoint(new Vector2(j, i), pColor, radius);
            }
        }

        IEnumerator correctionMarkPointI(Vector2 point, Color pColor, float length, float radius)
        {
            centerPoint = point;
            int minx = 0;
            int maxx = 0;
            int miny = 0;
            int maxy = 0;
            minx = (int)(point.x - length / 2);
            if (minx < 0)
                minx = 0;
            maxx = (int)(point.x + length / 2);
            if (maxx > originalTexture.width)
                maxx = originalTexture.width;

            miny = (int)(point.y - length / 2);
            if (minx < 0)
                minx = 0;
            maxy = (int)(point.y + length / 2);
            if (maxx > originalTexture.height)
                maxx = originalTexture.height;

            for (int i = minx, j = (int)point.y; i <= maxx; i++)
            {
                //setPoint(new Vector2(i, j), pColor, radius, PointStatus.Mark);
                for(int k=(int)point.y-3;k<(int)point.y+3;k++)
                {
                    mirrorTexture.SetPixel(i, k, pColor);
                }
            }

            for (int i = miny, j = (int)point.x; i <= maxy; i++)
            {
                //setPoint(new Vector2(j, i), pColor, radius, PointStatus.Mark);
                 for(int k=(int)point.x-3;k<(int)point.x+3;k++)
                {
                    mirrorTexture.SetPixel(k, i, pColor);
                }
            }
            yield return 0;
            mirrorTexture.Apply();
        }
        /// <summary>
        /// 重新校正标记点
        /// </summary>
        /// <param name="points"></param>
        /// <param name="pColor"></param>
        /// <param name="length"></param>
        /// <param name="radius"></param>
        public void correctionMarkPoint(Vector2 point, Color pColor, float length, float radius)
        {
            StartCoroutine(correctionMarkPointI(point, pColor, length, radius));
            return;
            centerPoint = point;
        
            int minx = 0;
            int maxx = 0;
            int miny = 0;
            int maxy = 0;
            minx = (int)(point.x - length / 2);
            if (minx < 0)
                minx = 0;
            maxx = (int)(point.x + length / 2);
            if (maxx > originalTexture.width)
                maxx = originalTexture.width;

            miny = (int)(point.y - length / 2);
            if (minx < 0)
                minx = 0;
            maxy = (int)(point.y + length / 2);
            if (maxx > originalTexture.height)
                maxx = originalTexture.height;

            for (int i = minx, j = (int)point.y; i <= maxx; i++)
            {
                setPoint(new Vector2(i, j), pColor, radius, PointStatus.Mark);
            }

            for (int i = miny, j = (int)point.x; i <= maxy; i++)
            {
                setPoint(new Vector2(j, i), pColor, radius, PointStatus.Mark);
            }
        }

        /// <summary>
        /// 判断顺时针逆时针
        /// (顺正逆负
        /// </summary>
        /// <param name="current">当前坐标</param>
        /// <param name="last">上个坐标(ref 更新</param>
        /// <param name="anchor">锚点</param>
        /// <returns></returns>
        private float TouchJudge(Vector2 current, ref Vector2 last, Vector2 anchor)
        {
            Vector2 lastDir = (last - anchor).normalized;  //上次方向
            Vector2 currentDir = (current - anchor).normalized;  //当前方向

            float lastDot = Vector2.Dot(Vector2.right, lastDir);
            float currentDot = Vector2.Dot(Vector2.right, currentDir);

            float lastAngle = last.y < anchor.y//用y点判断上下扇面
                ? Mathf.Acos(lastDot) * Mathf.Rad2Deg
                : -Mathf.Acos(lastDot) * Mathf.Rad2Deg;

            float currentAngle = current.y < anchor.y
                ? Mathf.Acos(currentDot) * Mathf.Rad2Deg
                : -Mathf.Acos(currentDot) * Mathf.Rad2Deg;

            last = current;
            return currentAngle - lastAngle;
        }

        //
        float getMaxRadius()
        {
            return curretMaxRadius;
        }

        public void writeLeaveNote()
        {
            for (int i = 0; i < originalTexture.width; i++)
            {
                for (int j = 0; j < originalTexture.height; j++)
                {
                    if (pixels[i, j] == Color.blue)
                        originalTexture.SetPixel(i, j, pixels[i, j]);
                }
            }
            originalTexture.Apply();
        }

        public ClockDirection judgeClock()
        {
            if(clockPoint.Count<2)
                return ClockDirection.None;
            float x1 =firstCenter.x;
            float x2 = clockPoint[0].x;
            float x3 = clockPoint[1].x;
            float y1 = firstCenter.y;
            float y2 = clockPoint[0].y;
            float y3 = clockPoint[1].y;
            float re = (x2-x1)*(y3-y1)-(y2-y1)*(x3-x1);
            if (re < 0)
                return ClockDirection.Clockwise;
            else if (re > 0)
                return ClockDirection.Counterclockwise;
            else
                return ClockDirection.None;
        }

        public float getStartDistance()
        {
            return Vector2.Distance(centerPoint,firstCenter);
        }
    }
}
