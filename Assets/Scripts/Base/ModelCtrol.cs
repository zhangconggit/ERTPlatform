// ////////////////////////////////////////// 功能分析 ////////////////////////////////////////////////
//                                                                                                  //
//                                                                                                  //
// ///////////////////////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using System.Collections;
using DG.Tweening;
namespace CFramework
{
    /// <Summary>
    /// 模型控制类，控制模型动态操作：移动，旋转
    /// </Summary>
    public class ModelCtrol : ModelsManager
    {
        float pi = 3.1415926f;

        //进入场景时自动旋转相机控制
        bool isCameraRotateAroundTarget = false;

        //运动的节点
        GameObject mainObject;


        //手动旋转相机-绕Gameobject
        bool isCanRotateCamera = false;

        //手动绕点旋转-绕点
        bool isCanRotateCameraByPoint = false;

        //鼠标首次按下位置
        Vector3 mousPos = Vector3.zero;

        //自动绕点旋转
        bool isCameraRotateAroundPoint = false;

        //围绕目标点
        Vector3 targetPoint;

        //围绕Gameobject
        GameObject target;

        public static ModelCtrol Instance
        {
            get
            {
                return CMonoSingleton<ModelCtrol>.Instance();
            }
        }

        public void OnDestroy()
        {
            CMonoSingleton<ModelCtrol>.OnDestroy();
        }

        public override void ProcessMsg(CMsg msg)
        {

            ;
        }
        static GameObject staModel;
        public static GameObject Find(string path)
        {
            if(staModel == null)
            {
                staModel = GameObject.Find("Models");
            }
            return staModel.transform.Find(path).gameObject;
        }
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            #region 旋转动画-绕Gameobject和position
            //以动画方式绕某个目标旋转
            if (isCameraRotateAroundTarget)
            {
                mainObject.transform.RotateAround(target.transform.position, Vector3.up, 30 * Time.deltaTime);
                mainObject.transform.LookAt(target.transform);
            }

            //绕点旋转
            if (isCameraRotateAroundPoint)
            {
                mainObject.transform.RotateAround(targetPoint, Vector3.up, 30 * Time.deltaTime);
                mainObject.transform.LookAt(targetPoint);
            }

            #endregion

            #region 旋转-绕Gameobject和position
            //根据鼠标移动绕某个目标旋转
            if (isCanRotateCamera)
            {
                if (mousPos == Vector3.zero)
                    mousPos = Input.mousePosition;
                if (Input.GetMouseButton(0))
                {
                    Vector3 offset = Input.mousePosition - mousPos;
                    Camera.main.transform.RotateAround(target.transform.position, Vector3.up * offset.x, 120 * Time.deltaTime);
                    Camera.main.transform.LookAt(target.transform);
                    mousPos = Input.mousePosition;

                }
            }

            //根据鼠标移动绕某个点旋转
            if (isCanRotateCameraByPoint)
            {
                if (mousPos == Vector3.zero)
                    mousPos = Input.mousePosition;
                if (Input.GetMouseButton(0))
                {
                    Vector3 offset = Input.mousePosition - mousPos;
                    Camera.main.transform.RotateAround(targetPoint, Vector3.up * offset.x, 120 * Time.deltaTime);
                    Camera.main.transform.LookAt(targetPoint);
                    mousPos = Input.mousePosition;
                }
            }
            #endregion

        }
        /// <summary>
        /// Start is called on the frame when a script is enabled just before
        /// any of the Update methods is called the first time.
        /// </summary>
        void Start()
        {



        }

        /// <summary>
        /// LateUpdate is called every frame, if the Behaviour is enabled.
        /// It is called after all Update functions have been called.
        /// </summary>
        void LateUpdate()
        {

        }
        #region 运动接口--平移
        /// <Summary>
        /// 平移自定义模型
        /// </Summary>
        public void MoveModel(ModelBase pModel, Vector3 to, float pTime, TweenCallback call, bool isWorld = false, Ease moveStyle = Ease.Linear)
        {
            Tween tweener;
            if(!isWorld)
                tweener = pModel.modelObject.transform.DOLocalMove(to, pTime);
            else
                tweener = pModel.modelObject.transform.DOMove(to, pTime);
            tweener.SetUpdate(true);
            tweener.SetEase(moveStyle);
            tweener.OnComplete(call);
        }

