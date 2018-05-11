using UnityEngine;
using System.Collections;

public class TriggerObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    /// <summary>
    /// 触发器传递
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter(Collider collider)
    {
        StepManager.Instance.OnTriggerEnter(gameObject,collider);
    }
    void OnTriggerExit(Collider collider)
    {
        StepManager.Instance.OnTriggerExit(gameObject, collider);
    }
    void OnTriggerStay(Collider collider)
    {
        StepManager.Instance.OnTriggerStay(gameObject, collider);
    }
}
