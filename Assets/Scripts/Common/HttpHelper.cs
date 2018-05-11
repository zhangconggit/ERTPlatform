using UnityEngine;
using System.Collections;
using System;
using System.Net;
using System.IO;

public class HttpHelper : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public static bool downfile(string url, string LocalPath)

    {

        try
        {

            Uri u = new Uri(url);

            HttpWebRequest mRequest = (HttpWebRequest)WebRequest.Create(u);

            mRequest.Method = "GET";

            mRequest.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse wr = (HttpWebResponse)mRequest.GetResponse();

            Stream sIn = wr.GetResponseStream();

            FileStream fs = new FileStream(LocalPath, FileMode.Create, FileAccess.Write);

            long length = wr.ContentLength;

            long i = 0;

            decimal j = 0;

            while (i < length)

            {

                byte[] buffer = new byte[1024];

                i += sIn.Read(buffer, 0, buffer.Length);

                fs.Write(buffer, 0, buffer.Length);

                if ((i % 1024) == 0)

                {

                    j = Math.Round(Convert.ToDecimal((Convert.ToDouble(i) / Convert.ToDouble(length)) * 100), 4);

                    Debug.Log("当前下载文件大小:" + length.ToString() + "字节   当前下载大小:" + i + "字节 下载进度" + j.ToString() + "%");

                }

                else

                {

                    Debug.Log("当前下载文件大小:" + length.ToString() + "字节   当前下载大小:" + i + "字节");

                }

            }

            sIn.Close();

            wr.Close();

            fs.Close();

            return true;

        }

        catch { return false; }

    }
}
