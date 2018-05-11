using UnityEngine;
using System.Collections;

namespace CFramework
{
	public class GameManager : CMgrBehaviour {
		
		public static GameManager Instance{
			get{
				return CMonoSingleton<GameManager>.Instance();
			}
		}

        public void OnDestroy()
		{
			CMonoSingleton<GameManager>.OnDestroy();
		}


		protected override void SetupMgrId ()
		{
			mMgrId = (ushort)CMgrID.GameManagerID;
		}

		protected override void SetupMgr ()
		{
			base.SetupMgr();
		}

		public override void ProcessMsg (CMsg msg)
		{
			
		}
	}
}

