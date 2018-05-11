using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using CFramework;

public class CMedicalHistoryItem : MonoBehaviour
{

    public bool isPicked = false;

    public bool isWrong = false;

    public string MedicalHistory = "病历名称";
    public GameObject ToggleButton;
    public GameObject Body;
    public List<GameObject> TextList;
    public List<GameObject> LineList;
    public MedicalRecord mdeicalRecord;
    /// <summary>
    /// 被选中
    /// </summary>
    public CEvent OnSelected = new CEvent();
    int FontNumber = 33;
    // Use this for initialization
    void Start()
    {
        ToggleButton.GetComponent<Toggle>().onValueChanged.AddListener(Toggle_Me);
    }

    // Update is called once per frame
    void Update()
    {

    }
    /// <summary>
    /// 选中病例
    /// </summary>
    /// <param name="_bl"></param>
    public void Toggle_Me(bool _bl)
    {
        if(_bl)
        {
            Body.SetActive(true);
            isPicked = true;
            //CMedicalHistoryChoice.SelectItem = this;
            TextList[5].GetComponent<Text>().text = "现病史：" + mdeicalRecord.medicalHistory;
            OnSelected.Invoke();
        }
        else
        {
            Body.SetActive(false);
            isPicked = false;
        }
    }
    public void LoadData(MedicalRecord target)
    {
        ToggleButton.transform.Find("Label").GetComponent<Text>().text = MedicalHistory;
        mdeicalRecord = target;// DB.getInstance().getMedicalRecords();GetComponent
        TextList[0].GetComponent<Text>().text = "姓名："+ mdeicalRecord.name.PadRight(6,'　') + "性别：" + mdeicalRecord.sex.PadRight(3, '　') + "年龄：" + mdeicalRecord.age + "岁";
        TextList[1].GetComponent<Text>().text = "籍贯："+ mdeicalRecord.nativePlace.PadRight(6, '　') + "名族："+ mdeicalRecord.ethnic.PadRight(3, '　') + "电话："+ mdeicalRecord.phone;
        TextList[2].GetComponent<Text>().text = "科别：" + mdeicalRecord.departments.PadRight(6, '　') + "住院号：" + mdeicalRecord.AD;
        TextList[3].GetComponent<Text>().text = "床号：" + mdeicalRecord.bed + "号";
        string text = "主诉：" + mdeicalRecord.mainSuit;
        TextList[4].GetComponent<Text>().text = text;
        int _hight = 57 + (int)TextList[4].transform.GetComponent<Text>().preferredHeight;
        TextList[4].transform.GetComponent<RectTransform>().localPosition = new Vector3(TextList[4].transform.localPosition.x, TextList[3].transform.localPosition.y - TextList[3].transform.GetComponent<RectTransform>().rect.height / 4 - _hight / 4, 0);
        TextList[4].transform.GetComponent<RectTransform>().sizeDelta = new Vector2(TextList[4].transform.GetComponent<RectTransform>().rect.width, _hight);
        LineList[4].transform.GetComponent<RectTransform>().localPosition = new Vector3(LineList[4].transform.GetComponent<RectTransform>().localPosition.x, TextList[4].transform.GetComponent<RectTransform>().localPosition.y - TextList[4].transform.GetComponent<RectTransform>().rect.height / 4, 0);

        text = "现病史：" + mdeicalRecord.medicalHistory;
        TextList[5].GetComponent<Text>().text = text;
        _hight = 57 + (int)TextList[5].transform.GetComponent<Text>().preferredHeight; //57 + line * 42;
        TextList[5].transform.GetComponent<RectTransform>().localPosition = new Vector3(TextList[5].transform.localPosition.x, TextList[4].transform.localPosition.y - TextList[4].transform.GetComponent<RectTransform>().rect.height / 4 - _hight / 4, 0);
        TextList[5].transform.GetComponent<RectTransform>().sizeDelta = new Vector2(TextList[5].transform.GetComponent<RectTransform>().rect.width, _hight);
        LineList[5].transform.GetComponent<RectTransform>().localPosition = new Vector3(LineList[5].transform.GetComponent<RectTransform>().localPosition.x, TextList[5].transform.GetComponent<RectTransform>().localPosition.y - TextList[5].transform.GetComponent<RectTransform>().rect.height / 4, 0);
        text = "考虑诊断：" + mdeicalRecord.diagnosis;
        TextList[6].GetComponent<Text>().text = text;
        _hight = 57 + (int)TextList[6].transform.GetComponent<Text>().preferredHeight; //57 + line * 42;
        TextList[6].transform.GetComponent<RectTransform>().localPosition = new Vector3(TextList[6].transform.localPosition.x, TextList[5].transform.localPosition.y - TextList[5].transform.GetComponent<RectTransform>().rect.height / 4 - _hight / 4, 0);
        TextList[6].transform.GetComponent<RectTransform>().sizeDelta = new Vector2(TextList[6].transform.GetComponent<RectTransform>().rect.width, _hight);
        LineList[6].transform.GetComponent<RectTransform>().localPosition = new Vector3(LineList[6].transform.GetComponent<RectTransform>().localPosition.x, TextList[6].transform.GetComponent<RectTransform>().localPosition.y - TextList[6].transform.GetComponent<RectTransform>().rect.height / 4, 0);

        text = "辅助检查：" + mdeicalRecord.auxiliaryExamination;
        TextList[7].GetComponent<Text>().text = text;
        _hight = 57 + (int)TextList[7].transform.GetComponent<Text>().preferredHeight; //57 + line * 42;
        TextList[7].transform.GetComponent<RectTransform>().localPosition = new Vector3(TextList[7].transform.localPosition.x, TextList[6].transform.localPosition.y - TextList[6].transform.GetComponent<RectTransform>().rect.height / 4 - _hight / 4, 0);
        TextList[7].transform.GetComponent<RectTransform>().sizeDelta = new Vector2(TextList[7].transform.GetComponent<RectTransform>().rect.width, _hight);
        LineList[7].transform.GetComponent<RectTransform>().localPosition = new Vector3(LineList[7].transform.GetComponent<RectTransform>().localPosition.x, TextList[7].transform.GetComponent<RectTransform>().localPosition.y - TextList[7].transform.GetComponent<RectTransform>().rect.height / 4, 0);
       
        text = "体格检查：" + mdeicalRecord.physicalExamination;
        TextList[8].GetComponent<Text>().text = text;
        _hight = 57 + (int)TextList[8].transform.GetComponent<Text>().preferredHeight;

        TextList[8].transform.GetComponent<RectTransform>().localPosition = new Vector3(TextList[8].transform.localPosition.x, TextList[7].transform.localPosition.y - TextList[7].transform.GetComponent<RectTransform>().rect.height / 4 - _hight / 4, 0);
        TextList[8].transform.GetComponent<RectTransform>().sizeDelta = new Vector2(TextList[8].transform.GetComponent<RectTransform>().rect.width, _hight);
        LineList[8].transform.GetComponent<RectTransform>().localPosition = new Vector3(LineList[8].transform.GetComponent<RectTransform>().localPosition.x, TextList[8].transform.GetComponent<RectTransform>().localPosition.y - TextList[8].transform.GetComponent<RectTransform>().rect.height / 4, 0);
        
        if (mdeicalRecord.contraindication == "" || mdeicalRecord.medicalHistory.IndexOf(mdeicalRecord.contraindication) < 0)
        {
            isWrong = false;
        }
        else
        {
            isWrong = true;
        }
        //if(CMedicalHistoryChoice.SelectItem == null)
        //{
        //    ToggleButton.GetComponent<Toggle>().isOn = true;
        //    Toggle_Me(true);
        //}
    }
    /// <summary>
    /// 字符串换行 返回行数
    /// </summary>
    /// <param name="str"></param>
    /// <returns></returns>
    int AutoLineFeed(ref string str)
    {
        int length = getStringLength(ref str); //str.Length;
        int line = 1;
        for (int i = FontNumber*2; i < length; i += FontNumber * 2)
        {
            line++;
        }
        return line;
    }
    /// <summary>
    /// 获取中英文混排字符串的实际长度(字节数)
    /// </summary>
    /// <param name="str">要获取长度的字符串</param>
    /// <returns>字符串的实际长度值（字节数）</returns>
    public int getStringLength(ref string str)
    {
        if (str.Equals(string.Empty))
            return 0;
        int strlen = 0;
        //int line = 1;
        System.Text.ASCIIEncoding strData = new System.Text.ASCIIEncoding();
        //将字符串转换为ASCII编码的字节数字
        byte[] strBytes = strData.GetBytes(str);
        for (int i = 0,k = 0; i <= strBytes.Length - 1; i++, k++)
        {
            if (strBytes[i] == 63)  //中文都将编码为ASCII编码63,即"?"号
                strlen++;
            strlen++;
        }
        return strlen;
    }
    public void ShowError()
    {
        string str2 = "<color=#ff0000ff>" + mdeicalRecord.contraindication + "</color>";
        TextList[5].GetComponent<Text>().text = "现病史：" + mdeicalRecord.medicalHistory.Replace(mdeicalRecord.contraindication, str2);
    } 

}
public class MedicalRecord
{
    /// <summary>
    /// 病例ID
    /// </summary>
    public int ID;

