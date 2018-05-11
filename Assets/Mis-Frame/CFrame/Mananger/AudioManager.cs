using UnityEngine;
using System.Collections;

namespace CFramework
{
    public class AudioManager : CMgrBehaviour
    {

        public static AudioManager Instance
        {
            get
            {
                return CMonoSingleton<AudioManager>.Instance();
            }
        }

        public void OnDestroy()
        {
            CMonoSingleton<AudioManager>.OnDestroy();
        }


        protected override void SetupMgrId()
        {
            mMgrId = (ushort)CMgrID.AudioManagerID;
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
