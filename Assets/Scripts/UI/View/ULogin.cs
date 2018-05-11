
using UnityEngine;
using UnityEngine.UI;

namespace CFramework
{
    public class ULogin : UPageBase
    {
        /// <summary>
        /// 重启按钮
        /// </summary>
        public CEvent OnResetButton=new CEvent();
        /// <summary>
        /// 关机按钮
        /// </summary>
        public CEvent OnShutdownButton = new CEvent();
        /// <summary>
        /// 登录按钮
        /// </summary>
        public CEvent OnLoginButton = new CEvent();
        /// <summary>
        /// 访客按钮
        /// </summary>
        public CEvent OnVisitorButton = new CEvent();
        GameObject loginBase;
        public ULogin()
        {
            gameObejct.name = "Login";
            SetAnchored(AnchoredPosition.full);
            SetBorderSpace(0, 0, 0, 0);
            loginBase = UIRoot.Instance.InstantiatePrefab(UIRoot.Instance.GetCustomObject("Login"));
            loginBase.transform.parent = transform;
            loginBase.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            loginBase.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(100, 100);
            loginBase.transform.localScale = new Vector3(1, 1, 1);
            loginBase.name = "loginBase";
            loginBase.SetActive(true);
            loginBase.transform.Find("error").gameObject.SetActive(false);
            loginBase.transform.Find("loginBt").GetComponent<UButton>().AddListionEvent(() => { OnLoginButton.Invoke(); });
            loginBase.transform.Find("loginBt_null").GetComponent<UButton>().AddListionEvent(() => { OnVisitorButton.Invoke(); });
            loginBase.transform.Find("reSetBt").GetComponent<UButton>().AddListionEvent(() => { OnResetButton.Invoke(); });
            loginBase.transform.Find("exitBt").GetComponent<UButton>().AddListionEvent(() => { OnShutdownButton.Invoke(); });
        }
        /// <summary>
        /// 显示错误提示
        /// </summary>
        /// <param name="errorText">内容</param>
        public void ShowErrorText(string errorText)
        {
            loginBase.transform.Find("error").GetComponent<Text>().text = errorText;
            loginBase.transform.Find("error").gameObject.SetActive(true);
        }
        /// <summary>
        /// 隐藏错误提示
        /// </summary>
        public void HideErrorText()
        {
            loginBase.transform.Find("error").gameObject.SetActive(false);
        }
        /// <summary>
        /// 设置项目名称
        /// </summary>
        /// <param name="name"></param>
        public void SetProjectName(string name)
        {
            loginBase.transform.Find("title").GetComponent<Text>().text = name;
        }
        /// <summary>
        /// 设置项目英文名称
        /// </summary>
        /// <param name="name"></param>
        public void SetProjectNameEnglish(string name)
        {
            loginBase.transform.Find("titleEn").GetComponent<Text>().text = name;
        }
        /// <summary>
        /// 取得输入的用户名
        /// </summary>
        /// <returns></returns>
        public string GetInputUserName()
        {
            return loginBase.transform.Find("userField").GetComponent<InputField>().text;
        }
        /// <summary>
        /// 取得输入的密码
        /// </summary>
        /// <returns></returns>
        public string GetInputPassword()
        {
            return loginBase.transform.Find("passwordField").GetComponent<InputField>().text;
        }
    }
}
