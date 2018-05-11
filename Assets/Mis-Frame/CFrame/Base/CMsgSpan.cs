using UnityEngine;
using System.Collections;

public class CMsgSpan {
		public const int Count = 3000;
	}

	public enum CMgrID
	{
		Game = 0,
		UIManagerID = CMsgSpan.Count,
		NetManagerID = CMsgSpan.Count*2,
		GameManagerID = CMsgSpan.Count*3,
        SceneManagerID = CMsgSpan.Count*4,
		AudioManagerID = CMsgSpan.Count*5,
		InputManagerID = CMsgSpan.Count*6,
		AnimationManagerID = CMsgSpan.Count*7,
		ModelsManagerID = CMsgSpan.Count*8,
		AssetManagerID = CMsgSpan.Count*9,
	}
