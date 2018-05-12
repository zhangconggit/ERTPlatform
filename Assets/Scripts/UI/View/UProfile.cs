using UnityEngine;
using System.Collections;
using CFramework;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
/// 剖面辅助图
/// </summary>
public class UProfile : UPageBase
{
    UImage background;
    int formatWidth = 6;
    int formatHight = 8;
    //Sprite mainSprite;
    UImage qiu;
    int qiuSize = 0;
    int qiuBaseSize = 2;
    Vector2 qiuPos = new Vector2(0, 0);
    LoadTextAsset textAsset;
    List<Vector3> pathList;
    Vector2 baseVector = new Vector2(814, -284);
    public UProfile(string imagesrc, Rect rect)
    {
        name = "UProfile";
        SetAnchored(AnchoredPosition.full);
        SetBorderSpace(0, 0, 0, 0);
        //mainSprite = UIRoot.OnlineResources[strimage] as Sprite;
        background = new UImage();
        background.SetParent(this);
        background.name = "background";
        background.SetAnchored(AnchoredPosition.center);
        background.rect = rect;
        background.LoadImage(imagesrc);
        baseVector = rect.position - baseVector;
        baseVector.y = -baseVector.y;

        qiu = new UImage(AnchoredPosition.center);
        qiu.SetParent(background);
        qiu.LoadImage("qinang");
        //qiu.color = new Color(0, 1, 0);
        qiu.rect = new Rect(0, 0, qiuBaseSize+qiuSize, qiuBaseSize + qiuSize);
        LineRenderManager.Instance.SetLineColor(new Color(243f / 255, 225f / 255, 36f / 255));
       // GLRender.Instance.SetColor(new Color(243f / 255, 230f / 255, 71f / 255));
    }
    public UProfile()
    {
        name = "UProfile";
    }
    public void SetImageFormat(int width,int hight)
    {
        formatWidth = width;
        formatHight = hight;
    }
    public void Start()
    {
        LineRenderManager.Instance.StartRender();
        LineRenderManager.Instance.Draw(new List<Vector3>());
        gameObejct.SetActive(true);

    }
    public void End()
    {
        LineRenderManager.Instance.EndRender();
        gameObejct.SetActive(false);
    }
    /// <summary>
    /// 设置球的大小
    /// </summary>
    /// <param name="f"></param>
    public void SetQiuSize(int f)
    {
        qiuSize = f;
        qiu.rect = new Rect(qiuPos.x, qiuPos.y, qiuBaseSize + qiuSize, qiuBaseSize + qiuSize);
    }
    /// <summary>
    /// 设置插入路径
    /// </summary>
    /// <param name="path"></param>
    public void SetInsertPath(string path)
    {
        textAsset = new LoadTextAsset(path);
       // textAsset = new LoadTextAsset("text/"+path,true);

        pathList = new List<Vector3>();
        while (textAsset.ReadLine())
        {
            string[] s = textAsset.GetLine().Split(' ');
            if(s.Length >=3)
            {
                pathList.Add(new Vector3(float.Parse( s[0]), float.Parse(s[1]),float.Parse(s[2])));
            }
        }
    }
    /// <summary>
    /// 设置背景图片
    /// </summary>
    /// <param name="path"></param>
    public void SetBackground(string path)
    {
        background.LoadImage(path);
    }

    /// <summary>
    /// 设置球的位置
    /// </summary>
    /// <param name="f"></param>
    void SetQiuPos(float f)
    {
        f -= 3f/pathList.Count;
        if(f < 0)
        {
            qiu.gameObejct.SetActive(false);
        }
        else
        {
            qiu.gameObejct.SetActive(true);
        }
        {
            qiuPos = LineRenderManager.Instance.m_camera.WorldToScreenPoint(pathList.GetLinearValue(f));
            qiuPos = ((Vector3)qiuPos).BaseAdapter();
            qiuPos = qiuPos - new Vector2(1920 / 2, 1080 / 2);// + baseVector;
            qiuPos = (Vector3)qiuPos - background.rectTransform.localPosition;
            qiuPos.y = -qiuPos.y;
            SetQiuSize(qiuSize);
        }
        
    }
    /// <summary>
    /// 取得球的位置
    /// </summary>
    /// <param name="f">0-1.0</param>
    public Vector2 GetQiuPos(float f)
    {
        Vector2 reslut = Vector2.zero;
        int k = (int)(f * pathList.Count);
        float diff = f * pathList.Count - k;
        if (k < pathList.Count && pathList.Count > 0)
            qiuPos = pathList[k];
        reslut = qiuPos;
        Vector2 qiuPosNext = Vector2.zero;
        if (diff > 0 && k + 1 < pathList.Count)
        {
            qiuPosNext = pathList[k + 1];
            reslut = Vector2.Lerp(qiuPos, qiuPosNext, diff);
        }
        return reslut;
    }
    
    public void SetInsertLength(float f)
    {
        List<Vector3> paths = new List<Vector3>();

        int k = (int)(f * pathList.Count);
        //Vector3 vec = baseVector;
        for (int i = 0; i < pathList.Count && i<= k; i++)
        {
            //Vector3 screenpos =LineRenderManager.Instance.m_camera.WorldToScreenPoint(pathList[i]);// .ScreenToWorldPoint(vec);
            //screenpos = screenpos.BaseAdapter() + vec;
            //screenpos.z = pathList[i].z;
            //Vector3 vec_t = LineRenderManager.Instance.m_camera.ScreenToWorldPoint(screenpos);
            paths.Add(pathList[i]);
        }
        Vector3 tmp = pathList.GetLinearValue(f);
        //Vector3 screenposLast = LineRenderManager.Instance.m_camera.WorldToScreenPoint(tmp);// .ScreenToWorldPoint(vec);
        //screenposLast = screenposLast.BaseAdapter() + vec;
        //screenposLast.z = tmp.z;
        //Vector3 vec_tt = LineRenderManager.Instance.m_camera.ScreenToWorldPoint(screenposLast);

        paths.Add(tmp);

        LineRenderManager.Instance.Draw(paths);
        SetQiuPos(f);
    }
}
