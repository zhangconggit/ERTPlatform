using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

namespace CFramework
{
    public class ModelEventSystem : CMonoSingleton<ModelEventSystem>
    {

        //3D模型点击事件委托
        public delegate void OnClick3DModelDelegate(RaycastHit obj);

        OnClick3DModelDelegate On3DModelListen;

        public static ModelEventSystem Instance
        {
            get
            {
                return CMonoSingleton<ModelEventSystem>.Instance();
            }
        }

        public void OnDestroy()
        {
            CMonoSingleton<ModelEventSystem>.OnDestroy();
        }


        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())//碰到了UI层
                {
                    return;
                }
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);//射线的起点
                RaycastHit hit;//射线的终点
                if (Physics.Raycast(ray, out hit))
                {
                    On3DModelListen.Invoke(hit);
                }
            }
        }

        /// <Summary>
        /// 为可点击的3d模型增加监听事件.参数依次为，可点击模型名称，点击事件
        /// </Summary>
        public void Add3DModelListenEvent(OnClick3DModelDelegate callBackFun)
        {
            On3DModelListen += callBackFun;
        }

        /// <Summary>
        /// 移除3D模型点击事件
        /// </Summary>
        public void Remove3DModelListenEvent(OnClick3DModelDelegate callBackFun)
        {
            On3DModelListen -= callBackFun;
        }

    }

}
