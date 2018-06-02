using System;
using CFramework;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

struct CrossPoint
{
    /// <summary>
    /// 穿刺点UV
    /// </summary>
    public Vector2 Punctureuv;
    /// <summary>
    /// 穿刺点世界坐标
    /// </summary>
    public Vector3 PuncturePoint;
    /// <summary>
    /// 穿刺点法线
    /// </summary>
    public Vector3 PunctureNormal;
    /// <summary>
    /// 是否被绘制
    /// </summary>
    public Boolean Draw;
    public CrossPoint(Vector2 m_Punctureuv, Vector3 m_PuncturePoint, Vector3 m_PunctureNormal)
    {
        this.Punctureuv = m_Punctureuv;
        this.PuncturePoint = m_PuncturePoint;
        this.PunctureNormal = m_PunctureNormal;
        this.Draw = false;
    }

    public CrossPoint(Vector2 m_Punctureuv, Vector3 m_PuncturePoint, Vector3 m_PunctureNormal, Boolean m_Draw)
    {
        this.Punctureuv = m_Punctureuv;
        this.PuncturePoint = m_PuncturePoint;
        this.PunctureNormal = m_PunctureNormal;
        this.Draw = m_Draw;
    }
}

/// <summary>
/// 标记穿刺点
/// </summary>
public class Biaojichuancidian : StepBase
{
    /// <summary>
    /// 标记穿刺点结束按钮
    /// </summary>
    UPageButton btnBiaojiOk;
    /// <summary>
    /// 标记笔
    /// </summary>
    GameObject biaojibi;
    /// <summary>
    /// 第一条线点序列
    /// </summary>
    List<CrossPoint> line1 = new List<CrossPoint>();
    /// <summary>
    /// 第二条线点序列
    /// </summary>
    List<CrossPoint> line2 = new List<CrossPoint>();
    /// <summary>
    /// 正在划第几线(默认划第1根线)
    /// </summary>
    int drawingLine = 1;
    /// <summary>
    /// 划线矩阵(高)
    /// </summary>
    int drawingLineH = 5;
    /// <summary>
    /// 划线矩阵(宽)
    /// </summary>
    int drawingLineW = 5;
    /// <summary>
    /// 划线颜色
    /// </summary>
    Color drawingLineColor = Color.blue;
    /// <summary>
    /// 划线矩阵颜色组
    /// </summary>
    List<Color> lstDrawingLineColor;
    /// <summary>
    /// 标记点颜色
    /// </summary>
    Color crossColor = Color.black;
    /// <summary>
    /// 标记点颜色组
    /// </summary>
    List<Color> lstCrossColor;
    /// <summary>
    /// 保存初期获取的材质
    /// </summary>
    Color[] baseMaterialColors;
    /// <summary>
    /// 保存上次接触的model
    /// </summary>
    RaycastHit hit;
    /// <summary>
    /// 默认穿刺点
    /// </summary>
    CrossPoint defaultCrossPoint;

    /// <summary>
    /// 默认穿刺点2
    /// </summary>
    CrossPoint defaultCopyCrossPoint = new CrossPoint(Vector2.zero
                                                , Vector3.zero
                                                , Vector3.zero);

    string[] BjbPosWC;

    /// <summary>
    /// 处理完成
    /// </summary>
    bool isFinish;

