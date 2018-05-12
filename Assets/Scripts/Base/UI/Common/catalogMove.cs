using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class catalogMove : MonoBehaviour {

    public Button OnMoveButton;
    public Sprite IconOut;
    public Sprite IconIn;

    public float speed = 1;
    GameObject OnMoveIcon;
    Vector2 start;
    float width = 0;
    bool isOut = true;
    // Use this for initialization
    void Start () {
        OnMoveButton.onClick.AddListener(OnClickIcon);
        OnMoveIcon = OnMoveButton.transform.Find("icon").gameObject;
        OnMoveIcon.GetComponent<Image>().sprite = IconIn;
        start = gameObject.GetComponent<RectTransform>().localPosition;
        width = gameObject.GetComponent<RectTransform>().rect.width;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void OnClickIcon()
    {
        isOut = !isOut;
        if(isOut)
            OnMoveIcon.GetComponent<Image>().sprite = IconIn;
        else
            OnMoveIcon.GetComponent<Image>().sprite = IconOut;
        StopCoroutine(MoveGameObejct());
        StartCoroutine(MoveGameObejct());
    }
    IEnumerator MoveGameObejct()
    {
        bool bl = true;
        while (bl)
        {
            if(isOut)
            {

                gameObject.GetComponent<RectTransform>().localPosition = Vector3.Lerp(gameObject.GetComponent<RectTransform>().localPosition, start, speed / 10 * width * Time.deltaTime);
                if (Vector3.Distance(gameObject.GetComponent<RectTransform>().localPosition, start) < 1)
                {
                    gameObject.GetComponent<RectTransform>().localPosition = start;
                    bl = false;
                }
            }
            else
            {
                gameObject.GetComponent<RectTransform>().localPosition = Vector3.Lerp(gameObject.GetComponent<RectTransform>().localPosition, start - new Vector2(width, 0), speed / 10 * width * Time.deltaTime);
                if (Vector3.Distance(gameObject.GetComponent<RectTransform>().localPosition, start - new Vector2(width, 0)) < 1)
                {
                    gameObject.GetComponent<RectTransform>().localPosition = start - new Vector2(width, 0);
                    bl = false;
                }
            }
            yield return 0;
        }
    }
    
}
