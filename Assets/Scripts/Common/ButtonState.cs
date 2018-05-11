using UnityEngine;  
using UnityEngine.EventSystems;  
using System.Collections;
using UnityEngine.UI;  
/// <summary>  
/// 脚本位置：UGUI按钮组件身上  
/// 脚本功能：实现按钮长按状态的判断  
/// 创建时间：2015年11月17日  
/// </summary>  

public enum UButtonStates
{
    NULL,//无状态
    down,//按下
    press,//按住
    up,//弹起
    click //单击
}
// 继承：按下，抬起和离开的三个接口  
public class ButtonState :MonoBehaviour,IPointerDownHandler,IPointerUpHandler,IPointerExitHandler  
{
    //public Color prssedColor;
    // 延迟时间  
    private float delay = 0.2f;
    public UButtonStates state = UButtonStates.NULL;
    // 按钮是否是按下状态  
    private bool isDown = false;  
      
    // 按钮最后一次是被按住状态时候的时间  
    private float lastIsDownTime;
    //bool isExit;
    //Color beforColor;
    void Start()
    {
        //beforColor = prssedColor;
        //isExit = false;
    }
    void Update ()  
    {  
        // 如果按钮是被按下状态  
        //if (isDown) {  
        //// 当前时间 -  按钮最后一次被按下的时间 > 延迟时间0.2秒  
        //    if (Time.time - lastIsDownTime > delay) {  
        //        // 触发长按方法  
        //        Debug.Log("长按");  
        //        // 记录按钮最后一次被按下的时间  
        //        lastIsDownTime = Time.time;
        //        state = UButtonStates.press;
      
        //    }  
        //}
        if(state != UButtonStates.NULL)
            Debug.Log("Button is:" + state);
      
    }  
    void OnEnable()
    {
        state = UButtonStates.NULL;
        isDown = false;
    }
    void LateUpdate()
    {
        if (state == UButtonStates.down)
        {
            state = UButtonStates.press;
        }
        else if (!isDown && state == UButtonStates.press)
        {
            state = UButtonStates.up;
        }
        else if (state == UButtonStates.up && isDown)
        {
            state = UButtonStates.click;
            isDown = false;
        }
        else if (state == UButtonStates.up || state == UButtonStates.click)
            state = UButtonStates.NULL;
    }
    // 当按钮被按下后系统自动调用此方法  
    public void OnPointerDown (PointerEventData eventData)
    {
        state = UButtonStates.down;
        isDown = true;
        lastIsDownTime = Time.time;
        Debug.Log("Button is down");
        //if (gameObject.GetComponent<Image>() != null)
        //{
        //    beforColor = gameObject.GetComponent<Image>().color;
        //    gameObject.GetComponent<Image>().color = prssedColor;
        //}
    }  
      
    // 当按钮抬起的时候自动调用此方法  
    public void OnPointerUp (PointerEventData eventData)  
    {
        if (state != UButtonStates.press)
            state = UButtonStates.press;
        else
            state = UButtonStates.up;
        //isDown = false;  
        Debug.Log("Button is up");
        //if (gameObject.GetComponent<Image>() != null)
        //{
        //    //beforColor = gameObject.GetComponent<Image>().color;
        //    gameObject.GetComponent<Image>().color = beforColor;
        //}
    }  
      
    // 当鼠标从按钮上离开的时候自动调用此方法  
    public void OnPointerExit (PointerEventData eventData)  
    {
        //if (gameObject.GetComponent<Image>() != null)
        //{
        //    //beforColor = gameObject.GetComponent<Image>().color;
        //    gameObject.GetComponent<Image>().color = beforColor;
        //}
        state = UButtonStates.NULL;
        isDown = false;
        Debug.Log("Button is Exit");
    }
}  