using CFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.UI.View
{
    public delegate T CreateUIDelegate<T>();
    public class UPuncture
    {
        UImage imageZhen; //针
        UImage imageRound; //旋转点
        UImage image;  //胸腔图片
        UImage imageText; //胸腔层级文字
        Texture2D imageColor; //胸腔层级颜色

        int injectionIndex; //当前点注射次数
        int dept;//插入深度 像素
        public CreateUIDelegate<UImage> CreateUI;

        float angle;
        int angle_y;
        public UPuncture(CreateUIDelegate<UImage> create)
        {
            CreateUI = create;
            imageColor = UIRoot.Instance.UIResources["check_ChestCrossSection" + "_T"] as Texture2D;

            image = CreateUI.Invoke();
            image.name = "";
            image.SetAnchored(AnchoredPosition.center);
            image.rect = new Rect(820, -400, 256, 256);
            image.LoadImage("ChestCrossSection");

            imageText = CreateUI.Invoke();
            imageText.name = "";
            imageText.SetAnchored(AnchoredPosition.center);
            imageText.rect = new Rect(820, -400, 256, 256);
            imageText.LoadImage("text_ChestCrossSection");

            imageZhen = CreateUI.Invoke();
            imageZhen.name = "zhen";
            imageZhen.SetAnchored(AnchoredPosition.center);
            imageZhen.LoadImage("T_mini_port_needle_round");

            imageRound = CreateUI.Invoke();
            imageZhen.SetParent(imageRound);
            imageRound.name = "round";
            imageRound.SetAnchored(AnchoredPosition.center);
            imageRound.rect = new Rect(913, -399, 2, 2);
            imageRound.color = new Color(1, 0, 0);
            //imageRound.LoadImage("");
            imageZhen.rect = new Rect(195, 0, 256, 7);
        }

        /// <summary>
        /// 旋转
        /// </summary>
        /// <param name="isAddAngle"></param>
        public void DeflectionUp(bool isAddAngle)
        {
            injectionIndex = 1;
            imageRound.transform.Rotate(Vector3.forward, isAddAngle ? 5 : -5);
            angle += isAddAngle?5:-5;
        }

        /// <summary>
        /// 获取上下方向旋转角度
        /// </summary>
        /// <returns></returns>
        public float GetAngle()
        {
            return angle;
        }

        /// <summary>
        /// 获取穿刺针插入深度
        /// </summary>
        /// <returns></returns>
        public float GetDept()
        {
            return dept;
        }

        /// <summary>
        /// 穿刺针 图片 移动
        /// </summary>
        /// <param name="isAddDept"></param>
        public void SyringeForward(bool isAddDept)
        {
            dept += isAddDept ? 20 : -20 ;
            injectionIndex = 1;
            imageZhen.Position -= new Vector2(isAddDept ? 20 : -20, 0);
        }

        /// <summary>
        /// 注射麻药平面图 效果
        /// </summary>
        public void InjectionMapping()
        {
            if (injectionIndex == 5)
                return;
            UImage circular = CreateUI.Invoke();
            circular.name = "10";
            circular.SetAnchored(AnchoredPosition.center);
            circular.LoadImage("circular");
            circular.SetParent(imageZhen);
            circular.rect = new Rect(-128, 0, injectionIndex * 8, injectionIndex * 8);
            circular.SetParent(imageText);
            injectionIndex++;
        }

        /// <summary>
        /// 根据颜色判断插入的位置
        /// </summary>
        public Color GetColor()
        {
            var res = imageZhen.Position + imageRound.Position - image.Position;
            var col = imageColor.GetPixel((int)res.x, (int)res.y);
            Debug.Log(string.Format("{0}    {1}     {2}", col.r, col.g, col.b));
            return col;
        }

        /// <summary>
        /// 根据颜色判断插入的位置
        /// </summary>
        public string GetXQCCLocation()
        {
            var res = imageZhen.Position + imageRound.Position - image.Position;
            Color col = imageColor.GetPixel((int)res.x, (int)res.y);
            Debug.Log(string.Format("{0}    {1}     {2}", col.r, col.g, col.b));
            if (col == new Color(0, (147f / 255f), 0))//体外
            {
                return "体外";
            }
            else if (col == new Color(40f / 255f, 40f / 255f, 40f / 255f))//体外
            {
                return "皮下及脂肪";
            }
            else if (col == new Color(0, 0, 1))//背阔肌
            {
                return "背阔肌";
            }
            else if (col == new Color(1, 1, 0) || col == new Color(128f / 255f, 0, 0))//肋间内肌肉
            {
                return "肋间内肌肉";
            }
            else if (col == new Color(1, 1, 1))//胸腔
            {
                return "胸腔";
            }
            else if (col == new Color(1, 0, 0))//肺
            {
                return "肺";
            }
            else
            {
                return "";
            }
        }
    }
}
