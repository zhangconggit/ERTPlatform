using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class merun : MonoBehaviour {
    // Use this for initialization
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    void Start () {
        
        StartCoroutine(RunImage());
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    IEnumerator RunImage()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.02f);
            float x = gameObject.GetComponent<RawImage>().uvRect.x - 0.05f;
            x = x < -10 ? 0 : x;
            gameObject.GetComponent<RawImage>().uvRect = new Rect(x, gameObject.GetComponent<RawImage>().uvRect.y, gameObject.GetComponent<RawImage>().uvRect.width, gameObject.GetComponent<RawImage>().uvRect.height);
        }

    }
}
