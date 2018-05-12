using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LineRenderManager : MonoBehaviour {
    #region 注册单例
    private LineRenderManager()
    {

    }
    static LineRenderManager _instance = null;
    public static LineRenderManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<LineRenderManager>();
                if (FindObjectsOfType<LineRenderManager>().Length > 1)
                {
                    return _instance;
                }
                if (_instance == null)
                {
                    string instanceName = "LineRenderManager";
                    GameObject instanceGO = null;

                    if (GameObject.Find(instanceName) == null)
                    {
                        instanceGO = new GameObject();//new GameObject(instanceName);
                        instanceGO.name = instanceName;
                        instanceGO.AddComponent<LineRenderManager>();
                        instanceGO.AddComponent<LineRenderer>();
                        Camera cam = instanceGO.AddComponent<Camera>();
                        cam.cullingMask = 32;
                        cam.clearFlags = CameraClearFlags.Depth;
                    }
                    else
                        instanceGO = GameObject.Find(instanceName);
                    instanceGO.layer = 5;
                    instanceGO.transform.position = new Vector3(35.74f, -1.3f, 0);
                    _instance = instanceGO.GetComponent<LineRenderManager>();
                    _instance.m_camera = instanceGO.GetComponent<Camera>();
                    _instance.m_canvas = FindObjectOfType<Canvas>();
                    _instance.line = instanceGO.GetComponent<LineRenderer>();
                    _instance.line.SetVertexCount(0);
                    _instance.line.SetColors(Color.red, Color.blue);
                    _instance.line.material = new Material(Shader.Find("Particles/Additive"));
                    
                    //_instance.g_camera = new GameObject();
                    //_instance.g_camera.name = "line Camera";
                    //_instance.g_camera.AddComponent<Camera>();
                    //_instance.m_camera = _instance.g_camera.GetComponent<Camera>();
                    //_instance.m_camera.clearFlags = CameraClearFlags.Depth;

                }
                else
                {
                    Debug.LogWarning("Already exist: " + _instance.name);
                }
            }
            return _instance;
        }
    }
    #endregion
    //GameObject g_camera;
    Canvas m_canvas;
    public Camera m_camera;
    LineRenderer line;//划线类;
    void Awake()
    {
        //line = GetComponent<LineRenderer>();
        
    }
    // Use this for initialization
    void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void SetLineColor(Color color)
    {
        line.SetColors(color, color);
        _instance.line.material.SetColor("_TintColor", color);
    }
    public void StartRender()
    {
        //设置起始和结束的颜色  
        //line.SetColors(Color.red, Color.blue);
        //设置起始和结束的宽度  
        line.SetWidth(0.10f, 0.10f);
        m_camera.enabled = true;
        //startRender = true;
        m_canvas.renderMode = RenderMode.ScreenSpaceCamera;
        m_canvas.worldCamera = m_camera;
    }
    public void EndRender()
    {
        m_camera.enabled = false;
        //startRender = false;
        m_canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        //glRenderList.Clear();
    }
    public void Draw(List<Vector3> _points)//Draw方法;
    {
        //line.material = new Material(Shader.Find("Unlit/Transparent Colored"));
        line.SetVertexCount(_points.Count);//设置点数量;
        for (int i = 0; i < _points.Count; i++)
        {
            line.SetPosition(i, _points[i]);//每个赋值;
        }
    }
}
