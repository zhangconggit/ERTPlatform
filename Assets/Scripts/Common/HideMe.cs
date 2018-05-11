using UnityEngine;
using System.Collections;

public class HideMe : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnHideMe()
    {
        gameObject.SetActive(false);
        GlobalClass.sMainIsWait = false;
    }
}
