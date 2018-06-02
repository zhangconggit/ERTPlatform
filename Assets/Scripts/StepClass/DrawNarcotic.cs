using UnityEngine;
using System.Collections;
using CFramework;
using System.Collections.Generic;

public class DrawNarcotic : StepBase
{
    Transform transform;
    SkinnedMeshRenderer renderer;
    SkinnedMeshRenderer renderer_water;

    bool isScore = false;
    public DrawNarcotic()
    {
        switch (CGlobal.productName)
        {
            case "xqcc":
                cameraEnterPosition = new Vector3(-0.191f, 0.933f, 0.714f);
                cameraEnterEuler = new Vector3(12.0851f, 169.0346f, 359.6248f);
                break;
            case "yzcc":
                cameraEnterPosition = new Vector3(-0.872f, 1.188f, 0.042f);
                cameraEnterEuler = new Vector3(0.8371f, 238.1956f, -0.147f);
                break;
            default:
                break;
        }
        transform = GameObject.Find("Models").transform.Find("tools/抽麻药");
        transform.gameObject.SetActive(true);
        transform.localPosition = new Vector3(-0.945f, 1.165f, -0.095f);
        transform.localEulerAngles = new Vector3(358.9196f, 296.9396f, 346.9359f);
        renderer = GameObject.Find("Models").transform.Find("tools/抽麻药/injector").GetComponent<SkinnedMeshRenderer>();
        renderer_water = GameObject.Find("Models").transform.Find("tools/抽麻药/liquid_bottle").GetComponent<SkinnedMeshRenderer>();


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

        VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "check_anesthetic_correct");
    }

    public override void StepStartState()
    {
        base.StepEndState();
        GameObject.Find("Models").transform.Find("tools/抽麻药").gameObject.SetActive(false);
    }

    public override void StepEndState()
    {
        base.StepEndState();
        GameObject.Find("Models").transform.Find("tools/抽麻药").gameObject.SetActive(false);
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
        if (currentLength >= 0.4)
        {
            if (!isScore)
            {
                isScore = true;
                AddScoreItem("10016110");
            }
            ButtonVirtualScript.Instance.RemoveVirtualButton();
            ButtonVirtualScript.Instance.ClearDicButton();
            transform.gameObject.SetActive(false);
            State = StepStatus.did;
        }
        else if (currentLength > 0)
        {
            if (!isScore)
            {
                isScore = true;
                AddScoreItem("10016112");
            }
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "extract_anesthetic_not_enough");
        }
        else
        {
            if (!isScore)
            {
                isScore = true;
                AddScoreItem("10016111");
            }
            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "extract_anesthetic_not_enough");
        }
    }
}
