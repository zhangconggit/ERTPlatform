using UnityEngine;
using System.Collections;
using CFramework;
using System.Collections.Generic;
using UnityEngine.UI;

public class AnimationScript : AnimationManager
{
    /// <summary>
    /// 动画结束回调
    /// </summary>
    VoidDelegate OnAnimationEnd;
    /// <summary>
    /// 是否开始播放节点动画
    /// </summary>
    bool startPlay;
    /// <summary>
    /// 模型动画偏移量
    /// </summary>
    float value;
    /// <summary>
    /// Clip组件
    /// </summary>
    AnimationClip animClip;
    /// <summary>
    /// animation组件
    /// </summary>
    Animation anim;
    /// <summary>
    /// animator组件
    /// </summary>
    Animator animTor;

    /// <summary>
    /// 动画状态名称
    /// </summary>
    string animatorName;
    /// <summary>
    /// animator回调
    /// </summary>
    VoidDelegate OnAnimatorEnd;
    /// <summary>
    /// 是否开始播放骨骼动画
    /// </summary>
    bool startPlayAvtar;
    public new static AnimationScript Instance
    {
        get
        {
            return CMonoSingleton<AnimationScript>.Instance();
        }
    }
    public new void OnDestroy()
    {
        CMonoSingleton<AnimationScript>.OnDestroy();
    }
    public AnimationScript()
    {
        value = 0;
        startPlay = false;
        startPlayAvtar = false;
        m_SliceTexsDic.Clear();
        m_PlayBg = UIRoot.Instance.UIResources["zhuye-1" + "_T"] as Texture2D;
    }
    /// <summary>
    /// 接收处理消息
    /// </summary>
    /// <param name="msg">消息</param>
    public override void ProcessMsg(CMsg msg)
    {

    }
    void Awake()
    {
        if (anim == null)
            anim = this.gameObject.AddComponent<Animation>();
    }
    // Use this for initialization
    void Start()
    {
        m_iCurFramControl = 0;
        isShow = false;

        m_tPointImage = UIRoot.Instance.UIResources["qinang" + "_T"] as Texture2D; //缺数据



        //m_tPointImage.SetPixel(0,0,m_cPointColor);
        //m_texturesss = new Texture2D[cellCount];
        //m_texturesss1 = new Texture2D[cellCount];
        //m_texturesss2 = new Texture2D[cellCount];
        //StartCoroutine(Depack("Intubating1"));
    }
    public void SetIntubationImage(Vector2 pos)
    {
        m_ShowPos = pos;
    }
    /// <summary>
    /// 初始化坡面图
    /// </summary>
    /// <param name="names"></param>
    /// <param name="cellWidth"></param>
    /// <param name="cellHight"></param>
    /// <param name="cellRow"></param>
    /// <param name="cellCol"></param>
    /// <param name="cellCount"></param>
    public void InitIntubationImageData(List<string> names, int cellWidth, int cellHight, int cellRow, int cellCol, int cellCount)
    {
        m_sLoadTextureName = names;
        row = cellRow;
        col = cellCol;
        this.cellWidth = cellWidth;
        this.cellHeight = cellHight;
        this.cellCount = cellCount;
        m_texturesssList.Clear();
        StartCoroutine(FinishSetTexture());
    }
    /// <summary>
    /// 初始化坡面图
    /// </summary>
    /// <param name="OffsetStart">开始偏移点</param>
    /// <param name="OffsetEnd">开始偏移点</param>
    public void InitIntubationImageData(List<string> names, int cellWidth, int cellHight, int cellRow, int cellCol, int cellCount, Vector2 OffsetStart, Vector2 OffsetEnd)
    {
        InitIntubationImageData(names, cellWidth, cellHight, cellRow, cellCol, cellCount);
        m_OffsetPositon = OffsetStart;
        m_OffsetSize = OffsetEnd - OffsetStart;//224,334
        m_OffsetSize = new Vector2(cellWidth, cellHeight) - m_OffsetSize;//76,66
    }
    IEnumerator FinishSetTexture()
    {
        m_texturesss = new Texture2D[cellCount];
        for (int i = 0; i < m_sLoadTextureName.Count; i++)
        {
            m_texturesssList.Add(new Texture2D[cellCount]);
            yield return StartCoroutine(Depack(m_sLoadTextureName[i], i));

        }
        setTexture(0);
    }
    IEnumerator Depack(string name, int k)
    {
        yield return new WaitForSeconds(0);
        #region  手动控制帧动画
        {

            if (UIRoot.Instance.UIResources[name + "_T"] == null)
                yield break;
            Texture2D _texture = UIRoot.Instance.UIResources[name + "_T"] as Texture2D; //Resources.Load("IntubatingReverse/Intubating1") as Texture2D;

            for (int i = 0; i < col; ++i)
            {
                for (int j = 0; j < row; ++j)
                {
                    if (((row - 1 - j) * col + i) < cellCount)
                    {
                        DePackTexture(_texture, k, i, j);
                        yield return new WaitForSeconds(0);
                    }

                    else
                    {
                        yield return new WaitForSeconds(0);
                        continue;
                    }

                }
            }
        }

        #endregion
    }
    // Update is called once per frame
    void Update()
    {
        if (anim != null)
        {
            if (anim.isPlaying == false && startPlay == true)
            {
                if (OnAnimationEnd != null)
                    OnAnimationEnd();
                startPlay = false;
            }
        }
        if (animTor != null)
        {
            if (animTor.IsInTransition(0) == true && startPlayAvtar == true)
            {
                if (OnAnimatorEnd != null)
                    OnAnimatorEnd();
                startPlayAvtar = false;
            }
        }
    }
    void OnGUI()
    {
        if (isPlaySprite)
        {
            GUI.DrawTexture(new Rect((1600 * Screen.width / 1920), (750 * Screen.height / 1080), (300 * Screen.width / 1920), (300 * Screen.height / 1080)), m_PlayBg, ScaleMode.StretchToFill, true, 0);
            //绘制当前帧
            DrawAnimationSliceTets(m_SliceTexsDic[playName], new Rect((1600 * Screen.width / 1920), (750 * Screen.height / 1080), (300 * Screen.width / 1920), (300 * Screen.height / 1080)));
            return;
        }
        #region 手动控制帧动画
        if (isShow == true)
        {
            DrawAnimation(m_texturesss, new Rect((m_ShowPos.x + m_OffsetPositon.x) * Screen.width / 1920, (m_ShowPos.y + m_OffsetPositon.y) * Screen.height / 1080, (cellWidth - m_OffsetSize.x) * Screen.width / 1920, (cellHeight - m_OffsetSize.y) * Screen.height / 1080));
        }
        if (m_bShowPoint)
        {
            GUI.DrawTexture(new Rect(m_vPointPos - new Vector2(m_iPointSize / 2, m_iPointSize / 2), new Vector2(m_iPointSize, m_iPointSize)), m_tPointImage, ScaleMode.StretchToFill, true, 0);
        }
        #endregion
    }
    #region  自动帧动画播放
    //背景
    private Texture2D m_PlayBg;
    //小图图集字典
    Dictionary<string, Texture2D[,]> m_SliceTexsDic = new Dictionary<string, Texture2D[,]>();

