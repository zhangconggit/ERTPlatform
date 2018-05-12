using UnityEngine;
using System.Collections;
using CFramework;
using System.Collections.Generic;
using UnityEngine.UI;

namespace CFramework
{
    /// <summary>
    /// 声音类型
    /// </summary>
    public enum AudioStyle
    {
        /// <summary>
        /// 提示语音
        /// </summary>
        Attentions,

        /// <summary>
        /// 护士询问
        /// </summary>
        NurseQuestions,

        /// <summary>
        /// 病人回答
        /// </summary>
        PatientAnswers,
        /// <summary>
        /// 环境声音
        /// </summary>
        Environment,
    }
    /// <summary>
    /// 声音管理脚本
    /// </summary>
    public class VoiceControlScript : AudioManager
    {
        /// <summary>
        /// 对话、及提示语音UI
        /// </summary>
        UMessageBox Umb;
        /// <summary>
        /// 数字播放UI
        /// </summary>
        UMessageBox Umb1;
        /// <summary>
        /// 对话框头像UI
        /// </summary>
        UImage touxiang;
        AudioSource audioComponent;//声音组件
        static Dictionary<string, AudioClip> myAudioDic = new Dictionary<string, AudioClip>();//语音文件字典组
        AssetBundleRequest voiceAsset;
        public delegate void Callback();
        public Callback CallBack;
        public Callback CallBackNumber;
        VoidDelegate _callback;
        float lifeTime;
        float timeTimer = 0;
        bool firstPlay = true;
        string _man = "man";
        int bednumber = 0;
        int oldnumber = 0;
        bool isnumber = true; //标志读数字位还是表示位（个十百千）
        /// <summary>
        /// 播放的语音
        /// </summary>
        public string audioName { get; set; }

        Dictionary<int, GameObject> EnvironmentAudioDic = new Dictionary<int, GameObject>();//环境语音对象字典组

        public static VoiceControlScript Instance
        {
            get
            {
                return CMonoSingleton<VoiceControlScript>.Instance();
            }
        }
        public void OnDestroy()
        {
            CMonoSingleton<VoiceControlScript>.OnDestroy();
        }
        /// <summary>
        /// 处理消息
        /// </summary>
        /// <param name="msg">消息内容</param>
        public override void ProcessMsg(CMsg msg)
        {
        }
        void Awake()
        {
            audioComponent = this.gameObject.GetComponent<AudioSource>();//添加声音组件
            TextAsset txt = Resources.Load("Audios/AudioText") as TextAsset;
            fileHelper.setConfigText("Resource/Ini", txt.ToString().Split('\n'));
        }
        void Start()
        {


        }
        void Update()
        {
            if (IsVoicePlaying())
            {
                timeTimer = 0;
            }
            else
            {
                timeTimer += Time.deltaTime;
                if (timeTimer > lifeTime)
                {
                    if (Umb != null)
                        Umb.gameObejct.SetActive(false);
                    if (Umb1 != null)
                        Umb.gameObejct.SetActive(false);
                    timeTimer = -99999;
                }
                if (CallBack != null)
                    CallBack();
                if (_callback != null)
                {
                    _callback();
                    _callback = null;
                }
            }

            #region 销毁播放完成的环境声音
            List<int> tmpList = new List<int>();
            foreach (var item in EnvironmentAudioDic)
            {
                if (item.Value.GetComponent<AudioSource>().isPlaying == false)
                {
                    tmpList.Add(item.Key);
                }
            }
            for (int i = 0; i < tmpList.Count; i++)
            {
                StopEnvironment(tmpList[i]);
            }
            #endregion
        }

        public void SetCallBack(Callback callback)
        {
            CallBack = callback;
        }

        public void setVoiceAsset(Dictionary<string, Object> tmp)
        {
            foreach (KeyValuePair<string, Object> info in tmp)
                myAudioDic.Add(info.Key, info.Value as AudioClip);
        }

