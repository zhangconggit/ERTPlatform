using UnityEngine;
using System.Collections;

public class CMsg  {

	public ushort msgId;//表示消息 两个字节 都使用65535个消息

	/// <summary>
	///  获取消息id属于哪一个Manager
	/// </summary>
	/// <returns>The mgr I.</returns>
	public CMgrID GetMgrID()
	{
		int tmpID = msgId/CMsgSpan.Count;
		return (CMgrID)(tmpID*CMsgSpan.Count);
	}

	#region 构造函数
	public CMsg(){}

	public CMsg(ushort msg)
	{
		msgId = msg;
	}
	#endregion
}