    public Biaojichuancidian()
    {
        //设置默认穿刺点的坐标
        string[] defaul = fileHelper.ReadIni(GetType().Name, "default", "StepConfig").Split(',');
        defaultCrossPoint = new CrossPoint(new Vector2(float.Parse(defaul[0]), float.Parse(defaul[1]))
                                                , new Vector3(float.Parse(defaul[2]), float.Parse(defaul[3]), float.Parse(defaul[4]))
                                                , new Vector3(float.Parse(defaul[5]), float.Parse(defaul[6]), float.Parse(defaul[7])));
        string[] defaultCopy = fileHelper.ReadIni(GetType().Name, "defaultCopy", "StepConfig").Split(',');
        if (defaultCopy != null & defaultCopy.Length == 8)
        {
            defaultCopyCrossPoint = new CrossPoint(new Vector2(float.Parse(defaultCopy[0]), float.Parse(defaultCopy[1]))
                                                    , new Vector3(float.Parse(defaultCopy[2]), float.Parse(defaultCopy[3]), float.Parse(defaultCopy[4]))
                                                    , new Vector3(float.Parse(defaultCopy[5]), float.Parse(defaultCopy[6]), float.Parse(defaultCopy[7])));
        }
        //设置镜头的位置和欧拉角
        string[] CameraPos = fileHelper.ReadIni(GetType().Name, "CameraPos", "StepConfig").Split(',');
        string[] CameraRot = fileHelper.ReadIni(GetType().Name, "CameraRot", "StepConfig").Split(',');
        cameraEnterPosition = new Vector3(float.Parse(CameraPos[0]), float.Parse(CameraPos[1]), float.Parse(CameraPos[2]));
        cameraEnterEuler = new Vector3(float.Parse(CameraRot[0]), float.Parse(CameraRot[1]), float.Parse(CameraRot[2]));

        //获取标记笔和设置本地欧拉角
        string Bjc = fileHelper.ReadIni(GetType().Name, "Bjb", "StepConfig");
        biaojibi = GameObject.Find("Models").transform.Find(Bjc).gameObject;
        string[] BJBLocalPos = fileHelper.ReadIni(GetType().Name, "BJBLocalPos", "StepConfig").Split(',');
        if (BJBLocalPos != null & BJBLocalPos.Length == 3)
        {
            biaojibi.transform.localRotation = Quaternion.Euler(float.Parse(BJBLocalPos[0]), float.Parse(BJBLocalPos[1]), float.Parse(BJBLocalPos[2]));
        }
        BjbPosWC = fileHelper.ReadIni(GetType().Name, "BjbPosWC", "StepConfig").Split(',');
        //其他初期化
        isFinish = false;
        //追加评分
        AddScore(0);
    }

    /// <summary>
    /// 镜头移动结束
    /// </summary>
    public override void CameraMoveFinished()
    {
        //划线矩阵颜色组
        lstDrawingLineColor = new List<Color>();
        lstCrossColor = new List<Color>();
        for (int i = 0; i < drawingLineW * drawingLineH; i++)
        {
            lstDrawingLineColor.Add(drawingLineColor);
            lstCrossColor.Add(crossColor);
        }

        //标记笔显示
        biaojibi.SetActive(true);

        //标记穿刺点结束按钮
        btnBiaojiOk = new UPageButton(AnchoredPosition.bottom);
        btnBiaojiOk.rect = new UnityEngine.Rect(0, -20, 200, 70);
        btnBiaojiOk.text = "标记穿刺点结束";
        btnBiaojiOk.LoadSprite("anniu-160");
        btnBiaojiOk.LoadPressSprite("anniu-160h");
        btnBiaojiOk.button.text.color = UnityEngine.Color.white;
        btnBiaojiOk.onClick.AddListener(onClickBiaojiOk);
        btnBiaojiOk.SetActive(false);

        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "check_chuanci");

