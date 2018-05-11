using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/********************************************************************
    created:	2016/07/15
    created:	15:7:2016   15:42
    filename: 	E:\Workspace\Unity Workspace\Gastrolavage\Assets\Scripts\ThingsPick\thingItem.cs
    file path:	E:\Workspace\Unity Workspace\Gastrolavage\Assets\Scripts\ThingsPick
    file base:	thingItem
    file ext:	cs
    author:		XingRui.Chen
	
    purpose:	The Class will give thing  some properties.
*********************************************************************/

public class thingItem : MonoBehaviour {

    public bool isPicked = false;
    
    public bool isShouldPick = true;

    public bool isOutDate = false;

    public bool isPacketBreak = false;
    public bool isAutoLoad = true;

    public string chineseName="中文名";

    public GameObject textHint;
    public GameObject imageHint;
    public GameObject errorImageHint;

    public void AutoRun()
    {
        GameObject obj = GameObject.Find("Canvas").transform.Find("inventoryLeft").Find("showImage").gameObject;
        Transform tra;
        if (obj != null)
        {
            tra = obj.transform.Find(transform.name);
            if (tra != null)
            {
                if (tra.Find("text") != null)
                {
                    if (chineseName == "中文名")
                    {
                        chineseName = tra.Find("text").GetComponent<Text>().text;
                        if(DB.getInstance().getArticle(chineseName,GlobalClass.g_CurrentRecord.ID) == null)
                        {
                            isShouldPick = false;
                        }
                        else
                        {
                            isShouldPick = true;
                        }
                    }
                    if (textHint == null)
                    {
                        textHint = tra.Find("text").gameObject;
                    }
                }
                if (imageHint == null && tra.Find("gou"))
                {
                    imageHint = tra.Find("gou").gameObject;
                    imageHint.SetActive(false);
                }
                if (errorImageHint == null && tra.Find("cha"))
                {
                    tra.Find("cha").gameObject.SetActive(false);
                    if (isShouldPick == false)
                        errorImageHint = tra.Find("cha").gameObject;
                }
            }
        }
    }
	// Use this for initialization
	void Start () {
        if(transform.GetComponent<BoxCollider>()==null)
        {
            gameObject.AddComponent<BoxCollider>();
        }
        if (isAutoLoad)
        {
            AutoRun();
        }
	}
	
	// Update is called once per frame
	void Update () {
        
	}

}
