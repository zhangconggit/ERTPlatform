using UnityEngine;
using System.Collections;
using CFramework;
using System.Collections.Generic;

public class DrawNarcotic : StepBase
{
    SkinnedMeshRenderer renderer;
    SkinnedMeshRenderer renderer_water;
    public DrawNarcotic()
    {
        renderer = (GameObject.Find("Models").transform.Find("tools/抽麻药/injector").GetComponent<SkinnedMeshRenderer>());
        renderer_water = (GameObject.Find("Models").transform.Find("tools/抽麻药/liquid_bottle").GetComponent<SkinnedMeshRenderer>());
        cameraEnterPosition = new Vector3(-0.191f, 0.933f, 0.714f);
        cameraEnterEuler = new Vector3(12.0851f, 169.0346f, 359.6248f);

        UPageButton okButton = CreateUI<UPageButton>();
        okButton.name = "okButton";
        okButton.SetAnchored(AnchoredPosition.bottom);
        okButton.rect = new Rect(0, -100, 180, 60);
        okButton.LoadSprite("keys_030");
        okButton.LoadPressSprite("keys_031");
        okButton.text = "确定";
        okButton.onClick.AddListener(OnClickOkButton);
    }
    public override void CameraMoveFinished()
    {
        base.CameraMoveFinished();
        ButtonVirtualScript.Instance.VirtualButton(KeyCode.UpArrow,new Vector2(100,100),"抽注射器");
        ButtonVirtualScript.Instance.GetButton(KeyCode.UpArrow).onClick.AddListener(PlayAnimation);
        ButtonVirtualScript.Instance.VirtualButton(KeyCode.DownArrow, new Vector2(220, 100), "推注射器");
        ButtonVirtualScript.Instance.GetButton(KeyCode.DownArrow).onClick.AddListener(BackPlayAnimation);
        GameObject.Find("Models").transform.Find("tools/抽麻药").gameObject.SetActive(true);
    }

    public override void StepStartState()
    {
        base.StepEndState();
        GameObject.Find("Models").transform.Find("tools/抽麻药").gameObject.SetActive(false);
    }

    public override void StepEndState()
    {
        base.StepEndState();
        //GameObject.Find("Models").transform.Find("tools/抽麻药").gameObject.SetActive(false);
    }

    void PlayAnimation()
    {
        if (renderer.GetBlendShapeWeight(0) + 10 > 100)
            return;
        else
            renderer.SetBlendShapeWeight(0, renderer.GetBlendShapeWeight(0) + 10);

        if (renderer_water.GetBlendShapeWeight(0) + 10 > 100)
            return;
        else
            renderer_water.SetBlendShapeWeight(0, renderer_water.GetBlendShapeWeight(0) + 10);

    }
    void BackPlayAnimation()
    {
        if (renderer.GetBlendShapeWeight(0) - 10 < 0)
            return;
        else
            renderer.SetBlendShapeWeight(0, renderer.GetBlendShapeWeight(0) - 10);

        if (renderer_water.GetBlendShapeWeight(0) - 10 < 0)
            return;
        else
            renderer_water.SetBlendShapeWeight(0, renderer_water.GetBlendShapeWeight(0) - 10);
    }

    void OnClickOkButton()
    {
        var currentLength = renderer_water.GetBlendShapeWeight(0) / 100;
        if(currentLength >= 0.4)
            State = StepStatus.did;
        else if(currentLength >0)
        {
            Debug.Log("扣分");
        }
        else
        {
            Debug.Log("0分");
        }
        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "");
    }
}
