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
        if (Instance == null)
        {

            Debug.Log("MainU3d Instance");
            Instance = this;
            DontDestroyOnLoad(gameObject);
            gameObject.name = "Main";
            StepManager.Instance.nextStep = firstStep;

        }
        else
        {
            if (GameObject.Find("Canvas").transform.FindChild("WaitLoading") != null)
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
#endif
            CGrade.LoadGradingRuleFromLocalXml();


        });
    }

    // Update is called once per frame
    void Update () {

        if(isLoading)
        {
            return;
        }
        StepManager.Instance.Update();//执行步骤内容
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
        FileStream fs = new System.IO.FileStream(path, FileMode.Create, FileAccess.Write);

        fs.Write(info, 0, length);
        fs.Close();
        fs.Dispose();
    }
#endif
}
