/********************************************************************
	created:	2016/09/22
	created:	22:9:2016   15:07
	filename: 	E:\Workspace\Unity Workspace\GY\TestCloth\Assets\progressEffect\Scripts\progressControl.cs
	file path:	E:\Workspace\Unity Workspace\GY\TestCloth\Assets\progressEffect\Scripts
	file base:	progressControl
	file ext:	cs
	author:		Cyrus.Chen
	
	purpose:	Icon effect
*********************************************************************/

using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class myProgressControl : MonoBehaviour {

    ////////额外增加变量//////////////////////
    public Sprite arrowSourceSprite;
    public Sprite arrowHightLightSprite;

    public Transform catalogButton;
    bool isReverse = false;
    ///////////////////////////////////////
    Coroutine sparkCoroutine;
	
    // Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	   
	}

    //////////////////////////// 额外增加接口 //////////////////////////////////
    public void spreadOrShrinkageCatalog_bt()
    {
        Image icon = catalogButton.Find("icon").GetComponent<Image>();
        //TweenPosition tp = GameObject.Find("Canvas/public/catalog").GetComponent<TweenPosition>();
        if(icon.sprite == arrowSourceSprite)//点击展开
        {
            isReverse = false;
            icon.color = new Color(1/255.0f,68/255.0f,142/255.0f);
            //tp.enabled = true;
            //tp.PlayReverse();

        }
        else//点击收缩
        {
            isReverse = true;
            icon.color = new Color(1, 1, 1);
            //tp.enabled = true;
            //tp.PlayForward();
        }
    }

    /// <summary>
    /// 获取当前是否展开步骤目录
    /// </summary>
    /// <returns></returns>
    public bool getCatalogIsSpread()
    {
        Image icon = catalogButton.Find("icon").GetComponent<Image>();
        if (icon.sprite == arrowSourceSprite)
        {
            return false;
        }
        else
            return true;
    }

    public void moveFinished()
    {
        Image icon = catalogButton.Find("icon").GetComponent<Image>();
        if(isReverse==false)
        {
            icon.sprite = arrowHightLightSprite;
        }
        else
        {
            icon.sprite = arrowSourceSprite;
        }
    }
}
