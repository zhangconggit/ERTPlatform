using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class DataFormat : MonoBehaviour
{
    string text = "";
    public bool isTime = true;
    public int model = 2;
    string inputText = "0000";
    int lastPost = 0;
    bool lastfouce = false;
    // Use this for initialization
    void Start()
    {
        if (model == 2)
        {
            transform.GetComponent<InputField>().text = "00:00";
            transform.GetComponent<InputField>().readOnly = true;
        }
        if(model == 3)
        {
            transform.GetComponent<InputField>().readOnly = true;
        }
    }
    void OnEnable()
    {
        if (model == 3)
        {
            transform.GetComponent<InputField>().text = System.DateTime.Now.ToString("HH:mm");
            transform.GetComponent<Input3DText>().OutputText.GetComponent<TextMesh>().text = transform.GetComponent<InputField>().text;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if (transform.GetComponent<InputField>().isFocused == false)
        {
            if (lastfouce)
                lastfouce = true;
        }
        if (isTime && model == 2 && transform.GetComponent<InputField>().isFocused)
        {
           
        }
        if (transform.GetComponent<InputField>().text != text)
        {
            if (isTime)
            {
                if (model == 1)
                {
                    string tmp = transform.GetComponent<InputField>().text;
                    if (tmp.Length > 5)
                    {
                        tmp = text;
                    }
                    else if (tmp.Length > text.Length)
                    {
                        if (tmp.IndexOf("::") >= 0)
                        {
                            tmp = tmp.Replace("::", ":");
                        }
                        if (tmp.IndexOf(':') < 0)
                        {
                            if (tmp.Length == 2)
                                tmp += ":";
                            //if (tmp.Length > 3)
                            //{
                            //    tmp.Insert(2, ":");
                            //}
                        }
                        for (int i = 0; i < tmp.Length; i++)
                        {

                            if ((tmp[i] >= '0' && tmp[i] <= '9') || tmp[i] == ':')
                            {

                            }
                            else
                            {
                                tmp = text;
                                break;
                            }
                        }
                    }
                    transform.GetComponent<InputField>().text = tmp;
                    if (tmp.Length > text.Length + 1)
                    {
                        transform.GetComponent<InputField>().caretPosition = tmp.Length;
                        //WindowsInput.InputSimulator.SimulateKeyPress(WindowsInput.VirtualKeyCode.RIGHT);
                    }
                    text = tmp;
                }
                else if (model == 2)
                {
                    string temp = transform.GetComponent<InputField>().text;//.Remove(transform.GetComponent<InputField>().text.IndexOf(':'), 1);
                    if (temp.IndexOf(':') >= 0)
                    {
                        temp = temp.Remove(temp.IndexOf(':'), 1);
                        if (transform.GetComponent<InputField>().selectionFocusPosition == 4)
                        {
                            temp = inputText;
                        }
                    }
                    else if (temp.Length == 5)
                        temp = temp.Remove(2, 1);
                    int k = 0;
                    if (int.TryParse(temp, out k))
                    {
                        if (temp.Length < 4)
                            temp = temp.PadRight(5, '0');
                        if (temp.Length > 4)
                            temp = temp.Remove(0, temp.Length - 4);
                    }
                    else
                        temp = inputText;
                    inputText = temp;
                    temp = temp.Insert(2, ":");
                    transform.GetComponent<InputField>().text = temp;
                    text = temp;
                }
            }
            else
            {
                string tmp = transform.GetComponent<InputField>().text;
                if (tmp.Length > 10)
                {
                    tmp = text;
                }
                else if (tmp.Length > text.Length)
                {
                    tmp = tmp.Replace('.', '-');
                    tmp = tmp.Replace('/', '-');
                    for (int i = 0; i < tmp.Length; i++)
                    {

                        if ((tmp[i] >= '0' && tmp[i] <= '9') || tmp[i] == '-')
                        {

                        }
                        else
                        {
                            tmp = text;
                            break;
                        }
                    }
                }
                transform.GetComponent<InputField>().text = tmp;
                text = tmp;
            }
        }
    }
    int GetNumber()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0) || Input.GetKeyDown(KeyCode.Keypad0))
            return 0;
        if (Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown(KeyCode.Keypad1))
            return 1;
        if (Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown(KeyCode.Keypad2))
            return 2;
        if (Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown(KeyCode.Keypad3))
            return 3;
        if (Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown(KeyCode.Keypad4))
            return 4;
        if (Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown(KeyCode.Keypad5))
            return 5;
        if (Input.GetKeyDown(KeyCode.Alpha6) || Input.GetKeyDown(KeyCode.Keypad6))
            return 6;
        if (Input.GetKeyDown(KeyCode.Alpha7) || Input.GetKeyDown(KeyCode.Keypad7))
            return 7;
        if (Input.GetKeyDown(KeyCode.Alpha8) || Input.GetKeyDown(KeyCode.Keypad8))
            return 8;
        if (Input.GetKeyDown(KeyCode.Alpha9) || Input.GetKeyDown(KeyCode.Keypad9))
            return 9;
        return -1;
    }
}
