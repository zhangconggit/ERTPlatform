using UnityEngine;
using CFramework;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// 虚拟按键脚本
/// <summary>

/// <summary>
/// 按钮枚举
/// </summary>
public enum virtualButtonEnum
{

    Button_0=48,
    /// <summary>
    /// 插管
    /// </summary>
    Button_1,
    /// <summary>
    /// 拔管
    /// </summary>
    Button_2,
    /// <summary>
    /// 嘱咐吞咽
    /// </summary>
    Button_3,
    /// <summary>
    /// 停止吞咽
    /// </summary>
    Button_4,
    /// <summary>
    /// 停止吞咽
    /// </summary>
    Button_5,
    /// <summary>
    /// 停止吞咽
    /// </summary>
    Button_6,
    Button_7,
    Button_8,
    Button_9,
    /// <summary>
    /// 抽注射器
    /// </summary>
    Button_A=97,
    Button_B,
    Button_C,
    /// <summary>
    /// 断开注射器
    /// </summary>
    Button_D,
    /// <summary>
    /// 注射器连接到鼻饲液
    /// </summary>
    Button_E,
    /// <summary>
    /// 分开阴唇
    /// </summary>
    Button_F,
    Button_G,
    Button_H,
    /// <summary>
    /// 插管
    /// </summary>
    Button_I,
    /// <summary>
    /// 连接集尿袋
    /// </summary>
    Button_J,
    Button_K,
    Button_L,
    Button_M,
    Button_N,
    Button_O,
    Button_P,
    /// <summary>
    /// 连接注射器到胃管
    /// </summary>
    Button_Q,
    Button_R,
    /// <summary>
    /// 推注射器
    /// </summary>
    Button_S,
    Button_T,
    Button_U,
    Button_V,
    /// <summary>
    ///注射器放到温开水中
    /// </summary>
    Button_W,
    Button_X,
    Button_Y,
    /// <summary>
    /// 连接注射器 和 注射生理盐水
    /// </summary>
    Button_Z,
    Button_Down=274,
    Button_Up=273,
    Button_Right=275,
    Button_Left=276
}

public class ButtonVirtualScript : InputManager
{
    public static new ButtonVirtualScript Instance
    {
        get
        {
            return CMonoSingleton<ButtonVirtualScript>.Instance();        
        }
    }

    public new void OnDestroy()
    {
        CMonoSingleton<ButtonVirtualScript>.OnDestroy();
    }
   

    // Dictionary<string, GameObject> myDicButton = new Dictionary<string, GameObject>();//Button字典
    UPageBase VirtualButtons;
    Dictionary<KeyCode, string> buttonImageList = new Dictionary<KeyCode, string>();
    Dictionary<KeyCode, string> buttonDefultText = new Dictionary<KeyCode, string>();
    private void Awake()
    {
        VirtualButtons = new UImage();
        VirtualButtons.gameObejct.GetComponent<Image>().enabled = false;
        VirtualButtons.name = "VirtualButtons";

        #region 填充显示字符
        for (int i = (int)KeyCode.A; i <= (int)KeyCode.Z; i++)
        {
            buttonImageList[(KeyCode)i] = ((KeyCode)i).ToString();
        }
        int k = 0;
        for (int i = (int)KeyCode.Alpha0; i <= (int)KeyCode.Alpha9; i++)
        {
            buttonImageList[(KeyCode)i] = k.ToString();
            k++;
        }
        buttonImageList[KeyCode.UpArrow] = "↑";
        buttonImageList[KeyCode.DownArrow] = "↓";
        buttonImageList[KeyCode.RightArrow] = "→";
        buttonImageList[KeyCode.LeftArrow] = "←";
        #endregion
        #region 默认文字
        buttonDefultText[KeyCode.A] = "抽注射器";
        buttonDefultText[KeyCode.D] = "断开注射器";
        buttonDefultText[KeyCode.E] = "注射器连接到鼻饲液";
        buttonDefultText[KeyCode.F] = "分开阴唇";
        buttonDefultText[KeyCode.Z] = "连接注射器";
        buttonDefultText[KeyCode.W] = "连接温开水中";
        buttonDefultText[KeyCode.S] = "推注射器";
        buttonDefultText[KeyCode.Q] = "连接到胃管";
        buttonDefultText[KeyCode.J] = "连接集尿袋";
        buttonDefultText[KeyCode.Alpha1] = "插管";
        buttonDefultText[KeyCode.Alpha2] = "回拉";
        buttonDefultText[KeyCode.Alpha3] = "嘱咐吞咽";
        buttonDefultText[KeyCode.Alpha4] = "停止吞咽";
        buttonDefultText[KeyCode.UpArrow] = "上调";
        buttonDefultText[KeyCode.DownArrow] = "下调";
        buttonDefultText[KeyCode.RightArrow] = "左调";
        buttonDefultText[KeyCode.LeftArrow] = "右调";
        #endregion
    }

