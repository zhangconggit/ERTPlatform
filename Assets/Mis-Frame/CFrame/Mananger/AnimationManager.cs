using UnityEngine;
using System.Collections;

namespace CFramework
{
    public class AnimationManager : CMgrBehaviour
    {

        public static AnimationManager Instance
        {
            get
            {
                return CMonoSingleton<AnimationManager>.Instance();
            }
        }

        public void OnDestroy()
        {
            CMonoSingleton<AnimationManager>.OnDestroy();
        }


        protected override void SetupMgrId()
        {
            mMgrId = (ushort)CMgrID.AnimationManagerID;
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

