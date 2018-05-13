/// <summary>
///  ini文件读取类
///  作者：Cyrus.Chen
/// </summary>
/// 
using UnityEngine;
using System.Collections;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System;
/// <summary>
/// Ini unit.
/// </summary>
public class iniUnit
{
	/// <summary>
	/// The section.
	/// </summary>
	public string section ;

	/// <summary>
	/// The pairs list.
	/// </summary>
	public List<Dictionary<string,string>> pairsList = new List<Dictionary<string, string>>() ;
}

public class fileHelper {

	/// <summary>
	/// The ini data.
	/// </summary>
	//static List<iniUnit> iniData = new List<iniUnit>();

    static Dictionary<string, List<iniUnit>> dataCache = new Dictionary<string, List<iniUnit>>();

	/// <summary>
	/// The index.
	/// </summary>
	//static int index = -1;


	static List<iniUnit> read(string path)
	{
      
        List<iniUnit> tmpUnit = new List<iniUnit>();
        if (dataCache.ContainsKey(path))
        {

            tmpUnit = dataCache[path];
        }
        else
        {
            //index = -1;
            //iniData.Clear();
            List<string> allLines = new List<string>();
            
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                if(line.IndexOf(';')>=0)
                {
                    line = line.Remove(line.IndexOf(';'));
                }
                if(line !=string.Empty)
                    allLines.Add(line.ToString());
            }
            sr.Close();

            tmpUnit = paraIni(allLines);
            dataCache.Add(path, tmpUnit);
        }
        return tmpUnit;
	}

    //void paraStringToLine(string str)
    //{
    //    List<string> allLines = new List<string>();

    //    string line = "";
    //    for(int i=0;i<str.Length;i++)
    //    {
    //        char str1 = str[i];
    //        char str2 = str[i + 1];
    //        if (str[i] == '\r' && str[i + 1] == '\n')
    //        {
    //            allLines.Add(line);
    //            i = i + 1;
    //            line = "";
    //        }
    //        else
    //        {
    //            if (str[i] == '\r' || str[i] == '\n')
    //                continue;
    //            line += str[i];
    //        }

    //    }

    //}

    static List<iniUnit> paraIni(List<string> allLines )
	{
        int index = -1;
        List<iniUnit> iniData = new List<iniUnit>();

        foreach (string str in allLines)
		{
			if(str.Length<=0)
				continue ;
			if(str.Contains("[") && str.Contains("]"))
			{
				++index;
				iniUnit iu = new iniUnit();
				iu.section = str.Substring(1,str.Length-2);
				iniData.Add(iu);
			}
			else
			{
				int indexEqual = str.IndexOf('=');
				Dictionary<string,string> temp = new Dictionary<string, string>();
				string key = str.Substring(0,indexEqual);
				string value = str.Substring(indexEqual+1,str.Length-indexEqual-1);
				temp.Add(key,value);
				iniData[index].pairsList.Add(temp);
			}
		}
        return iniData;
	}

 //   void debugAllLines()
	//{
	//	foreach(iniUnit iuu in iniData)
	//	{

	//		for(int i=0;i<iuu.pairsList.Count;i++)
	//		{
	//			foreach(KeyValuePair<string,string> pair in iuu.pairsList[i])
	//			{
	//				Debug.Log("key="+pair.Key);
	//				Debug.Log("value="+pair.Value);
	//			}
	//		}
	//	}
	//}

        /// <summary>
        /// 设置配置文件
        /// </summary>
        /// <param name="path"></param>
    public static void addConfigFile(string path)
    {
        if (!dataCache.ContainsKey(path))
        {
            //index = -1;
            //iniData.Clear();
            List<string> allLines = new List<string>();
            StreamReader sr = new StreamReader(path, Encoding.Default);
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                allLines.Add(line.ToString());
            }
            sr.Close();
            dataCache.Add(path, paraIni(allLines));
        }
    }

    public static void setConfigText(string  path,string[] s)
    {
        List<iniUnit> tmpUnit = new List<iniUnit>();
        List<string> allLines = new List<string>();
        foreach(string t in s)
        {
            string line = t;
            if (line.IndexOf(';') >= 0)
            {
                line = line.Remove(line.IndexOf(';'));
            }
            if (line.IndexOf('\r') >= 0)
            {
                line = line.Replace("\r","");
            }
            if (line != string.Empty)
                allLines.Add(line);
        }
        tmpUnit = paraIni(allLines);
        if(!dataCache.ContainsKey(path))
            dataCache.Add(path, tmpUnit);
    }

	/// <summary>
	/// Reads the ini.
	/// </summary>
	/// <returns>The ini.</returns>
	/// <param name="section">Section.</param>
	/// <param name="key">Key.</param>
	/// <param name="path">Path.</param>
	public static string ReadIni(string section,string key,string path)
    {
        List<iniUnit> iniData = read(path);
		iniUnit  findSection = iniData.Find(data=>
			{
				if(data.section == section)
					return true;
				else
					return false;
			}) ;
        
		if(findSection!=null)
		{
			for(int i=0;i<findSection.pairsList.Count;i++)
			{
				foreach(KeyValuePair<string,string> pair in findSection.pairsList[i])
				{
					if(pair.Key == key)
						return pair.Value;
				}
			}
			return "";
		}
		else
        {
            return "";

        }
        
	}

    /// <summary>
    /// 写INI文件，但webplayer模式下不支持
    /// </summary>
    /// <param name="section"></param>
    /// <param name="key"></param>
    /// <param name="value"></param>
    /// <param name="path"></param>
    public static void writeIni(string section,string key,string value,string path)
    {
#if UNITY_STANDALONE_WIN
        List<iniUnit> iniData = read(path);
        string[] ary = File.ReadAllLines(path);
        iniUnit findSection = iniData.Find(data =>
        {
            if (data.section == section)
                return true;
            else
                return false;
        });
        if (findSection != null)
        {
            for (int i = 0; i < findSection.pairsList.Count; i++)
            {
              if(findSection.pairsList[i].ContainsKey(key))
              {
                  findSection.pairsList[i][key] = value;
                  for(int j=0;j<ary.Length;j++)
                    {
                        if(ary[j].Contains("["+section+"]"))
                        {
                            for(int k=j+1; k<ary.Length;k++)
                            {
                                if(ary[k].Contains("[") || ary[k].Contains("]"))
                                {
                                    break;
                                }
                                else
                                {
                                    if (ary[k].Contains(key + "="))
                                    {
                                        ary[k] = key + "=" + value;
                                        break;
                                    }
                                }
                            }
                        }
                       
                    }
              }
            }
        }
        
        File.WriteAllLines(path,ary);
#endif

    }

    static void write(string path)
    {
        List<iniUnit> iniData = read(path);
        using (FileStream fs = File.Open(path,FileMode.OpenOrCreate))
        {
            //根据上面创建的文件流创建写数据流
            StreamWriter w = new StreamWriter(fs);
            for(int i=0;i<iniData.Count;i++)
            {
                w.WriteLine(iniData[i].section);
                for (int j = 0; j < iniData[i].pairsList.Count; j++)
                {
                    foreach (KeyValuePair<string, string> pair in iniData[i].pairsList[j])
                    {
                        w.WriteLine(pair.Key + "=" + pair.Value);
                    }
                }
            }
            w.Close();
            fs.Close();
           
        }
        
    }
  
    /// <summary>
    /// check is exist section
    /// </summary>
    /// <param name="section"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static bool isExistSection(string section, string path)
    {
        List<iniUnit> iniData = read(path);
        iniUnit findSection = iniData.Find(data =>
        {
            if (data.section == section)
                return true;
            else
                return false;
        });
        if (findSection != null)
        {
            return true;
        }
        else
            return false;
    }

    /// <summary>
    /// 获取section下面的所有键值对
    /// </summary>
    /// <param name="section"></param>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Dictionary<string, string> getAllPair(string section, string path)
    {
        Dictionary<string, string> returnValue = new Dictionary<string, string>();
        List<iniUnit> iniData = read(path);
        iniUnit findSection = iniData.Find(data =>
        {
            if (data.section == section)
                return true;
            else
                return false;
        });
        if (findSection != null)
        {
            for (int i = 0; i < findSection.pairsList.Count; i++)
            {
                foreach (KeyValuePair<string, string> pair in findSection.pairsList[i])
                {
                    returnValue.Add(pair.Key, pair.Value);
                }
            }
            
        }
        return returnValue;
    }

}