        /// <Summary>
        /// 平移GameObject模型
        /// </Summary>
        public void MoveModel(GameObject pModel, Vector3 to, float pTime, TweenCallback call, bool isWorld=false,Ease moveStyle = Ease.Linear)
        {
            Tween tweener;
            if (!isWorld)
                tweener = pModel.transform.DOLocalMove(to, pTime);
            else
                tweener = pModel.transform.DOMove(to, pTime);
            tweener.SetUpdate(true);
            tweener.SetEase(moveStyle);
            tweener.OnComplete(call);
        }

        /// <Summary> 
        /// 平移模型-指定From和To
        /// </Summary>
        public void MoveModel(ModelBase pModel, Vector3 from, Vector3 to, float pTime, TweenCallback call, bool isWorld = false, Ease moveStyle = Ease.Linear)
        {
            pModel.modelObject.transform.localPosition = from;
            MoveModel(pModel, to, pTime, call, isWorld,moveStyle);
        }

        /// <Summary> 
        /// 平移模型-指定From和To
        /// </Summary>
        public void MoveModel(GameObject pModel, Vector3 from, Vector3 to, float pTime, TweenCallback call, bool isWorld = false, Ease moveStyle = Ease.Linear)
        {
            pModel.transform.localPosition = from;
            MoveModel(pModel, to, pTime, call,isWorld, moveStyle);
        }
        #endregion

        #region 运动接口--旋转
        /// <Summary>
        /// 旋转自定义模型
        /// </Summary>
        public void RotateModel(ModelBase pModel, Vector3 to, float pTime, TweenCallback call, bool isWorld = false, RotateMode mode = RotateMode.Fast)
        {
            Vector3 iEuler = Vector3.zero;
            iEuler.x = float.Parse(iEuler.x.ToString("F4"));
            iEuler.y = float.Parse(iEuler.y.ToString("F4"));
            iEuler.z = float.Parse(iEuler.z.ToString("F4"));
            if (isWorld)
            {
                iEuler = pModel.modelObject.transform.rotation.eulerAngles;
            }
            else
            {
                iEuler = pModel.modelObject.transform.localRotation.eulerAngles;
            }

            iEuler.x = iEuler.x % 360;
            iEuler.y = iEuler.y % 360;
            iEuler.z = iEuler.z % 360;

            if (iEuler.x < 0)
                iEuler.x += 360;
            if (iEuler.y < 0)
                iEuler.y += 360;
            if (iEuler.z < 0)
                iEuler.z += 360;

            if (isWorld)
            {
                pModel.modelObject.transform.rotation = Quaternion.Euler(iEuler);
            }
            else
            {
                pModel.modelObject.transform.localRotation = Quaternion.Euler(iEuler);
            }

            Vector3 euler = to;

            if (euler.x != 360)
                euler.x = euler.x % 360;
            if (euler.y != 360)
                euler.y = euler.y % 360;
            if (euler.z != 360)
                euler.z = euler.z % 360;

            if (euler.x < 0)
                euler.x += 360;
            if (euler.y < 0)
                euler.y += 360;
            if (euler.z < 0)
                euler.z += 360;

            if (iEuler.x > 180 && iEuler.x <= 360)
            {
                if ((euler.x > iEuler.x && euler.x <= 360) || (euler.x > 0 && euler.x <= iEuler.x - 180))
                {
                    euler.x = 360 + euler.x;
                }
            }
            else if (iEuler.x >= 0 && iEuler.x <= 180)
            {
                if (euler.x > iEuler.x + 180 && euler.x <= 360)
                {
                    euler.x = euler.x - 360;
                }
            }

            if (iEuler.y > 180 && iEuler.y <= 360)
            {
                if ((euler.y > 0 && euler.y <= iEuler.y - 180))
                {
                    euler.y = euler.y - 360;
                }
            }
            else if (iEuler.y >= 0 && iEuler.y <= 180)
            {
                if (euler.y > iEuler.y + 180 && euler.y <= 360)
                {
                    euler.y = euler.y - 360;
                }
            }

            if (iEuler.z > 180 && iEuler.z <= 360)
            {
                if ((euler.z > iEuler.z && euler.z <= 360) || (euler.z > 0 && euler.z <= iEuler.z - 180))
                {
                    euler.z = 360 + euler.z;
                }
            }
            else if (iEuler.z >= 0 && iEuler.z <= 180)
            {
                if (euler.z > iEuler.z + 180 && euler.z <= 360)
                {
                    euler.z = euler.z - 360;
                }
            }

            Quaternion quat = Quaternion.Euler(euler);
            Tweener tweener;
            if (isWorld)
            {
                tweener = pModel.modelObject.transform.DORotateQuaternion(quat, pTime);
            }
            else
            {
                tweener = pModel.modelObject.transform.DOLocalRotateQuaternion(quat, pTime);
            }
            tweener.SetUpdate(true);
            tweener.OnComplete(call);
        }

