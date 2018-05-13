
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
        Debug.Log("time = " + cameraTime);
        base.EnterStep(cameraTime);
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
