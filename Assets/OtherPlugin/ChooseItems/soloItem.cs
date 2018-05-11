using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class soloItem : MonoBehaviour {
    public string textName {
        get
        {
            return transform.GetComponent<Text>().text;
        }
        set
        {
            transform.GetComponent<Text>().text = value;
        }
    }
    public GameObject okIcon;
    public GameObject errorIcon;
    public bool bSelected = false;
    /// <summary>
    /// 这个物品是对的
    /// </summary>
    public bool bIsOk = true;
    public GameObject target;
    bool NoErrorMode = false;
    Vector3 startModePos;
    Vector3 startTextPos;
    [HideInInspector]
    public float speed = 1;
    public Camera LookAtCamera = null;
    // Use this for initialization
    void Start () {
        startModePos = target.transform.position;
        startTextPos = transform.position;
        okIcon.SetActive(false);
        errorIcon.SetActive(false);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    /// <summary>
    /// 初始化
    /// </summary>
    public void Init()
    {
        transform.position = startTextPos;
        target.transform.position = startModePos;
        okIcon.SetActive(false);
        errorIcon.SetActive(false);
        bSelected = false;
    }
    /// <summary>
    /// 是否多选
    /// </summary>
    /// <returns></returns>
    public bool IsMoreSelect()
    {
        return !bIsOk && bSelected;
    }
    /// <summary>
    /// 是否漏选
    /// </summary>
    /// <returns></returns>
    public bool IsLostSelect()
    {
        return bIsOk && !bSelected;
    }
    /// <summary>
    ///位移后改变状态
    /// </summary>
    public void ChangeState(Vector3 _dir,float _speed = -1)
    { 
        if(_speed > 0 )
        {
            speed = _speed;
        }
        bSelected = !bSelected;
        StopCoroutine("MoveMe");
        StartCoroutine("MoveMe", _dir);
    }
    IEnumerator MoveMe(Vector3 _dir)
    {
        Vector3 _textDir = GetUIMoveDir(startModePos, startModePos + _dir);
        if (bSelected)
        {
            while ((target.transform.position - startModePos).magnitude < _dir.magnitude)
            {
                yield return 0;
                target.transform.position += speed * Time.deltaTime * 10 * _dir;
                gameObject.transform.position += speed * Time.deltaTime * 10 * _textDir;
            }
            target.transform.position = startModePos + _dir;
            gameObject.transform.position = startTextPos + _textDir;
            UpdateImage();
        }
        else
        {
            UpdateImage();
            while (Vector3.Dot(target.transform.position - startModePos, _dir) > 0)
            {
                yield return 0;
                target.transform.position -= speed * Time.deltaTime * 10 * _dir;
                gameObject.transform.position -= speed * Time.deltaTime * 10 * _textDir;
            }
            target.transform.position = startModePos;
            gameObject.transform.position = startTextPos;
        }
        
    }
    Vector3 GetUIMoveDir(Vector3 start, Vector3 end)
    {
        return LookAtCamera.WorldToScreenPoint(end) - LookAtCamera.WorldToScreenPoint(start);
    }
    /// <summary>
    /// 改变选择状态
    /// </summary>
    public void ChangeState()
    {
        bSelected = !bSelected;
        UpdateImage();
        
    }
    public void UpdateImage()
    {
        if (bSelected)
        {
            if (NoErrorMode)
                okIcon.SetActive(true);
            else
            {
                okIcon.SetActive(bIsOk);
                errorIcon.SetActive(!bIsOk);
            }
        }
        else
        {
            okIcon.SetActive(false);
            errorIcon.SetActive(false);
        }
    }
}
