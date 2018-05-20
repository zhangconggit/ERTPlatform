using UnityEngine;
using System.Collections;
using CFramework;

public class Model_Tweezer : ModelBase {

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
    public Model_Tweezer() : base("夹棉球的镊子", ModelCtrol.Find("镊子"))
    {
        //初始位置
        IniLocalPosition = new Vector3(-0.1523f, 0.7091f, 0.0743f);
        InitLocalRotation = new Vector3(0f, 0f, -52.2820f);
        m_mianqiu = modelObject.transform.Find("棉球").gameObject;
    }

    /// <Summary>
    /// 构造函数，参数为模型唯一描述
    /// </Summary>
    public void SetTweezerPos(Vector3 pIniLocalPos,Vector3 pIniLocalRot)
    {
         IniLocalPosition = pIniLocalPos;
         InitLocalRotation  = pIniLocalRot;
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

}
