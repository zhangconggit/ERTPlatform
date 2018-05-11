using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Xml;
using System.IO;
using System.Net;
using System.Text;
using System;

public class RequestState
{
    const int m_buffetSize = 1024;
    public StringBuilder m_requestData;
    public byte[] m_bufferRead;
    public HttpWebRequest m_request;
    public HttpWebResponse m_response;
    public Stream m_streamResponse;

    public RequestState()
    {
        m_bufferRead = new byte[m_buffetSize];
        m_requestData = new StringBuilder("");
        m_request = null;
        m_streamResponse = null;
    }
}
public class CXML_pach : MonoBehaviour
{
    private string VideoXMLaddress = Config.webIp+"/online/sengstakenWeb/StreamingAssets/XML/VoiceAndVideoConfiguration.xml";
    private string GradeXMLaddress = Config.webIp + "/online/sengstakenWeb/StreamingAssets/XML/GradingRule.xml";
    private string dbPath = Config.webIp + "/online/EnemaWeb/StreamingAssets/DBtools/db.xml";

    string currentVoiceName;
    string currentVideoName;
    bool VideoFlish =false;
    bool ScoreFlish = false;
    public bool Loaded = false;
    bool wait = false;
    FileStream fileStream = null;
    void Start ()
    {
        wait = false;
    }
    void Update()
    {
        if (Config.sendIp == "")
        {
            return;
        }
        if (Config.webIp == "")
        {
            return;
        }
        if (wait == false)
        {
#if UNITY_WEBPLAYER
            DownloadDB();
#else
            initXml();
#endif
            wait = true;
        }
        if (Loaded)
            return;
        if(VideoFlish && ScoreFlish)
        {
            Loaded = true;
#if UNITY_WEBPLAYER
            ;
#else
            IDataComponentDLL.IDataComponent.GetInstance().sendQueryTrainID(CGrade.GradeTableId);
#endif
        }
    } 
    void Awake()
    {
        
            
    }

    public void initXml()
    {
        CGrade.LoadGradingRuleFromLocalXml();
        ScoreFlish = true;
        VideoFlish = true;
    }
#if UNITY_WEBPLAYER
    void DownloadDB()
    {
        string localFile = Application.temporaryCachePath + "/MisSqlite.db";

        fileStream = new FileStream(localFile, FileMode.Create);
        try
        {

            HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(dbPath);

            RequestState myRequestState = new RequestState();
            myRequestState.m_request = myHttpWebRequest;

            Debug.Log("BeginGetResponse Start");
            //异步获取;  
            IAsyncResult result = (IAsyncResult)myHttpWebRequest.BeginGetResponse(new AsyncCallback(RespCallback), myRequestState);
            Debug.Log("BeginGetResponse End");
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }
    void RespCallback(IAsyncResult result)
    {
        Debug.Log("RespCallback 0");

        try
        {
            RequestState myRequestState = (RequestState)result.AsyncState;
            HttpWebRequest myHttpWebRequest = myRequestState.m_request;

            Debug.Log("RespCallback EndGetResponse");
            myRequestState.m_response = (HttpWebResponse)myHttpWebRequest.EndGetResponse(result);

            Stream responseStream = myRequestState.m_response.GetResponseStream();
            myRequestState.m_streamResponse = responseStream;

            //开始读取数据;  
            IAsyncResult asyncreadresult = responseStream.BeginRead(myRequestState.m_bufferRead, 0, 1024, new AsyncCallback(ReadCallBack), myRequestState);
            return;
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }
    void ReadCallBack(IAsyncResult result)
    {
        Debug.Log("ReadCallBack");
        try
        {
            RequestState myRequestState = (RequestState)result.AsyncState;
            Stream responseStream = myRequestState.m_streamResponse;
            int read = responseStream.EndRead(result);

            Debug.Log("read size =" + read);

            if (read > 0)
            {
                //将接收的数据写入;  
                fileStream.Write(myRequestState.m_bufferRead, 0, 1024);
                fileStream.Flush();
                //继续读取数据;  
                myRequestState.m_bufferRead = new byte[1024];
                IAsyncResult asyncreadresult = responseStream.BeginRead(myRequestState.m_bufferRead, 0, 1024, new AsyncCallback(ReadCallBack), myRequestState);
            }
            else
            {
                initXml();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex.ToString());
        }
    }
    void TimeoutCallback(object state, bool timeout)
    {
        if (timeout)
        {
            HttpWebRequest request = state as HttpWebRequest;
            if (request != null)
            {
                request.Abort();
            }

        }
    }
#endif
}
