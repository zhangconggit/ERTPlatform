using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class screen : MonoBehaviour {
    int DefWidth=1080;
    int DefHight=1920;
    int fullWidth = 1080;
    int fullHight = 1920;
    bool full;
    bool px;
    Text text;
    public GameObject FullIcon;//全屏图标
    public GameObject UnFullIcon;//退出全屏图标
    public bool hasText = false;
	// Use this for initialization
	void Start () {
        full = false;
        
        var button = gameObject.GetComponent<Button>();
        if (FullIcon != null && !hasText)
            button.targetGraphic = FullIcon.GetComponent<Image>();
        if (button != null)
        {
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(onFull);
        }
        if(gameObject.transform.Find("Text") != null)
            text = gameObject.transform.Find("Text").gameObject.GetComponent<Text>();
        if (UnFullIcon != null)
            UnFullIcon.SetActive(false);
        if (FullIcon != null)
            FullIcon.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {

        //if (Input.GetKeyDown(KeyCode.Escape))
        //{
        //    full = false;
        //    if (text != null)
        //        text.text = "全屏模式";
        //}
        if(Screen.fullScreen && !full)
        {
            full = true;
            if (text != null)
                text.text = "退出全屏";
            if (UnFullIcon != null)
            {
                UnFullIcon.SetActive(true);
                if (!hasText)
                    gameObject.GetComponent<Button>().targetGraphic = UnFullIcon.GetComponent<Image>();
            }
            if (FullIcon != null)
                FullIcon.SetActive(false);
        }
        if (!Screen.fullScreen && full)
        {
            full = false;
            if (text != null)
                text.text = "全屏模式";
            if (UnFullIcon != null)
                UnFullIcon.SetActive(false);
            if (FullIcon != null)
            {
                FullIcon.SetActive(true);
                if (!hasText)
                    gameObject.GetComponent<Button>().targetGraphic = FullIcon.GetComponent<Image>();
            }
        }
        if (Input.GetKey(KeyCode.AltGr) && Input.GetKeyDown(KeyCode.S))
            if (px)
                px = false;
            else
                px = true;

	}
    void OnGUI()
    {
        if (px)
        {
            GUILayout.Label("screen: " + Screen.currentResolution.width + "×" + Screen.currentResolution.height);
            //int i = 0;
            //foreach(StepManager.string st in StepManager.StepClass.Instance.OperatingSequence)
            //{
            //    GUI.Label(new Rect(600+i/20*300,20+(i%20)*40,300,40),st.ToString());
            //    i++;
            //}
        }
    }
    public void onFull()
    {
        if (Screen.fullScreen)
        {
            Screen.SetResolution(DefHight, DefWidth, false);
            full = false;
            if (text != null)
                text.text = "全屏模式";
            if (UnFullIcon != null)
                UnFullIcon.SetActive(false);
            if (FullIcon != null)
            {
                FullIcon.SetActive(true);
                if (!hasText)
                    gameObject.GetComponent<Button>().targetGraphic = FullIcon.GetComponent<Image>();
            }
        }
        else
        {
            DefWidth = Screen.width;
            DefHight = Screen.height;
            Screen.SetResolution(fullHight, fullWidth, true);
            full = true;
            if (text != null)
                text.text = "退出全屏";
            if (UnFullIcon != null)
            {
                UnFullIcon.SetActive(true);
                if(!hasText)
                    gameObject.GetComponent<Button>().targetGraphic = UnFullIcon.GetComponent<Image>();
            }
            if (FullIcon != null)
                FullIcon.SetActive(false);
        }

    }
}
