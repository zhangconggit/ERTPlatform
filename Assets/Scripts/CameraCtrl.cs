using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ModelCtrl : MonoBehaviour {
    /// <summary>
    /// 模型对象
    /// </summary>
    GameObject modelObject;//记录摄像机位置

    /// <summary>
    /// 临时使用变量
    /// </summary>
    bool _bl = false;
    bool _bl2 = false;
    bool isMove;
    bool isFixedMove;
    int startIndex;//进度

    // run param
    Transform runStart;
    Transform runEnd;
    CameraPathBezierAnimator runCameraPath = null;
    float runMoveTime;
    float runAllPathTime;
    float runAllAngleTime;

    public bool IsMove
    {
        get { return isMove; }
        set { isMove = value; }
    }
    private string current = "";
    /// <summary>
    /// 当前正在执行的步骤
    /// </summary>
    public string CurrentStep { get { return current; } }

	void Start () {
        isMove = true;
	}
	
	// Update is called once per frame
	void Update () {
        if (GlobalClass.operatorIsEnd)
            return;
        if (!GlobalClass.stIsLoading)
            SetStepBeforeAnimation();
	}
    public void StopCameraMove()
    {
        StopCoroutine("CamareMove");
        if(runCameraPath != null)
        {
            runCameraPath.Stop();
            runCameraPath = null;
        }
        current = "";
        StepManager.Instance.WaitCameraMove = false;
    }
    public void SetCameraWaitPostion(string i)
    {
        
    }
    /// <summary>
    /// 设置摄像机的动画编号
    /// </summary>
    /// <param name="i">步骤</param>
    public void SetStats(string i)
    {
       
    }
    /// <summary>
    /// 相機動畫實現
    /// </summary>
    public void SetStepBeforeAnimation()
    {
        
    }
    /// <summary>
    /// 設置跳到固定鏡頭
    /// </summary>
    /// <param name="fixedPoint">固定位置名稱</param>
    public void SetCameraPostion(string fixedPoint)
    {
        if (GlobalClass.operatorIsEnd)
            return;
        Transform start = ObjectManager.Instance.GetObject("FixedPoint", "CameraPath").transform.FindChild(fixedPoint.ToString()); ;
        if (fixedPoint != "")
        {
            GameObject.Find("Main Camera").transform.position = start.position;
            GameObject.Find("Main Camera").transform.rotation = start.rotation;
        }
    }
    /// <summary>
    /// 點到點移動
    /// </summary>
    /// <param name="startPoint">起始點名稱</param>
    /// <param name="endPoint">結束點名稱</param>
    public void RunFixedMove(string startPoint, string endPoint)
    {
        if (GlobalClass.operatorIsEnd)
            return;
        // StepClass.Instance.WaitCameraMove = true;
        StopCoroutine("CamareMove");
        ObjectManager.Instance.GetObject("FixedMove", "CameraPath").GetComponent<CameraPathBezierAnimator>().Stop();
        StepManager.Instance.WaitCameraMove = true;
        Transform start = ObjectManager.Instance.GetObject("FixedPoint", "CameraPath").transform.FindChild(startPoint.ToString());
        if (startPoint == "")
        {
            start = GameObject.Find("Main Camera").transform;
        }
        //Debug.Log("endPoint=" + endPoint.ToString());
        Transform end = ObjectManager.Instance.GetObject("FixedPoint", "CameraPath").transform.FindChild(endPoint.ToString());
        if (endPoint == "")
        {
            end = GameObject.Find("Main Camera").transform;
        }
        float pathTime = Vector3.Distance(start.position, end.position) / 0.6f;
        float angle = Quaternion.Angle(start.rotation, end.rotation);
        //Debug.Log("angle=" + angle);
        float angleTime = angle / 60;
        //Debug.Log("pathTime=" + pathTime);
        runStart = start;
        runEnd = end;
        runMoveTime = 0;
        runAllPathTime = pathTime;
        runAllAngleTime = angleTime;
        StartCoroutine("CamareMove");
        //StopCoroutine(CamareMove(start, end, 0, pathTime, angleTime));
        //StartCoroutine(CamareMove(start, end, 0, pathTime, angleTime));
    }
    /// <summary>
    /// 點到點移動方法
    /// </summary>
    /// <param name="start">開始對象</param>
    /// <param name="end">結束對象</param>
    /// <param name="moveTime">當前時間</param>
    /// <param name="allPathTime">路徑總時間</param>
    /// <param name="allAngleTime">角度總時間</param>
    /// <returns></returns>
    IEnumerator CamareMove()
    {

        yield return new WaitForEndOfFrame();
        runMoveTime += Time.deltaTime;
        float t = runMoveTime / runAllPathTime;
        float t2 = runMoveTime / runAllAngleTime;
        
        t = t > 1 ? 1 : t;
        t2 = t2 > 1 ? 1 : t2;
        Quaternion newQua = Quaternion.Lerp(runStart.rotation, runEnd.rotation, t2);
        Vector3 pos = Vector3.Lerp(runStart.position, runEnd.position, t);
        if(t != 1 && Vector3.Distance(pos, runEnd.position) < 0.05f)
        {
            runAllPathTime = runMoveTime;
            //moveTime = allPathTime;
        }
        GameObject.Find("Main Camera").transform.position = pos;
        GameObject.Find("Main Camera").transform.rotation = newQua;
        if (t != 1 || t2 != 1)
        {
            StartCoroutine("CamareMove");
            //StartCoroutine(CamareMove(start, end, moveTime, allPathTime, allAngleTime));
        }
        else
            StepManager.Instance.WaitCameraMove = false;
    }
    IEnumerator OpenDoorZhiliaoshi(float dor)
    {
        if (dor > 0)
        {
            //Debug.Log("update = " + ObjectManager.Instance.GetObject("men2", "changjing").transform.localRotation.eulerAngles);
            ObjectManager.Instance.GetObject("men2", "changjing").transform.localRotation = Quaternion.Euler(-90, dor, 0);
            yield return 0;
            StartCoroutine("OpenDoorZhiliaoshi", dor - Time.deltaTime * 45);
        }
        else
            ObjectManager.Instance.GetObject("men2", "changjing").transform.localRotation = Quaternion.Euler(-90, 0, 0);
    }
    IEnumerator OpenDoorBingfang(float dor)
    {
        if (dor > -180)
        {
            ObjectManager.Instance.GetObject("men1", "changjing").transform.localRotation = Quaternion.Euler(0, dor, 0);
            yield return 0;
            StartCoroutine("OpenDoorBingfang", dor - Time.deltaTime * 45);
        }
        else
            ObjectManager.Instance.GetObject("men1", "changjing").transform.localRotation = Quaternion.Euler(0, -180, 0);
    }
}
