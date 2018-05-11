using UnityEngine;
using System.Collections;

namespace CFramework
{
    public class InputManager : CMgrBehaviour
    {


        public static InputManager Instance
        {
            get
            {
                return CMonoSingleton<InputManager>.Instance();
            }
        }

        public void OnDestroy()
        {
            CMonoSingleton<InputManager>.OnDestroy();
        }


        protected override void SetupMgrId()
        {
            mMgrId = (ushort)CMgrID.InputManagerID;
        }

        protected override void SetupMgr()
        {
            base.SetupMgr();
        }

        public override void ProcessMsg(CMsg msg)
        {

        }
    }
}