    //播放图片名称
    string playName = "";

    //添加图片名称
    string addName = "";
    //当前帧
    private int m_iCurFram;
    //当前动画
    private int m_iCurAnimation;
    //播放速度
    private float timerInterval = 0.2f;
    private float lasttime = 0;
    //动画控制
    private bool isPlaySprite = false;
    private bool isPlayOppsite = false;

    VoidDelegate OnPlaySpriteEnd;
    /// <summary>
    /// 切图（需在初始化中切图）
    /// </summary>
    /// <param name="path">图集路径</param>
    /// <param name="lines">行数</param>
    /// <param name="rows">列数</param>
    public void PlaySprites(string path, int lines = 1, int rows = 1)
    {
        StartCoroutine(IEPlaySprites(path, lines, rows));
        //addName = path;
        //Texture2D m_originTexture = UIRoot.OnlineResources[path + "_T"] as Texture2D;//获取图片
        //Texture2D[,] m_SliceTexs = new Texture2D[rows, lines];
        //for (int i = 0; i < rows; ++i)//7
        //{
        //    for (int j = 0; j < lines; ++j)//1
        //    {
        //        DePackTexture(i, j, m_SliceTexs, m_originTexture);
        //    }
        //}
        //if (!m_SliceTexsDic.ContainsKey(path))
        //    m_SliceTexsDic.Add(path, m_SliceTexs);
    }
    IEnumerator IEPlaySprites(string path, int lines = 1, int rows = 1)
    {
        yield return 0;
        Texture2D m_originTexture = UIRoot.Instance.UIResources[path + "_T"] as Texture2D;//获取图片
        Texture2D[,] m_SliceTexs = new Texture2D[rows, lines];
        for (int i = 0; i < rows; ++i)//7
        {
            for (int j = 0; j < lines; ++j)//1
            {
                DePackTexture(i, j, m_SliceTexs, m_originTexture);
                yield return 0;
            }
        }
        if (!m_SliceTexsDic.ContainsKey(path))
            m_SliceTexsDic.Add(path, m_SliceTexs);
    }
    public void StopPlay()
    {
        isPlaySprite = false;
    }

