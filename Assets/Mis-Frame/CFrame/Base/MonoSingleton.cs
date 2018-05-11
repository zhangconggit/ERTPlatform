using UnityEngine;
using System.Collections;

namespace CFramework
{
	public abstract class CMonoSingleton<T> : MonoBehaviour where T : MonoBehaviour
	{
		protected static T instance = null;

		public static T Instance()
		{
			if (instance == null)
			{
				instance = FindObjectOfType<T>();

				if (FindObjectsOfType<T>().Length > 1)
				{
					//Debug.LogError ("More than 1!");
					return instance;
				}

				if (instance == null)
				{
					string instanceName = typeof(T).Name;
                    //Debug.Log("Instance Name: " + instanceName); 
					GameObject instanceGO = GameObject.Find(instanceName);

					if (instanceGO == null)
						instanceGO = new GameObject(instanceName);
					instance = instanceGO.AddComponent<T>();
					DontDestroyOnLoad(instanceGO);  //保证实例不会被释放
                    //Debug.Log("Add New Singleton " + instance.name + " in Game!");
				}
				else
				{
					Debug.LogWarning ("Already exist: " + instance.name);
				}
			}

			return instance;
		}


		public static void OnDestroy()
		{
			instance = null;
		}
	}
}
