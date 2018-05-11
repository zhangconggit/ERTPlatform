// ////////////////////////////////////////// 功能分析 ////////////////////////////////////////////////
// 1. 虚拟按键与真实按键绑定 2. 单击与长按事件功能 3. 按下时保持按下图标                                     //
// 4. 提供参数Magnification来设置按下时按钮尺寸的变化 5. 提供参数改变长按事件多少秒执行一次                    //
// //////////////////////////////////////////////////////////////////////////////////////////////////
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System;
using UnityEngine.UI;

namespace CFramework
{
    public class UButton : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
    {

        [Tooltip("虚拟键位")]
        public KeyCode code;

        //单击事件
        VoidDelegate OnKeyEvent = null;

        //长按事件
        VoidDelegate OnLongKeyEvent = null;

        //常态图标
        public Sprite normalImage;

        //按下图标
        public Sprite pressImage;

        Image buttonImage;

        [HideInInspector]
        public Text text;

        [Tooltip("是否启用保持按钮属性")]
        public bool enabledKeep = false;

        [Tooltip("点击后改变scale的倍率")]
        [Range(0, 2)]
        public float magnification = 0;

        [Tooltip("是否启用长按事件")]
        public bool enabledLongDown = false;

        [HideInInspector]//长按事件执行间隔时长(秒)
        public float durationThreshold = 0.1f;
        //按下
        bool isDown = false;

        //长按触发
        bool longPressTrigger = false;

        //按下计时
        float timePressStarted;

        //是否是首次执行长按事件
        bool firstLongClick = true;

        // Use this for initialization
        void Start()
        {
            buttonImage = GetComponent<Image>();
        }

        
        // Update is called once per frame
        void Update()
        {
            if (enabledLongDown)
            {
                if(Input.GetKeyDown(code))
                {
                    Down();
                }

                if(Input.GetKeyUp(code))
                {
                    Up();
                }

                if (isDown && !longPressTrigger && OnLongKeyEvent != null)
                {
                    if (Time.time - timePressStarted > durationThreshold || firstLongClick)
                    {
                        timePressStarted = Time.time;
                        firstLongClick = false;
                        OnLongKeyEvent.Invoke();
                    }
                }
            }
            else
            {
                if (Input.GetKeyDown(code) && OnKeyEvent != null)
                {
                    OnKeyEvent();
                }
            }

        }
        //事件触发方法
        public void OnPointerClick(PointerEventData eventData)
        {
            if (!enabledLongDown)
            {
                if (OnKeyEvent != null)
                    OnKeyEvent();
            }

        }


        public void AddListionEvent(VoidDelegate CallBack)
        {
            if (!enabledLongDown)
                OnKeyEvent += CallBack;
            else
                OnLongKeyEvent += CallBack;
        }
        public void RemoveListionEvent(VoidDelegate CallBack)
        {
            if (!enabledLongDown)
                OnKeyEvent -= CallBack;
            else
                OnLongKeyEvent -= CallBack;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            Down();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            Up();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (enabledLongDown)
            {
                isDown = false;
                longPressTrigger = true;
            }
        }


        public void ClearAllListener()
        {
            OnKeyEvent = null;
        }

        void Down()
        {
            if (enabledLongDown)
            {
                timePressStarted = Time.time;
                isDown = true;
                longPressTrigger = false;
                firstLongClick = true;
            }

            if (enabledKeep == true)
            {
                if (buttonImage.sprite == pressImage)
                    buttonImage.sprite = normalImage;
                else
                    buttonImage.sprite = pressImage;
            }
            else
            {
                if (buttonImage != null && pressImage != null)
                {
                    buttonImage.sprite = pressImage;
                }
            }

            transform.localScale = transform.localScale * magnification;
        }

        void Up()
        {
            if (enabledLongDown)
            {
                isDown = false;
                longPressTrigger = true;
            }
            if (enabledKeep == false)
            {
                if (buttonImage != null && pressImage != null)
                {
                    buttonImage.sprite = normalImage;
                }
            }

            transform.localScale = transform.localScale * 1.0f / magnification;
        }
    }
}