    /// <summary>
    /// 切图方法
    /// </summary>
    /// <param name="i"></param>
    /// <param name="j"></param>
    /// <param name="m_SliceTexs"></param>
    /// <param name="m_originTexture"></param>
    void DePackTexture(int i, int j, Texture2D[,] m_SliceTexs, Texture2D m_originTexture)
    {
        int cur_x = i * (int)((float)m_originTexture.width / m_SliceTexs.GetLength(0));
        int cur_y = j * (int)((float)m_originTexture.height / m_SliceTexs.GetLength(1));
        Texture2D newTexture = new Texture2D((int)((float)m_originTexture.width / m_SliceTexs.GetLength(0)), (int)((float)m_originTexture.height / m_SliceTexs.GetLength(1)));
        newTexture.SetPixels(m_originTexture.GetPixels(cur_x, cur_y, (int)((float)m_originTexture.width / m_SliceTexs.GetLength(0)), (int)((float)m_originTexture.height / m_SliceTexs.GetLength(1))));
        newTexture.Apply();
        m_SliceTexs[i, j] = newTexture;
    }

    /// <summary>
    /// 播放切图动画（执行之前需执行切图操作）
    /// </summary>
    /// <param name="path">图片路径</param>
    /// <param name="speed">播放方向</param>
    /// <param name="animEndCallBack">结束回调</param>
    public void GoPlaySprites(string path, float speed = 0.2f, VoidDelegate animEndCallBack = null)
    {
        OnPlaySpriteEnd = animEndCallBack;
        timerInterval = Mathf.Abs(speed);
        m_iCurFram = 0;
        m_iCurAnimation = m_SliceTexsDic[path].GetLength(1) - 1;
        if (speed < 0)
        {
            m_iCurFram = m_SliceTexsDic[path].GetLength(0) - 1;
            m_iCurAnimation = 0;
            isPlayOppsite = true;
        }
        playName = path;
        isPlaySprite = true;
    }
    /// <summary>
    /// 渲染逻辑
    /// </summary>
    /// <param name="tex">小图集合</param>
    /// <param name="rect">渲染位置</param>
    void DrawAnimationSliceTets(Texture[,] tex, Rect rect)
    {
        //绘制当前帧
        GUI.DrawTexture(rect, tex[m_iCurFram, m_iCurAnimation], ScaleMode.StretchToFill, true, 0);

        if (Time.time - timerInterval > lasttime)
        {
            m_iCurFram = m_iCurFram % tex.GetLength(0);
            lasttime = Time.time;
            if (isPlayOppsite)
            {
                m_iCurFram--;
                if (m_iCurFram <= 0)
                {
                    m_iCurAnimation++;
                    if (m_iCurFram <= 0 && m_iCurAnimation > tex.GetLength(1) - 1)
                    {
                        isPlaySprite = false;
                        isPlayOppsite = false;
                        //播放完之后
                        if (OnPlaySpriteEnd != null)
                            OnPlaySpriteEnd();
                    }
                    if (m_iCurAnimation > tex.GetLength(1) - 1)
                        m_iCurAnimation = 0;
                    m_iCurFram = tex.GetLength(0) - 1;
                }
            }
            else
            {
                m_iCurFram++;
                if (m_iCurFram >= tex.GetLength(0))
                {
                    m_iCurAnimation--;
                    if (m_iCurFram >= tex.GetLength(0) && m_iCurAnimation < 0)
                    {
                        isPlaySprite = false;
                        //播放完之后
                        OnPlaySpriteEnd();
                    }
                    if (m_iCurAnimation < 0)
                        m_iCurAnimation = tex.GetLength(1) - 1;
                    m_iCurFram = 0;
                }
            }
        }
    }
    #endregion
    #region 手动帧动画
    public Texture2D m_texture;//原序列图

