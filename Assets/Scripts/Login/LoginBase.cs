#define owner2
#define QRCode
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Diagnostics;
using UnityEngine.SceneManagement;
using CFramework;

public class LoginBase : MonoBehaviour
{

    InputField userName;
    InputField userPassword;
    Text loginError;
    Transform exitWindow;
    Transform loadingImage;
    Text loadingText;
    [System.NonSerialized]
    public bool startCheckLogin = false;

    Transform userFieldInputCaret;
    Transform passwordFieldInputCaret;

    bool isGetCaret = false;

    Coroutine loadingCoroutine;
    bool isExit = false;
    bool Web = false;
    // Use this for initialization
    void Start()
    {
        
#if QRCode
        if (fileHelper.ReadIni("Fun", "userme", Config.ConfigPath) == "true")
        {
            //GameObject.Find("Canvas").SetActive(false);
            for (int i = 0; i < GameObject.Find("Canvas").transform.childCount; i++)
            {
                if (GameObject.Find("Canvas").transform.GetChild(i).name != "tuicu")
                {
                    GameObject.Find("Canvas").transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            isGetCaret = true;
            isExit = true;
            return;
        }
        else
        {
            GameObject.Find("BrowserQuad").SetActive(false);
            GameObject.Find("Canvas").transform.FindChild("tuicu").gameObject.SetActive(false);
        }
#endif
        loadingImage = GameObject.Find("Canvas/loadingImage").transform;
        if (loadingImage != null)
        {
            loadingText = loadingImage.Find("Text").GetComponent<Text>();
            loadingImage.gameObject.SetActive(false);
        }

        exitWindow = GameObject.Find("Canvas/exitWindow").transform;
        if (exitWindow != null)
            exitWindow.gameObject.SetActive(false);

        userName = GameObject.Find("Canvas/userField").GetComponent<InputField>();
        userPassword = GameObject.Find("Canvas/passwordField").GetComponent<InputField>();
        loginError = GameObject.Find("Canvas/error").GetComponent<Text>();
        UnityEngine.EventSystems.EventSystem.current.SetSelectedGameObject(GameObject.Find("Canvas/loginBt"));
        loginError.text = "";
        IDataComponentDLL.IDataComponent.GetInstance().setURL(Config.serverIp);
        Web = false;
       // IDataComponentDLL.IDataComponent.GetInstance().setURL(Config.sendIp);
        //IDataComponentDLL.IDataComponent.GetInstance().notifyComponentPlatform(false);
        IDataComponentDLL.IDataComponent.GetInstance().synchronizationServerTimeSystem();

    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.A))
        {
            userName.keyboardType = TouchScreenKeyboardType.NintendoNetworkAccount;
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            userName.keyboardType = TouchScreenKeyboardType.NumberPad;
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            userName.keyboardType = TouchScreenKeyboardType.NumbersAndPunctuation;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            userName.keyboardType = TouchScreenKeyboardType.ASCIICapable;
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            userName.keyboardType = TouchScreenKeyboardType.NamePhonePad;
        }
        if (isGetCaret == false)
        {
            userFieldInputCaret = GameObject.Find("Canvas/userField").transform.Find("userField Input Caret");
            passwordFieldInputCaret = GameObject.Find("Canvas/passwordField").transform.Find("passwordField Input Caret");

            if (passwordFieldInputCaret != null)
            {
                passwordFieldInputCaret.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 42);
                passwordFieldInputCaret.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.7f);
            }

