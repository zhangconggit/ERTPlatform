using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/********************************************************************
	created:	2016/07/15
	created:	15:7:2016   14:19
	filename: 	E:\Workspace\Unity Workspace\Gastrolavage\Assets\Scripts\ThingsPick\thingItemControl.cs
	file path:	E:\Workspace\Unity Workspace\Gastrolavage\Assets\Scripts\ThingsPick
	file base:	thingItemControl
	file ext:	cs
	author:		XingRui.Chen
	
	purpose:	The Class will achieve to pick things.
*********************************************************************/


public class thingItemControl : MonoBehaviour {

    List<thingItem> thingLists = new List<thingItem>();//All
    List<thingItem> leakThingLists = new List<thingItem>();//Leak ps:The list should save data of right.If our picked one that we should remove it.
    List<thingItem> errorThingLists = new List<thingItem>();//Error
    List<thingItem> rightThingLists = new List<thingItem>();//Right

    int pickRightNumber = 0;
    int pickErrorNumber = 0;
	// Use this for initialization
	void Start () {
        thingItem[] tis =  GameObject.FindObjectsOfType<thingItem>();
        foreach(thingItem ti in tis)
        {
            thingLists.Add(ti);
            if(ti.isShouldPick==true)
            {
                leakThingLists.Add(ti);
            }
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && !specificItem.waiting )
        {
            GameObject pickedObject = getPickedModel();
            if(pickedObject!=null)
            {
               bool isFind =  thingLists.Find(
                    delegate(thingItem ti)
                    {
                        if (pickedObject.GetComponent<thingItem>() == ti)
                            return true;
                        else
                            return false;
                    });
                if(isFind==true)
                {
                    if (pickedObject.GetComponent<thingItem>().isPicked == false)
                    {
                        //pickedObject.GetComponent<thingItem>().textHint.SetActive(true);
                        pickedObject.GetComponent<thingItem>().isPicked = true;
                        if (pickedObject.GetComponent<thingItem>().isShouldPick == true)
                        {
                            pickRightNumber++;
                            pickedObject.GetComponent<thingItem>().imageHint.SetActive(true);
                            rightThingLists.Add(pickedObject.GetComponent<thingItem>());//add right
                            leakThingLists.Remove(pickedObject.GetComponent<thingItem>());
                        }
                        else
                        {
                            pickErrorNumber++;
                            //if(GlobalClass.g_OperatorSchema == OperatorSchema.examModel)
                                pickedObject.GetComponent<thingItem>().imageHint.SetActive(true);
                            //else
                            //    pickedObject.GetComponent<thingItem>().errorImageHint.SetActive(true);
                            errorThingLists.Add(pickedObject.GetComponent<thingItem>());//add error
                        }

                    }
                    else
                    {
                        //pickedObject.GetComponent<thingItem>().textHint.SetActive(false);
                        pickedObject.GetComponent<thingItem>().imageHint.SetActive(false);
                        pickedObject.GetComponent<thingItem>().isPicked = false;
                        if (pickedObject.GetComponent<thingItem>().isShouldPick == true)
                        {
                            pickRightNumber--;
                            rightThingLists.Remove(pickedObject.GetComponent<thingItem>());//remove right
                            leakThingLists.Add(pickedObject.GetComponent<thingItem>());// 
                        }
                        else
                        {
                            pickErrorNumber--;
                            errorThingLists.Remove(pickedObject.GetComponent<thingItem>());//remove error
                            //if (GlobalClass.g_OperatorSchema  == OperatorSchema.examModel)
                                pickedObject.GetComponent<thingItem>().imageHint.SetActive(false);
                            //else
                            //    pickedObject.GetComponent<thingItem>().errorImageHint.SetActive(false);
                        }
                    }
                    
                }
                
            }
        }
	}

    GameObject getPickedModel()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("collision gameObject name = " + hit.collider.gameObject.name);
            return hit.collider.gameObject;
        }
        return null;
    }

    public void ReSet()
    {
        pickRightNumber = 0;
        pickErrorNumber = 0;
        leakThingLists.Clear();
        errorThingLists.Clear();
        rightThingLists.Clear();
        foreach (var item in thingLists)
        {
            item.imageHint.SetActive(false);
            item.isPicked = false;
            if (item.isShouldPick)
                leakThingLists.Add(item);
        }
    }
    //////////////////////////////// Communication /////////////////////////////////////////////
    public int  getPickedRightCount()
    {
        return pickRightNumber;
    }

    public int getPickedErrorCount()
    {
        return pickErrorNumber;
    }

    int getPickedAllCount()
    {
        return pickRightNumber+pickErrorNumber;
    }


    void OnGUI()
    {
    }
}
