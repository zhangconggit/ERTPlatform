using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StepJump : MonoBehaviour {
    public string JumpToStep;
    // Use this for initialization
    void Start () {
        if (gameObject.GetComponent<Button>() == null)
            gameObject.AddComponent<Button>();
        gameObject.GetComponent<Button>().onClick.AddListener(ClickMe);

    }
	
	// Update is called once per frame
	void Update () {
	    
	}
    void ClickMe()
    {
        if(GlobalClass.g_OperatorSchema == OperatorSchema.teachModel && GlobalClass.sys != SYS.sickbed && GlobalClass.sys != SYS.sickbed_hard)
            StepManager.Instance.SetToStepEnd("choiceDob", JumpToStep);
    }
}
