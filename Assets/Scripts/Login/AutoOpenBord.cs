using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Diagnostics;
using System.Runtime.InteropServices;

public class AutoOpenBord : MonoBehaviour
{
    bool isOpen = false;
    //static System.Diagnostics.Process proce = null;
    // Use this for initialization
    void Start()
    {
#if UNITY_WEBPLAYER
        gameObject.GetComponent<AutoOpenBord>().enabled = false;
#endif
        //OpenKeyBord(false);
        //if(proce == null)
        //    proce = System.Diagnostics.Process.Start(System.Environment.GetEnvironmentVariable("ProgramFiles") + "\\Common Files\\microsoft shared\\ink\\TabTip.exe");
        //proce.Close();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.GetComponent<InputField>().isFocused && !isOpen)
        {
            OpenKeyBord(true);
            isOpen = true;
        }
        if (!gameObject.GetComponent<InputField>().isFocused && isOpen)
        {
            OpenKeyBord(false);
            isOpen = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            UnityEngine.Debug.Log("start");

        }

    }
    void OpenKeyBord(bool bl)
    {
#if UNITY_STANDALONE_WIN && !UNITY_EDITOR
        //System.Diagnostics.Process[] pros = System.Diagnostics.Process.GetProcessesByName("SogouCloud");
        if (true)//pros.Length == 0)//
        {
            if (bl)
            {
                System.Diagnostics.Process.Start(System.Environment.GetEnvironmentVariable("ProgramFiles") + "\\Common Files\\microsoft shared\\ink\\TabTip.exe");
            }
            else
            {
                System.Diagnostics.Process[] proc = System.Diagnostics.Process.GetProcessesByName("TabTip");

                //关闭进程
                foreach (System.Diagnostics.Process pro in proc)
                {
                    pro.Kill();
                }
            }
        }
        else
        {
            if (bl)
            {
                InputSimulator.SimulateModifiedKeyStroke(new[] { VirtualKeyCode.CONTROL, VirtualKeyCode.SHIFT }, VirtualKeyCode.VK_K);

            }
            else
            {
                InputSimulator.SimulateModifiedKeyStroke(new[] { VirtualKeyCode.CONTROL, VirtualKeyCode.SHIFT }, VirtualKeyCode.VK_K);
            }
        }
#endif
    }

}