            if (userFieldInputCaret != null)
            {
                userFieldInputCaret.GetComponent<RectTransform>().SetInsetAndSizeFromParentEdge(RectTransform.Edge.Bottom, 0, 42);
                userFieldInputCaret.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 0.7f);
            }
            if (passwordFieldInputCaret != null && userFieldInputCaret != null)
                isGetCaret = true;
        }
        if (startCheckLogin == true)
        {
            
            {
                string re = IDataComponentDLL.IDataComponent.GetInstance().checkLoginIsSucceed().Desc;
                if (re == "succeed")
                {
                    startCheckLogin = false;
                    CGlobal.user.chineseName = IDataComponentDLL.IDataComponent.GetInstance().getWebAccountChineseName();
                    loadingImage.gameObject.SetActive(false);
                    StopCoroutine(loadingCoroutine);
                    StepManager.Instance.SetStepEnd();
                    IDataComponentDLL.IDataComponent.GetInstance().sendQueryTrainID(CGrade.GradeTableId);
                    if (Web)
                    {
                        CGlobal.user.status = UserStatus.user;
                    }
                    else
                    {
                        CGlobal.user.status = UserStatus.server;
                    }
                }
                else if (re == "")
                {

                }
                else if (!Web)
                {
                    Web = true;
                    //失败 尝试服务器；
                    UnityEngine.Debug.Log("失败一次");
                    IDataComponentDLL.IDataComponent.GetInstance().setURL(Config.serverIp);
                    IDataComponentDLL.IDataComponent.GetInstance().sendLogin(userName.text, userPassword.text);
                }
                else
                {
                    loadingImage.gameObject.SetActive(false);
                    StopCoroutine(loadingCoroutine);
                    if (re == "invalid accountname or password.")
                    {
                        loginError.text = "*输入的用户名或密码错误";
                    }
                    else
                    {
                        loginError.text = "*网络故障，请检测网是否连接";
                    }
                    UnityEngine.Debug.LogWarning(re);
                    startCheckLogin = false;
                    GameObject.Find("Canvas/loginBt").GetComponent<Button>().enabled = true;
                }
            }
        }
        if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
        {
            login_button();
        }
    }

    public void userOrPasswordFieldChange()
    {
        loginError.text = "";
    }

    public void login_button()
    {
        ////CModelManager.currentSceneName = "ModelChoice";
        //SceneManager.LoadScene("loading");
        //return;
        if (userName.text == "")
        {
            loginError.text = "*用户名不能为空";
            //Speak.Instance.playAudio("loginError");
            return;
        }
        else if (userPassword.text == "")
        {
            loginError.text = "*密码不能为空";
            //Speak.Instance.playAudio("loginError");
            return;
        }
#if owner
        if(userName.text == "misrobot" && userPassword.text == "123456")
        {
            CGlobal.user.accountName = userName.text;
            CGlobal.user.chineseName = "misrobot";
            CGlobal.user.isLogin = true;
            CGlobal.user.sex = "v";
            CGlobal.user.status = UserStatus.server;
            UnityEngine.SceneManagement.SceneManager.LoadScene("ModelChoice");
        }
#else
        loadingImage.gameObject.SetActive(true);
        loadingCoroutine = StartCoroutine(showLoading(true));
        GameObject.Find("Canvas/loginBt").GetComponent<Button>().enabled = false;
        CGlobal.user.accountName = userName.text;
        CGlobal.user.sex = "v";
            IDataComponentDLL.IDataComponent.GetInstance().sendLogin(userName.text, userPassword.text);
        startCheckLogin = true;
#endif
    }
    public void login_button_null()
    {
        CGlobal.user.chineseName = "访客";
        StepManager.Instance.SetStepEnd();
        CGlobal.user.status = UserStatus.visitor;
    }
    public void reStart()
    {
        if (exitWindow.gameObject.activeSelf == false)
        {
            exitWindow.gameObject.SetActive(true);
        }
        exitWindow.Find("bg/contentTxt").GetComponent<Text>().text = "即将重新启动系统";
        isExit = true;

    }

    public void yesBt()
    {

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        if (isExit)
        {
            GameObject.Find("Main Camera").GetComponent<shutDown>().shutDownBt();
        }
        else
        {
            GameObject.Find("Main Camera").GetComponent<shutDown>().restartBt();
            //DestroyImmediate(GameObject.Find("PCJson"));
            //DestroyImmediate(GameObject.Find("IDataComponent"));
            //DestroyImmediate(GameObject.Find("PCCameraControl"));
            //SceneManager.LoadScene("userlogin");
            //GameObject.Find("Main Camera").GetComponent<shutDown>().restartBt();
        }
#endif

    }

    public void noBt()
    {
        exitWindow.gameObject.SetActive(false);
    }

    public void exit()
    {
        if (exitWindow.gameObject.activeSelf == false)
        {
            exitWindow.gameObject.SetActive(true);
        }
        exitWindow.Find("bg/contentTxt").GetComponent<Text>().text = "即将关闭计算机";
        isExit = true;
        //StartCoroutine(showExitWindow());
    }

    IEnumerator showExitWindow()
    {
        if (exitWindow.gameObject.activeSelf == false)
        {
            exitWindow.gameObject.SetActive(true);
            yield return 0;
        }
        else
        {
            exitWindow.gameObject.SetActive(false);
            yield return new WaitForSeconds(0.05f);
            exitWindow.gameObject.SetActive(true);
        }
    }

    IEnumerator showLoading(bool isReset = false)
    {
        if (isReset == true)
        {
            loadingText.text = ".";
        }
        yield return new WaitForSeconds(0.3f);
        loadingText.text += ".";
        yield return new WaitForSeconds(0.3f);
        loadingText.text += ".";
        yield return new WaitForSeconds(0.3f);
        loadingText.text += ".";
        yield return new WaitForSeconds(0.3f);
        loadingText.text += ".";
        yield return new WaitForSeconds(0.3f);
        loadingText.text += ".";
        yield return new WaitForSeconds(0.3f);
        StartCoroutine(showLoading(true));
    }

    public void openUrl()
    {

    }
}
