using UnityEngine;
using System.Collections;
using UnityEditor;
using System.IO;
using System.Text;
using System.Linq;
using System.Security.Cryptography;
using System.Collections.Generic;
using System;

public class AssetsBundle : MonoBehaviour
{
    static string AssetPath = Application.dataPath + "/AssetBundle";
    static string updatePath = Application.dataPath + "/AssetBundle/_file_/assetFileVersions.txt";
    static string VisonPath = Application.dataPath + "/StreamingAssets/Version/version.txt";
    /// <summary>
    /// 点击后，所有设置了AssetBundle名称的资源会被 分单个打包出来
    /// </summary>
    [MenuItem("AssetBundle/资源打包")]
    static void Build_AssetBundle()
    {
        BuildPipeline.BuildAssetBundles(AssetPath, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
        //刷新
        AssetDatabase.Refresh();
        BuildVersion();
    }

    [MenuItem("AssetBundle/清除本地资源包缓存")]
    static void Clean_Cache()
    {
        Caching.CleanCache();
    }
    static void BuildVersion()
    {
        Caching.CleanCache();
        loadVersionFile();
        WriteUpdateTXT();
        WriteVersionTXT();
        AssetDatabase.Refresh();
    }



    /// <summary>
    /// 更新版本文档
    /// </summary>
    static void WriteUpdateTXT()
    {
        string[] files = Directory.GetFiles(AssetPath, "*.*", SearchOption.AllDirectories);
        StringBuilder stringBuilder = new StringBuilder();
        foreach (string filePath in files)
        {
            if (!filePath.EndsWith(".meta") && !filePath.Contains(".manifest") && !filePath.Contains("_file_"))
            {
                string md5 = BuildFileMd5(filePath);
                string fileName = filePath.Substring(filePath.LastIndexOf("AssetBundle\\") + ("AssetBundle\\").Length);
                visonCompare(fileName, md5);
                stringBuilder.AppendLine(string.Format("{0}:{1}", fileName, md5));
            }
        }
        WriteTXT(stringBuilder.ToString(), updatePath);
    }

    static string BuildFileMd5(string filePath)
    {
        string fileMd5 = string.Empty;
        try
        {
            using (FileStream fs = File.OpenRead(filePath))
            {
                MD5 md5 = MD5.Create();
                byte[] fileMd5Bytes = md5.ComputeHash(fs);
                fileMd5 = System.BitConverter.ToString(fileMd5Bytes).Replace("-", "").ToLower();
            }
        }
        catch (System.Exception ex)
        {
            Debug.LogError(ex);
        }

        return fileMd5;
    }

    static void WriteTXT(string content, string filePath)
    {
        string directory = Path.GetDirectoryName(filePath);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        using (FileStream fs = File.Create(filePath))
        {
            StreamWriter sw = new StreamWriter(fs, Encoding.ASCII);
            try
            {
                sw.Write(content);
                sw.Close();
                fs.Close();
                fs.Dispose();
            }
            catch (IOException e)
            {
                Debug.Log(e.Message);
            }
        }
    }

    static Dictionary<string, string> serverUpdateDic = new Dictionary<string, string>();
    static Dictionary<string, string> visonControl = new Dictionary<string, string>();
    static void loadVersionFile()
    {
        serverUpdateDic.Clear();
        visonControl.Clear();
        
        string CodeSTR = string.Empty;
        StreamReader sR;
        sR = new StreamReader(updatePath, Encoding.Default);
        while ((CodeSTR = sR.ReadLine()) != null)
        {
            int i = CodeSTR.IndexOf(":");
            if(i != -1)
            {
                string key = CodeSTR.Substring(0, i);
                string value = CodeSTR.Substring(i + 1);
                serverUpdateDic.Add(key, value);
            }
        }
        sR.Close();

        sR = new StreamReader(VisonPath, Encoding.Default);
        while ((CodeSTR = sR.ReadLine()) != null)
        {
            int i = CodeSTR.IndexOf("=");
            if(i!= -1)
            {
                string key = CodeSTR.Substring(0, i);
                string value = DecryptDES(CodeSTR.Substring(i + 1));
                visonControl.Add(key, value);
            }
        }
        sR.Close();

    }

    static void visonCompare(string fileName, string md5code, bool isNewEdition = true)
    {
        if (!visonControl.ContainsKey(fileName))
        {
            visonControl.Add(fileName, "1");
            return;
        }
        if (!serverUpdateDic.ContainsKey(fileName))
        {
            serverUpdateDic.Add(fileName, md5code);
            return;
        }
        if (serverUpdateDic[fileName] != md5code)
        {
            //int i = visonControl[fileName].LastIndexOf("=");
            //int j = visonControl[fileName].LastIndexOf(".");
            int vison_tmp = 0;
            if (isNewEdition)
            {
                if (int.TryParse(visonControl[fileName], out vison_tmp))
                {
                    visonControl[fileName] = (vison_tmp + 1).ToString();//.Replace("=" + vison_tmp.ToString(), "=" + (vison_tmp + 1).ToString());
                }
            }
            else
            {
                //小版本更新用，暂时不用
                //if (int.TryParse(visonControl[fileName].Substring(j+1), out vison_tmp))
                //{
                //    visonControl[fileName].Replace("." + vison_tmp.ToString(), "." + (vison_tmp + 1).ToString());
                //}        
            }
            return;
        }
    }

    static void WriteVersionTXT()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.AppendLine("[AB]");
        foreach (KeyValuePair<string, string> item in visonControl)
        {
            stringBuilder.AppendLine(string.Format("{0}={1}", item.Key, EncryptDES(item.Value)));// "a", EncryptDES("中文加密")));//, 
        }
        WriteTXT(stringBuilder.ToString(), VisonPath);
        serverUpdateDic.Clear();
        visonControl.Clear();
    }

    //DES加密秘钥，要求为8位  
    static string desKey = "Platform";
    //默认密钥向量
    static byte[] Keys = { 0x21, 0x34, 0x65, 0x78, 0x09, 0xAB, 0xDC, 0xFE };

    /// <summary>  
    /// DES加密  
    /// </summary>  
    /// <param name="encryptString">待加密的字符串，未加密成功返回原串</param>  
    /// <returns></returns>  
    static string EncryptDES(string encryptString)
    {
        try
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(desKey);
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Encoding.UTF8.GetBytes(encryptString);
            DESCryptoServiceProvider dCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, dCSP.CreateEncryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Convert.ToBase64String(mStream.ToArray());
        }
        catch
        {
            return encryptString;
        }
    }

    /// <summary>  
    /// DES解密  
    /// </summary>  
    /// <param name="decryptString">待解密的字符串，未解密成功返回原串</param>  
    /// <returns></returns>  
    static string DecryptDES(string decryptString)
    {
        try
        {
            byte[] rgbKey = Encoding.UTF8.GetBytes(desKey);
            byte[] rgbIV = Keys;
            byte[] inputByteArray = Convert.FromBase64String(decryptString);
            DESCryptoServiceProvider DCSP = new DESCryptoServiceProvider();
            MemoryStream mStream = new MemoryStream();
            CryptoStream cStream = new CryptoStream(mStream, DCSP.CreateDecryptor(rgbKey, rgbIV), CryptoStreamMode.Write);
            cStream.Write(inputByteArray, 0, inputByteArray.Length);
            cStream.FlushFinalBlock();
            return Encoding.UTF8.GetString(mStream.ToArray());
        }
        catch
        {
            return decryptString;
        }
    }
}
