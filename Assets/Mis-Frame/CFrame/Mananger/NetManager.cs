using UnityEngine;
using System.Collections;

namespace CFramework
{
	public class NetManager : CMgrBehaviour {

		public static NetManager Instance{
			get{
				return CMonoSingleton<NetManager>.Instance();
			}
		}

		public void OnDestroy()
		{
			CMonoSingleton<NetManager>.OnDestroy();
		}

		protected override void SetupMgr ()
		{
			base.SetupMgr();
		}

		protected override void SetupMgrId ()
		{
			mMgrId = (ushort)CMgrID.NetManagerID;
		}

		void Start()
		{
			
		}

		void Update()
		{

		}
		 
		public override void ProcessMsg (CMsg msg)
		{
		}
	}
}

