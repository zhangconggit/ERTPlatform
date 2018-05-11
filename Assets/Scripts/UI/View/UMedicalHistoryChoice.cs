using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

namespace CFramework
{
    public class UMedicalHistoryChoice : UPageBase
    {
        public CEvent OnConfirm = new CEvent();
        GameObject medicalBase;
        public CMedicalHistoryItem selected;

        float ButtonSpace = 80;
        public UMedicalHistoryChoice()
        {
            gameObejct.name = "MedicalHistoryChoice";
            SetAnchored(AnchoredPosition.full);
            SetBorderSpace(0, 0, 0, 0);
            medicalBase = UIRoot.Instance.InstantiatePrefab(UIRoot.Instance.GetCustomObject("MedicalHistoryChoice"));
            medicalBase.transform.parent = transform;
            medicalBase.transform.GetComponent<RectTransform>().localPosition = new Vector3(0, 0, 0);
            medicalBase.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);
            medicalBase.transform.localScale = new Vector3(1, 1, 1);
            medicalBase.name = "medicalBase";
            medicalBase.SetActive(true);

            medicalBase.transform.Find("Button_OK").GetComponent<UButton>().AddListionEvent(() => { OnConfirm.Invoke(); });
        }
        /// <summary>
        /// 设置欢迎用户字样
        /// </summary>
        /// <param name="name"></param>
        public void SetWelcome(string value)
        {
            medicalBase.transform.Find("Welcome_text").GetComponent<Text>().text = name;
        }
        /// <summary>
        /// 设置模式名称
        /// </summary>
        /// <param name="name"></param>
        public void SetModelName(string model)
        {
            medicalBase.transform.Find("Text_Model").GetComponent<Text>().text = model;
        }
        public void LoadMedicalRecord(List<MedicalRecord> recordList)
        {
            GameObject MedicalHistoryRoot = medicalBase.transform.Find("Panel_MedicalHistory").gameObject;
            int id = 0;
            foreach (MedicalRecord item in recordList)
            {
                GameObject obj = UIRoot.Instance.InstantiatePrefab(UIRoot.Instance.GetCustomObject("MedicalHistory"));//Resources.Load<GameObject>("UIPrefabs/MedicalHistory")); //(GameObject)Resources.Load("Prefab/MedicalRecord.prefab");
                obj.transform.SetParent(MedicalHistoryRoot.transform);
                obj.transform.localPosition = new Vector3(0, 0);
                obj.transform.localScale = new Vector3(1, 1, 1);
                obj.SetActive(true);
                CMedicalHistoryItem historyItem = obj.GetComponent<CMedicalHistoryItem>();
                historyItem.ToggleButton.GetComponent<Toggle>().group = MedicalHistoryRoot.GetComponent<ToggleGroup>();
                historyItem.ToggleButton.transform.localPosition -= new Vector3(0, ButtonSpace * id, 0);
                id++;
                historyItem.MedicalHistory = "病例" + id.ToString();
                historyItem.LoadData(item);
                historyItem.OnSelected.AddListener(()=> { selected = historyItem;  });

                if (selected == null)
                {
                    selected = historyItem;
                    historyItem.ToggleButton.GetComponent<Toggle>().isOn = true;
                    historyItem.Toggle_Me(true);
                }
            }

        }
        public void OnChange(CMedicalHistoryItem item)
        {
            selected = item;
        }
    }
}
