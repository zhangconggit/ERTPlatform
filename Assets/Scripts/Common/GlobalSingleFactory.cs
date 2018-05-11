using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/// <summary>
/// 单例工厂
/// </summary>
public class GlobalSingleFactory
{
    private static Dictionary<System.Type, object>  objList = new Dictionary<System.Type, object>();
    /// <summary>
    /// 取得实例
    /// </summary>
    /// <typeparam name="T">单例类型</typeparam>
    /// <returns>单例对象</returns>
    public static T GetInstance<T>()
    {
        T ty = default(T);
        if (!objList.Keys.Equals(typeof(T)))
        {
            object obj = Activator.CreateInstance(typeof(T));
            objList[typeof(T)] = obj;
        }
        ty = (T)objList[typeof(T)];
        return ty;
    }
    /// <summary>
    /// 摧毁单例
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static void DestroyInstance<T>()
    {
        objList.Remove(typeof(T));
    }
    /// <summary>
    /// 摧毁所有单例
    /// </summary>
    public static void DestroyInstanceAll()
    {
        objList.Clear();
    }
}
