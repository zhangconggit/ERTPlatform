using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 通用方法 
/// </summary>
public static class MyCommon {
    /// <summary>
    /// 取得一组不超过MaxValue不重复随机数
    /// </summary>
    /// <param name="MaxValue">最大值</param>
    /// <returns>不重复随机数组</returns>
    public static int[] RandomRepeat(int MaxValue)
    {
        int[] re = new int[MaxValue];

        for (int i = 0; i < MaxValue; i++)
        {
            re[i] = i;
        }
        UnityEngine.Random ran = new UnityEngine.Random();
        for (int i = 0; i < MaxValue; )
        {
            int k = UnityEngine.Random.Range(0, MaxValue);
            int tmp = re[k];
            re[k] = re[MaxValue - 1];
            re[MaxValue - 1] = tmp;
            MaxValue--;
        }
        return re;
    }
    /// <summary>
    /// 取得一组最小值值为MinValue不超过MaxValue不重复随机数
    /// </summary>
    /// <param name="MinValue">最小值</param>
    /// <param name="MaxValue">最大值</param>
    /// <returns>不重复随机数组</returns>
    /// <summary>
    public static int[] RandomRepeat(int MinValue, int MaxValue)
    {
        int count = MaxValue - MinValue;
        //参数错误
        if (count <= 0)
            return null;
        int[] re = new int[count];

        for (int i = 0; i < count; i++)
        {
            re[i] = i + MinValue;
        }
        UnityEngine.Random ran = new UnityEngine.Random();
        for (int i = 0; i < count; )
        {
            int k = UnityEngine.Random.Range(0, count);
            int tmp = re[k];
            re[k] = re[count - 1];
            re[count - 1] = tmp; 
            count--;
        }
        return re;
    }
    public enum Vector3Axis
    {
        X,
        Y,
        Z
    }
    /// <summary>
    /// 三维坐标排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="axis">按那个轴排序</param>
    /// <param name="Asc">是否顺序</param>
    /// <returns>结果</returns>
    public static List<Vector3> RankingMethod(ref List<Vector3> list, Vector3Axis axis, bool Asc = true)
    {
        Vector3 temp = Vector3.zero;
        for (int i = 0; i < list.Count; i++)
            for (int j = i + 1; j < list.Count; j++)
            {
                switch (axis)
                {
                    case Vector3Axis.X:
                        if((Asc && (list[i].x > list[j].x)) || (!Asc && (list[i].x < list[j].x)))
                        {
                            temp = list[i];
                            list[i] = list[j];
                            list[j] = temp;
                        }
                        break;
                    case Vector3Axis.Y:
                        if ((Asc && (list[i].y > list[j].y)) || (!Asc && (list[i].y < list[j].y)))
                        {
                            temp = list[i];
                            list[i] = list[j];
                            list[j] = temp;
                        }
                        break;
                    case Vector3Axis.Z:
                        if ((Asc && (list[i].z > list[j].z)) || (!Asc && (list[i].z < list[j].z)))
                        {
                            temp = list[i];
                            list[i] = list[j];
                            list[j] = temp;
                        }
                        break;
                    default:
                        break;
                }
            }
        return list;
    }
    /// <summary>
    /// 二维坐标排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="axis">按那个轴排序</param>
    /// <param name="Asc">是否顺序</param>
    /// <returns>结果</returns>
    public static List<Vector2> RankingMethod(ref List<Vector2> list, Vector3Axis axis, bool Asc = true)
    {
        Vector3 temp = Vector3.zero;
        for (int i = 0; i < list.Count; i++)
            for (int j = i + 1; j < list.Count; j++)
            {
                switch (axis)
                {
                    case Vector3Axis.X:
                        if ((Asc && (list[i].x > list[j].x)) || (!Asc && (list[i].x < list[j].x)))
                        {
                            temp = list[i];
                            list[i] = list[j];
                            list[j] = temp;
                        }
                        break;
                    case Vector3Axis.Y:
                        if ((Asc && (list[i].y > list[j].y)) || (!Asc && (list[i].y < list[j].y)))
                        {
                            temp = list[i];
                            list[i] = list[j];
                            list[j] = temp;
                        }
                        break;
                    default:
                        break;
                }
            }
        return list;
    }
    /// <summary>
    /// 数组排序
    /// </summary>
    /// <param name="list">列表</param>
    /// <param name="axis">按那个轴排序</param>
    /// <param name="Asc">是否顺序</param>
    /// <returns>结果</returns>
    public static List<string> RankingMethod(ref List<string> list, bool Asc = true)
    {
        string temp = "";
        for (int i = 0; i < list.Count; i++)
            for (int j = i + 1; j < list.Count; j++)
            {
                if ((Asc && (list[i].CompareTo(list[j]) > 0)) || (!Asc && (list[i].CompareTo(list[j]) < 0)))
                {
                    temp = list[i];
                    list[i] = list[j];
                    list[j] = temp;
                }
            }
        return list;
    }
}
public class MethodMaker
{

    /// <summary>
    /// 创建对象（当前程序集）
    /// </summary>
    /// <param name="typeName">类型名</param>
    /// <returns>创建的对象，失败返回 null</returns>
    public static object CreateObject(string typeName)
    {
        object obj = null;
        try
        {
            Type objType = Type.GetType(typeName, true);
            obj = Activator.CreateInstance(objType);
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }
        return obj;
    }

    /// <summary>
    /// 创建对象(外部程序集)
    /// </summary>
    /// <param name="path"></param>
    /// <param name="typeName">类型名</param>
    /// <returns>创建的对象，失败返回 null</returns>
    public static object CreateObject(string path, string typeName)
    {
        object obj = null;
        try
        {

            obj = System.Reflection.Assembly.Load(path).CreateInstance(typeName);
        }
        catch (Exception ex)
        {
            Debug.Log(ex);
        }

        return obj;
    }
}
