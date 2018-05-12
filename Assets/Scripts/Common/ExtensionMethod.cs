using UnityEngine;
using System.Collections;
using System.Reflection;
using System.Collections.Generic;
using System;
/// <summary>
/// RectTransform的位置旋转与缩放的基础信息
/// </summary>
public class RectTransformBase
{
    #region RectTransform信息记录
    Vector3 _position = new Vector3();
    Quaternion _rotation = new Quaternion();
    Vector3 _scale = new Vector3();
    Vector3 _localPosition = new Vector3();
    Quaternion _localRotation = new Quaternion();
    Vector3 _localScale = new Vector3();

    public Vector3 position
    {
        get
        {
            return _position;
        }

        set
        {
            _position = value;
        }
    }

    public Vector3 localPosition
    {
        get
        {
            return _localPosition;
        }

        set
        {
            _localPosition = value;
        }
    }

    public Quaternion rotation
    {
        get
        {
            return _rotation;
        }

        set
        {
            _rotation = value;
        }
    }

    public Vector3 lossyScale
    {
        get
        {
            return _scale;
        }

        set
        {
            _scale = value;
        }
    }

    public Quaternion localRotation
    {
        get
        {
            return _localRotation;
        }

        set
        {
            _localRotation = value;
        }
    }

    public Vector3 localScale
    {
        get
        {
            return _localScale;
        }

        set
        {
            _localScale = value;
        }
    }
    #endregion
}
/// <summary>
/// 方法扩展
/// </summary>
public static class ExtensionMethod
{
    /// <summary>
    /// 首字母转化为大写
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    public static string ToTitleCase(this string str)
    {
        if (str.Length > 0 && (int)str[0] <= 122 && str[0] >= 97)
        {
            return (char)(str[0] - 32) + str.Substring(1);
        }
        return str;
    }
    /// <summary>
    /// 父类查询
    /// </summary>
    /// <param name="type"></param>
    /// <param name="baseType"></param>
    /// <returns></returns>
    public static bool IsSubClassOf(this System.Type type, System.Type baseType)
    {
        var b = type.BaseType;
        while (b != null)
        {
            if (b.Equals(baseType))
            {
                return true;
            }
            b = b.BaseType;
        }
        return false;
    }
    /// <summary>
    /// 设置相对状态属性
    /// </summary>
    public static void setLocalRectTransForm(this Transform me, RectTransformBase pos)
    {
        if (me == null)
            return;
        me.localPosition= pos.localPosition;
        me.localRotation = pos.localRotation;
        me.localScale = pos.localScale;
    }
    /// <summary>
    /// 设置世界状态属性
    /// </summary>
    /// <param name="me"></param>
    /// <param name="pos"></param>
    public static void setRectTransForm(this Transform me, RectTransformBase pos)
    {
        if (me == null)
            return;
        me.position = pos.position;
        me.rotation = pos.rotation;
        me.localScale =  pos.localScale;
    }
    /// <summary>
    /// 获取本状态属性
    /// </summary>
    public static RectTransformBase getRectTransFormBase(this Transform me)
    {
        if (me == null)
            return null;
        RectTransformBase pos= new RectTransformBase();
        pos.localPosition = me.localPosition;
        pos.localRotation = me.localRotation;
        pos.localScale = me.localScale;
        pos.position = me.position;
        pos.rotation = me.rotation;
        pos.lossyScale = me.lossyScale;
        return pos;
    }
    /// <summary>
    /// 字典连接
    /// </summary>
    /// <param name="me"></param>
    /// <param name="dic"></param>
    /// <returns></returns>
    public static void Connect(this Dictionary<string, float> me, Dictionary<string, float> dic)
    {
        foreach (var item in dic)
        {
            me[item.Key] = item.Value;
        }
    }
    /// <summary>
    /// 排序
    /// </summary>
    /// <param name="me"></param>
    /// <param name="abs">是否升序</param>
    public static void Ranking(this Dictionary<string, float> me, bool abs = true)
    {
        Dictionary<string, float> temp = new Dictionary<string, float>();
        temp.Connect(me);
        me.Clear();
        List<string> keys = new List<string>();
        foreach (var item in temp)
        {
            keys.Add(item.Key);
        }
        MyCommon.RankingMethod(ref keys, abs);
        foreach (var item in keys)
        {
            me.Add(item, temp[item]);
        }
    }
    /// <summary>
    /// 旋转值转换,将自身旋转值转换到目标的±180之间
    /// </summary>
    /// <param name="me">自己</param>
    /// <param name="target">目标旋转值</param>
    /// <returns></returns>
    public static Quaternion Change(this Quaternion me, Quaternion target)
    {
        
        Vector3 meV = me.eulerAngles;
        Vector3 trV = target.eulerAngles;
        if (meV.x - trV.x < -180)
            meV.x = meV.x + 360;
        else if (meV.x - trV.x > 180)
            meV.x = meV.x - 360;

        if (meV.y - trV.y < -180)
            meV.y = meV.y + 360;
        else if (meV.y - trV.y > 180)
            meV.y = meV.y - 360;

        if (meV.z - trV.z < -180)
            meV.z = meV.z + 360;
        else if (meV.z - trV.z > 180)
            meV.z = meV.z - 360;
        me = Quaternion.Euler(meV);
        return me;
    }
    

