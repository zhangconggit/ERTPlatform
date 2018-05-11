using UnityEngine;
using System.Collections;

/// <summary>
/// 旋转角度
/// </summary>
public class RevolveAroundPoint : MonoBehaviour {

    protected float m_degree_2;//旋转的度数
    public GameObject Center;//中心物体
    public GameObject Trigge;//触发物体
    protected Vector3 yaoguanCenter;//摇杆中心点
    Vector3 lastMousePos;//上一次鼠标的位置
    Vector3 currentMousePos;//当前鼠标的位置
    public bool useMe=false;//
    bool isDown;
    public bool is2D=false;
    Vector2 oldScreen;

    public GameObject roRoot;
    public GameObject roc1;
    public GameObject roc2;

    public GameObject inc1;
    public GameObject inc2;
	// Use this for initialization
	void Start () {
        return;
        if(is2D)
            yaoguanCenter = Center.GetComponent<RectTransform>().position;//Camera.main.ScreenToWorldPoint(Center.transform.position);;
        else
            yaoguanCenter = Camera.main.WorldToScreenPoint(Center.transform.position);
        yaoguanCenter = new Vector3(yaoguanCenter.x, yaoguanCenter.y, 0);
        isDown = false;
        oldScreen = new Vector2(Screen.width, Screen.height);
	}
	
	// Update is called once per frame
	void Update () {
        if (!useMe)
            return;
        if (Trigge == null || onMouseRaySelect() == Trigge.name)
        {
            currentMousePos = Input.mousePosition;
            isDown = true;
        }
        if (isDown && Input.GetMouseButton(0))
        {
            if (oldScreen.x != Screen.width || oldScreen.y != Screen.height)
            {
                oldScreen = new Vector2(Screen.width, Screen.height);
                if (is2D)
                    yaoguanCenter = Center.GetComponent<RectTransform>().position;//Camera.main.ScreenToWorldPoint(Center.transform.position);;
                else
                    yaoguanCenter = Camera.main.WorldToScreenPoint(Center.transform.position);
                yaoguanCenter = new Vector3(yaoguanCenter.x, yaoguanCenter.y, 0);
            }
            xuanzhuan();
        }
        else if (isDown)
        {
            isDown = false;
        }
	}
    void xuanzhuan()
    {

        currentMousePos = Input.mousePosition;
        Vector3 dir1 = currentMousePos - yaoguanCenter;
        Vector3 dir2 = lastMousePos - yaoguanCenter;
        Vector2 v2_1 = new Vector2(dir1.x, dir1.y);
        Vector2 v2_2 = new Vector2(dir2.x, dir2.y);
        //float degree=Vector3.Angle(dir1, dir2);
        Vector3 vir = Vector3.Cross(dir1, dir2);
        Debug.Log("dir1:" + dir1 + " dir2:" + dir2 + "vir:" + vir);
        float degree2;
        if (vir.z > 0)
            degree2 = Vector2.Angle(v2_1, v2_2);
        else
            degree2 = Vector2.Angle(v2_1, v2_2) * (-1);
        if (Mathf.Abs(degree2) > 0.5)
        {
            m_degree_2 += degree2;
            lastMousePos = currentMousePos;
        }
    }
    void OnGUI()
    {
        //GUILayout.Label("m_degree_2:" + m_degree_2);
        //GUILayout.Label("m_degree_2:" + yaoguanCenter);
        //GUILayout.Label("m_degree_2:" + Input.mousePosition);
        if(GUILayout.Button("cc") )
            aa();
    }
    /// <summary>
    /// 取得旋转度数
    /// </summary>
    /// <returns>度数</returns>
    public float getDegree()
    {
        return m_degree_2;
    }
    /// <summary>
    /// 重置度数
    /// </summary>
    public void reset()
    {
        m_degree_2 = 0;
    }
    public string onMouseRaySelect()
    {
        string _name = null;
        //在鼠标按下的位置检查
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                _name = hit.transform.name;
            }
        }
        return _name;
    }
    void aa()
    {

        Vector3 a = roc2.transform.position - roc1.transform.position;
        Vector3 b = inc2.transform.position - inc1.transform.position;
        Quaternion t = Quaternion.FromToRotation(a, b);
        roRoot.transform.rotation = t * roRoot.transform.rotation;
        Vector3 dir = inc1.transform.position - roc1.transform.position;
        roRoot.transform.position += dir;
        //Debug.Log(c);
    }
}