        /// <Summary>
        /// 旋转自定义模型
        /// </Summary>
        public void RotateModel(ModelBase pModel, Quaternion to, float pTime, TweenCallback call, bool isWorld = false)
        {
            RotateModel(pModel, to.eulerAngles, pTime, call, isWorld);
        }

        /// <Summary>
        /// 旋转模型
        /// </Summary>
        public void RotateModel(GameObject pObject, Quaternion to, float pTime, TweenCallback call,bool isWorld = false)
        {
            RotateModel(pObject, to.eulerAngles, pTime, call, isWorld);
        }

        /// <Summary>
        /// 旋转模型
        /// </Summary>
        public void RotateModel(GameObject pObject, Vector3 to, float pTime, TweenCallback call, bool isWorld = false, RotateMode mode = RotateMode.Fast)
        {
            Vector3 iEuler = Vector3.zero;
            iEuler.x = float.Parse(iEuler.x.ToString("F4"));
            iEuler.y = float.Parse(iEuler.y.ToString("F4"));
            iEuler.z = float.Parse(iEuler.z.ToString("F4"));
            if (isWorld)
            {
                iEuler = pObject.transform.rotation.eulerAngles;
            }
            else
            {
                iEuler = pObject.transform.localRotation.eulerAngles;
            }

            iEuler.x = iEuler.x % 360;
            iEuler.y = iEuler.y % 360;
            iEuler.z = iEuler.z % 360;

            if (iEuler.x < 0)
                iEuler.x += 360;
            if (iEuler.y < 0)
                iEuler.y += 360;
            if (iEuler.z < 0)
                iEuler.z += 360;

            if (isWorld)
            {
                pObject.transform.rotation = Quaternion.Euler(iEuler);
            }
            else
            {
                pObject.transform.localRotation = Quaternion.Euler(iEuler);
            }

            Vector3 euler = to;

            if (euler.x != 360)
                euler.x = euler.x % 360;
            if (euler.y != 360)
                euler.y = euler.y % 360;
            if (euler.z != 360)
                euler.z = euler.z % 360;

            if (euler.x < 0)
                euler.x += 360;
            if (euler.y < 0)
                euler.y += 360;
            if (euler.z < 0)
                euler.z += 360;

            if (iEuler.x > 180 && iEuler.x <= 360)
            {
                if ((euler.x > iEuler.x && euler.x <= 360) || (euler.x > 0 && euler.x <= iEuler.x - 180))
                {
                    euler.x = 360 + euler.x;
                }
            }
            else if (iEuler.x >= 0 && iEuler.x <= 180)
            {
                if (euler.x > iEuler.x + 180 && euler.x <= 360)
                {
                    euler.x = euler.x - 360;
                }
            }

            if (iEuler.y > 180 && iEuler.y <= 360)
            {
                if ((euler.y > 0 && euler.y <= iEuler.y - 180))
                {
                    euler.y = euler.y - 360;
                }
            }
            else if (iEuler.y >= 0 && iEuler.y <= 180)
            {
                if (euler.y > iEuler.y + 180 && euler.y <= 360)
                {
                    euler.y = euler.y - 360;
                }
            }

            if (iEuler.z > 180 && iEuler.z <= 360)
            {
                if ((euler.z > iEuler.z && euler.z <= 360) || (euler.z > 0 && euler.z <= iEuler.z - 180))
                {
                    euler.z = 360 + euler.z;
                }
            }
            else if (iEuler.z >= 0 && iEuler.z <= 180)
            {
                if (euler.z > iEuler.z + 180 && euler.z <= 360)
                {
                    euler.z = euler.z - 360;
                }
            }

            Quaternion quat = Quaternion.Euler(euler);
            Tweener tweener;
            if (isWorld)
            {
                tweener = pObject.transform.DORotateQuaternion(quat, pTime);
            }
            else
            {
                tweener = pObject.transform.DOLocalRotateQuaternion(quat, pTime);
            }
            tweener.SetUpdate(true);
            tweener.OnComplete(call);
        }
        #endregion


        #region 运动接口--平移+旋转