        /// <summary>
        /// 只显示UI文字
        /// </summary>
        public void OnlyText(string strText, AudioStyle audioStyle)
        {
            timeTimer = 0;
            try
            {
                lifeTime = 999;
            }
            catch
            {
                lifeTime = 0;
            }

            string audioUIText = strText;// 

            if (audioUIText != null)
            {
                if (Umb == null)
                {
                    Umb = new UMessageBox();
                    Umb.back.LoadImage("duihuakuang"); Umb.okButton.Destroy(); Umb.tilte.Destroy();
                    Umb.back.gameObejct.AddComponent<VerticalLayoutGroup>();
                    Umb.back.gameObejct.GetComponent<VerticalLayoutGroup>().padding = new RectOffset(60, 30, 10, 10);
                    Umb.back.gameObejct.AddComponent<ContentSizeFitter>();
                    Umb.back.gameObejct.GetComponent<ContentSizeFitter>().horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
                    Umb.back.gameObejct.GetComponent<ContentSizeFitter>().verticalFit = ContentSizeFitter.FitMode.PreferredSize;
                    Umb.context.SetParent(Umb.back);
                    Umb.context.SetBorderSpace(0, 0, 0, 0);
                    Umb.context.rect = new Rect(7, 0, 430, 90);
                    Umb.context.SetAnchored(AnchoredPosition.center);
                    Umb.context.baseText.alignment = TextAnchor.MiddleLeft;
                    Umb.context.baseText.color = Color.white;
                }
                Umb.context.text = audioUIText;
                Umb.gameObejct.SetActive(true);
                Umb.transform.SetAsLastSibling();
                if (touxiang == null)
                    touxiang = new UImage();
                touxiang.gameObejct.SetActive(true);
                touxiang.SetParent(Umb);
                //提示语音
                if (audioStyle == AudioStyle.Attentions)
                {
                    Umb.rect = new Rect(-500, 500, 460, 100);
                    touxiang.rect = new Rect(-275, 0, 80, 80);
                    touxiang.LoadImage("nurse_F");
                }
                //病人回答
                if (audioStyle == AudioStyle.PatientAnswers)
                {
                    Umb.rect = new Rect(700, 0, 460, 100);
                    touxiang.rect = new Rect(-275, 0, 80, 80);
                    if (CGlobal.currentMedicalRecord.sex == "男")
                        touxiang.LoadImage("patient_M");
                    else
                        touxiang.LoadImage("patient_F");
                }
                ///护士询问
                if (audioStyle == AudioStyle.NurseQuestions)
                {
                    Umb.rect = new Rect(100, 300, 460, 180);
                    touxiang.rect = new Rect(-275, 0, 80, 80);
                    touxiang.LoadImage("nurse_F");
                }
            }
            else
            {
                Debug.Log("未找到对应声音文字，声音名：" + audioUIText);
            }
        }
        /// <summary>
        /// 声音播放
        /// </summary>
        /// <param name="audioStyle">声音类型</param>
        /// <param name="audioName">声音名</param>
        ///  /// <param name="isDisplayText">是否显示UI文字</param>
        public void AudioPlay(AudioStyle audioStyle, string audioName, bool isDisplayText = true, VoidDelegate callback = null)
        {
            if (firstPlay)
            {
                firstPlay = false;
            }
            _callback = callback;
            this.audioName = audioName;
            if (isDisplayText == true)
            {
                OnlyText(fileHelper.ReadIni(audioStyle.ToString(), audioName, "Resource/Ini"), audioStyle);
            }
            if (myAudioDic.ContainsKey(audioName))
            {
                if (myAudioDic[audioName] != null)
                {
                    audioComponent.clip = myAudioDic[audioName];
                    audioComponent.Play();
                }
            }
            else
            {
                Debug.Log("没有找到音频文件,文件名：" + audioName);
            }
        }


        /// <summary>
        /// 播放床号函数
        /// </summary>
        /// <param name="BedNumber">播放的床号</param>
        /// <param name="callback">播放完的回调</param>
        public void AudioPlayBedNumber(int BedNumber, Callback callback, bool isMan = true)
        {
            _man = isMan ? "man" : "";
            _callback = null;
            OnlyText(BedNumber.ToString() + "号床。", AudioStyle.PatientAnswers);
            bednumber = BedNumber;
            CallBackNumber = callback;
            SetCallBack(PlayNumberNode);
        }
        void PlayNumberNode()
        {
            if (isnumber == true)
            {
                if (oldnumber.ToString().Length - bednumber.ToString().Length > 1)
                {
                    VoiceControlScript.Instance.AudioPlay(AudioStyle.PatientAnswers, "0" + _man, false);
                    oldnumber = bednumber;
                    VoiceControlScript.Instance.SetCallBack(PlayNumberNode);
                    return;
                }

                if (999 < bednumber && bednumber <= 9999)
                {
                    VoiceControlScript.Instance.AudioPlay(AudioStyle.PatientAnswers, (bednumber / 1000).ToString() + _man, false);
                    isnumber = false;
                    VoiceControlScript.Instance.SetCallBack(PlayNumberNode);
                    return;
                }

                if (99 < bednumber && bednumber <= 999)
                {
                    VoiceControlScript.Instance.AudioPlay(AudioStyle.PatientAnswers, (bednumber / 100).ToString() + _man, false);
                    isnumber = false;
                    VoiceControlScript.Instance.SetCallBack(PlayNumberNode);
                    return;
                }

                if (9 < bednumber && bednumber <= 99)
                {
                    VoiceControlScript.Instance.AudioPlay(AudioStyle.PatientAnswers, (bednumber / 10).ToString() + _man, false);
                    isnumber = false;
                    VoiceControlScript.Instance.SetCallBack(PlayNumberNode);
                    return;
                }

                if (0 < bednumber && bednumber <= 9)
                {
                    VoiceControlScript.Instance.AudioPlay(AudioStyle.PatientAnswers, bednumber.ToString() + _man, false);
                    VoiceControlScript.Instance.SetCallBack(PlayNumberOver);
                    return;
                }

                if (bednumber == 0)
                {
                    VoiceControlScript.Instance.SetCallBack(PlayNumberOver);
                    return;
                }
            }
            else
            {
                if (9 < bednumber && bednumber <= 99)
                {
                    VoiceControlScript.Instance.AudioPlay(AudioStyle.PatientAnswers, "shi" + _man, false);
                    oldnumber = bednumber;
                    bednumber = bednumber % 10;
                    isnumber = true;
                    VoiceControlScript.Instance.SetCallBack(PlayNumberNode);
                    return;
                }

                if (99 < bednumber && bednumber <= 999)
                {
                    VoiceControlScript.Instance.AudioPlay(AudioStyle.PatientAnswers, "bai" + _man, false);
                    oldnumber = bednumber;
                    bednumber = bednumber % 100;
                    isnumber = true;
                    VoiceControlScript.Instance.SetCallBack(PlayNumberNode);
                    return;
                }

                if (999 < bednumber && bednumber <= 9999)
                {
                    VoiceControlScript.Instance.AudioPlay(AudioStyle.PatientAnswers, "qian" + _man, false);
                    oldnumber = bednumber;
                    bednumber = bednumber % 1000;
                    isnumber = true;
                    VoiceControlScript.Instance.SetCallBack(PlayNumberNode);
                    return;
                }
            }
        }

