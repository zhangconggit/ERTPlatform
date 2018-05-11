using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class shutDown : MonoBehaviour {


    public bool isShutDown = true;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void shutDownBt()
    {
#if UNITY_WEBPLAYER
        ;
#else
        if (isShutDown)
            Process.Start("shutdown.exe", "-s -t 0");
        else
            Application.Quit();
#endif
    }

    public void restartBt()
    {
#if UNITY_WEBPLAYER
        ;
#else
        if (isShutDown)
            Process.Start("shutdown.exe", "-r -t 0");
#endif
    }
}
