
namespace CFramework
{
    public delegate void VoidDelegate();
    public delegate void ParamDelegate<T>(T param);

    public class CEventBase
    {
        public CEventBase()
        {

        }
        
    }
    /// <summary>
    /// 无参事件
    /// </summary>
    public class CEvent: CEventBase
    {
        event VoidDelegate OnEvent;
        /// <summary>
        /// 添加一个监听
        /// </summary>
        /// <param name="callback"></param>
        public void AddListener(VoidDelegate callback)
        {
            OnEvent += callback;
        }
        public void RemoveListener(VoidDelegate callback)
        {
            OnEvent -= callback;
        }
        public void Invoke()
        {
            if (OnEvent != null)
                OnEvent.Invoke();
        }
        public void RemoveAllListener()
        {
            OnEvent = null;
        }
    }
    /// <summary>
    /// 有参事件
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class CEvent<T> : CEventBase
    {
        event ParamDelegate<T> OnEventParam;

        public void AddListener(ParamDelegate<T> callback)
        {
            OnEventParam += callback;
        }
        public void RemoveListener(ParamDelegate<T> callback)
        {
            OnEventParam -= callback;
        }
        public void Invoke(T _object)
        {
            if (OnEventParam != null)
                OnEventParam.Invoke(_object);
        }
        public void RemoveAllListener()
        {
            OnEventParam = null;
        }
    }
}
