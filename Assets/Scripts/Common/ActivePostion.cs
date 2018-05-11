using UnityEngine;
using System.Collections;

public class ActivePostion : MonoBehaviour {
    public GameObject postionObject;
    int screenWidth = 0;
    int screenHeight = 0;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        return;
        if (screenWidth != Screen.currentResolution.width || screenHeight != Screen.currentResolution.height)
        {
            gameObject.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(postionObject.transform.position);
            screenWidth = Screen.currentResolution.width;
            screenHeight = Screen.currentResolution.height;
        }
    }
}
