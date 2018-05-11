using UnityEngine;
using System.Collections;

namespace CFramework
{
    public class ModelsManager : CMgrBehaviour
    {

        public static ModelsManager Instance
        {
            get
            {
                return CMonoSingleton<ModelsManager>.Instance();
            }
        }

        public void OnDestroy()
        {
            CMonoSingleton<ModelsManager>.OnDestroy();
        }


        protected override void SetupMgrId()
        {
            mMgrId = (ushort)CMgrID.ModelsManagerID;
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

