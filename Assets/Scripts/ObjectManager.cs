using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections.Generic;

[System.Serializable]
public class ObjectGroup
{
    public string GroupName;
    public GameObject GroupRoot;
    public bool isRecursive;
    [System.NonSerialized]
    public List<GameObject> ObjectList;
}
/// <summary>
/// 对象管理
/// </summary>
public class ObjectManager : MonoBehaviour{

    public static ObjectManager Instance = null;
    void Awake()
    {
        Instance = this;
        foreach (ObjectGroup item in ObjectGroups)
        {
            item.ObjectList = new List<GameObject>();
            item.ObjectList.Add(item.GroupRoot);
            LoadObject(ref item.ObjectList, item.GroupRoot, item.isRecursive);
        }
    }
    /// <summary>
    /// 递归加载
    /// </summary>
    /// <param name="ObjectList">获取的对象加载到该列表中</param>
    /// <param name="objectRoot">父节点</param>
    /// <param name="isRecursive">是否递归</param>
    void LoadObject(ref List<GameObject> ObjectList, GameObject objectRoot,bool isRecursive)
    {
        for (int i = 0; i < objectRoot.transform.childCount; i++)
        {
            ObjectList.Add(objectRoot.transform.GetChild(i).gameObject);
            if (isRecursive)
                LoadObject(ref ObjectList, objectRoot.transform.GetChild(i).gameObject, isRecursive);
        }
    }
    /// <summary>
    /// 对象列表
    /// </summary>
    public List<ObjectGroup> ObjectGroups;

    void Update()
    {
        int ti = GlobalClass.getOperatorTime();
        string text = ((ti / 3600) > 9 ? "" : "0") + ti / 3600 + ":" + (((ti % 3600) / 60) > 9 ? "" : "0") + (ti % 3600) / 60 + ":" + ((ti % 60) > 9 ? "" : "0") + ti % 60;
        GameObject.Find("Canvas").transform.Find("public/catalog/time").Find("Text").GetComponent<Text>().text = text;
    }
    /// <summary>
    /// 查找指定名称的对象 返回找到的第一个对象
    /// </summary>
    /// <param name="objectName"></param>
    /// <param name="groupName"></param>
    /// <returns></returns>
    public GameObject GetObject(string objectName,string groupName = "")
    {
        foreach (ObjectGroup item in ObjectGroups)
        {
            if(groupName == "" || item.GroupName == groupName)
            {
                if(item.ObjectList != null)
                {
                    GameObject obj = item.ObjectList.Find(name => {
                        if (name != null && name.name == objectName)
                            return true;
                        else
                            return false;
                    });
                    if (obj != null)
                    {
                        return obj;
                    }
                }
                
            }
        }
        return null;
    }
    /// <summary>
    /// 查找指定组名称的对象列表
    /// </summary>
    /// <param name="groupName"></param>
    /// <returns></returns>
    public List<GameObject> GetObjectsFromGroup(string groupName)
    {
        ObjectGroup result = ObjectGroups.Find(name=>
        {
            if (name.GroupName == groupName)
                return true;
            else
                return false;
        });
        if (result != null)
            return result.ObjectList;
        else
            return new List<GameObject>();
    }
    /// <summary>
    /// 终止所有协程
    /// </summary>
    public void StopAllCoroutine()
    {
        StopAllCoroutines();//   Coroutine
    }
    /// <summary>
    /// 终止指定协程
    /// </summary>
    public void StopOneCoroutine(string name)
    {
        StopCoroutine(name);
    }
}

