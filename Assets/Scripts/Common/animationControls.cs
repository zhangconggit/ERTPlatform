using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AnimationStateEnum
{
    /// <summary>
    /// 正在播放
    /// </summary>
    isPlaying = 0,

    /// <summary>
    /// 播放完毕或者尚未播放
    /// </summary>
    isPlayed,
}

[System.Serializable]
public class animationModel
{
    /// <summary>
    /// 带有动画的对象
    /// </summary>
    public GameObject animObject;

    /// <summary>
    /// 对这个动画对象的描述
    /// </summary>
    public string animDesc;

    [SerializeField]
    public List<string> clipName = new List<string>();

    [HideInInspector]
    public AnimationStateEnum animState = AnimationStateEnum.isPlayed;
}

public class animationControls : MonoBehaviour {

    public List<animationModel> animList = new List<animationModel>();

	// Use this for initialization
	void Start () {
	
	}

   
    void OnGUI()
    {
        
    }
	// Update is called once per frame
	void Update () {  

	}

    /// <summary>
    /// 检测指定动画是否播放完毕
    /// </summary>
    /// <param name="pAnimDesc"></param>
    /// <param name="pClipName"></param>
    /// <returns></returns>
    public AnimationStateEnum getPlayAnimationState(string pAnimDesc,string pClipName)
    {
        animationModel findItem = animList.Find(name =>
        {
            if (name.animDesc == pAnimDesc)
                return name.animObject;
            return false;
        });
        if (findItem != null)
        {
            Animation anim = findItem.animObject.GetComponent<Animation>();

            return (anim.IsPlaying(pClipName))?AnimationStateEnum.isPlaying:AnimationStateEnum.isPlayed;
        }
        else
        {
            Debug.LogWarning("不能检测此动画播放状态：" + pAnimDesc);
            return AnimationStateEnum.isPlayed;
        }
    }

    /// <summary>
    /// 播放指定动画
    /// </summary>
    /// <param name="pAnimDesc"></param>
    /// <param name="pClipName"></param>
    public void playAnimation(string pAnimDesc,string pClipName)
    {
        animationModel findItem = animList.Find(name=>
        {
            if (name.animDesc == pAnimDesc)
                return name.animObject;
            return false;
        });
        if(findItem!=null)
        {
            Animation anim = findItem.animObject.GetComponent<Animation>();
            findItem.animState = AnimationStateEnum.isPlaying;
            anim[pClipName].speed = 1;
            anim.Stop();
            anim.Play(pClipName);
        }
        else
        {
            Debug.LogWarning("不能播放指定动画："+pAnimDesc);
        }
    }

	public void setManAnimationState(string pAnimDesc,string pClipName,bool isStart = true)
	{
		animationModel findItem = animList.Find(name=>
		                                        {
			if (name.animDesc == pAnimDesc)
				return name.animObject;
			return false;
		});
		if(findItem!=null)
		{
			Animation anim = findItem.animObject.GetComponent<Animation>();
			if(isStart == true )
				anim[pClipName].time = 0;
			else
				anim[pClipName].time = anim[pClipName].length;
			anim[pClipName].speed = 0;
			anim.Play(pClipName);

		}
		else
		{
			Debug.LogWarning("不能Reset指定动画："+pAnimDesc);
		}
	}
    /// <summary>
    /// 设置进度
    /// </summary>
    /// <param name="pAnimDesc"></param>
    /// <param name="pClipName"></param>
    /// <param name="f"></param>
    public void setManAnimationState(string pAnimDesc, string pClipName, float f = 0)
    {
        animationModel findItem = animList.Find(name =>
        {
            if (name.animDesc == pAnimDesc)
                return name.animObject;
            return false;
        });
        if (findItem != null)
        {
            Animation anim = findItem.animObject.GetComponent<Animation>();
            anim[pClipName].time = anim[pClipName].length * f;
            //anim[pClipName].normalizedTime = f;
          //  if (!anim.IsPlaying(pClipName))
            {
                anim[pClipName].speed = 0;
                anim.Play(pClipName);
            }
            

        }
        else
        {
            Debug.LogWarning("不能Reset指定动画：" + pAnimDesc);
        }
    }
    /// <summary>
    /// 设置速度
    /// </summary>
    /// <param name="pAnimDesc"></param>
    /// <param name="pClipName"></param>
    /// <param name="f"></param>
    public void setManAnimationSpeed(string pAnimDesc, string pClipName, float speed = 1)
    {
        animationModel findItem = animList.Find(name =>
        {
            if (name.animDesc == pAnimDesc)
                return name.animObject;
            return false;
        });
        if (findItem != null)
        {
            Animation anim = findItem.animObject.GetComponent<Animation>();
            anim[pClipName].speed = speed;
            anim.Play(pClipName);
        }
        else
        {
            Debug.LogWarning("不能Reset指定动画：" + pAnimDesc);
        }
    }

}
