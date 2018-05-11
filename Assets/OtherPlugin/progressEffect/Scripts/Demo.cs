using UnityEngine;
using System.Collections;

public class Demo : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnGUI()
    {
        GUILayout.BeginArea(new Rect(500, 500, 100, 100),"通过索引控制");
        if (GUILayout.Button("步骤1亮") == true)
        {
            GameObject.Find("Canvas/catalog").GetComponent<progressControl.CProgressControl>().setStepHightLightByIndex(0);
        }

        if (GUILayout.Button("步骤2亮") == true)
        {
            GameObject.Find("Canvas/catalog").GetComponent<progressControl.CProgressControl>().setStepHightLightByIndex(1);
        }

        if (GUILayout.Button("步骤3亮") == true)
        {
            GameObject.Find("Canvas/catalog").GetComponent<progressControl.CProgressControl>().setStepHightLightByIndex(2);
        }
        GUILayout.EndArea();


        GUILayout.BeginArea(new Rect(610, 500, 100, 100));
        if (GUILayout.Button("步骤1暗") == true)
        {
            GameObject.Find("Canvas/catalog").GetComponent<progressControl.CProgressControl>().setStepSourceByIndex(0);
        }

        if (GUILayout.Button("步骤2暗") == true)
        {
            GameObject.Find("Canvas/catalog").GetComponent<progressControl.CProgressControl>().setStepSourceByIndex(1);
        }

        if (GUILayout.Button("步骤3暗") == true)
        {
            GameObject.Find("Canvas/catalog").GetComponent<progressControl.CProgressControl>().setStepSourceByIndex(2);
        }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(500, 610, 100, 100), "通过描述控制");
        if (GUILayout.Button("步骤1亮") == true)
        {
            GameObject.Find("Canvas/catalog").GetComponent<progressControl.CProgressControl>().setStepHightLightByDesc("核对患者");
        }

        if (GUILayout.Button("步骤2亮") == true)
        {
            GameObject.Find("Canvas/catalog").GetComponent<progressControl.CProgressControl>().setStepHightLightByDesc("术前准备");
        }

        if (GUILayout.Button("步骤3亮") == true)
        {
            GameObject.Find("Canvas/catalog").GetComponent<progressControl.CProgressControl>().setStepHightLightByDesc("术后清理");
        }
        GUILayout.EndArea();


        GUILayout.BeginArea(new Rect(610, 610, 100, 100));
        if (GUILayout.Button("步骤1暗") == true)
        {
            GameObject.Find("Canvas/catalog").GetComponent<progressControl.CProgressControl>().setStepSourceByDesc("核对患者");
        }

        if (GUILayout.Button("步骤2暗") == true)
        {
            GameObject.Find("Canvas/catalog").GetComponent<progressControl.CProgressControl>().setStepSourceByDesc("术前准备");
        }

        if (GUILayout.Button("步骤3暗") == true)
        {
            GameObject.Find("Canvas/catalog").GetComponent<progressControl.CProgressControl>().setStepSourceByDesc("术后清理");
        }
        GUILayout.EndArea();

        GUILayout.BeginArea(new Rect(720, 500, 100, 100));
        if (GUILayout.Button("开始闪烁") == true)
        {
            GameObject.Find("Canvas/catalog").GetComponent<progressControl.CProgressControl>().twinklingStepIconByDesc("术前准备");
        }

        if (GUILayout.Button("停止闪烁") == true)
        {
            GameObject.Find("Canvas/catalog").GetComponent<progressControl.CProgressControl>().stopTwinklingStepIcon();
        }
        GUILayout.EndArea();

        //GUILayout.BeginArea(new Rect(720, 610, 100, 100));
        //if (GUILayout.Button("开始闪烁") == true)
        //{
        //    GameObject.Find("Canvas/catalog").GetComponent<progressControl.CProgressControl>().twinklingStepIconByIndex(0);
        //}

        //if (GUILayout.Button("停止闪烁") == true)
        //{
        //    GameObject.Find("Canvas/catalog").GetComponent<progressControl.CProgressControl>().stopTwinklingStepIcon();
        //}
        //GUILayout.EndArea();
    }
}