    public Texture2D[] m_texturesss;//序列图集

    public int cellWidth = 375;//单张宽度

    public int cellHeight = 456;//单张高度

    public int row = 7;//行

    public int col = 6;//列

    public int cellCount = 41;//总序列图数

    Texture2D[] m_texturesss1;
    Texture2D[] m_texturesss2;

    public List<string> m_sLoadTextureName = new List<string>();
    List<Texture2D[]> m_texturesssList = new List<Texture2D[]>();
    public Vector2 m_ShowPos = new Vector2(175, 90);

    Vector2 m_OffsetPositon = Vector2.zero;
    Vector2 m_OffsetSize = Vector2.zero;

    //当前帧
    public static int m_iCurFramControl = 0;


    public static bool isShow = false;
    /// <summary>
    /// 设置图片源
    /// </summary>
    /// <param name="id">0：嘴序列，1：鼻腔序列</param>
    public void setTexture(int id = 0)
    {
        m_OffsetPositon = Vector2.zero;
        m_OffsetSize = Vector2.zero;
        m_texturesss = m_texturesssList[id];
    }
    /// <summary>
    /// 设置图片源
    /// </summary>
    /// <param name="id">0：嘴序列，1：鼻腔序列</param>
    public void setTexture(int id, Vector2 OffsetStart, Vector2 OffsetEnd)
    {
        setTexture(id);
        m_OffsetPositon = OffsetStart;
        m_OffsetSize = OffsetEnd - OffsetStart;
        m_OffsetSize = new Vector2(cellWidth, cellHeight) - m_OffsetSize;
    }
    void DePackTexture(int i, int j)
    {
        int cur_x = i * cellWidth;
        int cur_y = j * cellHeight;

        Texture2D newTexture = new Texture2D(cellWidth, cellHeight);

        for (int m = cur_y; m < cur_y + cellHeight; ++m)
        {
            for (int n = cur_x; n < cur_x + cellWidth; ++n)
            {
                newTexture.SetPixel(n - cur_x, m - cur_y, m_texture.GetPixel(n, m));
            }
        }
        newTexture.Apply();
        m_texturesss[(row - 1 - j) * col + i] = newTexture;
    }
    /// <summary>
    /// 切图逻辑
    /// </summary>
    /// <param name="src"></param>
    /// <param name="k"></param>
    /// <param name="i"></param>
    /// <param name="j"></param>
    void DePackTexture(Texture2D src, int k, int i, int j)
    {
        int cur_x = i * cellWidth;
        int cur_y = j * cellHeight;

        Texture2D newTexture = new Texture2D(cellWidth - (int)m_OffsetSize.x, cellHeight - (int)m_OffsetSize.y);
        newTexture.SetPixels(src.GetPixels(cur_x + (int)m_OffsetPositon.x, cur_y + (int)m_OffsetPositon.y, cellWidth - (int)m_OffsetSize.x, cellHeight - (int)m_OffsetSize.y));
        //for (int m = cur_y; m < cur_y + cellHeight; ++m)
        //{
        //    for (int n = cur_x; n < cur_x + cellWidth; ++n)
        //    {
        //        newTexture.SetPixel(n - cur_x, m - cur_y, src.GetPixel(n, m));
        //    }
        //}
        newTexture.Apply();
        m_texturesssList[k][(row - 1 - j) * col + i] = newTexture;
        //if (k == 0)
        //    m_texturesss1[(row - 1 - j) * col + i] = newTexture;
        //else if (k == 1)
        //    m_texturesss2[(row - 1 - j) * col + i] = newTexture;
    }
    /// <summary>
    /// 渲染逻辑
    /// </summary>
    /// <param name="tex"></param>
    /// <param name="rect"></param>
    void DrawAnimation(Texture[] tex, Rect rect)
    {

        if (m_iCurFramControl < cellCount && m_iCurFramControl >= 0)
        {
            //绘制当前帧
            GUI.DrawTexture(rect, tex[m_iCurFramControl], ScaleMode.StretchToFill, true, 0);
        }
        else
        {
            if (m_iCurFramControl >= cellCount)
            {
                GUI.DrawTexture(rect, tex[cellCount - 1], ScaleMode.StretchToFill, true, 0);
            }
            if (m_iCurFramControl < 0)
            {
                GUI.DrawTexture(rect, tex[0], ScaleMode.StretchToFill, true, 0);
            }

        }
    }
    #endregion
    #region 绘制点
    public bool m_bShowPoint = false;

