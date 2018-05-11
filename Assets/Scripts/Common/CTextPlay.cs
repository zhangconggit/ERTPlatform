using UnityEngine;
using System.Collections;
using CFramework;

public class CTextPlay : MonoBehaviour {

    /*
     *	打字速度
     */
    public float speed = 0.2f;

    /*
     * 对话框文本-问
     */
    public UText speak_content;

    /*
     * 对话框文本-答
     */
    public UText speak_AnswerContent;

    /*
     *	开始打字状态位
     */
    bool isStart = false;

    /*
     *	打字内容
     */
    public string currentText = "";

    /*
     *	打字内容-答
     */
    string currentAnswerText = "";

    public bool isPlayEnd = false;
	// Use this for initialization
	void Start () {
        
        
	}
	
	// Update is called once per frame
	void Update () {
       
        if(isStart == true)
        {
            isStart = false;
            //显示问
            //UIModelCtrl.getInstance().showSpokenNurseText(true);
            StartCoroutine(setSpeakContent());
        }
	}

    IEnumerator setSpeakContent()
    {
        if (currentText.Length == 0)
        {
            yield return new WaitForSeconds(1.0f);
            //显示回答
            //UIModelCtrl.getInstance().showSpokenNurseText(false);
            //Speak.Instance.playAudio("Answer");
            if(currentAnswerText!="")
            {
                //UIModelCtrl.getInstance().showSpokenPatientText(true);
                StartCoroutine(setAnswerSpeakContent());
            }
            else
                isPlayEnd = true;
        }
        else
        {
            string ch = currentText.Substring(0, 1);
            //speak_content.text += ch;
            currentText = currentText.Substring(1, currentText.Length - 1);
            yield return new WaitForSeconds(speed);
            StartCoroutine(setSpeakContent());
        }

    }

    IEnumerator setAnswerSpeakContent()
    {
        if (currentAnswerText.Length == 0)
        {
            yield return new WaitForSeconds(1.0f);
            //UIModelCtrl.getInstance().showSpokenPatientText(false);
            //UIModelCtrl.getInstance().showSpokenNurseText(false);
            isPlayEnd = true;
        }
        else
        {
            string ch = currentAnswerText.Substring(0, 1);
            //speak_AnswerContent.text += ch;
            currentAnswerText = currentAnswerText.Substring(1, currentAnswerText.Length - 1);
            yield return new WaitForSeconds(speed);
            StartCoroutine(setAnswerSpeakContent());
        }
    }

    public void playText(string pText,string pAnswerText = "")
    {
        StopAllCoroutines();
        isPlayEnd = false;
        speak_content.text = pText;
        speak_AnswerContent.text = pAnswerText;
        currentAnswerText = pAnswerText;
        currentText = pText;
        isStart = true;
    }

    void OnGUI()
    {

    }

}
