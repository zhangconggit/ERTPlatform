#define Test0
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using UnityEngine;


namespace MisTexturePoint
{

    public enum brushStyle
    {
        Circle = 0,
        Rect,
    }

    /// <summary>
    /// 多边形类
    /// </summary>
    public class Polygon
    {
        #region CalculateClockDirection
        /// <summary>  
        /// 判断多边形是顺时针还是逆时针
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

        /// <summary>  
        /// 判断屏幕多边形是顺时针还是逆时针（夹角大小）
        /// </summary>  
        /// <param name="points">鼠标点</param>  
        /// <param name="isYAxixToDown">true:Y轴向下为正(屏幕坐标系),false:Y轴向上为正(一般的坐标系)</param>  
        /// <returns></returns>  
        public static ClockDirection CalculateClockDirection(List<Vector2> points, bool isYAxixToDown, int AllowErrorTimes)
        {
            int errorTime = 0;
            int RightTime = 0;
            Vector2 OPoint = new Vector2(-1f, -1f);
            Vector2 nextPoint = new Vector2(-2f, -2f);
            Vector2 lastPoint = new Vector2(-3f, -3f);
            int yTrans = isYAxixToDown ? (-1) : (1);
            if (points.Count < 2)
                return (0);
            if (OPoint.x == -1)
                OPoint = points[0];
            for (int i = 1; i < points.Count; i++)
            {
                lastPoint = nextPoint;
                nextPoint = points[i];
                if (lastPoint.x != -2)
                {
                    float lastAngle = Vector2.Angle(new Vector2(yTrans, 0), lastPoint - OPoint);
                    float nowAngle = Vector2.Angle(new Vector2(yTrans, 0), nextPoint - OPoint);
                    lastAngle = (lastPoint - OPoint).y > 0 ? lastAngle : 360f - lastAngle;
                    nowAngle = (nextPoint - OPoint).y > 0 ? nowAngle : 360f - nowAngle;

                    if ((lastPoint - OPoint).y == 0)
                    {
                        if ((lastPoint - OPoint).x < 0)
                            lastAngle = 0f;
                    }
                    if ((nextPoint - OPoint).y == 0)
                    {
                        if ((nextPoint - OPoint).x < 0)
                            nowAngle = 360f;

                    }
                    //Debug.Log(lastAngle);
                    if (lastAngle > nowAngle)
                        errorTime++;
                    if (lastAngle < nowAngle)
                        RightTime++;
                }
            }
            //Debug.Log("errorTime="+ errorTime);
            //Debug.Log("RightTime=" + RightTime);
            if (errorTime <= AllowErrorTimes)
                return ClockDirection.Clockwise;
            else if (RightTime <= AllowErrorTimes)
                return ClockDirection.Counterclockwise;
            else
                return (0);
        }
        /// <summary>
        /// 判断多边形是顺时针还是逆时针 （夹角大小差值）
        /// </summary>
        /// <param name="points">鼠标点</param>
        /// <param name="isXAxixToLeft">true:X轴向左(屏幕坐标系),false:X轴向右为正(一般的坐标系)</param>
        /// <param name="AllowErrorAngle">允许范围误差</param>
        /// <returns></returns>
        public static ClockDirection CalculateClockDirection(List<Vector2> points, bool isXAxixToLeft, float AllowErrorAngle)
        {
            float lastAngle = 0.0f;
            Vector2 centerPoint = new Vector2(-1f, -1f);
            int yTrans = isXAxixToLeft ? (-1) : (1);
            Vector2 normalDir = new Vector2(yTrans, 0);
            if (points.Count < 3)
                return (0);
            if (centerPoint.x == -1)
                centerPoint = points[0];
            for (int i = 1; i < points.Count; i++)
            {
                Vector2 ndir = points[i] - centerPoint;
                ndir.Normalize();
                float currentAngle = Vector2.Angle(normalDir, ndir);
                if (points[i].y < centerPoint.y)
                {
                    currentAngle = 360 - currentAngle;
                }
                if (currentAngle != lastAngle && Mathf.Abs(currentAngle - lastAngle) > AllowErrorAngle && currentAngle != 90 && currentAngle != 0 && currentAngle != 180 && currentAngle != 270)
                {
                    if (Mathf.Abs(currentAngle - lastAngle) <= 300)
                    {
                        if (currentAngle <= lastAngle)
                            return ClockDirection.Counterclockwise;
                    }
                    lastAngle = currentAngle;
                }
            }
            return ClockDirection.Clockwise;
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

    public enum CastMode
    {
        Ray,
        Rays,
        Plane
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

        private CastMode m_castMode = CastMode.Ray;

        float m_castSize = 0.2f;

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

        Vector2 uvPoint = Vector2.zero;



#region 贴图变量
        /// <summary>
        /// 用以保存最开始的贴图
        /// </summary>
        Texture2D textureMap;

        /// <summary>
        /// 拿来修改用的贴图,显示给用户看的
        /// </summary>
        Texture2D originalTexture;
        
        /// <summary>
        /// 检测的图片
        /// </summary>
        public Texture2D CheckTexture
        {
            get
            {
                return originalTexture;
            }
        }
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
        
        public brushStyle mBrushStyle;

        /// <summary>
        /// 画刷圆形半径
        /// </summary>
        [Tooltip("画刷圆形半径")]
        public int mRadius = 1;
    

        /// <summary>
        /// 画刷矩形宽高
        /// </summary>
        [Tooltip("画刷矩形宽高")]
        public Vector2 mRect = Vector2.zero;

        /// <summary>
        /// 是否显示边界区域
        /// </summary>
        bool showBoard = false;

        /// <summary>
        /// 区域范围边界粗细
        /// </summary>
        //[Tooltip("边界绘图半径")]
         int areaRadius = 5;

        /// <summary>
        /// 区域范围边界颜色
        /// </summary>
        //[Tooltip("边界绘图颜色")]
         Color areaColor = Color.blue;
#endregion

       
#region 检测线程

        /// <summary>
        /// 像素数组
        /// </summary>
        Color[,] pixels;

        /// <summary>
        /// 材质
        /// </summary>
        public Material material;

        /// <summary>
        /// 消毒区域点
        /// </summary>
        List<Vector2> drawPoint = new List<Vector2>();
#endregion


        void Awake()
        {
            mInstance = this;
            if (material.name == "assistBoardMan" || material.name == "assistBoardWoMan")// || material.name == "daoniaoguan"*/
            {
                material = UIRoot.Instance.UIResources[material.name] as Material;
                material.shader = Shader.Find("Standard");
                gameObject.GetComponent<SkinnedMeshRenderer>().material = material;              
            }
            textureMap = material.mainTexture as Texture2D;
            if (textureMap == null)
            {
                textureMap = new Texture2D(1024, 1024);
            }

            originalTexture = new Texture2D(textureMap.width, textureMap.height,TextureFormat.ARGB32,false);
            mirrorTexture = new Texture2D(textureMap.width, textureMap.height);
            byte[] bys = textureMap.GetRawTextureData();
            mirrorTexture.LoadRawTextureData(bys);
            mirrorTexture.Apply();
            
            material.mainTexture = mirrorTexture;
            material.SetTexture("_DetailAlbedoMap", originalTexture);
            material.EnableKeyword("_DETAIL_MULX2");
            if (pixels == null || pixels.Length == 0)
            {
                pixels = new Color[originalTexture.height, originalTexture.width];
                StartCoroutine("fillPixels");
            }

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
#if Test
            f = 100;
#endif
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

        }
        // Use this for initialization
        void Start()
        {
            setRenderParam(mColor, mRadius);
            fixedTex = UIRoot.Instance.UIResources["shengzhiqi_White" + "_T"] as Texture2D;
            getFixBoard();
        }

        public void setMainTextureColor(Color pColor)
        {
            material.SetColor("_Color",pColor);
        }
#if Test
        float f=200;
#endif
        GameObject cut = null;
        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButton(0))
            {
                if (Vector2.Distance(forwardMousePosition, Input.mousePosition) != 0)
                {
                    forwardMousePosition = Input.mousePosition;
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);


                    if (m_castMode == CastMode.Ray)
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.collider.name == gameObject.name)
                            {
                                uvPoint = hit.textureCoord;
                                uvPoint.x *= originalTexture.width;
                                uvPoint.y *= originalTexture.height;
                                pointQueue.Enqueue(uvPoint);
                            }
                        }
                    }
                    else if (m_castMode == CastMode.Plane)
                    {
                        RaycastHit hit;
                        if (Physics.Raycast(ray, out hit))
                        {
                            if (hit.collider.name == gameObject.name)
                            {
                                //Debug.Log("碰！！！！！");
                                Vector2 temp = uvPoint;
                                uvPoint = hit.textureCoord;
                                uvPoint.x *= originalTexture.width;
                                uvPoint.y *= originalTexture.height;
                                if (temp != Vector2.zero)
                                {
                                    float count= Vector2.Distance(temp, uvPoint) / (2 * mRadius);

                                    while (count > 0.5f)
                                    {
                                        temp += (uvPoint - temp).normalized * mRadius;
                                        pointQueue.Enqueue(temp);
                                        count = Vector2.Distance(temp, uvPoint) / (2 * mRadius);
                                    }
                                }
                                pointQueue.Enqueue(uvPoint);
                            }
                            else
                            {
                                uvPoint = Vector2.zero;
                            }
                        }
                    }
                    else if (m_castMode == CastMode.Rays)
                    {

                    }
                }

            }
            else
            {
                uvPoint = Vector2.zero;
            }
        }
        /// <summary>
        /// LateUpdate is called every frame, if the Behaviour is enabled.
        /// It is called after all Update functions have been called.
        /// </summary>
        void LateUpdate()
        {
            #region  绘制点
            if (pointQueue.Count > 0)
            {
                // Debug.Log("绘制点");
                if (mBrushStyle == brushStyle.Rect)
                {
                    Vector2 r = Vector2.zero;
                    if (mRect.x == 0)
                        r.x = originalTexture.height;
                    else
                        r.x = mRect.x;

                    if (mRect.y == 0)
                        r.y = originalTexture.width;
                    else
                        r.y = mRect.y;

                    setPoint(pointQueue.Dequeue(), mColor, r.x, r.y);
                }

                else if (mBrushStyle == brushStyle.Circle)
                {
                    if (CGlobal.productName.Contains("男"))
                        setFixPoint(pointQueue.Dequeue(), mColor, mRadius);
                    else
                        setPoint(pointQueue.Dequeue(), mColor, mRadius);
                }


            }
            #endregion
        }

        public void SetCastMode(CastMode _mode, float radius)
        {
            m_castMode = _mode;
            m_castSize = radius;
        }
        CTexturePoint yinjing;
        /// <summary>
        /// 退出
        /// </summary>
        private void OnApplicationQuit()
        {
            StopAllCoroutines();
            if (CGlobal.productCode!="200")
            {
                if (textureMap != null)
                    material.mainTexture = textureMap;
            }
            else
            {
                yinjing = GameObject.Find("Models").transform.Find("sickRoom/patient/UC_Patient/man/man_status/yinjing").GetComponent<CTexturePoint>();//下垂
                if (yinjing != null)
                    yinjing.material.mainTexture = UIRoot.Instance.UIResources["shengzhiqi_B" + "_T"] as Texture2D;
            }
        }

        /// <summary>
        /// 清空缓存
        /// </summary>
        public void clearCheckCache()
        {
            StartCoroutine("fillPixels");
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
        /// 恢复材质,清楚所有内容
        /// </summary>
        public void resetMaterial()
        {
            material.mainTexture = textureMap;
        }

        /// <summary>
        /// 恢复材质
        /// </summary>
        public void resetMaterial(Vector2 markPoint, Color pColor, float length, float radius)
        {
            mirrorTexture.LoadRawTextureData(textureMap.GetRawTextureData());
            mirrorTexture.Apply();
        }

        /// <summary>
        /// 清空所有数据，如已着色，色彩不会清空
        /// </summary>
        public void clearCache()
        {
            pointList.Clear();
            limitPointList.Clear();
            threadMutex = false;
            threadRadius = false;
            threadDistance = false;
            firstCenter = Vector2.zero;
            currentPoint = Vector2.zero;
            isLeaveWhite = true;
            isRadiusError = false;
            isFromOutToIn = false;
            StartCoroutine("fillPixels");


            drawPoint.Clear();
            leftPoints.Clear();
            rightPoints.Clear();
        }

        /// <summary>
        /// 绘制点-圆形
        /// </summary>
        /// <param name="pPoint">点</param>
        /// <param name="pColor">绘制颜色</param>
        /// <param name="pRadius">范围半径</param>
        /// <param name="isLimitBoard">是否显示边框</param>
        void setPoint(Vector2 pPoint, Color pColor, float pRadius, PointStatus status = PointStatus.Draw)
        {
#if Test
            f += 0.5f;
            pColor.a = f / 255;
#endif
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
                            drawPoint.Add(new Vector2(i, j));
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
        /// 绘制点-矩形
        /// </summary>
        /// <param name="pPoint">点</param>
        /// <param name="pColor">绘制颜色</param>
        /// <param name="pWeight">宽度(x)</param>
        /// <param name="pHeight">高度(y)</param>
        /// <param name="isLimitBoard">是否显示边框</param>
        void setPoint(Vector2 pPoint, Color pColor, float pWeight,float pHeight, PointStatus status = PointStatus.Draw)
        {

#if Test
            f += 0.5f;
            pColor.a = f/255;
#endif
            //Debug.Log("point="+pPoint);
            int maxX = (int)(pPoint.x + pWeight);
            int minX = (int)(pPoint.x - pWeight);
            int maxY = (int)(pPoint.y + pHeight);
            int minY = (int)(pPoint.y - pHeight);

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
            }

            for (int i = minX; i < maxX; i++)
            {
                for (int j = minY; j < maxY; j++)
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
        /// 绘制点-修补圆形
        /// </summary>
        /// <param name="pPoint">点</param>
        /// <param name="pColor">绘制颜色</param>
        /// <param name="pRadius">范围半径</param>
        /// <param name="isLimitBoard">是否显示边框</param>
        void setFixPoint(Vector2 pPoint, Color pColor, float pRadius, PointStatus status = PointStatus.Draw)
        {
            pPoint = new Vector2((int)(pPoint.x), (int)(pPoint.y));
            Vector2 tempPoint1 = new Vector2(-1f,-1f);//同y近侧点
            Vector2 tempPoint2 = new Vector2(-1f, -1f);//同y对侧点

            Vector2 tempPoint3 = new Vector2(-1f, -1f);//同y近侧点
            Vector2 tempPoint4 = new Vector2(-1f, -1f);//同y对侧点
            Vector2 pPointReflect = new Vector2(-1f, -1f);//映射圆心点

            Vector2 Apoint = Vector2.zero;
            Vector2 Bpoint = Vector2.zero;
            float firstDis = -2;
            bool isLeft=false;

            int index1 = -1;
            int index2 = -1;
            int indexTpm = -1;
            #region 近侧点在左
            foreach (var item in leftPoints)
            {
                indexTpm++;
                if (Vector2.Distance(item, pPoint) <= pRadius)
                {
                    if(index1 == -1)
                    {
                        index1 = indexTpm;
                    }
                    else
                    {
                        index2 = indexTpm;
                    }
                }
            }
            
            if (index1 != -1 && index2 != -1)
            {
                tempPoint1 = leftPoints[index1];
                tempPoint2 = leftPoints[index2];
                //Apoint = (tempPoint1 + tempPoint2) / 2;
                Apoint = leftPoints[(index2 + index1) / 2];
                Apoint = new Vector2((int)Apoint.x, (int)Apoint.y);
                firstDis = Vector2.Distance(Apoint, pPoint);
                tempPoint3 = rightPoints[(index1 / leftPoints.Count) * rightPoints.Count];
                tempPoint4 = rightPoints[(index2 / leftPoints.Count) * rightPoints.Count];
                //Bpoint = (tempPoint3 + tempPoint4) / 2;
                Bpoint = rightPoints[(index1 + index2) / 2];
                Bpoint = new Vector2((int)Bpoint.x, (int)Bpoint.y);

                Vector2 nortmp = tempPoint3 - tempPoint4;
                Vector2 normal = new Vector2(1, -nortmp.x / nortmp.y);
                pPointReflect = Bpoint+ normal.normalized * (firstDis);
                //pPointReflect = Bpoint - new Vector2(Apoint.x - pPoint.x- pRadius, 0);
            }
            #endregion
            else
            {
                indexTpm = -1;
                index1 = -1;
                index2 = -1;
                #region  近侧点在右
                foreach (var item in rightPoints)
                {
                    indexTpm++;
                    if (Vector2.Distance(item, pPoint) <= pRadius)
                    {
                        if (index1 == -1)
                        {
                            index1 = indexTpm;
                        }
                        else
                        {
                            index2 = indexTpm;
                        }
                    }
                }
                if (index1 != -1 && index2 != -1)
                {
                    tempPoint1 = rightPoints[index1];
                    tempPoint2 = rightPoints[index2];
                    //Apoint = (tempPoint1 + tempPoint2) / 2;
                    Apoint = rightPoints[(index2+ index1)/2];
                    Apoint = new Vector2((int)Apoint.x, (int)Apoint.y);
                    firstDis = Vector2.Distance(Apoint, pPoint);
                    tempPoint3 = leftPoints[(index1 / rightPoints.Count) * leftPoints.Count];
                    tempPoint4 = leftPoints[(index1 / rightPoints.Count) * leftPoints.Count];
                    //foreach (var item2 in leftPoints)
                    //{
                    //    if (item2.y == tempPoint1.y)
                    //        tempPoint3 = item2;
                    //    if (item2.y == tempPoint2.y)
                    //        tempPoint4 = item2;
                    //    if (tempPoint3.x != -1 || tempPoint4.x != -1)
                    //        break;
                    //}
                    //Bpoint= (tempPoint3 + tempPoint4) / 2;
                    Bpoint = leftPoints[(index1 + index2) / 2];
                    Bpoint = new Vector2((int)Bpoint.x, (int)Bpoint.y);
                    Vector2 nortmp = tempPoint3 - tempPoint4;
                    Vector2 normal = new Vector2(-1, nortmp.x / nortmp.y);
                     pPointReflect = Bpoint+ normal.normalized * (firstDis);
                    //pPointReflect = Bpoint - new Vector2(Apoint.x - pPoint.x - pRadius, 0);
                    //Debug.Log("tempPoint1" + tempPoint1);
                    //Debug.Log("tempPoint2" + tempPoint2);
                    //Debug.Log("tempPoint3" + tempPoint3);
                    //Debug.Log("tempPoint4" + tempPoint4);
                    //Debug.Log("normal" + normal);
                    //Debug.Log("firstDis" + firstDis);
                    //Debug.Log(pPoint);
                    //Debug.Log(pPointReflect);
                }
                #endregion
            }

            int maxX1;
            int minX1;
            int maxY1;
            int minY1;
            if (pPointReflect.x != -1)
            {
                maxX1 = (int)(pPointReflect.x + pRadius);
                minX1 = (int)(pPointReflect.x - pRadius);
                maxY1 = (int)(pPointReflect.y + pRadius);
                minY1 = (int)(pPointReflect.y - pRadius);

                if (minX1 < 0)
                    minX1 = 0;
                if (minY1 < 0)
                    minY1 = 0;
                if (maxX1 > originalTexture.width)
                    maxX1 = originalTexture.width;
                if (maxY1 > originalTexture.height)
                    maxY1 = originalTexture.height;

                if (status == PointStatus.Draw)
                {
                    pointList.Add(pPointReflect);
                }
                for (int i = minX1; i < maxX1; i++)
                {
                    for (int j = minY1; j < maxY1; j++)
                    {
                        if (Vector2.Distance(pPointReflect, new Vector2(i, j)) <= pRadius)
                        {
                            if (status == PointStatus.Draw)
                            {
                                originalTexture.SetPixel(i, j, pColor);
                                pixels[i, j] = pColor;
                                drawPoint.Add(new Vector2(i, j));
                            }
                            else
                            {
                                mirrorTexture.SetPixel(i, j, pColor);
                            }
                        }
                    }
                }
            }

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
                            drawPoint.Add(new Vector2(i, j));
                        }
                        else
                        {
                            mirrorTexture.SetPixel(i, j, pColor);
                        }
                    }
                }
            }
            //if (pPointReflect.x != -1)
            //{
            //    originalTexture.SetPixel((int)tempPoint1.x, (int)tempPoint1.y, new Color(1, 0, 0, 1));
            //    originalTexture.SetPixel((int)tempPoint2.x, (int)tempPoint2.y, new Color(1, 0, 0, 1));
            //    originalTexture.SetPixel((int)tempPoint3.x, (int)tempPoint3.y, new Color(1, 0, 0, 1));
            //    originalTexture.SetPixel((int)tempPoint4.x, (int)tempPoint4.y, new Color(1, 0, 0, 1));
            //    originalTexture.SetPixel((int)pPoint.x, (int)pPoint.y, new Color(1, 0, 0, 1));
            //    originalTexture.SetPixel((int)pPointReflect.x, (int)pPointReflect.y, new Color(1, 0, 0, 1));

            //}
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


        /// <summary>
        /// 设置有效边界区域，并且显示出来，若无需显示，则不必使用
        /// </summary>
        /// <param name="points"></param>
        public void setAreaByPoints(Vector2 pCenter, int pRadius)
        {
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



        List<Vector2> leftPoints = new List<Vector2>();
        List<Vector2> rightPoints = new List<Vector2>();
        Texture2D fixedTex;
        Color texColorR=new Color(1,0,0,1);
        Color texColorB = new Color(0, 0, 1, 1);
        public void getFixBoard()
        {
            for (int i = 0; i < fixedTex.height; i++)
            {
                //bool isFirst = true;
                //Vector2 firstVector = new Vector2(-1, -1);
                //Vector2 lastVector = new Vector2(-1, -1);
                for (int j = 0; j < fixedTex.width; j++)
                {
                    //if (fixedTex.GetPixel(j,i) == texColor)
                    //{
                    //    if(isFirst == true)
                    //    {
                    //        firstVector = new Vector2(j,i);
                    //        isFirst = false;
                    //    }
                    //    else
                    //    {
                    //        lastVector= new Vector2(j, i);
                    //    }
                    //}

                    if (fixedTex.GetPixel(j, i) == texColorR)
                        rightPoints.Add(new Vector2(j, i));
                    if (fixedTex.GetPixel(j, i) == texColorB)
                        leftPoints.Add(new Vector2(j, i));
                }
                //if(firstVector.x != -1 && lastVector.x != -1)
                //{
                //    leftPoints.Add(firstVector);
                //    rightPoints.Add(lastVector);
                //}
            }
            // originalTexture.Apply();
        }
        /// <summary>
        /// 设置模型第二贴图
        /// </summary>
        /// <param name="secondImage"></param>
        public void SetMateral(Texture2D secondImage)
        {
            if (secondImage != null)
                originalTexture = secondImage;
            else
                originalTexture = new Texture2D(textureMap.width, textureMap.height, TextureFormat.ARGB32, false);
            material.SetTexture("_DetailAlbedoMap", originalTexture);
        }
        /// <summary>
        /// 临时图保存
        /// </summary>
        /// <param name="inTexture"></param>
        /// <param name="changeLastTex"></param>
        public void resetDisinfection(Texture2D inTexture, out Texture2D changeLastTex)
        {
            Texture2D temp=null;
            if (inTexture != null)
            {
                temp = new Texture2D(inTexture.width, inTexture.height, TextureFormat.ARGB32, false);
                temp.LoadRawTextureData(inTexture.GetRawTextureData());
                temp.Apply();
            }
            changeLastTex = temp;
        }

        /// <summary>
        /// 消毒颜色随时间变浅
        /// </summary>
        public void changeColor()
        {
            List<Vector2> list = new List<Vector2>();
            for (int i = 0; i < drawPoint.Count; i++)
            {
                list.Add(drawPoint[i]);
            }
            StartCoroutine("change", list);
            drawPoint.Clear();
        }
        IEnumerator change(List<Vector2> list)
        {
            int count = 4;
            Color temp = originalTexture.GetPixel((int)list[0].x, (int)list[0].y);
            float max = Mathf.Max(temp.r, temp.g, temp.b,temp.a);
            for (int j = 0; j < count; j++)
            {
                temp = new Color(temp.r < max ? (temp.r + 10 / 255f) : temp.r, temp.g < max ? (temp.g + 10 / 255f) : temp.g, temp.b < max ? (temp.b + 10 / 255f) : temp.b, temp.a);
                for (int i = 0; i < list.Count; i++)
                {
                    originalTexture.SetPixel((int)list[i].x, (int)list[i].y, temp);
                    pixels[(int)list[i].x, (int)list[i].y] = temp;
                }
                originalTexture.Apply();
                yield return new WaitForSeconds(0.2f);
            }
        }
    }
}
