using UnityEngine;
using System.Collections;

public class CoordinateTransformation : MonoBehaviour
{

    public Transform CoordO;
    public Transform CoordY;
    public Transform CoordZ;
    public Transform CoordX;
    private Vector3 OX;
    private Vector3 OY;
    private Vector3 OZ;


    public GameObject target;
	// Use this for initialization
	void Start () {
        init();
	}
	
	// Update is called once per frame
	void Update () {
	    if(Input.GetKey(KeyCode.DownArrow))
        {
            //text.transform.position = new Vector3(text.transform.position.x)
        }
	}
    /// <summary>
    /// 初始化
    /// </summary>
    void init()
    {
        OX = CoordX.position - CoordO.position;
        OX.Normalize();
        OY = CoordY.position - CoordO.position;
        OY.Normalize();
        OZ = CoordZ.position - CoordO.position;
        OZ.Normalize();
    }
    /// <summary>
    /// 转换为新坐标
    /// </summary>
    /// <param name="vector">世界坐标</param>
    /// <returns>新坐标</returns>
    Vector3 newVector(Vector3 vector)
    {
        Vector3 dir=vector - CoordO.position;
        Vector3 vx = Vector3.Project(dir, OX);
        Vector3 vy = Vector3.Project(dir, OY);
        Vector3 vz = Vector3.Project(dir, OZ);

        Vector3 newV3 = new Vector3(Vector3.Project(dir, OX).magnitude,Vector3.Project(dir, OY).magnitude,Vector3.Project(dir, OZ).magnitude);
        return newV3;
    }

    void OnGUI()
    {
        GUI.Label(new Rect(200, 200, 200, 200), "newVector:" + newVector(target.transform.position));
    }

}
