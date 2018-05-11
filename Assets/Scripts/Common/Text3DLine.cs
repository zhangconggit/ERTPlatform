using UnityEngine;
using System.Collections;

/// <summary>
/// 3D文字换行
/// </summary>
public class Text3DLine : MonoBehaviour {
    [Range(0, 100)]
    public int lineNumber = 3;
    string oldText = "";
	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	    if(transform.GetComponent<TextMesh>().text != oldText)
        {
            oldText = transform.GetComponent<TextMesh>().text;
            while(oldText.IndexOf('\n') >=0)
            {
                Debug.Log("this is while");
                oldText = oldText.Remove(oldText.IndexOf('\n'),1);
            }
            for (int i = oldText.Length - oldText.Length % lineNumber; i > 0; i-= lineNumber)
            {
                Debug.Log("this is for");
                if (i == oldText.Length)
                    continue;
                oldText = oldText.Insert(i, "\n");
                Debug.Log("" + (i - 1).ToString());
            }
            transform.GetComponent<TextMesh>().text = oldText;
            Debug.Log("oldText"+ oldText);
        }
	}
}
