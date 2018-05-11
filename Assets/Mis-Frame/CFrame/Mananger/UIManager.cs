using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

namespace CFramework
{
	
	public class UIManager :  CMgrBehaviour{

		public static UIManager Instance{
			get{
				return CMonoSingleton<UIManager>.Instance();
			}
		}

		public void OnDestroy()
		{
			CMonoSingleton<UIManager>.OnDestroy();
		}
			
		protected override void SetupMgrId ()
		{
			mMgrId = (ushort)CMgrID.UIManagerID;
		}

		protected override void SetupMgr ()
		{
			base.SetupMgr();
		}

        public override void ProcessMsg(CMsg msg)
        {
            ;
        }
	}
}

