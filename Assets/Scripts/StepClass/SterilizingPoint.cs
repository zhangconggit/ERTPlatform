using UnityEngine;
using System.Collections;
using CFramework;
using System.Collections.Generic;
using MisTexturePoint;

/// <summary>
/// 消毒过穿刺点
/// </summary>
public class SterilizingPoint : StepBase
{
    UToogleItem mianqian;
    bool Start = false;
    GameObject man;
    private CTexturePoint texturePoint;
    private Vector3 startPos = Vector3.zero;
    float keyTime = 0;
    public SterilizingPoint()
    {
        cameraEnterPosition = new Vector3(-0.32f, 0.75f, 0.34f);
        cameraEnterEuler = new Vector3(11.6962f, 147.2081f, 354.9553f);
        mianqian = CreateUI<UToogleItem>("tweezers_plestic", new Rect(-150, 0, 200, 200));
        mianqian.SetAnchored(AnchoredPosition.right);
        mianqian.LoadSelectedImage("tweezers_plestic_h");
        mianqian.OnChange.AddListener(onNieziChange);
        man = ModelCtrol.Find(fileHelper.ReadIni("Main", "manmodel", "StepConfig"));
        texturePoint = man.GetComponent<MisTexturePoint.CTexturePoint>();
        texturePoint.material = man.transform.GetComponent<MeshRenderer>().materials[1];
    }

    public override void CameraMoveFinished()
    {
        base.CameraMoveFinished();
    }
    public override void EnterStep(float cameraTime = 1)
    {
        Start = false;
        base.EnterStep(cameraTime);
    }
    public override void StepUpdate()
    {
        base.StepUpdate();
        if (Start)
        {
            keyTime += Time.deltaTime;
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;//射线的终点
            if (Physics.Raycast(_ray, out hit))
            {
                if (hit.collider.name == man.name)
                {
                    Model_Tweezer.Instance.SetTweezerPos(hit.point, hit.normal);
                    if (Input.GetMouseButtonDown(0))
                    {
                        startPos = hit.point;
                        keyTime = 0;
                        if (Vector3.Distance(startPos, Model_PunctureInfo.Instance.m_PuncturePoint) > 0.02f)
                        {
                            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "SterilizingPointError");
                            startPos = Vector3.zero;
                        }
                    }
                    if (Input.GetMouseButtonUp(0) && startPos != Vector3.zero)
                    {
                        if (Vector3.Distance(startPos, hit.point) < 0.02f && keyTime > 2)
                        {
                            State = StepStatus.did;
                        }
                        else
                        {
                            VoiceControlScript.Instance.AudioPlay(AudioStyle.Attentions, "SterilizingPointErrorKeep"); 
                        }
                    }
                }
            }

        }
    }

    public override void StepFinish()
    {
        base.StepFinish();
        Model_Tweezer.Instance.EnabledTweezel(false);
        texturePoint.enabled = false;
    }
    private void onNieziChange(bool value)
    {
        if (value)
        {
            Start = true;
            Model_Tweezer.Instance.EnabledTweezel(true);
            texturePoint.enabled = true;
            setColor(new Color(1, 158 / 255.0f, 13 / 255.0f, 1));
        }
    }
    //设置消毒颜色
    public void setColor(Color pColor)
    {
        //pColor = Color.red;
        //disinfectionColor = pColor;
        texturePoint.MColor = pColor;
        texturePoint.mRadius = 30;
    }
}
