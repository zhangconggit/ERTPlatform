using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Input3DText : MonoBehaviour {
    GameObject InputText;
    public GameObject OutputText;
    bool isFocus;
    public Color backColor;
    int screenWidth = 0;
    int screenHeight = 0;

    // Use this for initialization
    void Start () {
        isFocus = false;
        InputText = gameObject;
        OutputText.GetComponent<TextMesh>().text = InputText.GetComponent<InputField>().text;
        InputText.GetComponent<InputField>().targetGraphic.color = new Color(0, 0, 0, 0);
        InputText.GetComponent<InputField>().textComponent.color = new Color(0, 0, 0, 0);
        Transform text = InputText.transform.FindChild("Text");
        if(text != null)
        {
            text.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        }
    }

	// Update is called once per frame
	void Update () {
        if(screenWidth != Screen.currentResolution.width || screenHeight != Screen.currentResolution.height)
        {
            gameObject.GetComponent<RectTransform>().position = Camera.main.WorldToScreenPoint(OutputText.transform.position);
            screenWidth = Screen.currentResolution.width;
            screenHeight = Screen.currentResolution.height;
        }
	    if(InputText.GetComponent<InputField>().isFocused && isFocus == false)
        {
            OutputText.GetComponent<TextMesh>().text = "";
            isFocus = InputText.GetComponent<InputField>().isFocused;
            InputText.GetComponent<InputField>().targetGraphic.color = backColor;
            InputText.GetComponent<InputField>().textComponent.color = new Color(0, 0, 0, 1);

        }
        if(!InputText.GetComponent<InputField>().isFocused && isFocus == true)
        {
            isFocus = InputText.GetComponent<InputField>().isFocused;
            InputText.GetComponent<InputField>().targetGraphic.color = new Color(0, 0, 0, 0);
            InputText.GetComponent<InputField>().textComponent.color = new Color(0, 0, 0, 0);
            OutputText.GetComponent<TextMesh>().text = InputText.GetComponent<InputField>().text;
        }
	}
}
