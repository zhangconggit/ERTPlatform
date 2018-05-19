using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;
using CFramework;
using System.Xml;

//namespace System.Runtime.CompilerServices { 
//    public class ExtensionAttribute : Attribute { } 
//}
public class MainApp : MonoBehaviour
{
    static MainApp Instance = null;
    public GameObject loadUI;
    public string firstStep;
    bool isLoading = false;
    List<StepInfo> stepList = new List<StepInfo>();

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
        loadUI.SetActive(true);
        isLoading = true;
        StartCoroutine(StartLoading());

        //IDataComponentDLL.IDataComponent.GetInstance().notifyComponentPlatform(true);
    }
    // Update is called once per frame
    void Update()
    {

        if (isLoading)
        {

            return;
        }
        StepManager.Instance.Update();//执行步骤内容
    }

    /// <summary>
    /// 开始配置
    /// </summary>
    /// <returns></returns>
    private IEnumerator StartLoading()
    {
        yield return 0;
        //加载配置文件
        yield return StartCoroutine(LoadConfigFile());
        //加载资源
        yield return StartCoroutine(LoadAssetVersion());

        //加载步骤信息
        yield return StartCoroutine(LoadProjectInfo());
        if (stepList.Count == 0)
        {
            UMessageBox.Show("错误", "没有步骤信息", "确定", null);
        }
        else
        {
            StepManager.Instance.Init(stepList);

            //加载评分表
            //CGrade.LoadGradingRuleFromLocalXml();

            //完成
            loadUI.SetActive(false);
            isLoading = false;
            Debug.Log("加载完成，开始操作！");
        }
    }

    /// <summary>
    /// 加载配置文件
    /// </summary>
    /// <returns></returns>
    private IEnumerator LoadConfigFile()
    {
        yield return 0;
        string str = "";
        //#if UNITY_EDITOR
        //        str =File.ReadAllText(Application.dataPath + Config.ConfigPath);
        //#else
        WWW www = new WWW(Config.ConfigPath);
        yield return www;
        str = www.text;
        //#endif
        string[] s = str.Split('\n');
        fileHelper.setConfigText("config", s);
        Config.webIp = fileHelper.ReadIni("System", "webip", "config");
        Config.serverIp = fileHelper.ReadIni("System", "serverip", "config");
        CGlobal.productName = fileHelper.ReadIni("Project", "name", "config");
        Config.assetsPath = fileHelper.ReadIni("System", "assetsPath", "config") + "/" + CGlobal.productName;
    }

#if UNITY_WEBPLAYER
    IEnumerator LoadAssetVersion()
    {
        yield return new WaitForSeconds(0);


        List<_CLASS_ASSETBUNDLE> assetList = new List<_CLASS_ASSETBUNDLE>();
        Dictionary<string, Hash128> loadList = new Dictionary<string, Hash128>();

        //string localVerPath = Application.persistentDataPath + "/" + CGlobal.productName + "/assetFileVersions.txt";

        WWW www2 = new WWW(Config.assetsPath + "/_file_/assetFileVersions.txt");
        while (!www2.isDone)
        {
            yield return 0;
        }

        #region 解析下载资源
        string[] webVersion = www2.text.Split('\n');
        for (int i = 0; i < webVersion.Length; i++)
        {
            _CLASS_ASSETBUNDLE item = new _CLASS_ASSETBUNDLE();
            string[] strs = webVersion[i].Split(':');
            if (strs.Length != 2)
                continue;
            item.assetpackagename = strs[0];
            item.packageh5 = Hash128.Parse(strs[1]);

            string[] type = item.assetpackagename.Split('_');
            if (type.Length != 2)
            {
                item.style = AssetBundleStyleEnum.None;
                item.packagedesc = "关联包";
            }
            else
            {
                switch (type[0])
                {
                    case "audio":
                        item.style = AssetBundleStyleEnum.Audios;
                        item.packagedesc = "语音";
                        break;
                    case "models":
                        item.style = AssetBundleStyleEnum.Model;
                        item.packagedesc = "模型";
                        break;
                    case "prefab":
                        item.style = AssetBundleStyleEnum.Prefab;
                        item.packagedesc = "预设体";
                        break;
                    case "ui":
                        item.style = AssetBundleStyleEnum.UI;
                        item.packagedesc = "图片";
                        break;
                    case "txt":
                        item.style = AssetBundleStyleEnum.Txt;
                        item.packagedesc = "文本";
                        break;
                    default:
                        break;
                }
            }
            assetList.Add(item);
        }
        #endregion
        if (assetList.Count <= 0)
            Debug.Log("没有找到资源文件！");
        else
            yield return StartCoroutine(LoadAssets.Instance.DownLoadAsset(assetList));

    }
#endif

    IEnumerator LoadProjectInfo()
    {

        yield return 0;
#if UNITY_EDITOR0
        string xmlConnent = System.IO.File.ReadAllText(Application.dataPath + "/StreamingAssets/project/" + projectName + ".xml");
#elif UNITY_WEBPLAYER
        string xmlConnent = string.Empty;
        WWW w = new WWW(Config.appPath + "/StreamingAssets/project/" + CGlobal.productName + ".xml");
        while (!w.isDone)
        {
            yield return 0;
        }
        xmlConnent = w.text;
#endif
        ReadProjectData(xmlConnent);
    }
    public void ReadProjectData(string xml)
    {

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xml);
        var root = doc.SelectSingleNode("//main");
        if (root != null)
        {
            var steps = root.SelectNodes("step");


            foreach (XmlNode node in steps)
            {

                if (node.Attributes["name"] != null)  //获取语言提示信息节点  
                {
                    StepInfo step = new StepInfo();
                    step.stepName = node.Attributes["name"].Value;
                    step.isAutoGo = node.SelectSingleNode("auto").InnerText == "1";
                    step.navigation = node.SelectSingleNode("navigation").InnerText;
                    float time = 1;
                    float.TryParse(node.SelectSingleNode("intotime").InnerText, out time);
                    step.intoTime = time;

                    step.mustDoStep = new List<string>();
                    foreach (XmlNode item in node.SelectNodes("mustdo"))
                    {
                        if (item.InnerText != null && item.InnerText != string.Empty)
                            step.mustDoStep.Add(item.InnerText);
                    }

                    step.cannotDoStep = new List<string>();
                    foreach (XmlNode item in node.SelectNodes("cantdo"))
                    {
                        if (item.InnerText != null && item.InnerText != string.Empty)
                            step.cannotDoStep.Add(item.InnerText);
                    }
                    stepList.Add(step);
                }
            }
        }
    }
}
