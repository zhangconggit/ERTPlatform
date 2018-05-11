using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

[System.Serializable]
public enum MouseState
{
    NULL,
    down,
    up,
    click,
}
[System.Serializable]
public class HandObject
{
    public string name;
    public List<PointState> pointObejct;
}
[System.Serializable]
public class PointState
{
    public Vector3 pos;
    public MouseState state;
}
public class handTips : MonoBehaviour {
    public GameObject hand;
    public GameObject circle;
    public float circleSpeed = 0.05f;
    public float moveSpeed = 1f;
    /// <summary>
    /// 弃用
    /// </summary>
    public List<Vector3> clickPosList;
    /// <summary>
    /// 弃用
    /// </summary>
    public List<Vector3> movePosList;
    /// <summary>
    /// 新对象
    /// </summary>
    public List<HandObject> HandObjectList;
    Vector3 startPos;
    Vector3 endPos;
    Vector3 moveDir;
	// Use this for initialization
	void Start () {
        hand.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

	}
    /// <summary>
    /// 未完，待续
    /// </summary>
    /// <param name="name"></param>
    public void run(string name)
    {
        HandObject obj = HandObjectList.Find(me => {
            if (me.name == name)
                return true;
            else
                return false;
                });
        if(obj != null)
        {
            bool first = true;
            foreach (var item in obj.pointObejct)
            { 
                if(first)
                    hand.GetComponent<RectTransform>().position = item.pos;
                switch (item.state)
                {
                    case MouseState.NULL:
                        break;
                    case MouseState.down:
                        break;
                    case MouseState.up:
                        break;
                    case MouseState.click:
                        break;
                    default:
                        break;
                }
            }
            
        }
    }
    /// <summary>
    /// 点击提示
    /// </summary>
    /// <param name="clickId"></param>
    public void clickTips(int clickId)
    {
        StartCoroutine(circleChange(clickId));
    }
    /// <summary>
    /// 拖动提示
    /// </summary>
    /// <param name="clickId"></param>
    public void moveTips(int moveId)
    {
        StartCoroutine(postionChange(moveId));
    }
    IEnumerator circleChange(int clickId)
    {
        hand.SetActive(true);
        hand.GetComponent<RectTransform>().localPosition = clickPosList[clickId];
        circle.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        circle.GetComponent<Image>().color = new Color(1, 1, 1, 1);// = 1f;
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(circleChangeC());
        //yield return new WaitForSeconds(0.2f);
        circle.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        circle.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        yield return StartCoroutine(circleChangeC());
        circle.GetComponent<RectTransform>().localScale = new Vector3(0.5f, 0.5f, 0.5f);
        circle.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        yield return StartCoroutine(circleChangeC());
        yield return new WaitForSeconds(0.5f);
        circle.GetComponent<RectTransform>().localScale.Set(1, 1, 1);
        circle.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        hand.SetActive(false);
    }
    IEnumerator circleChangeC()
    {
        Vector3 sc= circle.GetComponent<RectTransform>().localScale;
        circle.GetComponent<RectTransform>().localScale += new Vector3(circleSpeed,circleSpeed,circleSpeed);//.Set(sc.x + circleSpeed, sc.y + circleSpeed, sc.z + circleSpeed);
        circle.GetComponent<Image>().color -= new Color(1, 1, 1, circleSpeed*2);
        if (circle.GetComponent<RectTransform>().localScale.x >= 1)
        {
            yield return 0;
        }
        else
        {
            yield return new WaitForFixedUpdate();// WaitForSeconds(0.01f);
            yield return StartCoroutine(circleChangeC());
        }
    }
    IEnumerator postionChange(int moveId)
    {
        startPos = movePosList[moveId * 2];
        endPos = movePosList[moveId * 2 + 1];
        circle.GetComponent<RectTransform>().localScale = new Vector3(0.3f, 0.3f, 0.3f);
        moveDir = endPos - startPos;
        moveDir.Normalize();
        moveDir *= moveSpeed;
        hand.GetComponent<RectTransform>().localPosition = startPos;
        hand.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        yield return StartCoroutine(postionChangeC());
        yield return new WaitForSeconds(0.2f);
        hand.GetComponent<RectTransform>().localPosition = startPos;
        yield return StartCoroutine(postionChangeC());
        yield return new WaitForSeconds(0.5f);
        hand.SetActive(false);
    }
    IEnumerator postionChangeC()
    {
        hand.GetComponent<RectTransform>().localPosition += moveDir;
        if (Vector3.Distance(hand.GetComponent<RectTransform>().localPosition, endPos) < moveDir.magnitude*2)
            yield return 0;
        else
        {
            yield return new WaitForFixedUpdate();// WaitForSeconds(0.01f);
            yield return StartCoroutine(postionChangeC());
        }
    }
}
