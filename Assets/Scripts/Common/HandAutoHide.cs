using UnityEngine;
using System.Collections;

public class HandAutoHide : MonoBehaviour {

    Vector3 pos = Vector3.zero;
    public GameObject hand;
	// Use this for initialization
	void Start () {
        pos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	    if(pos == transform.position)
        {
            if(!hand.activeSelf)
                hand.SetActive(true);
        }
        else
        {
            if (hand.activeSelf)
                hand.SetActive(false);
        }
	}
}
