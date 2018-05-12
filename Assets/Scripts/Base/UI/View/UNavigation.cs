
using progressControl;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CFramework
{
    /// <summary>
    /// 导航栏
    /// </summary>
    internal class UNavigation : UPageBase
    {
        GameObject Navigation;
        NavigationInfo currentNavigationInfo;
        Text time;
        bool startCount = false;
        List<NavigationInfo> map = new List<NavigationInfo>();
        public CEvent onClickFull = new CEvent();
        public CEvent onClickCommit = new CEvent();
        public CEvent onClickBack = new CEvent();
        public CEvent<string> onClickPorgress = new CEvent<string>();
        UPageButton commitButton;
        UPageButton backButton;

        private Vector2 defultScreen = new Vector2(0f, 0f);

        public UNavigation(List<_CLASS_CatalogProperty> list)
        {
            name = "UNavigation";
            onClickFull.AddListener(FullScreen);
            onClickCommit.AddListener(CommitScore);
            onClickBack.AddListener(GoBack);

            CsScoreManager.Instance.init();
           
            //Debug.Log("list.count = " + list.Count);
            Navigation = UIRoot.Instance.InstantiateCustom("Navigation");
            Navigation.SetActive(true);
            Navigation.transform.SetParent(transform);
            Navigation.transform.localScale = new Vector3(1 * fAdapterWidth, 1 * fAdapterHeight, 1);
            Navigation.transform.localPosition = new Vector3(0, 0, 0);
            Navigation.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(140 * fAdapterWidth, 40 * fAdapterHeight);
            CProgressControl porgress = Navigation.transform.Find("catalog").GetComponent<CProgressControl>();
            porgress.stepInfo.Clear();
            float _scale = 1;
            if (list.Count > 7)
            {
                _scale = 7f / list.Count;
            }

            for (int i = 0; i < list.Count; i++)
            {
                StepCollection item = new StepCollection();
                GameObject porgressImage = UIRoot.Instance.InstantiateCustom("progressImage");
                porgressImage.SetActive(true);
                porgressImage.transform.SetParent(Navigation.transform.Find("catalog/background"));
                porgressImage.transform.localScale = new Vector3(1 * fAdapterWidth * _scale, 1 * fAdapterHeight * _scale, 1);
                porgressImage.transform.localPosition = new Vector3(0, (125 * _scale * list.Count / 2f - 125 * _scale * i) * fAdapterHeight, 0);
                string ClickStepName = list[i].catalogClickStepName;
                porgressImage.GetComponent<UButton>().AddListionEvent(() =>
                {
                    onClickPorgress.Invoke(ClickStepName);
                    UpdateAllNavigation();
                });

                item.stepDesc = list[i].catalogName;
                item.stepObject = porgressImage.transform;
                if (i == list.Count - 1)
                    item.isLastOne = true;
                porgress.stepInfo.Add(item);

                NavigationInfo info = new NavigationInfo();
                info.navigationName = list[i].catalogName;
                for (int k = 0; k < list[i].catalogChildStepName.Length; k++)
                {
                    info.childStepState.Add(list[i].catalogChildStepName[k], false);
                }
                map.Add(info);
            }

            //if(CGlobal.mode != OperationMode.Game)
            {
                backButton = new UPageButton(AnchoredPosition.bottom);
                backButton.SetParent(Navigation.transform.Find("catalog/background"));
                backButton.button.transition = ButtonTransition.ColorTint;
                backButton.button.GetComponent<Image>().color = new Color(0, 0, 0, 0);
                backButton.button.normalColor = new Color(0, 0, 0, 0);
                backButton.button.pressColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);
                backButton.button.text.color = Color.white;
                backButton.name = "backButton";
                backButton.text = "返回登陆";
                backButton.rect = new Rect(0, -66, 150, 65);
                backButton.onClick.AddListener(() => { onClickBack.Invoke(); });
            }

            commitButton = new UPageButton(AnchoredPosition.bottom);
            commitButton.SetParent(Navigation.transform.Find("catalog/background"));
            //commitButton.button.
            commitButton.button.GetComponent<Image>().color = new Color(0, 0, 0, 0);
            commitButton.button.normalColor = new Color(0, 0, 0, 0);
            commitButton.button.pressColor = new Color(0.1f, 0.1f, 0.1f, 0.8f);
            commitButton.button.text.color = Color.white;
            commitButton.button.transition = ButtonTransition.ColorTint;
            commitButton.name = "commitButton";
            commitButton.text = "提交成绩";
            commitButton.rect = new Rect(0, -110f, 150, 65);
            commitButton.onClick.AddListener(() => { onClickCommit.Invoke(); });

            #region 计时器
            TimeManager.Instance.timeText = Navigation.transform.Find("catalog/background/timeCounter/timeText").gameObject;
            #endregion

        }
        public void IntoStep(string _stepName)
        {
            NavigationInfo current = map.Find(tmp =>
            {
                if (tmp.childStepState.ContainsKey(_stepName))
                    return true;
                else
                    return false;
            });
            if (current != null)
            {
                CProgressControl porgress = Navigation.transform.Find("catalog").GetComponent<CProgressControl>();
                porgress.twinklingStepIconByDesc(current.navigationName);
                current.childStepState[_stepName] = true;
            }
            currentNavigationInfo = current;
        }
        public void OutStep(string _stepName)
        {
            NavigationInfo current = map.Find(tmp =>
            {
                if (tmp.childStepState.ContainsKey(_stepName))
                    return true;
                else
                    return false;
            });
            current.childStepState[_stepName] = StepManager.Instance.checkStepFlished(_stepName);
            if (current != null)
            {
                CProgressControl porgress = Navigation.transform.Find("catalog").GetComponent<CProgressControl>();
                if (current.IsFinish())
                    porgress.setStepHightLightByDesc(current.navigationName);
                else
                    porgress.setStepSourceByDesc(current.navigationName);
            }
            currentNavigationInfo = null;
        }
        void UpdateAllNavigation()
        {
            CProgressControl porgress = Navigation.transform.Find("catalog").GetComponent<CProgressControl>();
            foreach (var item in map)
            {
                item.UpdateStepState();
                if (currentNavigationInfo != null)
                {
                    if (item.navigationName == currentNavigationInfo.navigationName)
                        continue;
                }
                if (item.IsFinish())
                {
                    porgress.setStepHightLightByDesc(item.navigationName);
                }
                else
                {
                    porgress.setStepSourceByDesc(item.navigationName);
                }
            }
        }
        void CommitScore()
        {
            UMessageBox.Show("提交成绩", "是否确认提交成绩？", "是", () => {
                CsScoreManager.Instance.CommitScore();
            }, "否", ()=>
            {

            });
        }

        void FullScreen()
        {
            string text = Navigation.transform.Find("catalog/background").Find("fullButton").GetComponent<UButton>().text.text;
            if (text == "退出全屏")
            {
                Screen.SetResolution((int)defultScreen.x, (int)defultScreen.y, true);
            }
            else
            {
                if (defultScreen.x == 0 && defultScreen.y == 0)
                {
                    defultScreen.x = Screen.width;
                    defultScreen.y = Screen.height;
                }
                Screen.SetResolution(1920, 1080, true);
            }
        }
        void GoBack()
        {
           
        }

        internal void EnabledNavigation(bool isEnabled)
        {
            if (isEnabled && !startCount)
            {
                startCount = true;
                if (CGlobal.userName == "" || CGlobal.userName == "访客")
                    commitButton.gameObejct.SetActive(false);
                else
                    commitButton.gameObejct.SetActive(true);

                string maxtime = "30";
                float fMaxTime = 30;
                if (float.TryParse(maxtime, out fMaxTime))
                {
                    TimeManager.Instance.SetMaxTime(fMaxTime * 60);
                }
                else
                {
                    fMaxTime = 30;
                    TimeManager.Instance.SetMaxTime(30 * 60);
                }
                string abs = "1";
                TimeManager.Instance.StartTime(abs != "1");
            }
            else if (isEnabled == false)
            {
                TimeManager.Instance.StopTime();
                startCount = false;

            }
            this.gameObejct.SetActive(isEnabled);
        }
        internal void OnDestory()
        {
            TimeManager.Instance.StopTime();
            if (this.commitButton != null)
            {
                this.commitButton.Destroy();
            }
            if (this.backButton != null)
            {
                this.backButton.Destroy();
            }
            Object.DestroyImmediate(Navigation);
            base.Destroy();
        }
    }
    /// <summary>
    /// 导航栏信息
    /// </summary>
    class NavigationInfo
    {
        public string navigationName;
        public Dictionary<string, bool> childStepState = new Dictionary<string, bool>();
        public bool IsFinish()
        {
            foreach (var item in childStepState)
            {
                if (item.Value == false)
                    return false;
            }
            return true;
        }
        public void UpdateStepState()
        {
            List<string> list = new List<string>();
            foreach (var item in childStepState.Keys)
            {
                list.Add(item);

            }
            for (int i = 0; i < list.Count; i++)
            {
                childStepState[list[i]] = StepManager.Instance.checkStepFlished(list[i]);
            }

        }
    }
}