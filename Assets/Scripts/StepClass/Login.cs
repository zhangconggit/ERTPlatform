
using CFramework;
using UnityEngine;

/// <summary>
/// 登陆
/// </summary>
class Login : StepBase
{
    LoginBase loginBase = null;
    public Login()
    {
        UPageBase bus = CreateUI<UPageBase>();
        bus.rect = new Rect(50, 50, 100, 100);

    }
    public override void EnterStep(float cameraTime = 1)
    {
        UText txt = CreateUI<UText>();
        txt.rect = new Rect(10, 10, 200, 200);
        txt.text = "123";

        Debug.Log("time = " + cameraTime);
        base.EnterStep(cameraTime);

        ModelEventSystem.Instance.Add3DModelListenEvent(click);
    }

    public override void StepUpdate()
    {
        if (mianqiu != null)
        {
            Vector3 pos = Input.mousePosition;
            pos.z = 100;
            mianqiu.transform.position = Camera.main.ScreenToWorldPoint(pos);
            if (Input.GetMouseButtonUp(0))
            {
                mianqiu = null;
            }
        }

    }
    GameObject mianqiu;
    public void click(RaycastHit obj)
    {
        if (obj.collider.name == "棉球")
        {
            mianqiu = obj.collider.gameObject;
        }
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
