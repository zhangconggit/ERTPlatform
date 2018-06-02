using UnityEngine;
/// <summary>
/// 穿刺信息类
/// </summary>
public class Model_PunctureInfo
{
    #region 单例
    static Model_PunctureInfo _instance = null;
    public static Model_PunctureInfo Instance
    {
        get
        {
            if(_instance == null)
            {
                _instance = new Model_PunctureInfo();
            }
            return _instance;
        }
    }
    private Model_PunctureInfo() { }
    #endregion
    /// <summary>
    /// 穿刺点世界坐标
    /// </summary>
    public Vector3 m_PuncturePoint = new Vector3(-0.140237689f, 0.668870866f, 0.127803117f);

    /// <summary>
    /// 穿刺点法线
    /// </summary>
    public Vector3 m_PunctureNormal = new Vector3(-0.924762547f, -0.0430717543f, 0.3780991f);

    /// <summary>
    /// 穿刺点UV
    /// </summary>
    public Vector2 m_Punctureuv = new Vector2(745.459045f, 403.3367f);
}