using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace CFramework
{
	/// <summary>
	/// 传递的消息节点
	/// </summary>
	public class CMsgNode
	{
		/// <summary>
		/// 当前的Behaviour
		/// </summary>
		public CMonoBehaviour behaviour;

		public CMsgNode next;

		public CMsgNode(CMonoBehaviour behaviour)
		{
			this.behaviour = behaviour;
			this.next = null;
		}
	}

	public abstract class CMgrBehaviour : CMonoBehaviour{

		public static Dictionary<ushort,CMsgNode> msgDic = new Dictionary<ushort, CMsgNode>();

		protected ushort mMgrId = 0;

		protected abstract void SetupMgrId();

		protected override void SetupMgr ()
		{
			mCurMgr = this;
		}

		protected CMgrBehaviour()
		{
			SetupMgrId();
		}

		// mono:要注册的脚本   
		// msgs:每个消息可以注册多个脚本
		public void RegisterMsg(CMonoBehaviour behaviour,ushort[] msgs)
		{
			for (int i = 0;i < msgs.Length;i++)
			{
				CMsgNode msgNode = new CMsgNode(behaviour);

				RegisterMsg(msgs[i],msgNode);
			}
		}

		// 根据: msgid
		// node链表
		public void RegisterMsg(ushort msgId,CMsgNode node)
		{
			// 数据链路里 没有这个消息id
			if(!msgDic.ContainsKey(msgId))
			{
				msgDic.Add(msgId,node);
			}
			else 
			{
				CMsgNode tmp = msgDic[msgId];

				// 找到最后一个车厢
				while(tmp.next != null)
				{
					tmp = tmp.next;
				}

				// 直接挂上
				tmp.next = node;
			}
		}

		// params 可变数组 参数
		// 去掉一个脚本的若干的消息
		public void UnRegisterMsg(CMonoBehaviour monoBase,params ushort[] msgs)
		{
			for (int i = 0;i < msgs.Length;i++)
			{
				UnRegistMsg(msgs[i],monoBase);
			}
		}

		// 释放 中间,尾部。
		public void UnRegistMsg(ushort msgId,CMonoBehaviour behaviour)
		{
			if (!msgDic.ContainsKey(msgId))
			{
				Debug.LogWarning("not contain id ==" + msgId);
				return;
			}
			else 
			{
				CMsgNode msgNode = msgDic[msgId];
				if (msgNode.behaviour == behaviour) // 去掉头部 包含两种情况
				{
					CMsgNode header = msgNode;

					// 已经存在这个消息
					// 头部
					if (header.next != null)
					{
						msgDic [msgId] = msgNode.next; // 直接指向下一个
						header.next = null;

						header.behaviour = msgNode.next.behaviour;
						header.next = msgNode.next.next;
					}
					else // 后面没有节点的情况 
					{
						header.next = null;
						msgDic.Remove(msgId);
					}
				}
				else // 去掉尾部 和中间的节点 
				{
					while(msgNode.next != null && msgNode.next.behaviour != behaviour) // 下一个不是我要找的 node 就一直遍历
					{
						msgNode = msgNode.next;
					} // 表示已经找到了 该节点

					// 没有引用 会自动释放
					if (msgNode.next.next != null) // 去掉中间的
					{
						CMsgNode curNode = msgNode.next; // 保存一下

						msgNode.next = curNode.next;
						//					tmp.next = tmp.next.next;
						curNode.next = null; // 把相关联的指针释放
					}
					else // 去掉尾部的
					{
						// tmp表示要找的节点的上一个节点
						msgNode.next = null;
					}
				}
			}
		}

		public void SendMessage(CMsg msg)
		{
			if ((ushort)msg.GetMgrID() == mMgrId)
			{
				aProcessMsg(msg);
//				ProcessMsg(msg);
			}
			else 
			{
				CMsgCenter.Instance.SendToMsg(msg);
			}
		}

		// 来了消息以后,通知整个消息链
		public  void aProcessMsg(CMsg msg)
		{
			if (!msgDic.ContainsKey(msg.msgId))
			{
				Debug.Log("msg not found:" + msg.msgId);
				return;
			}
			else 
			{
				CMsgNode msgNode = msgDic[msg.msgId];

				// 进行广播
				do 
				{	
					msgNode.behaviour.ProcessMsg(msg);

					msgNode = msgNode.next;
				} 
				while (msgNode != null);
			}
		}
	}
}

