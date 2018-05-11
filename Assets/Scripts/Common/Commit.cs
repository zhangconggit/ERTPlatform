using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Commit : MonoBehaviour
{

    // Use this for initialization
    public GameObject root;
    public GameObject text;
    public GameObject button;
    //public GameObject reSenderButton;
    //public GameObject closeButton;
    public static bool isCommit;
    public static bool isFail;
    private bool run;
    private float waitTime;
    void Start()
    {
        waitTime = 0;
        isCommit = false;
        isFail = false;
        run = false;
        //if (root.transform.Find("Button") != null)
        //    button = root.transform.Find("Button").gameObject;
        //else
        //    button = null;
        if (button != null)
            button.SetActive(false);
        if (root != null)
            root.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (isCommit)
        {
            waitTime += Time.deltaTime;
            
            {
                //以前的方法
                //在成绩提交成功部分添加函数：CGrade.commitAllScore();
                {
                    //waitTime = 0;
                    //if (GlobalClass.sys == SYS.pc || GlobalClass.sys == SYS.pc_hard || GlobalClass.sys == SYS.sickbed || GlobalClass.sys == SYS.sickbed_hard)
                    //{
                    //    if (GlobalClass.user.status == UserStatus.user)
                    //        IDataComponentDLL.IDataComponent.GetInstance().sendScoreData(Config.webIp, false);
                    //    else if (GlobalClass.user.status == UserStatus.server)
                    //        IDataComponentDLL.IDataComponent.GetInstance().sendScoreData(Config.webIpLoc, false);
                    //}
                    //else
                    //    IDataComponentDLL.IDataComponent.GetInstance().sendScoreData();
                    //if (button != null)
                    //    button.SetActive(true);
                    if (root != null)
                    {
                        root.SetActive(true);
                        if (text != null)
                        {
                            text.GetComponent<Text>().text = "成绩提交中...";
                            text.SetActive(true);
                        }
                    }
                    isCommit = false;
                    run = true;

                }
            }
        }
        if (run)
        {
            commitScore();
        }
    }
    void commitScore()
    {
        //waitTime += Time.deltaTime;
        //
        //IDataComponentDLL.IDataComponent.GetInstance().sendScoreData();
        {
            string re = "";// IDataComponentDLL.IDataComponent.GetInstance().checkSendScoreDataIsSucceed();
            if (re.IndexOf("success")>=0)
            {
                CGrade.isCommitScore = true;
                text.GetComponent<Text>().text = "成绩提交完成！";
                run = false;
                //text.SetActive(false);
                if (button != null)
                    button.SetActive(false);
               // IDataComponent.GetInstance().resetScoreData();
                CGrade.commitAllScore();
            }
            else if (re == "")//&& waitTime <30
            {

            }
            else
            {
                //if (waitTime >= 30)
                //    re = "连接超时，请检查网络！";
                run = false;
                isFail = true;
                GlobalClass.isCommiting = false;
                text.GetComponent<Text>().text = "由于网络原因，提交成绩出现异常";// "成绩提交失败:\n" +re;
                Debug.Log("成绩提交失败:" + re);
                if (button != null)
                    button.SetActive(true);

                //if (reSenderButton == null)
                //    CGrade.isCommitScore = true;
                //else
                //{
                //    if (text != null)
                //        if(waitTime <15)
                //            text.GetComponent<Text>().text = "成绩提交失败，请检查网络！";
                //        else
                //            text.GetComponent<Text>().text = "连接超时，请检查网络！";
                //    if (reSenderButton != null)
                //        reSenderButton.SetActive(true);
                //    if (closeButton != null)
                //        closeButton.SetActive(true);
                //}
            }
        }
        //yield return 0;
    }
    //void onReSenderButton()
    //{
    //    isCommit = true;
    //    if (reSenderButton != null)
    //        reSenderButton.SetActive(false);
    //    if (closeButton != null)
    //        closeButton.SetActive(false);
    //}
    //void onCloseButton()
    //{
    //    CGrade.isCommitScore = true;
    //    if (reSenderButton != null)
    //        reSenderButton.SetActive(true);
    //    if (closeButton != null)
    //        closeButton.SetActive(true);
    //    run = false;
    //}
}
