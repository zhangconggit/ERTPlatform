using CFramework;
using System.Collections.Generic;
using UnityEngine;
public class LoadTextAsset
{
    string alltext = "";
    string[] textList;
    int index = -1;
    public LoadTextAsset(string path)
    {
        TextAsset text = UIRoot.Instance.UIResources[path] as TextAsset;
        if(text != null)
        {
            alltext = text.text;
            textList = text.text.Split('\n');
        }
        else
        {
            Debug.Log("文件" + path + "不存在");
        }
    }
    public LoadTextAsset(string path,bool local)
    {
        TextAsset text;
        if (local)
            text = Resources.Load<TextAsset>(path);
        else
            text = UIRoot.Instance.UIResources[path] as TextAsset;
        if (text != null)
        {
            alltext = text.text;
            textList = text.text.Split('\n');
        }
        else
        {
            Debug.Log("文件" + path + "不存在");
        }
    }
    public void ReLoad(string path)
    {
        TextAsset text = UIRoot.Instance.UIResources[path] as TextAsset; 
        alltext = text.text;
    }
    /// <summary>
    /// 读一行
    /// </summary>
    /// <returns></returns>
    public bool ReadLine()
    {
        index++;
        if (index < textList.Length)
        {
            return true;
            
        }
        else
        {
            return false;
        }
    }
    /// <summary>
    /// 取得一行的文字
    /// </summary>
    /// <returns></returns>
    public string GetLine()
    {
        return textList[index];
    }
    /// <summary>
    /// 取得文本
    /// </summary>
    /// <returns></returns>
    public string GetText()
    {
        return alltext;
    }
}
