using UnityEngine;
using System.Collections;
using System;
using DG.Tweening;

namespace CFramework
{
    /// <Summary>
    /// 模型基类
    /// </Summary>
    public class ModelBase : CSingleton<ModelBase>
    {
        #region 构造

        protected ModelBase()
        {

        }

        /// <Summary>
        /// 构造函数，参数为模型唯一描述，默认初始方向和欧拉角均为（0，0，0）
        /// </Summary>
        public ModelBase(string desc, GameObject obj)
        {
            iniLocalPosition = Vector3.zero;
            initLocalRotation = Vector3.zero;
            initWorldRotation = Vector3.zero;
            iniWorldPosition = Vector3.zero;
            modelDesc = desc;
            modelObject = obj;
        }

        /// <Summary>
        /// 构造函数，参数为模型唯一描述
        /// </Summary>
        public ModelBase(Vector3 pIniLocalPos, Vector3 pIniLocalRot, string desc, GameObject obj)
        {
            iniLocalPosition = pIniLocalPos;
            initLocalRotation = pIniLocalRot;
            modelDesc = desc;
            modelObject = obj;
        }
        #endregion

        #region 属性

        /// <Summary>
        /// 模型
        /// </Summary>
        public GameObject modelObject;

        private string modelDesc;
        /// <Summary>
        /// 模型
        /// </Summary>
        public string ModelDesc
        {
            get { return modelDesc; }
        }

        private Vector3 iniLocalPosition;
        /// <Summary>
        /// 模型初始相对位置
        /// </Summary>
        public Vector3 IniLocalPosition
        {
            get { return iniLocalPosition; }
            set { iniLocalPosition = value; }
        }

        private Vector3 iniWorldPosition;
        /// <Summary>
        /// 模型初始世界位置
        /// </Summary>
        public Vector3 IniWorldPosition
        {
            get { return iniWorldPosition; }
            set { iniWorldPosition = value; }
        }

        private Vector3 initLocalRotation;
        /// <Summary>
        /// 模型初始相对欧拉角
        /// </Summary>
        public Vector3 InitLocalRotation
        {
            get { return initLocalRotation; }
            set { initLocalRotation = value; }
        }

        private Vector3 initWorldRotation;
        /// <Summary>
        /// 模型初始世界欧拉角
        /// </Summary>
        public Vector3 InitWorldRotation
        {
            get { return initWorldRotation; }
            set { initWorldRotation = value; }
        }

        /// <Summary>
        /// 模型结束状态世界坐标
        /// </Summary>
        private Vector3 finishedWorldPosition;
        public Vector3 FinishedWorldPosition
        {
            get { return finishedWorldPosition; }
            set { finishedWorldPosition = value; }
        }

        /// <Summary>
        /// 模型结束状态相对坐标
        /// </Summary>
        private Vector3 finishedLocalPosition;
        public Vector3 FinishedLocalPosition
        {
            get { return finishedLocalPosition; }
            set { finishedLocalPosition = value; }
        }

        /// <Summary>
        /// 模型结束状态世界方向
        /// </Summary>
        private Vector3 finishedWorldQuat;
        public Vector3 FinishedWorldQuat
        {
            get { return finishedWorldQuat; }
            set { finishedWorldQuat = value; }
        }

        /// <Summary>
        /// 模型结束状态相对方向
        /// </Summary>
        private Vector3 finishedLocalQuat;
        public Vector3 FinishedLocalQuat
        {
            get { return finishedLocalQuat; }
            set { finishedLocalQuat = value; }
        }

        /// <Summary>
        /// 要替换的模型
        /// </Summary>
        public ModelBase ReplaceModel;

        #endregion

        #region 函数

        /// <Summary>
        /// 模型重置
        /// </Summary>
        public void resetLocalModelTran()
        {
            modelObject.transform.localPosition = iniLocalPosition;
            modelObject.transform.localRotation = Quaternion.Euler(initLocalRotation);
        }

        /// <Summary>
        /// 模型重置
        /// </Summary>
        public void resetWorldModelTran()
        {
            modelObject.transform.position = iniWorldPosition;
            modelObject.transform.rotation = Quaternion.Euler(initWorldRotation);
        }

        /// <Summary>
        /// 设置模型去指定位置
        /// </Summary>
        public void SetModelToPos(DG.Tweening.TweenCallback callback,float pTime,bool isWorld=false, Ease moveStyle = Ease.Linear)
        {
            ModelCtrol.Instance.MoveAndRotateTo(modelObject, FinishedLocalPosition, FinishedLocalQuat, pTime, callback, isWorld,moveStyle);
        }

        /// <Summary>
        /// 设置模型去初始位置
        /// </Summary>
        public void SetReverseModelToPos(DG.Tweening.TweenCallback callback, float pTime, bool isWorld = false, Ease moveStyle = Ease.Linear)
        {
            ModelCtrol.Instance.MoveAndRotateTo(modelObject, IniLocalPosition, InitLocalRotation, pTime, callback, isWorld,moveStyle);
        }

        /// <Summary>
        /// 显示要替换的模型
        /// </Summary>
        public void ShowReplaceModel()
        {
            modelObject.SetActive(false);
            if (ReplaceModel.modelObject)
            {
                ReplaceModel.modelObject.SetActive(true);
                ReplaceModel.resetLocalModelTran();
            }
            else
            {
                Debug.Log("并无替换的模型");
            }
        }

        /// <Summary>
        /// 显示要替换的模型
        /// </Summary>
        public void ResetReplaceModel()
        {
            modelObject.SetActive(true);
            resetLocalModelTran();
            if (ReplaceModel.modelObject)
            {
                ReplaceModel.modelObject.SetActive(false);
                ReplaceModel.resetLocalModelTran();
            }
            else
            {
                Debug.Log("并无替换的模型");
            }
        }

        #endregion
    }
}





