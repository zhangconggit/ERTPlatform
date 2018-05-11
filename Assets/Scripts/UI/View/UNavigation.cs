
using progressControl;
using System.Collections.Generic;
using UnityEngine;

namespace CFramework
{
    /// <summary>
    /// 导航栏
    /// </summary>
    public class UNavigation : UPageBase
    {
        GameObject Navigation;
        /// <summary>
        /// 是否允许点击导航栏
        /// </summary>
        public bool IsCanClick {
           get { return bCanClick; }
            set { bCanClick = value; }
        }

        bool bCanClick;
        public UNavigation()
        {
            name = "UNavigation";

            //应许跳转步骤
            bCanClick = true;
            List<string> list = new List<string>();// AnalysisData.Instance.getProductCatalogProperty();
            Debug.Log("list.count = " + list.Count);
            Navigation = UIRoot.Instance.InstantiateCustom("Navigation");
            Navigation.SetActive(true);
            Navigation.transform.SetParent(transform);
            Navigation.transform.localScale = new Vector3(1, 1, 1);
            Navigation.transform.localPosition = new Vector3(0, 0, 0);
            Navigation.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(140, 40);
            CProgressControl porgress =Navigation.transform.Find("catalog").GetComponent<CProgressControl>();
            porgress.stepInfo.Clear();
            for (int i = 0; i < list.Count; i++)
            {
                StepCollection item = new StepCollection();
                GameObject porgressImage = UIRoot.Instance.InstantiateCustom("progressImage");
                porgressImage.SetActive(true);
                porgressImage.transform.SetParent(Navigation.transform.Find("catalog/background"));
                porgressImage.transform.localScale = new Vector3(1, 1, 1);
                porgressImage.transform.localPosition = new Vector3(0, (130* list.Count/2f - 130*i), 0);
                string ClickStepName = list[i];//.catalogClickStepName;
                porgressImage.GetComponent<UButton>().AddListionEvent(() => {
                    Debug.Log("bCanClick=" + bCanClick);
                    Debug.Log("StepName=" + ClickStepName);
                    if (bCanClick)
                    {
                        //StepConfigMain.mInstance.OnUIClick(ClickStepName);
                    }
                });

                item.stepDesc = list[i];//.catalogName;
                item.stepObject = porgressImage.transform;
                if (i == list.Count - 1)
                    item.isLastOne = true;
                porgress.stepInfo.Add(item);
            }
        }
    }
}