    public Vector2 m_vPointPos = Vector2.zero;

    public int m_iPointSize = 4;

    public Color m_cPointColor = Color.yellow;

    public Texture2D m_tPointImage = null;
    #endregion
    /// <summary>
    /// 通过模型播放动画
    /// </summary>
    /// <param name="animParent"></param>
    /// <param name="startTime"></param>
    /// <param name="playSpeed"></param>
    public void PlayAnimByModel(GameObject animParent, float startTime = 0, float playSpeed = 0.5f, VoidDelegate callBackEnd = null)
    {
        OnAnimationEnd = callBackEnd;
        anim = animParent.GetComponent<Animation>();
        if (anim != null)
        {
            animClip = animParent.GetComponent<Animation>().clip;
            if (animParent != null)
            {
                anim[animClip.name].time = startTime;
                anim[animClip.name].speed = playSpeed;
                anim.Play(animClip.name);
                startPlay = true;
            }
            else
            {
                throw new System.Exception("Could't find this GameObject");
            }
        }
    }
    /// <summary>
    /// 模型动画单位偏移量
    /// </summary>
    /// <param name="modelName">模型名称</param>
    /// <param name="unit">偏移量</param>
    public void SetAnimaMoveUnit(GameObject modelName, float startPos = 0, float unit = 0, int index = 0)
    {
        if (value < startPos)
        {
            value = startPos;
        }
        value += unit;
        modelName.GetComponent<SkinnedMeshRenderer>().SetBlendShapeWeight(index, value);
    }
    
    public void PlayAnimaByAvtar(GameObject modelName, string stateName, VoidDelegate animEndCallBack = null)
    {
        OnAnimatorEnd = animEndCallBack;
        animatorName = stateName;
        animTor = modelName.GetComponent<Animator>();
        if (animTor != null)
            animTor.Play(stateName);
        startPlayAvtar = true;
    }
}
