using UnityEngine;
using System.Collections;
using CFramework;
using System.Collections.Generic;

/// <summary>
/// 选择场景
/// </summary>
public class SelectScene : StepBase
{
    List<SceneInfo> infoList;
    public SelectScene()
    {
        UPlane back = CreateUI<UPlane>();
        back.LoadImage("abdopic");
        
        AnalysisData();
        float posY = -infoList.Count *100;
        float diff = 50;
        for (int i = 0; i < infoList.Count; i++)
        {
            USceneItem item = CreateUI<USceneItem>();
            string title = "场景" + (i + 1).ToStringNumber();
            item.SetContext(title, infoList[i].context, infoList[i].demand);

            item.Position = new Vector2(0, posY + diff);
            posY += item.Size.y+ diff;
            SceneInfo info = infoList[i];
            item.OnClick.AddListener(() => { InsertScene(info); });
        }
        
    }
    public void InsertScene(SceneInfo info)
    {
        CGlobal.currentSceneInfo = info;
        State = StepStatus.did;
    }
    public override void CameraMoveFinished()
    {
        base.CameraMoveFinished();
    }
    public override void EnterStep(float cameraTime = 1)
    {
        base.EnterStep(cameraTime);
    }
    public override void StepFinish()
    {
        base.StepFinish();
    }

    void AnalysisData()
    {
        infoList = new List<SceneInfo>();
        Dictionary<string,string> dic = fileHelper.getAllPair(GetType().Name, "StepConfig");
        foreach (var item in dic)
        {
            SceneInfo info = AnalysisOne(item.Value);
            if (info != null)
                infoList.Add(info);
        }
    }
    SceneInfo AnalysisOne(string str)
    {
        SceneInfo info = new SceneInfo();
        
        int startIndex = str.IndexOf('{');
        int endIndex = -1;
        if(startIndex >=0)
            endIndex = str.IndexOf('}',startIndex);
        while (startIndex >=0 && endIndex >= 0)
        {
            string onedate = str.Substring(startIndex + 1, endIndex - startIndex -1);
            if(onedate.IndexOf(':')>0 || onedate.IndexOf('：') > 0)
            {
                string[] keyvalue=new string[2];
                if(onedate.IndexOf(':') > 0)
                    keyvalue = onedate.Split(':');
                else if(onedate.IndexOf('：') > 0)
                    keyvalue = onedate.Split('：');
                switch (keyvalue[0])
                {
                    case "姓名":
                        info.name = keyvalue[1];
                        break;
                    case "性别":
                        info.sex = (keyvalue[1]=="男"?PeopleSex.男: PeopleSex.女);
                        break;
                    case "描述":
                        info.context = keyvalue[1];
                        break;
                    case "要求":
                        info.demand = keyvalue[1];
                        break;
                    case "年龄":
                        info.age = keyvalue[1];
                        break;
                    case "医嘱":
                        info.advice = keyvalue[1];
                        break;
                    case "床号":
                        info.bedNumber = keyvalue[1];
                        break;
                    default:
                        break;
                }
            }
            str = str.Substring(endIndex);
            startIndex = str.IndexOf('{');
            if(startIndex >=0)
                endIndex = str.IndexOf('}', startIndex);
        }
        if (info.context == "" || info.demand == "")
            return null;
        else
            return info;
    }
}