        void PlayNumberOver()
        {
            AudioPlay(AudioStyle.PatientAnswers, "haochuang" + _man, false);
            //MessageboxDestroy();
            SetCallBack(CallBackNumber);
        }

        /// <summary>
        /// 停止播放声音
        /// </summary>
        public void VoiceStop()
        {
            if (audioComponent)
                audioComponent.Stop();
            if (Umb != null)
                Umb.gameObejct.SetActive(false);
            if (Umb1 != null)
                Umb1.gameObejct.SetActive(false);
        }
        /// <summary>
        /// 暂停播放声音
        /// </summary>
        public void VoicePause()
        {
            if (audioComponent.isPlaying)
            {
                audioComponent.Pause();
            }
        }
        /// <summary>
        /// 判断音频是否正在播放
        /// </summary>
        public bool IsVoicePlaying()
        {
            if (audioComponent.isPlaying)
                return true;
            else
                return false;
        }

        #region 环境声音
        /// <summary>
        /// 开始一个环境音
        /// </summary>
        /// <param name="name">声音名称</param>
        /// <param name="_loop">是否循环</param>
        /// <returns>播放失败 返回-1</returns>
        public int StartOneEnvironment(string name, bool _loop = false)
        {
            int _index = -1;
            if (myAudioDic.ContainsKey(name))
            {
                if (myAudioDic[name] != null)
                {
                    GameObject voice = CreatVoiceObject(name);
                    voice.GetComponent<AudioSource>().clip = myAudioDic[name];
                    voice.GetComponent<AudioSource>().Play();
                    voice.GetComponent<AudioSource>().loop = _loop;
                    _index = 0;
                    foreach (var item in EnvironmentAudioDic)
                    {
                        if (_index <= item.Key)
                            _index++;
                    }
                    EnvironmentAudioDic.Add(_index, voice);
                }
            }
            else
            {
                Debug.Log("没有找到音频文件,文件名：" + name);
            }
            return _index;

        }

        GameObject CreatVoiceObject(string name)
        {
            GameObject voice = new GameObject();
            voice.name = name;
            GameObject EnvironmentObj = GameObject.Find("EnvironmentAudios");
            if (EnvironmentObj == null)
            {
                EnvironmentObj = new GameObject("EnvironmentAudios");
            }
            voice.transform.SetParent(EnvironmentObj.transform);
            voice.AddComponent<AudioSource>();
            return voice;
        }

        /// <summary>
        /// 停止环境声音
        /// </summary>
        /// <param name="id"></param>
        public void StopEnvironment(int id)
        {
            if (EnvironmentAudioDic.ContainsKey(id))
            {
                Object.DestroyImmediate(EnvironmentAudioDic[id]);
                EnvironmentAudioDic.Remove(id);
            }
        }

        /// <summary>
        /// 停止环境声音
        /// </summary>
        /// <param name="id"></param>
        public void StopEnvironment(string name)
        {
            List<int> list = new List<int>();
            foreach (var item in EnvironmentAudioDic)
            {
                if (item.Value.name == name)
                {
                    list.Add(item.Key);

                }
            }
            foreach (var item in list)
            {
                Object.DestroyImmediate(EnvironmentAudioDic[item]);
                EnvironmentAudioDic.Remove(item);
            }
        }

        /// <summary>
        /// 检查环境声音是否存在
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>存在返回true</returns>
        public bool IsExistEnvironment(int id)
        {
            if (EnvironmentAudioDic.ContainsKey(id))
            {
                return true;
            }
            else
                return false;
        }
        /// <summary>
        /// 检查环境声音是否存在
        /// </summary>
        /// <param name="name">名字</param>
        /// <returns>存在返回true</returns>
        public bool IsExistEnvironment(string name)
        {
            foreach (var item in EnvironmentAudioDic)
            {
                if (item.Value.name == name)
                {
                    return true;
                }
            }
            return false;
        }
        #endregion
    }
}

