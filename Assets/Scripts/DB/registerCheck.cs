using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.IO;
public class registerCheck : MonoBehaviour {
    [DllImport("MisCheckLicense")]
    private static extern bool MisCheckLicense([In] byte[] strlic);

    public static bool bRegister = false;
	// Use this for initialization
	void Awake () {
        try
        {
            FileStream fs = new FileStream("lic.lics", FileMode.Open);
            //获取文件大小
            long size = fs.Length;
            byte[] array = new byte[size];
            //将文件读到byte数组中
            fs.Read(array, 0, array.Length);
            fs.Close();
            bool bCheckResult = MisCheckLicense(array);
            if (bCheckResult)
            {
                bRegister = true;
            }
        }
        catch (IOException e)
        {
            System.Console.WriteLine(e.ToString());
        }
	}
	void Start()
    {
        if(bRegister)
            gameObject.SetActive(false);
        else
            StartCoroutine(shutDown());
    }
	// Update is called once per frame
	void Update () {
	}
    IEnumerator shutDown()
    {
        yield return new WaitForSeconds(3f);
        
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_WEBPLAYER
        Application.Quit();
#else
        System.Diagnostics.Process.Start("shutdown.exe", "-s -t 0");
        Application.Quit();
#endif
    }
}
