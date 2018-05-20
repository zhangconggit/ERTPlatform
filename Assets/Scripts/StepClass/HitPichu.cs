using UnityEngine;
using System.Collections;
using CFramework;

public class HitPichu : StepBase
{
    Transform transform;
    Animation animation;
    Vector3 PuncturePoint;
    Vector3 PunctureNormal = Vector3.zero;

    int deflection_X_Number;//最大15
    int deflection_Y_Number;
    int syringeNumber; //最大10

    public HitPichu()
    {
        cameraEnterPosition = new Vector3(-0.22f, 0.82f, 0.37f);
        cameraEnterEuler = new Vector3(30.2087f, 139.0721f, 349.0117f);

        UPageButton okButton = CreateUI<UPageButton>();
        okButton.name = "okButton";
        okButton.SetAnchored(AnchoredPosition.bottom);
        okButton.rect = new Rect(0, -100, 180, 60);
        okButton.LoadSprite("keys_030");
        okButton.LoadPressSprite("keys_031");
        okButton.text = "确定";
        okButton.onClick.AddListener(OnClickOkButton);

        UImage image = CreateUI<UImage>();
        image.name = "";
        image.SetAnchored(AnchoredPosition.center);
        image.rect = new Rect(600,-350,256,256);
        image.LoadImage("ChestCrossSection");

        UImage imageText = CreateUI<UImage>();
        imageText.name = "";
        imageText.SetAnchored(AnchoredPosition.center);
        imageText.rect = new Rect(600, -350, 256, 256);
        imageText.LoadImage("text_ChestCrossSection");
    }

    public override void CameraMoveFinished()
    {
        base.CameraMoveFinished();
        transform = GameObject.Find("Models").transform.Find("tools/jumazhen");
        animation = transform.GetComponent<Animation>();
        ButtonVirtualScript.Instance.VirtualButton(KeyCode.UpArrow, new Vector2(-300, 300), "上转");
        ButtonVirtualScript.Instance.GetButton(KeyCode.UpArrow).onClick.AddListener(DeflectionUp);
        ButtonVirtualScript.Instance.VirtualButton(KeyCode.DownArrow, new Vector2(-300, 160), "下转");
        ButtonVirtualScript.Instance.GetButton(KeyCode.DownArrow).onClick.AddListener(DeflectionDown);
        ButtonVirtualScript.Instance.VirtualButton(KeyCode.LeftArrow, new Vector2(-400, 230), "左转");
        ButtonVirtualScript.Instance.GetButton(KeyCode.LeftArrow).onClick.AddListener(DeflectionLeft);
        ButtonVirtualScript.Instance.VirtualButton(KeyCode.RightArrow, new Vector2(-200, 230), "右转");
        ButtonVirtualScript.Instance.GetButton(KeyCode.RightArrow).onClick.AddListener(DeflectionRight);

        ButtonVirtualScript.Instance.VirtualButton(KeyCode.Q, new Vector2(-400, 100), "穿刺");
        ButtonVirtualScript.Instance.GetButton(KeyCode.Q).onClick.AddListener(SyringeForward);

        ButtonVirtualScript.Instance.VirtualButton(KeyCode.W, new Vector2(-200, 100), "退出");
        ButtonVirtualScript.Instance.GetButton(KeyCode.W).onClick.AddListener(SyringeBackOff);

        ButtonVirtualScript.Instance.VirtualButton(KeyCode.E, new Vector2(-400, 0), "注射");
        ButtonVirtualScript.Instance.GetButton(KeyCode.E).onClick.AddListener(DeflectionRight);

        ButtonVirtualScript.Instance.VirtualButton(KeyCode.R, new Vector2(-200, 0), "回退");
        ButtonVirtualScript.Instance.GetButton(KeyCode.R).onClick.AddListener(DeflectionRight);

        transform.gameObject.SetActive(true);

        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "filled_drag_start_juma");

        On3DModelListen += SetPuncturePosition;

    }

    //3D模型点击事件委托
    public delegate void OnClick3DModelDelegate(RaycastHit obj);
    OnClick3DModelDelegate On3DModelListen;

    public override void MouseClickModel(RaycastHit obj)
    {
        base.MouseClickModel(obj);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//射线的起点
        RaycastHit hit;//射线的终点
        if (Physics.Raycast(ray, out hit))
        {
             On3DModelListen.Invoke(hit);
        }
    }

    public void SetPuncturePosition(RaycastHit info)
    {
        PuncturePoint = info.point;
        PunctureNormal = info.normal;

        transform.transform.position = PuncturePoint + 0.01f * PunctureNormal;
        ModelCtrol.Instance.setModelsOnNormalline(transform.gameObject, info.normal, new Vector3(0, 0, 1), 0);
    }

    public void SyringeForward()
    {
        if (syringeNumber < 10)
        {
            syringeNumber++;
            SetPunctureMove(-0.003f);
        }
        //TextureEffect.mInstance.MoveNeedle(-10f);
    }

    public void SyringeBackOff()
    {
        if (syringeNumber > -10)
        {
            syringeNumber--;
            SetPunctureMove(0.003f);
        }
    }

    public void SyringeInjection()
    {
        
    }

    public void SyringeBack()
    {
        
    }

    public void DeflectionUp()
    {

        if (deflection_X_Number < 15)
        {
            deflection_X_Number++;
            SetPunctureRoate(new Vector3(2f, 0, 0));
            //TextureEffect.mInstance.Rotate(2);
        }

    }

    public void DeflectionDown()
    {
        if (deflection_X_Number > -15)
        {
            deflection_X_Number--;
            SetPunctureRoate(new Vector3(-2f, 0, 0));
            //TextureEffect.mInstance.Rotate(2);
        }
    }

    public void DeflectionLeft()
    {
        if (deflection_Y_Number < 15)
        {
            deflection_X_Number++;
            SetPunctureRoate(new Vector3(0, 2f, 0));
            //TextureEffect.mInstance.Rotate(2);
        }
    }

    public void DeflectionRight()
    {
        if (deflection_Y_Number > -15)
        {
            deflection_Y_Number--;
            SetPunctureRoate(new Vector3(0, -2f, 0));
            //TextureEffect.mInstance.Rotate(2);
        }
    }

    public void SetPunctureRoate(Vector3 v3)
    {
        transform.transform.RotateAround(PuncturePoint, Vector3.left , v3.x * 3);
        transform.transform.RotateAround(PuncturePoint, Vector3.up , v3.y * 3);
    }

    public void SetPunctureMove(float f)
    {
        transform.position += f * PunctureNormal;
    }

    void OnClickOkButton()
    {

    }
}
