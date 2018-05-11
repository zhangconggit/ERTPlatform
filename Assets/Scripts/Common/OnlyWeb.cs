using UnityEngine;
using System.Collections;

public class OnlyWeb : MonoBehaviour {

   void Awake()
    {
#if !UNITY_WEBPLAYER
        DestroyObject(gameObject);
#endif
    }
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
