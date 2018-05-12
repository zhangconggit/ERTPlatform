using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 位置随机
/// </summary>
public class PositionChange : MonoBehaviour {
    public List<GameObject> objects;
    public bool isRandom = true;
    List<Vector3> postions;
    // Use this for initialization
    void Start () {
        postions = new List<Vector3>();
        foreach (var item in objects)
        {
            postions.Add(item.transform.position);
        }
        RandPostion();
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnEnable()
    {
        
    }
    void RandPostion()
    {
        if (isRandom)
        {
            int[] rand = MyCommon.RandomRepeat(postions.Count);
            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].transform.position = postions[rand[i]];
            }
        }
    }
}
