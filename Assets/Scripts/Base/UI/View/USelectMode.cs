﻿using UnityEngine;
using System.Collections;
using CFramework;
using UnityEngine.UI;
/// <summary>
/// 选择模式页面
/// 无配置信息
/// </summary>
public class USelectMode : UPageBase
{
    /// <summary>
    /// 确认按钮
    /// </summary>
    public CEvent OnOkButton = new CEvent();
    /// <summary>
    /// 返回按钮
    /// </summary>
    public CEvent OnGoBackButton = new CEvent();
    /// <summary>
    /// 教学模式按钮
    /// </summary>
    public CEvent OnTeachingButton = new CEvent();
    /// <summary>
    /// 练习模式按钮
    /// </summary>
    public CEvent OnPracticeButton = new CEvent();
    /// <summary>
    /// 考试模式按钮
    /// </summary>
    public CEvent OnExamButton = new CEvent();
    public enum Model
    {
        None = -1,
        Teaching = 0,
        Practice,
        Exam
    }

    GameObject modelBase;
    public USelectMode()
    {
        SetAnchored(AnchoredPosition.full);
        SetBorderSpace(0, 0, 0, 0);
        gameObejct.name = "SelectModel";
        modelBase = UIRoot.Instance.InstantiateCustom("UIPanel");
        modelBase.transform.parent = transform;
        modelBase.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
        modelBase.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
        modelBase.transform.localScale = new Vector3(1 * fAdapterWidth, 1 * fAdapterHeight, 1);
        modelBase.name = "modelBase";
        modelBase.SetActive(true);
        modelBase.transform.Find("Button_OK").GetComponent<UButton>().AddListionEvent(() => { OnOkButton.Invoke(); });
        modelBase.transform.Find("Button_quit").GetComponent<UButton>().AddListionEvent(() => { OnGoBackButton.Invoke(); });
        modelBase.transform.Find("Teaching_Model/Button").GetComponent<UButton>().AddListionEvent(() => { OnTeachingButton.Invoke(); });
        modelBase.transform.Find("Practice_Model/Button").GetComponent<UButton>().AddListionEvent(() => { OnPracticeButton.Invoke(); });
        modelBase.transform.Find("Exam_Model/Button").GetComponent<UButton>().AddListionEvent(() => { OnExamButton.Invoke(); });
    }
    /// <summary>
    /// 设置选择的模式
    /// </summary>
    /// <param name="model"></param>
    public void SetSelectedModel(Model model)
    {
        modelBase.transform.Find("Teaching_Model/background").gameObject.SetActive(false);
        modelBase.transform.Find("Practice_Model/background").gameObject.SetActive(false);
        modelBase.transform.Find("Exam_Model/background").gameObject.SetActive(false);
        switch (model)
        {
            case Model.None:
                break;
            case Model.Teaching:
                modelBase.transform.Find("Teaching_Model/background").gameObject.SetActive(true);
                break;
            case Model.Practice:
                modelBase.transform.Find("Practice_Model/background").gameObject.SetActive(true);
                break;
            case Model.Exam:
                modelBase.transform.Find("Exam_Model/background").gameObject.SetActive(true);
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="model"></param>
    /// <param name="ishow"></param>
    public void SetModelButtonState(Model model, bool ishow)
    {
        switch (model)
        {
            case Model.None:
                break;
            case Model.Teaching:
                if (ishow)
                {
                    modelBase.transform.Find("Teaching_Model/Button").GetComponent<UButton>().enabled = true;
                    modelBase.transform.Find("Teaching_Model/Button").GetComponent<Image>().color = new Color(1, 1, 1);
                }
                else
                {
                    modelBase.transform.Find("Teaching_Model/Button").GetComponent<UButton>().enabled = false;
                    modelBase.transform.Find("Teaching_Model/Button").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                }
                break;
            case Model.Practice:
                if (ishow)
                {
                    modelBase.transform.Find("Practice_Model/Button").GetComponent<UButton>().enabled = true;
                    modelBase.transform.Find("Practice_Model/Button").GetComponent<Image>().color = new Color(1, 1, 1);
                }
                else
                {
                    modelBase.transform.Find("Practice_Model/Button").GetComponent<UButton>().enabled = false;
                    modelBase.transform.Find("Practice_Model/Button").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                }
                break;
            case Model.Exam:
                if (ishow)
                {
                    modelBase.transform.Find("Exam_Model/Button").GetComponent<UButton>().enabled = true;
                    modelBase.transform.Find("Exam_Model/Button").GetComponent<Image>().color = new Color(1, 1, 1);
                }
                else
                {
                    modelBase.transform.Find("Exam_Model/Button").GetComponent<UButton>().enabled = false;
                    modelBase.transform.Find("Exam_Model/Button").GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f);
                }
                break;
            default:
                break;
        }
    }
    /// <summary>
    /// 设置欢迎用户字样
    /// </summary>
    /// <param name="name"></param>
    public void SetWelcome(string value)
    {
        modelBase.transform.Find("Welcome_text").GetComponent<Text>().text = value;
    }
}
