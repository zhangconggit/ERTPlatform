using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

//namespace System.Runtime.CompilerServices { 
//    public class ExtensionAttribute : Attribute { } 
//}
public class MainApp : MonoBehaviour {
    static MainApp Instance = null;
    StepManager g_Step = null;
    public GameObject loadUI;
    public string firstStep;
    bool isLoading = false;
    float fVersion = 0;
    bool isLoadXml = false;
    System.Diagnostics.Process pro = null;
 
    void Awake()
    {
        Debug.Log("MainU3d Aweke");
        if (Instance == null)
        {

            Debug.Log("MainU3d Instance is null");
            GlobalClass.sWebInitWait = true;
            Instance = this;
            DontDestroyOnLoad(gameObject);
            gameObject.name = "Main";
            StepManager.Instance.nextStep = firstStep;

        }
        else
        {
            if (!GlobalClass.stIsLoading && GameObject.Find("Canvas").transform.FindChild("WaitLoading") != null)
            {
                GameObject.Find("Canvas").transform.FindChild("WaitLoading").gameObject.SetActive(false);
            }
            Destroy(transform.gameObject);
        }
    }
    // Use this for initialization
    void Start()
    {
        Debug.Log("MainU3d:Start");
        g_Step = StepManager.Instance;
        IDataComponentDLL.IDataComponent.GetInstance().notifyComponentPlatform(true);

        Loom.RunAsync(() =>
        {
#if UNITY_WEBPLAYER
            isLoadXml = true;
            Loom.QueueOnMainThread(() =>
            {
                Debug.Log("MainU3d:Thread LoadLocalVersion");
                StartCoroutine("LoadLocalVersion");
            });
            while (isLoadXml)
                System.Threading.Thread.Sleep(10);
            GlobalClass.sWebInitWait = false;
#endif
            CGrade.LoadGradingRuleFromLocalXml();


        });
    }

    // Update is called once per frame
    void Update () {
        if(GlobalClass.stIsLoading != isLoading)
        {
            isLoading = GlobalClass.stIsLoading;
            if(isLoading)
            {
                if (GameObject.Find("Canvas").transform.FindChild("WaitLoading") != null)
                {
                    loadUI = GameObject.Find("Canvas").transform.FindChild("WaitLoading").gameObject;
                    loadUI.SetActive(true);
                }
                else
                    loadUI = null;
            }
            else
            {
                if (loadUI != null)
                {
                    loadUI = GameObject.Find("Canvas").transform.FindChild("WaitLoading").gameObject;
                    loadUI.SetActive(false);
                }
            }
        }
        if(isLoading)
        {
            return;
        }
        if (GlobalClass.sMainIsWait)
            return;
        g_Step.Working();//一直工作 处理随机事件
        if (g_Step.getStep() == "" && GlobalClass.operatorIsEnd)
            return;

        //MouseCtrl.getInstance().Update();//鼠标触发事件

        g_Step.Update();//执行步骤内容
        if (g_Step.getStep() != "" && (g_Step.CurrStepState == StepStatus.did || g_Step.CurrStepState == StepStatus.errorDid))
        {
            Debug.Log(g_Step.setStep(""));
        }
	}
    void OnGUI()
    {
    }
#if UNITY_WEBPLAYER
    IEnumerator LoadLocalVersion()
    {
        yield return new WaitForSeconds(1);
        Debug.Log("Start LoadVersion");
        float oldVersion = 0;
        float newVersion = 0;
        while (Config.webIp == "" || Config.sendIp == "")
        {
            yield return 0;
        }
       
        Debug.Log("go download vision:"+ Config.webIp + Config.dbPath);
        WWW www2 = new WWW(Config.webIp + Config.dbPath);
        while (!www2.isDone)
        {
            yield return 0;
        }
        DB.getInstance().SetData(www2);
       
        isLoadXml = false;
    }
    void CreateModelFile(string path, byte[] info, int length)
    {
        //文件流信息
        //StreamWriter sw;
        FileStream fs = new System.IO.FileStream(path, FileMode.Create, FileAccess.Write);
        //System.IO.Stream sw;
        //System.IO.FileInfo t = new System.IO.FileInfo(path);
        //if (!t.Exists)
        //{
        //    t.Delete();
            
        //}
        fs.Write(info, 0, length);
        fs.Close();
        fs.Dispose();
        //如果此文件不存在则创建
        //sw = t.Create(); 
        ////以行的形式写入信息
        ////sw.WriteLine(info);
        //sw.Write(info, 0, length);
        ////关闭流
        //sw.Close();
        ////销毁流
        //sw.Dispose();
    }
#endif
}