    /// <summary>
    /// 按键调用
    /// </summary>
    /// <param name="buttonEnum">按键的名字枚举</param>
    /// <param name="vec2">按键的本地坐标</param>
    public GameObject VirtualButton(virtualButtonEnum buttonEnum, Vector2 vec2)
    {
        return VirtualButton((KeyCode)(int)buttonEnum,vec2);       
    }
    /// <summary>
    /// 按键调用
    /// </summary>
    /// <param name="buttonEnum">按键的名字枚举</param>
    /// <param name="vec2">按键的本地坐标</param>
    public GameObject VirtualButton(KeyCode buttonEnum, Vector2 vec2)
    {
        GameObject gos = GetButton(buttonEnum).gameObejct;// GameObject.Find(str);//查找对应按钮
        ModelCtrol.Instance.MoveModel(gos, vec2, 0.4f, null);//改变字典中物体的位置
        return gos;
    }
    /// <summary>
    /// 按键调用
    /// </summary>
    /// <param name="buttonEnum">按键的名字枚举</param>
    /// <param name="vec2">按键的本地坐标</param>
    public GameObject VirtualButton(virtualButtonEnum buttonEnum, Vector2 vec2,string text)
    {
        return VirtualButton((KeyCode)(int)buttonEnum, vec2,text);
    }
    /// <summary>
    /// 按键调用
    /// </summary>
    /// <param name="buttonEnum">按键的名字枚举</param>
    /// <param name="vec2">按键的本地坐标</param>
    public GameObject VirtualButton(KeyCode buttonEnum, Vector2 vec2, string text)
    {
        GameObject gos = VirtualButton(buttonEnum, vec2);
        
        GetButton(buttonEnum).Text = text;
        return gos;
    }
    /// <summary>
    /// 按键调用
    /// </summary>
    /// <param name="buttonEnum">按键的名字枚举</param>
    /// <param name="vec2">按键的本地坐标</param>
    public GameObject VirtualButton(KeyCode buttonEnum, Vector2 vec2, string text,string showImage)
    {
        buttonImageList[buttonEnum] = showImage;
        GameObject gos = VirtualButton(buttonEnum, vec2);
        GetButton(buttonEnum).Text = text;
        return gos;
    }
    public void RemoveVirtualButton()//移除按键
    {
        foreach (var item in VirtualButtons.GetChildren())
        {
            ((VirtaulButton)item).InitPos();
        }
    }

    /// <summary>
    /// 清除字典里的Button
    /// </summary>
    public void ClearDicButton()
    {
        foreach (var item in VirtualButtons.GetChildren())
        {
            ((VirtaulButton)item).onClick.RemoveAllListener();
        }
        //foreach (var  item in myDicButton)
        //{
        //    item.Value.GetComponent<UButton>().ClearAllListener();
        //}
        //myDicButton.Clear();
    }
    /// <summary>
    /// 取得虚拟按钮对象
    /// </summary>
    /// <param name="button"></param>
    /// <returns></returns>
    public VirtaulButton GetButton(KeyCode button)
    {
        VirtaulButton obj = (VirtaulButton)VirtualButtons.GetChild(button.ToString());
        if(obj == null)
        {

            string showimage = button.ToString();
            if (buttonImageList.ContainsKey(button))
                showimage = buttonImageList[button];
            string showtext = "按钮";
            if (buttonDefultText.ContainsKey(button))
                showtext = buttonDefultText[button];
            obj = new VirtaulButton(button, showtext, showimage);
            obj.SetParent(VirtualButtons);
        }
        return obj;
    }
}
/// <summary>
/// 虚拟按钮
/// </summary>
public class VirtaulButton: UPageBase
{
    /// <summary>
    /// 点击事件
    /// </summary>
    public CEvent onClick = new CEvent();
    Vector3 startPos = new Vector3(2000, 2000, 0);
    UButton baseButton;
    UText baseText;
    UText ImageText;
    Image baseImage;
    public VirtaulButton(KeyCode button,string text,string imageText)
    {
        name = button.ToString();
        rect = new Rect(startPos.x, startPos.y, 80, 80);
        baseButton = gameObejct.AddComponent<UButton>();
        baseButton.magnification = 0.8f;
        baseImage = gameObejct.AddComponent<Image>();
        baseImage.sprite = UIRoot.OnlineResources["keys_back"] as Sprite;
        baseButton.AddListionEvent(OnPress);

        ImageText = new UText(AnchoredPosition.center);
        ImageText.SetParent(this);
        ImageText.name = "ImageText";
        ImageText.rect = new Rect(0, -12.9f, 60, 60);
        ImageText.baseText.color = Color.black;
        ImageText.gameObejct.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        ImageText.text = imageText;

        baseText = new UText( AnchoredPosition.center);
        baseText.SetParent(this);
        baseText.name = "Text";
        baseText.rect = new Rect(0, 48.5f, 120, 40);
        baseText.baseText.color = Color.black;
        baseText.baseText.raycastTarget = false;
        baseButton.text = baseText.baseText;
        baseButton.text.alignment = TextAnchor.MiddleCenter;

        Text = text;
        baseButton.code = button;
    }
    void OnPress()
    {
        onClick.Invoke();
    }
    /// <summary>
    /// 长按
    /// </summary>
    public bool EnabledLongDown
    {
        get
        {
            return baseButton.enabledLongDown;
        }
        set
        {
            baseButton.enabledLongDown = value;
        }
    }
    /// <summary>
    /// 长按间隔时间
    /// </summary>
    public float IntervalTime
    {
        get
        {
            return baseButton.durationThreshold;
        }
        set
        {
            baseButton.durationThreshold = value;
        }
    }
    /// <summary>
    /// 显示文字
    /// </summary>
    public string Text {
        get
        {
            return baseText.text;
        }
        set
        {
            baseText.text = value;
            baseText.rect = new Rect(0, 48.5f, baseText.text.Length*30, 40);
        }
    }
    /// <summary>
    /// 保持按下状态
    /// </summary>
    public bool EnabledKeep
    {
        get
        {
            return baseButton.enabledKeep;
        }
        set
        {
            baseButton.enabledKeep = value;
        }
    }
    /// <summary>
    /// 初始化位置
    /// </summary>
    public void InitPos()
    {
        rect= new Rect(startPos.x, startPos.y, 80, 80);
    }
}








