
using UnityEngine;

/// <summary>
/// 登陆
/// </summary>
class Login : StepBase
{
    LoginBase loginBase = null;
    public Login()
    {
    }
    public override void EnterStep(float cameraTime = 1)
    {
        base.EnterStep(cameraTime);
        if (GameObject.Find("Canvas").transform.FindChild("WaitLoading") != null)
        {
            GameObject.Find("Canvas").transform.FindChild("WaitLoading").gameObject.SetActive(false);
        }
    }

    public override void StepUpdate()
    {
       
    }

    public override void StepFinish()
    {

    }

    public override void StepReset()
    {
        base.StepReset();
    }
    public override void StepEndState()
    {
        State = StepStatus.did;
    }
}
