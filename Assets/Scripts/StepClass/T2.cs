
using System;
using CFramework;
using UnityEngine;

/// <summary>
/// 登陆
/// </summary>
class T2 : StepBase
{
    public T2()
    {
        cameraEnterPosition = new Vector3(0.21f, 1.014f, 1.11f);
        cameraEnterEuler = new Vector3(15.4009f, -182.6279f, 0);
    }
    public override void EnterStep(float cameraTime = 1)
    {
        UImage img = CreateUI<UImage>();
        img.rect = new Rect(0, 0, 512,1024);
        img.LoadImage("x_ray_chest2");

        Debug.Log(GetType().Name + " time = " + cameraTime);
        base.EnterStep(cameraTime);
        ButtonVirtualScript.Instance.VirtualButton(KeyCode.P, new Vector2(500, 500), "结束");
        ButtonVirtualScript.Instance.GetButton(KeyCode.P).onClick.AddListener(OnFinishButton);
        ModelEventSystem.Instance.Add3DModelListenEvent(click);
    }
    public override void CameraMoveFinished()
    {
        base.CameraMoveFinished();
        Debug.Log(GetType().Name + "move finish " );
    }

    private void OnFinishButton()
    {
        State = StepStatus.did;
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
