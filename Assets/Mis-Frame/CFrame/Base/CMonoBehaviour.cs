using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CFramework
{
	public abstract class CMonoBehaviour : MonoBehaviour {

		/// <summary>
		/// 消息处理
		/// </summary>
		/// <param name="msg">Message.</param>
		 public abstract void ProcessMsg(CMsg msg);

		/// <summary>
		///  设置所属Manager
		/// </summary>
		protected abstract void SetupMgr();

		/// <summary>
		/// The m private mgr.
		/// </summary>
		private CMgrBehaviour mPrivateMgr = null;

		/// <summary>
		/// Gets or sets the m current mgr.
		/// </summary>
		/// <value>The m current mgr.</value>
		protected CMgrBehaviour mCurMgr{
			get{
				if(mPrivateMgr == null)
				{
					SetupMgr();
				}
				if(mPrivateMgr == null)
				{
					Debug.Log("没有设置Mgr");
				}
				return mPrivateMgr;
			}

			set{
				mPrivateMgr = value;
			}
		}

		/// <summary>
		/// Registers the self.
		/// </summary>
		/// <param name="mono">Mono.</param>
		/// <param name="msg">Message.</param>
		public void RegisterSelf(CMonoBehaviour mono,ushort[] msg)
		{
			mCurMgr.RegisterMsg(mono,msg);
		}

		/// <summary>
		/// Registers the self.
		/// </summary>
		/// <param name="mono">Mono.</param>
		public void RegisterSelf(CMonoBehaviour mono)
		{
			mCurMgr.RegisterMsg(mono,msgIds);
		}

		/// <summary>
		/// Uns the register self.
		/// </summary>
		/// <param name="mono">Mono.</param>
		/// <param name="msg">Message.</param>
		public void UnRegisterSelf(CMonoBehaviour mono,ushort[] msg)
		{
			mCurMgr.UnRegisterMsg(mono,msg);
		}
			

		/// <summary>
		/// Sends the message.
		/// </summary>
		/// <param name="msg">Message.</param>
		public void SendMsg(CMsg msg)
		{
			mCurMgr.SendMessage(msg);
		}

		/// <summary>
		/// Raises the  event.
		/// </summary>
		public void Ondestory()
		{
			if(msgIds!=null)
			{
				UnRegisterSelf(this,msgIds);
			}
		}

		public ushort[] msgIds;
	}
}
