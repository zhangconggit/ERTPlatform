using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShowObject : MonoBehaviour {
    public List<GameObject> WebObjects;
    public List<GameObject> PcObjects;
	// Use this for initialization
	void Start () {
	    foreach(GameObject obj in WebObjects)
        {
            if (GlobalClass.sys == SYS.web)
                obj.SetActive(true);
            else
                obj.SetActive(false);
        }
        foreach (GameObject obj in PcObjects)
        {
            if (GlobalClass.sys == SYS.pc || GlobalClass.sys == SYS.pc_hard || GlobalClass.sys == SYS.sickbed || GlobalClass.sys == SYS.sickbed_hard)
                obj.SetActive(true);
            else
                obj.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
