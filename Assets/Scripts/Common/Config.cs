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
        if (sendIp == "" || webIp == "")
            Application.ExternalCall("setConfig", gameObject.name);
    }
    public void getWebIp(string ip)
    {
        webIp = ip;
        Debug.Log("webIP = " + webIp);
    }
    public void getServerIp(string ip)
    {
        sendIp = ip;
        Debug.Log("sendIp = " + sendIp);
        IDataComponentDLL.IDataComponent.GetInstance().setURL(Config.sendIp);
        IDataComponentDLL.IDataComponent.GetInstance().synchronizationServerTimeSystem();
    }
    public void getWebScoreUrl(string url)
    {
        IDataComponentDLL.IDataComponent.GetInstance().setWebScoreURL(url);
    }
#endif
    /// <summary>
    /// PC版读取本地配置文件
    /// </summary>
    public static void initPcConfig()
    {
        //if (sendIp != "" && webIp != "")
        //    return;
        if (sendIp == "")
            sendIp = DB.getInstance().getConfigValue("serverIP", "IP");
        if (webIp == "")
            webIp = DB.getInstance().getConfigValue("webIP", "IP");
        //IDataComponentDLL.IDataComponent.GetInstance().setURL(Config.sendIp);
        //IDataComponentDLL.IDataComponent.GetInstance().synchronizationServerTimeSystem();
    }
    public static string sendIp = "";//"http://192.168.1.121:9086";//
    public static string webIp = "";//"";//"http://192.168.1.121:8089";//
    public static string dbPath = "/online/EnemaWeb/StreamingAssets/XML/db.xml";
    public static string ConfigPath = "/online/EnemaWeb/StreamingAssets/Config.ini";
}