    /// <summary>
    /// 转为中文数子
    /// 以下属算法来自 http://www.shang11.com  
    /// </summary>
    /// <param name="me"></param>
    /// <param name="capital"></param>
    /// <returns></returns>
    public static string ToStringNumber(this int me, bool capital = false)
    {
        string x = me.ToString();
        //数字转换为中文后的数组 
        string[] P_array_num = new string[] { "零", "壹", "贰", "叁", "肆", "伍", "陆", "柒", "捌", "玖" };
       
        //为数字位数建立一个位数组  
        string[] P_array_digit = new string[] { "", "拾", "佰", "仟" };
        if (capital == false)
        {
            P_array_num = new string[] { "零", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
            P_array_digit= new string[] { "", "十", "百", "千" };
        }
        //为数字单位建立一个单位数组  
        string[] P_array_units = new string[] { "", "万", "亿", "万亿" };
        string P_str_returnValue = ""; //返回值  
        int finger = 0; //字符位置指针  
        int P_int_m = x.Length % 4; //取模  
        int P_int_k = 0;
        if (P_int_m > 0)
            P_int_k = x.Length / 4 + 1;
        else
            P_int_k = x.Length / 4;
        //外层循环,四位一组,每组最后加上单位: ",万亿,",",亿,",",万,"  
        for (int i = P_int_k; i > 0; i--)
        {
            int P_int_L = 4;
            if (i == P_int_k && P_int_m != 0)
                P_int_L = P_int_m;
            //得到一组四位数  
            string four = x.Substring(finger, P_int_L);
            int P_int_l = four.Length;
            //内层循环在该组中的每一位数上循环  
            for (int j = 0; j < P_int_l; j++)
            {
                //处理组中的每一位数加上所在的位  
                int n = System.Convert.ToInt32(four.Substring(j, 1));
                if (n == 0)
                {
                    if (j < P_int_l - 1 && Convert.ToInt32(four.Substring(j + 1, 1)) > 0 && !P_str_returnValue.EndsWith(P_array_num[n]))
                        P_str_returnValue += P_array_num[n];
                }
                else
                {
                    if (!(n == 1 && (P_str_returnValue.EndsWith(P_array_num[0]) | P_str_returnValue.Length == 0) && j == P_int_l - 2))
                        P_str_returnValue += P_array_num[n];
                    P_str_returnValue += P_array_digit[P_int_l - j - 1];
                }
            }
            finger += P_int_L;
            //每组最后加上一个单位:",万,",",亿," 等  
            if (i < P_int_k) //如果不是最高位的一组  
            {
                if (Convert.ToInt32(four) != 0)
                    //如果所有4位不全是0则加上单位",万,",",亿,"等  
                    P_str_returnValue += P_array_units[i - 1];
            }
            else
            {
                //处理最高位的一组,最后必须加上单位  
                P_str_returnValue += P_array_units[i - 1];
            }
        }
        return P_str_returnValue;
    }
    /// <summary>
    /// 字符串转Vector3 ,允许的分隔符 ',' ' '
    /// </summary>
    /// <param name="me"></param>
    /// <returns>失败返回0</returns>
    public static Vector3 ToVector3(this string me,char cr=',')
    {
        string[] list = me.Split(cr);
        Vector3 v3 = Vector3.zero;
        try
        {
            v3.x = float.Parse(list[0]);
        }
        catch
        {
            v3.x = 0;
        }
        try
        {
            v3.y = float.Parse(list[1]);
        }
        catch
        {
            v3.y = 0;
        }
        try
        {
            v3.z = float.Parse(list[2]);
        }
        catch
        {
            v3.z = 0;
        }
        return v3;
    }

    /// <summary>
    /// 字符串转float
    /// </summary>
    /// <param name="me"></param>
    /// <returns>失败返回0</returns>
    public static float ToFloat(this string me)
    {
        float f = 0;
        if(!float.TryParse(me,out f))
        {
            f = 0;
        }
        return f;
    }
    /// <summary>
    /// 基础坐标点屏幕适配转换
    /// </summary>
    /// <returns></returns>
    public static Vector3 ScreenAdapter(this Vector3 me)
    {
        return new Vector3(me.x / 1920 * Screen.width, me.y / 1080 * Screen.height);
    }

    /// <summary>
    /// 屏幕坐标点基础适配转换
    /// </summary>
    /// <returns></returns>
    public static Vector3 BaseAdapter(this Vector3 me)
    {
        return new Vector3(me.x / Screen.width * 1920, me.y / Screen.height *1080 );
    }

    /// <summary>
    /// 取得线性值
    /// </summary>
    /// <returns></returns>
    public static Vector3 GetLinearValue(this List<Vector3> me,float f)
    {
        float _index = me.Count * f;
        if (me.Count > 0 )
        {
            if (_index < 0)
            {
                return me[0];
            }
            else if(_index >= me.Count - 1)
            {
                return me[me.Count - 1];
            }
            else
            {
                return Vector3.Slerp(me[(int)_index], me[(int)_index + 1], _index - (int)_index);
            }
        }
        else
            return Vector3.zero;
    }

    /// <summary>
    /// 固定范围
    /// </summary>
    /// <param name="me"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float Range(this float me,float min,float max)
    {
        if (me > max)
            me = max;
        else if(me < min)
        {
            me = min;
        }
        return me;
    }
    /// <summary>
    /// 固定范围并转换输出值
    /// </summary>
    /// <param name="me"></param>
    /// <param name="min"></param>
    /// <param name="max"></param>
    /// <returns></returns>
    public static float RangeChange(this float me, float min, float max, float cmin, float cmax)
    {
        if (me > max)
            me = max;
        else if (me < min)
        {
            me = min;
        }
        float f = (me - min) / (max - min);//比值
        me = cmin + (cmax - cmin) * f;
        return me;
    }

    /// <summary>
    /// 查找子节点
    /// </summary>
    /// <param name="me"></param>
    /// <param name="name">节点名称</param>
    /// <param name="recursion">是否递归子节点</param>
    /// <returns></returns>
    public static GameObject FindChild(this GameObject me, string name,bool recursion = true)
    {
        return RecursionFindChild(me,name,recursion);
    }
    static GameObject RecursionFindChild(GameObject obj, string name, bool recursion = true)
    {
        GameObject result = null;
        for (int i = 0; i < obj.transform.childCount; i++)
        {
            if (obj.transform.GetChild(i).name == name)
            {
                result = obj.transform.GetChild(i).gameObject;
                break;
            }
            else if (obj.transform.GetChild(i).childCount > 0 && recursion)
            {
                result = RecursionFindChild(obj.transform.GetChild(i).gameObject, name, recursion);
                if(result != null)
                    break;
            }
        }
        return result;
    }
}