        /// <Summary>
        /// 设置模型运动到指定位置和方向
        /// </Summary>
        public void MoveAndRotateTo(GameObject pModelObject, Vector3 pPosition, Vector3 pEuler, float pTime, TweenCallback callback, bool isWorld = false,Ease moveStyle = Ease.Linear)
        {
            //TimeProfiler.Instance.SetTimeStart();
            MoveModel(pModelObject, pPosition, pTime, callback,isWorld, moveStyle);
            RotateModel(pModelObject, pEuler, pTime,null, isWorld);
            //TimeProfiler.Instance.SetTimeFinished2("Camera:Time="+pTime+",use time=");
        }

        /// <Summary>
        /// 设置模型运动到指定位置和方向
        /// </Summary>
        public void MoveAndRotateTo(GameObject pModelObject, Vector3 pPosition, Quaternion pEuler, float pTime, TweenCallback callback, bool isWorld=false,Ease moveStyle = Ease.Linear)
        {
            MoveModel(pModelObject, pPosition, pTime, null,isWorld, moveStyle);
            RotateModel(pModelObject, pEuler.eulerAngles, pTime, callback, isWorld);
        }

        #endregion

        /// <Summary>
        /// 相机镜头路径
        /// </Summary>
        public void CameraPath(Vector3 movePosition, Vector3 rotateTo, float pTime, TweenCallback call, bool isWorld = false, Ease moveStyle = Ease.Linear)
        {
            if (pTime == 0)
            {
               if(isWorld)
                {
                    Camera.main.transform.position = movePosition;
                    Camera.main.transform.rotation = Quaternion.Euler(rotateTo);
                }
               else
                {
                    Camera.main.transform.localPosition = movePosition;
                    Camera.main.transform.localRotation = Quaternion.Euler(rotateTo);
                }
                call();
            }
            else
            {
                // MoveModel(Camera.main.gameObject, movePosition, pTime, call,isWorld, moveStyle);
                // RotateModel(Camera.main.gameObject, rotateTo, pTime, null);
                MoveAndRotateTo(Camera.main.gameObject, movePosition, rotateTo, pTime, call, isWorld, moveStyle);
            } 
        }

       
        /// <Summary>
        /// 相机镜头路径
        /// </Summary>
        public void CameraPath(GameObject pObject, Vector3 movePosition, Vector3 rotateTo, float pTime, TweenCallback call, bool isWorld = false, Ease moveStyle = Ease.Linear)
        {
            MoveModel(pObject, movePosition, pTime, null,isWorld, moveStyle);
            RotateModel(pObject,rotateTo, pTime, call);
        }

        /// <Summary>
        /// 绕某点旋转是否启用
        /// </Summary>   
        public void EnabledRotateByPoint(bool isEnbaled, Vector3 point, GameObject obj)
        {
            mainObject = obj;
            isCanRotateCameraByPoint = isEnbaled;
            targetPoint = point;
        }

        /// <Summary>
        /// 绕某物旋转
        /// </Summary>    
        public void EnabledRotateAroundTarget(GameObject obj, GameObject mTarget)
        {
            mainObject = obj;
            target = mTarget;
            isCameraRotateAroundTarget = true;
        }

        /// <Summary>
        /// 停止绕某物旋转
        /// </Summary>   
        public void StopRotateAroundTarget()
        {
            isCameraRotateAroundTarget = false;
        }

        /// <Summary>
        /// 绕某物镜头旋转是否启用
        /// </Summary>   
        public void EnabledCameraRotate(bool isEnbaled, GameObject pTarget)
        {
            isCanRotateCamera = isEnbaled;
            target = pTarget;
        }

        /// <Summary>
        /// 指定模型状态方向与点击点的法线偏移角度
        /// </Summary>   
        public void setModelsOnNormalline(GameObject obj, Vector3 normal, Vector3 modelDir, float angleoffset)
        {
            Vector3 dir = modelDir;
            float angle = Vector3.Angle(normal, dir);
            Vector3 cross = -Vector3.Normalize(Vector3.Cross(normal, dir));
            Quaternion q;
            q.w = Mathf.Cos(((angle - angleoffset) / 2) * pi / 180);
            q.x = cross.x * Mathf.Sin(((angle - angleoffset) / 2) * pi / 180);
            q.y = cross.y * Mathf.Sin(((angle - angleoffset) / 2) * pi / 180);
            q.z = cross.z * Mathf.Sin(((angle - angleoffset) / 2) * pi / 180);
            obj.transform.DORotateQuaternion(q, 0);
        }

    }
}


