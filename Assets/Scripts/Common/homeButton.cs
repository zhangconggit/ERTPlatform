using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class homeButton : MonoBehaviour {
    public List<GameObject> WebObjects;
    public List<GameObject> PcObjects;
	// Use this for initialization
	void Start () {
        foreach (GameObject obj in WebObjects)
        {
            obj.SetActive(false);
        }
        Vector3 pos = Vector3.zero;
        for (int i = PcObjects.Count; i > 0 ; i--)
        {
            GameObject obj = PcObjects[i - 1];
            obj.SetActive(false);
            if (CGlobal.mode ==  OperationMode.Teaching)
            {
                if (obj.name == "commitScore")
                {
                    PcObjects.Remove(obj);
                }
            }
            continue;
        }
        foreach (GameObject obj in PcObjects)
        {
            obj.SetActive(false);
        }
        var button = gameObject.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(onHome);
        }
        var tog = gameObject.GetComponent<Toggle>();
        if (tog != null)
        {
            tog.onValueChanged.RemoveAllListeners();
            tog.onValueChanged.AddListener(onHome);
        }
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void onHome()
    {
#if UNITY_WEBPLAYER
        foreach (GameObject obj in WebObjects)
        {
            if (obj.activeSelf)
            {
                StopCoroutine(TimeOutHide());
                obj.SetActive(false);
            }
            else
            {
                obj.SetActive(true);
                StopCoroutine(TimeOutHide());
                StartCoroutine(TimeOutHide());
            }
        }
#else
        foreach (GameObject obj in PcObjects)
        {
            if (obj.activeSelf)
            {
                StopCoroutine(TimeOutHide());
                obj.SetActive(false);
            }
            else
            {
                obj.SetActive(true);
                StopCoroutine(TimeOutHide());
            }
        }
#endif
    }
    void onHome(bool bl)
    {
        if (bl)
        {
            gameObject.transform.Find("Background").GetComponent<Image>().enabled = false;
            StopCoroutine(TimeOutHide());
            StartCoroutine(TimeOutHide());
        }
        else
        {
            StopCoroutine(TimeOutHide());
            gameObject.transform.Find("Background").GetComponent<Image>().enabled = true;
        }
#if UNITY_WEBPLAYER
            foreach (GameObject obj in WebObjects)
            {
               obj.SetActive(bl);
            }
#else
        foreach (GameObject obj in PcObjects)
            {
                obj.SetActive(bl);
            }
#endif
    }
    IEnumerator TimeOutHide()
    {
        yield return new WaitForSeconds(5f);
        var button = gameObject.GetComponent<Button>();
        if (button != null)
        {
            onHome();
        }
        var tog = gameObject.GetComponent<Toggle>();
        if (tog != null)
        {
            tog.isOn = false;
        }
    }
}
