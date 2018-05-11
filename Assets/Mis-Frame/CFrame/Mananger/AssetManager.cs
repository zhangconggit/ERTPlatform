using UnityEngine;
using System.Collections;

namespace CFramework
{

    public class AssetManager : CMgrBehaviour
    {

        public static AssetManager Instance
        {
            get
            {
                return CMonoSingleton<AssetManager>.Instance();
            }
        }

        public void OnDestroy()
        {
            CMonoSingleton<AssetManager>.OnDestroy();
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