    /// <summary>
    /// 病例名字
    /// </summary>
    public string medicalname;


    /// <summary>
    /// 医嘱内容
    /// </summary>
    public string doctoradvice;

    /// <summary>
    /// 病例类别
    /// </summary>
    public string problemid;

    /// <summary>
    /// 姓名
    /// </summary>

    public string name;

    /// <summary>
    /// 性别
    /// </summary>
    public string sex;

    /// <summary>
    /// 年龄
    /// </summary>
    public int age;

    /// <summary>
    /// 籍贯
    /// </summary>
    public string nativePlace;

    /// <summary>
    /// 民族
    /// </summary>
    public string ethnic;

    /// <summary>
    /// 电话
    /// </summary>
    public string phone;

    /// <summary>
    /// 科别
    /// </summary>
    public string departments;

    /// <summary>
    /// 住院号
    /// </summary>
    public string AD;

    /// <summary>
    /// 床号
    /// </summary>
    public string bed;

    /// <summary>
    /// 主诉
    /// </summary>
    public string mainSuit;

    /// <summary>
    /// 现病史
    /// </summary>
    public string medicalHistory;

    /// <summary>
    /// 考虑诊断
    /// </summary>
    public string diagnosis;

    /// <summary>
    /// 考虑诊断
    /// </summary>
    public string Diagnosis
    {
        get { return diagnosis; }
        set { diagnosis = value; }
    }

    /// <summary>
    /// 辅助检查
    /// </summary>
    public string auxiliaryExamination;

    /// <summary>
    /// 体格检查
    /// </summary>
    public string physicalExamination;

    /// <summary>
    /// 禁忌症
    /// </summary>
    public string contraindication;

    /// <summary>
    /// 医师名字
    /// </summary>
    public string doctorName;

    /// <summary>
    /// 医嘱
    /// </summary>
    public string doctorAdvice;

    /// <summary>
    /// 产品ID
    /// </summary>
    public string productid;


}