        base.CameraMoveFinished();
    }

    /// <summary>
    /// 步骤更新
    /// </summary>
    public override void StepUpdate()
    {
        //标记完成
        if (isFinish)
        {
            return;
        }
        //追加评分
        AddScore(-1);

        var mouseVector3 = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0.1588f));
        
        mouseVector3 = new Vector3(mouseVector3.x + float.Parse(BjbPosWC[0]), mouseVector3.y + float.Parse(BjbPosWC[1]), mouseVector3.z + float.Parse(BjbPosWC[2]));
        //标记笔随鼠标移动
        biaojibi.transform.localPosition = mouseVector3;

        //按下鼠标左键
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//射线的起点
            RaycastHit rHit;
            if (Physics.Raycast(ray, out rHit))
            {
                string PeopleName = fileHelper.ReadIni(GetType().Name, "PeopleName", "StepConfig");
                //坐着人
                if (rHit.collider.name == PeopleName)
                {
                    hit = rHit;

                    Texture2D bodyMaterial = (Texture2D)hit.transform.GetComponent<MeshRenderer>().materials[1].mainTexture;
                    Vector2 drawPoint = hit.textureCoord;
                    float fx = bodyMaterial.width * drawPoint.x;
                    float fy = bodyMaterial.height * drawPoint.y;

                    Vector2 uvPoint = new Vector2(fx, fy);
                    Vector3 point = hit.point;
                    Vector3 normal = hit.normal;

                    if (baseMaterialColors == null)
                    {
                        //保存初期获取的材质
                        baseMaterialColors = (Color[])bodyMaterial.GetPixels().Clone();
                    }
                    //正在划第3根线，则情况第1根线的记录
                    if (drawingLine > 2)
                    {
                        //清除所有的划线
                        bodyMaterial.SetPixels(baseMaterialColors);

                        line1.Clear();
                        line1.AddRange(line2);
                        line2.Clear();
                        drawingLine = 2;

                        //重新绘制原来的第2根线作为当前最新的第1根线
                        foreach (CrossPoint line1Point in line1)
                        {
                            Vector2 line1UV = line1Point.Punctureuv;
                            if (line1Point.Draw)
                            {
                                bodyMaterial.SetPixels((int)(line1UV.x - drawingLineW / 2), (int)(line1UV.y - drawingLineH / 2), drawingLineW, drawingLineH, lstDrawingLineColor.ToArray());
                            }
                        }
                    }
                    //画第1根线
                    if (drawingLine == 1)
                    {
                        if (line1.Count > 0)
                        {
                            //补充差值点并绘制
                            AddDiffPoint(ref bodyMaterial, ref line1, uvPoint, point, normal);
                        }
                        //第1条线点序列
                        line1.Add(new CrossPoint(uvPoint, point, normal, true));
                    }
                    //画第2根线
                    else
                    {
                        if (line2.Count > 0)
                        {
                            //补充差值点并绘制
                            AddDiffPoint(ref bodyMaterial, ref line2, uvPoint, point, normal);
                        }
                        //第1条线点序列
                        line2.Add(new CrossPoint(uvPoint, point, normal, true));
                    }

                    bodyMaterial.SetPixels((int)(fx - drawingLineW / 2), (int)(fy - drawingLineH / 2), drawingLineW, drawingLineH, lstDrawingLineColor.ToArray());
                    bodyMaterial.Apply();
                }
                else
                {
                    if (VoiceControlScript.Instance.IsVoicePlaying() == false)
                    {
                        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "check_chuanci");
                    }
                }
            }
            else
            {
                if (VoiceControlScript.Instance.IsVoicePlaying() == false)
                {
                    VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "check_chuanci");
                }
            }
        }
        //松开鼠标左键
        else if (Input.GetMouseButtonUp(0))
        {
            drawingLine++;

            if (line1.Count > 0 && line2.Count > 0)
            {
                //交叉点
                CrossPoint cross = getCrossPoints();
                if (cross.Punctureuv != Vector2.zero)
                {
                    Debug.Log(string.Format("UV:{0},Point:{1},Normal:{2}", cross.Punctureuv, cross.PuncturePoint, cross.PunctureNormal));
                    float diff = Vector2.Distance(defaultCrossPoint.Punctureuv, cross.Punctureuv);
                    float diffCopy = 0;
                    if (defaultCopyCrossPoint.Punctureuv != Vector2.zero)
                    {
                        diffCopy = Vector2.Distance(defaultCopyCrossPoint.Punctureuv, cross.Punctureuv);
                    }
                    //交叉点与中心点误差超过80则为标记失败
                    if (diff > 60 || diffCopy > 60)
                    {
                        //偏差语音
                        //语音：当前位置有偏差,在正确区域标记
                        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "mark_a_little_offset");
                    }
                    else if (cross.PunctureNormal == Vector3.zero || cross.PuncturePoint == Vector3.zero)
                    {
                        //偏差语音
                        //语音：当前位置获取失败,请重新标记。
                        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "mark_a_little_offset2");
                    }
                    else
                    {
                        Texture2D bodyMaterial = (Texture2D)hit.transform.GetComponent<MeshRenderer>().materials[1].mainTexture;
                        bodyMaterial.SetPixels(baseMaterialColors);
                        bodyMaterial.SetPixels((int)(cross.Punctureuv.x - drawingLineW / 2), (int)(cross.Punctureuv.y - drawingLineH / 2), drawingLineW, drawingLineH, lstCrossColor.ToArray());
                        bodyMaterial.Apply();

                        //保存交叉点信息
                        Model_PunctureInfo.Instance.m_Punctureuv = ((CrossPoint)cross).Punctureuv;
                        Model_PunctureInfo.Instance.m_PuncturePoint = (Vector3)((CrossPoint)cross).PuncturePoint;
                        Model_PunctureInfo.Instance.m_PunctureNormal = (Vector3)((CrossPoint)cross).PunctureNormal;

                        isFinish = true;
                        //追加评分
                        AddScore(1);

                        //标记笔显示
                        biaojibi.SetActive(false);
                        //标记完成
                        //语音：标记完成，开始消毒
                        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "mark_success", true, Finish);
                    }
                }
                else
                {
                    //交叉语音
                    //语音：请以十字进行标记
                    VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "mark_error_shape");
                }
            }
        }
        base.StepUpdate();
    }

    /// <summary>
    /// 补充差值点并绘制
    /// </summary>
    private void AddDiffPoint(ref Texture2D bodyMaterial, ref List<CrossPoint> line, Vector2 endUV, Vector3 endPoint, Vector3 endNormal)
    {
        CrossPoint lastPoint = line[line.Count - 1];
        Vector2 startUV = lastPoint.Punctureuv;
        Vector3 startPoint = (Vector3)lastPoint.PuncturePoint;
        Vector3 startNormal = (Vector3)lastPoint.PunctureNormal;

        float diff = Vector2.Distance(startUV, endUV);
        for (int i = 1; i < diff; i++)
        {
            float quanzhong = i / diff;

            Vector2 nowUV = Vector2.Lerp(startUV, endUV, quanzhong);
            Vector3 nowPoint = Vector3.Lerp(startPoint, endPoint, quanzhong);
            Vector3 nowNormal = startNormal * (1 - quanzhong) + endNormal * quanzhong;
            bool isDraw = false;

            if (i % (drawingLineW / 2) < 1)
            {
                bodyMaterial.SetPixels((int)(nowUV.x - drawingLineW / 2), (int)(nowUV.y - drawingLineH / 2), drawingLineW, drawingLineH, lstDrawingLineColor.ToArray());
                isDraw = true;
            }

            //前后2点之间的差值点
            line.Add(new CrossPoint(nowUV, nowPoint, nowNormal, isDraw));
        }
    }

    public override void StepEndState()
    {
        base.StepEndState();
        Model_PunctureInfo.Instance.m_Punctureuv = new Vector2(745.459045f, 403.3367f);
        Model_PunctureInfo.Instance.m_PuncturePoint = new Vector3(-0.140237689f, 0.668870866f, 0.127803117f);
        Model_PunctureInfo.Instance.m_Punctureuv = new Vector3(-0.924762547f, -0.0430717543f, 0.3780991f);
    }

    /// <summary>
    /// 获取两条线的交叉序列点
    /// </summary>
    /// <returns></returns>
    private CrossPoint getCrossPoints()
    {
        CrossPoint cross = new CrossPoint(Vector2.zero, Vector3.zero, Vector3.zero);
        float minDiff = float.MaxValue;
        foreach (CrossPoint point1 in line1)
        {
            Vector2 line1UVpoint = point1.Punctureuv;
            foreach (CrossPoint point2 in line2)
            {
                Vector2 line2UVpoint = point2.Punctureuv;

                float diff = Vector2.Distance(line1UVpoint, line2UVpoint);
                if (diff < drawingLineW / 2)
                {
                    if (point1.PuncturePoint != Vector3.zero && point1.PunctureNormal != Vector3.zero)
                    {
                        if (minDiff > diff)
                        {
                            minDiff = diff;

                            cross = point1;
                        }
                    }
                    else if (point2.PuncturePoint != Vector3.zero && point2.PunctureNormal != Vector3.zero)
                    {
                        if (minDiff > diff)
                        {
                            minDiff = diff;

                            cross = point2;
                        }
                    }
                }
            }
        }
        return cross;
    }

    /// <summary>
    /// 步骤结束
    /// </summary>
    public override void StepFinish()
    {
        //标记笔隐藏
        biaojibi.SetActive(false);

        VoiceControlScript.Instance.VoiceStop();
        base.StepFinish();
    }

    /// <summary>
    /// 点击标记穿刺点结束按钮
    /// </summary>
    private void onClickBiaojiOk()
    {

        //坐着人model
        GameObject sitPeople = GameObject.Find("Models").transform.Find("chest_body-new/chest_body").gameObject;
        Texture2D bodyMaterial = (Texture2D)sitPeople.transform.GetComponent<MeshRenderer>().materials[1].mainTexture;
        if (baseMaterialColors != null)
        {
            bodyMaterial.SetPixels(baseMaterialColors);
        }
        bodyMaterial.SetPixels((int)(defaultCrossPoint.Punctureuv.x - drawingLineW / 2), (int)(defaultCrossPoint.Punctureuv.y - drawingLineH / 2), drawingLineW, drawingLineH, lstCrossColor.ToArray());
        bodyMaterial.Apply();

        //保存交叉点信息
        Model_PunctureInfo.Instance.m_Punctureuv = ((CrossPoint)defaultCrossPoint).Punctureuv;
        Model_PunctureInfo.Instance.m_PuncturePoint = (Vector3)((CrossPoint)defaultCrossPoint).PuncturePoint;
        Model_PunctureInfo.Instance.m_PunctureNormal = (Vector3)((CrossPoint)defaultCrossPoint).PunctureNormal;

        //标记完成
        //语音：标记完成，开始消毒
        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "mark_success");

        //当前步骤结束
        State = StepStatus.did;
    }

    private void Finish()
    {
        //当前步骤结束
        State = StepStatus.did;
    }

    /// <summary>
    /// 追加评分
    /// </summary>
    private void AddScore(int scoreKey)
    {
        string[] NoCC = fileHelper.ReadIni(GetType().Name, "NoCC", "StepConfig").Split(',');
        string[] ErroeNoCC = fileHelper.ReadIni(GetType().Name, "ErroeNoCC", "StepConfig").Split(',');
        string[] OkCC = fileHelper.ReadIni(GetType().Name, "OkCC", "StepConfig").Split(',');
        Dictionary<int, string> dicScore = new Dictionary<int, string>() {
             { int.Parse(NoCC[0]), NoCC[1] } //没有标记穿刺点
            ,{ int.Parse(ErroeNoCC[0]), ErroeNoCC[1]  } //穿刺点标记错误
            ,{ int.Parse(OkCC[0]), OkCC[1]  } //穿刺点标记正确
        };
        if (scoreKey == 0)
        {
            foreach (KeyValuePair<int, string> keyValue in dicScore)
            {
                string scoreItem = keyValue.Value;
                //评分是否存在
                if (IsExistCode(scoreItem))
                {
                    //移除评分编码
                    RemoveScoreItem(scoreItem);
                }
            }
            //追加评分编码
            AddScoreItem(dicScore[scoreKey]);
        }
        else
        {
            int recordCnt = 0;
            bool isShouldAdd = true;
            foreach (KeyValuePair<int, string> keyValue in dicScore)
            {
                string scoreItem = keyValue.Value;
                //评分是否存在
                if (IsExistCode(scoreItem))
                {
                    if (recordCnt == 0)
                    {
                        //移除评分编码
                        RemoveScoreItem(scoreItem);
                    }
                    else
                    {
                        isShouldAdd = false;
                        break;
                    }
                }
                recordCnt++;
            }
            if (isShouldAdd)
            {
                //追加评分编码
                AddScoreItem(dicScore[scoreKey]);
            }
        }
    }
}
