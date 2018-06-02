
using CFramework;
using UnityEngine;

/// <summary>
/// T1
/// </summary>
class T1 : StepBase
{
    public T1()
    {
        cameraEnterPosition = new Vector3(1.29f, 1.34f, 2.4f);
        cameraEnterEuler = new Vector3(88.06319f, 0, 0);
    }
    public override void EnterStep(float cameraTime = 1)
    {
        UText txt = CreateUI<UText>();
        txt.rect = new Rect(10, 10, 200, 200);
        txt.text = "这是一个什么什么";

        Debug.Log("time = " + cameraTime);
        base.EnterStep(cameraTime);
        ButtonVirtualScript.Instance.VirtualButton(KeyCode.F, new Vector2(500, 500),"结束");
        ButtonVirtualScript.Instance.GetButton(KeyCode.F).onClick.AddListener(OnFinishButton);
        ModelEventSystem.Instance.Add3DModelListenEvent(click);
    }
    public void OnFinishButton()
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
        Debug.Log(GetType().Name + " is finish");
        ButtonVirtualScript.Instance.GetButton(KeyCode.F).onClick.RemoveListener(OnFinishButton);
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
