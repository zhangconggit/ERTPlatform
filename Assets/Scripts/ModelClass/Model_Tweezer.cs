using UnityEngine;
using System.Collections;
using CFramework;

public class Model_Tweezer : ModelBase {

    Vector3 m_point = new Vector3();

    Vector3 m_normarl = new Vector3();

    float m_Length = 0.0967967f;
    float x_rotate = 0;
    float y_rotate = 0;
    private GameObject m_mianqiu = null;

    static Model_Tweezer _Instatce = null;
    public new static Model_Tweezer Instance
    {
        get
        {
            if(_Instatce == null)
            {
                _Instatce = new Model_Tweezer();
            }
            return _Instatce;
        }
    }
    /// <Summary>
    /// 构造函数，参数为模型唯一描述，默认初始方向和欧拉角均为（0，0，0）
    /// </Summary>
    public Model_Tweezer() : base("夹棉球的镊子", ModelCtrol.Find("tools/镊子"))
    {
        //初始位置
        IniLocalPosition = new Vector3(-0.1523f, 0.7091f, 0.0743f);
        InitLocalRotation = new Vector3(0f, 0f, -52.2820f);
        m_mianqiu = modelObject.transform.Find("棉球").gameObject;
    }

    /// <Summary>
    /// 构造函数，参数为模型唯一描述
    /// </Summary>
    public void SetTweezerPos(Vector3 point, Vector3 normal)
    {
        m_point = point;
        m_normarl = normal;

        ModelCtrol.Instance.setModelsOnNormalline(modelObject, normal, new Vector3(0, 0, -1), 0);
        
        ResetPos();
    }
    public void RotateX(float f)
    {
        x_rotate += f;
        ResetPos();
    }
    public void RotateY(float f)
    {
        y_rotate += f;
        ResetPos();
    }

    public void ResetPos()
    {
        modelObject.transform.position = m_point + m_normarl * m_Length;
        modelObject.transform.RotateAround(m_point, new Vector3(1, 0, 0), x_rotate);
        modelObject.transform.RotateAround(m_point, new Vector3(0,1, 0), y_rotate);
        //modelObject.transform.rotation.Change(Quaternion.Euler(x_rotate, y_rotate, 0));
        
    }
    public void Insert(float f)
    {
        m_Length -= f;
        ResetPos();
    }
	/// <Summary>
    /// 是否
    /// </Summary>
	public void EnabledTweezel(bool isEnabled)
	{
		  modelObject.SetActive(isEnabled);
          if(isEnabled == true)
          {
            resetLocalModelTran();
          }
	}

    /// <Summary>
    /// 是否
    /// </Summary>
	public void ShowCottonBall(bool isEnabled)
    {

        m_mianqiu.SetActive(isEnabled);
    }
    public Vector3 GetNormorl()
    {
        return new Vector3(0, 0, 1);
    }

    public float GetLength()
    {
        return m_Length;
    }
}
