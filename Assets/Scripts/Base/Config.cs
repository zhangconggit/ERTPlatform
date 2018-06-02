using UnityEngine;
using System.Collections;
using System.Configuration;
using System.Xml;

public class Config : MonoBehaviour
{
#if UNITY_WEBPLAYER
    void Start()
    {
#if UNITY_EDITOR
        getWebIp("http://192.168.1.121:80");
        getServerIp("http://192.168.1.121:18086");
        getWebScoreUrl("/pc/train-record-detail-devices.html?detail=");
#endif
    }
    void Update()
    {
        if (serverIp == "" || webIp == "")
            Application.ExternalCall("setConfig", gameObject.name);
    }
    public void getWebIp(string ip)
    {
        webIp = ip;
        Debug.Log("webIP = " + webIp);
    }
    public void getServerIp(string ip)
    {
        serverIp = ip;
        Debug.Log("sendIp = " + serverIp);
        //IDataComponentDLL.IDataComponent.GetInstance().setURL(Config.serverIp);
        //IDataComponentDLL.IDataComponent.GetInstance().synchronizationServerTimeSystem();
    }
    public void getWebScoreUrl(string url)
    {
        //IDataComponentDLL.IDataComponent.GetInstance().setWebScoreURL(url);
    }
#endif
 
    public static string serverIp = "";//"http://192.168.1.121:9086";//
    public static string webIp = "";//"";//"http://192.168.1.121:8089";//
    public static string assetsPath = "";//"http://192.168.1.121:9086";//
    public static string dbPath = "/online/EnemaWeb/StreamingAssets/XML/db.xml";
#if UNITY_EDITOR
    public static string appPath = "http://127.0.0.1:8080";
#else
    public static string appPath = Application.dataPath;
#endif

    public static string ConfigPath = appPath + "/StreamingAssets/Config.ini";
#if UNITY_EDITOR
#else
    public static string ConfigPath = webIp + "/online/EnemaWeb/StreamingAssets/Config.ini";
#endif
}