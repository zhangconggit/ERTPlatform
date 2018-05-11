using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScene : MonoBehaviour {
    public GameObject text;
    public GameObject barTop;
    public GameObject Loading;
    public RawImage runImage;
    public static bool isLoaded;
    //异步对象
    AsyncOperation asyncOperation;
	// Use this for initialization
	void Start () {
        isLoaded = false;
        
        StartCoroutine(loadScene());
        DontDestroyOnLoad(runImage);
	}
	
    void setLoadBar(int i)
    {
        text.GetComponent<Text>().text = "加载中 " + i + "%";
        barTop.GetComponent<Image>().fillAmount = 1 - i/100f;
    }
 
    IEnumerator loadScene()
    {
        int displayProgress = 0;
        int toProgress = 0;
        AsyncOperation op = SceneManager.LoadSceneAsync(GlobalClass.nextScene);// SceneManager.LoadSceneAsync(GlobalClass.nextScene);
        op.allowSceneActivation = false;
        while (op.progress < 0.9f)
        {
            toProgress = (int)op.progress * 100;
            while (displayProgress < toProgress)
            {
                ++displayProgress;
                setLoadBar(displayProgress);
                yield return new WaitForEndOfFrame();
            }
        }
        toProgress = 100;
        while (displayProgress < toProgress)
        {
            ++displayProgress;
            setLoadBar(displayProgress);
            yield return new WaitForEndOfFrame();
        }
        op.allowSceneActivation = true;
        isLoaded = true;
    }

}
