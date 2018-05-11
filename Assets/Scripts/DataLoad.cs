using CFramework;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class DataLoad : CMonoSingleton<DataLoad>
{
    ParamDelegate<List<StepInfo>> LoadProjectCallback;
    public void LoadProject(string projectName, ParamDelegate<List<StepInfo>> callback)
    {
        LoadProjectCallback = callback;
        StartCoroutine("LoadProjectInfo", projectName);
    }
    IEnumerator LoadProjectInfo(string projectName)
    {
        yield return 0;
#if UNITY_EDITOR0
        string xmlConnent = System.IO.File.ReadAllText(Application.dataPath + "/StreamingAssets/project/" + projectName + ".xml");
#elif UNITY_WEBPLAYER
        string xmlConnent = string.Empty;
        WWW w = new WWW(Application.dataPath + "/StreamingAssets/project/" + projectName + ".xml");
        while (!w.isDone)
        {
            yield return 0;
        }
        xmlConnent = w.text;
#endif
        List<StepInfo> stepList = new List<StepInfo>();
        ReadProjectData(xmlConnent, stepList);
        LoadProjectCallback.Invoke(stepList);
    }
    public void ReadProjectData(string xml,List<StepInfo> stepList)
    {

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);
        var root = doc.SelectSingleNode("//main");
        if (root != null)
        {
            var steps = root.SelectNodes("step");


            foreach (XmlNode node in steps)
            {

                if (node.Attributes["name"] != null)  //获取语言提示信息节点  
                {
                    StepInfo step = new StepInfo();
                    step.stepName = node.Attributes["name"].Value;
                    step.isAutoGo = node.SelectSingleNode("auto").Value == "1";
                    step.navigation = node.SelectSingleNode("navigation").Value;

                    float.TryParse(node.SelectSingleNode("intotime").Value, out step.intoTime);

                    step.mustDoStep = new List<string>();
                    foreach (XmlNode item in node.SelectNodes("mustdo"))
                    {
                        step.mustDoStep.Add(item.Value);
                    }

                    step.cannotDoStep = new List<string>();
                    foreach (XmlNode item in node.SelectNodes("cantdo"))
                    {
                        step.cannotDoStep.Add(item.Value);
                    }
                    stepList.Add(step);
                }
            }
        }
    }
}