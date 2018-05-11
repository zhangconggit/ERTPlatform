using UnityEngine;
using System.Collections;

namespace CFramework
{
    public class ScenesManager : CMgrBehaviour
    {

        public static ScenesManager Instance
        {
            get
            {
                return CMonoSingleton<ScenesManager>.Instance();
            }
        }

        public void OnDestroy()
        {
            CMonoSingleton<ScenesManager>.OnDestroy();
        }


        protected override void SetupMgrId()
        {
            mMgrId = (ushort)CMgrID.SceneManagerID;
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

