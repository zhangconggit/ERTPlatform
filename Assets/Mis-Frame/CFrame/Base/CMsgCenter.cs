using UnityEngine;
using System.Collections;
using CFramework;

public class CMsgCenter : MonoBehaviour {

	public static CMsgCenter Instance{
		get{
			return CMonoSingletonComponent<CMsgCenter>.Instance;
		}
	}

	public void OnDestroy()
	{
        CMonoSingletonComponent<CMsgCenter>.Dispose();
	}

	void Awake()
	{
		DontDestroyOnLoad(this);
	}

	public IEnumerator Init()
	{
		Debug.Log("CMsgCenter Init");
		yield return null;
	}

	public void SendToMsg(CMsg tmpMsg)
	{
		sendMsg(tmpMsg);
	}

	private void sendMsg(CMsg msg)
	{
		CMgrID tmpID = msg.GetMgrID();

		switch(tmpID)
		{
		case CMgrID.UIManagerID:
			{
				UIManager.Instance.SendMessage(msg);
				break;
			}
		case CMgrID.NetManagerID:
			{
				NetManager.Instance.SendMessage(msg);
				break;
			}
		case CMgrID.GameManagerID:
			{
				GameManager.Instance.SendMessage(msg);
				break;
			}
        case CMgrID.SceneManagerID:
            {
                ScenesManager.Instance.SendMessage(msg);
                break;
            }
            case CMgrID.ModelsManagerID:
                {
                    ModelsManager.Instance.SendMessage(msg);
                    break;
                }
            default:
			break;
		}
	}
}
