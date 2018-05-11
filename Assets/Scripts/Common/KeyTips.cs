using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using System;

public enum TipsStep
{
    isNULL,
    lianjie=1,
    duankai,
    zhuqi,
    chaguan
}
public class KeyTips : MonoBehaviour {
    public RectTransform target;
    public GameObject body;
    public List<GameObject> KeysList;
    public List<GameObject> BorderList;
    private List<GameObject> workBorder;
    private List<string> KeysDown;
    public Color addColor;

    Vector3 bodyPositon;
    public bool isOpen;
    bool first;
    /// <summary>
    /// 打开关闭提示框的时间
    /// </summary>
    public float openSpeed = 0.02f;
   // public TipsStep step;
    public string wordkeys;
    private Vector3 bodyScale;
	// Use this for initialization
	void Start () {
        first = true;
        bodyPositon = body.transform.localPosition;//new Vector3(0, 0, 0);//
        bodyScale = new Vector3(body.transform.localScale.x,body.transform.localScale.y,body.transform.localScale.z);
        isOpen = false;
        body.SetActive(false);
        body.transform.localScale  = new Vector3(0.01f, 0.01f, 1);
        body.transform.localPosition = target.localPosition + target.parent.localPosition;
        //step = TipsStep.isNULL;
        wordkeys = "";
        workBorder = new List<GameObject>();
        KeysDown = new List<string>();
        foreach (GameObject obj in BorderList)
        {
            obj.SetActive(false);
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!isOpen)
            return;
        if (Input.GetKey(KeyCode.Alpha1) && !KeysDown.Contains("key_1"))
        {
            foreach(GameObject obj in KeysList)
            {
                if(obj.name == "key_1")
                {
                    obj.GetComponent<Image>().color = addColor;
                    KeysDown.Add("key_1");
                }
            }
        }
        else if(!Input.GetKey(KeyCode.Alpha1) && KeysDown.Contains("key_1"))
        {
            foreach (GameObject obj in KeysList)
            {
                if (obj.name == "key_1")
                {
                    obj.GetComponent<Image>().color = new Color(1, 1, 1);
                    KeysDown.Remove("key_1");
                }
            }
        }

        if (Input.GetKey(KeyCode.Alpha2) && !KeysDown.Contains("key_2"))
        {
            foreach (GameObject obj in KeysList)
            {
                if (obj.name == "key_2")
                {
                    obj.GetComponent<Image>().color = addColor;
                    KeysDown.Add("key_2");
                }
            }
        }
        else if (!Input.GetKey(KeyCode.Alpha2) && KeysDown.Contains("key_2"))
        {
            foreach (GameObject obj in KeysList)
            {
                if (obj.name == "key_2")
                {
                    obj.GetComponent<Image>().color = new Color(1, 1, 1);
                    KeysDown.Remove("key_2");
                }
            }
        }

        if (Input.GetKey(KeyCode.Alpha3) && !KeysDown.Contains("key_3"))
        {
            foreach (GameObject obj in KeysList)
            {
                if (obj.name == "key_3")
                {
                    obj.GetComponent<Image>().color = addColor;
                    KeysDown.Add("key_3");
                }
            }
        }
        else if (!Input.GetKey(KeyCode.Alpha3) && KeysDown.Contains("key_3"))
        {
            foreach (GameObject obj in KeysList)
            {
                if (obj.name == "key_3")
                {
                    obj.GetComponent<Image>().color = new Color(1, 1, 1);
                    KeysDown.Remove("key_3");
                }
            }
        }

        if (Input.GetKey(KeyCode.Alpha4) && !KeysDown.Contains("key_4"))
        {
            foreach (GameObject obj in KeysList)
            {
                if (obj.name == "key_4")
                {
                    obj.GetComponent<Image>().color = addColor;
                    KeysDown.Add("key_4");
                }
            }
        }
        else if (!Input.GetKey(KeyCode.Alpha4) && KeysDown.Contains("key_4"))
        {
            foreach (GameObject obj in KeysList)
            {
                if (obj.name == "key_4")
                {
                    obj.GetComponent<Image>().color = new Color(1, 1, 1);
                    KeysDown.Remove("key_4");
                }
            }
        }

        if (Input.GetKey(KeyCode.I) && !KeysDown.Contains("key_I"))
        {
            foreach (GameObject obj in KeysList)
            {
                if (obj.name == "key_I")
                {
                    obj.GetComponent<Image>().color = addColor;
                    KeysDown.Add("key_I");
                }
            }
        }
        else if (!Input.GetKey(KeyCode.I) && KeysDown.Contains("key_I"))
        {
            foreach (GameObject obj in KeysList)
            {
                if (obj.name == "key_I")
                {
                    obj.GetComponent<Image>().color = new Color(1, 1, 1);
                    KeysDown.Remove("key_I");
                }
            }
        }

        if (Input.GetKey(KeyCode.O) && !KeysDown.Contains("key_O"))
        {
            foreach (GameObject obj in KeysList)
            {
                if (obj.name == "key_O")
                {
                    obj.GetComponent<Image>().color = addColor;
                    KeysDown.Add("key_O");
                }
            }
        }
        else if (!Input.GetKey(KeyCode.O) && KeysDown.Contains("key_O"))
        {
            foreach (GameObject obj in KeysList)
            {
                if (obj.name == "key_O")
                {
                    obj.GetComponent<Image>().color = new Color(1, 1, 1);
                    KeysDown.Remove("key_O");
                }
            }
        }

        if (Input.GetKey(KeyCode.J) && !KeysDown.Contains("key_J"))
        {
            foreach (GameObject obj in KeysList)
            {
                if (obj.name == "key_J")
                {
                    obj.GetComponent<Image>().color = addColor;
                    KeysDown.Add("key_J");
                }
            }
        }
        else if (!Input.GetKey(KeyCode.J) && KeysDown.Contains("key_J"))
        {
            foreach (GameObject obj in KeysList)
            {
                if (obj.name == "key_J")
                {
                    obj.GetComponent<Image>().color = new Color(1, 1, 1);
                    KeysDown.Remove("key_J");
                }
            }
        }

        if (Input.GetKey(KeyCode.K) && !KeysDown.Contains("key_K"))
        {
            foreach (GameObject obj in KeysList)
            {
                if (obj.name == "key_K")
                {
                    obj.GetComponent<Image>().color = addColor;
                    KeysDown.Add("key_K");
                }
            }
        }
        else if (!Input.GetKey(KeyCode.K) && KeysDown.Contains("key_K"))
        {
            foreach (GameObject obj in KeysList)
            {
                if (obj.name == "key_K")
                {
                    obj.GetComponent<Image>().color = new Color(1, 1, 1);
                    KeysDown.Remove("key_K");
                }
            }
        }
	}
    public void setTipsStep(string keys)//TipsStep i)
    {
        foreach (GameObject obj in workBorder)
        {
            obj.SetActive(false);
        }
        workBorder.Clear();
        //step = i;
        wordkeys = keys;
        foreach (GameObject obj in BorderList)
        {
            foreach (char key in keys)
            {
                if(obj.name == "kuang_"+key.ToString())
                {
                    obj.GetComponent<RectTransform>().sizeDelta = new Vector2(200, 200);
                    obj.SetActive(true);
                    workBorder.Add(obj);
                }
            }
        }
        StartCoroutine(LockKeys());
    }
    IEnumerator OpenTips()
    {
        body.transform.localScale += new Vector3(openSpeed, openSpeed, openSpeed);
        body.transform.localPosition = body.transform.localPosition + (bodyPositon - (target.localPosition + target.parent.localPosition)) / ((bodyScale.x - 0.1f) / openSpeed); //Vector3.Slerp(target.localPosition, bodyPositon, 0.05f * (0.99f / openSpeed));
        yield return new WaitForSeconds(0.005f) ;
        Debug.Log("x:"+body.transform.localScale.x);
        Debug.Log("localPosition:" + body.transform.localPosition);
        if (body.transform.localScale.x < bodyScale.x)
        {
            StartCoroutine(OpenTips());
        }
        else
        {
            body.transform.localScale = bodyScale;// new Vector3(1, 1, 1);
            body.transform.localPosition = bodyPositon;
            isOpen = true;
        }
    }
    IEnumerator CloseTips()
    {
        body.transform.localScale=(new Vector3(body.transform.localScale.x - openSpeed, body.transform.localScale.y - openSpeed, body.transform.localScale.z));
        body.transform.localPosition = body.transform.localPosition + (target.localPosition + target.parent.localPosition - bodyPositon) / 49; //Vector3.Slerp(bodyPositon, target.localPosition, 0.05f * (0.99f / openSpeed));
        yield return new WaitForSeconds(0.005f);
        if (body.transform.localScale.x > 0.01)
        {
            StartCoroutine(CloseTips());
        }
        else
        {
            isOpen = false;
            body.SetActive(false);
        }
    }
    IEnumerator LockKeys()
    {
        if (isOpen)
        {
            bool bl = false;
            for (int i = 0; i < workBorder.Count; i++)
            {
                GameObject obj = workBorder[i];
                Rect rect = obj.GetComponent<RectTransform>().rect;
                if (rect.width == 100 && rect.height == 100)
                {
                    continue;
                }
                else
                {
                    rect.width = rect.width - 5 > 100 ? rect.width - 5 : 100;
                    rect.height = rect.height - 5 > 100 ? rect.height - 5 : 100;
                    obj.GetComponent<RectTransform>().sizeDelta = new Vector2(rect.width, rect.height);
                    bl = true;
                }
            }
            if (bl)
            {
                yield return new WaitForSeconds(0.05f);
                StartCoroutine(LockKeys());
            }
        }
        else
        {
            yield return new WaitForSeconds(0.05f);
            StartCoroutine(LockKeys());
        }
    }
    /// <summary>
    /// 打开关闭提示
    /// </summary>
    public void OpenClose()
    {
        //if (GlobalClass.sys != SYS.web)
        //    return;
        if (isOpen)
        {
            body.transform.localPosition = bodyPositon;
            StartCoroutine(CloseTips());
        }
        else
        {
            body.SetActive(true);
            body.transform.localPosition = target.localPosition + target.parent.localPosition;
            Debug.Log("1");
            StartCoroutine(OpenTips());
        }
    }
}
