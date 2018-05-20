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
    [HideInInspector]
    public bool NoErrorMode = false;
    Vector3 startModePos = Vector3.zero;
    Vector3 startTextPos = Vector3.zero;
    [HideInInspector]
    public float speed = 1;
    public Camera LookAtCamera = null;
    // Use this for initialization
    void Start () {
        if (LookAtCamera==null)
        {
            LookAtCamera = Camera.main;
        }
       
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
        if(startModePos == Vector3.zero && startTextPos == Vector3.zero)
        {
            startModePos = target.transform.position;
            startTextPos = transform.position;
        }
        else
        {
            transform.position = startTextPos;
            target.transform.position = startModePos;
        }

        okIcon.SetActive(false);
        errorIcon.SetActive(false);
        bSelected = false;//选中UI显示效果（打钩显示）
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
        Vector3 _textDir = GetUIMoveDir(startModePos, startModePos + _dir);//取得UI移动位置
        if (bSelected)
        {
            while ((target.transform.position - startModePos).magnitude < _dir.magnitude)//magnitude单位向量（target.transform.position - startModePos).magnitude模型移动的距离）
            {
                yield return 0;//休息一帧（ IEnumerator为协程）
                target.transform.position += speed * Time.deltaTime * 10 * _dir;//刷新一帧的间隔时间（speed * Time.deltaTime * 10 * _dir为向方向移动的位置）
                gameObject.transform.position += speed * Time.deltaTime * 10 * _textDir;
            }
            target.transform.position = startModePos + _dir;
            gameObject.transform.position = startTextPos + _textDir;
            if(!NoErrorMode)
                UpdateImage();
        }
        else
        {
            if (!NoErrorMode)
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
        return LookAtCamera.WorldToScreenPoint(end) - LookAtCamera.WorldToScreenPoint(start);//移动UI的位置
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
