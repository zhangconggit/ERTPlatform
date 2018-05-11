
using UnityEngine;

    /// <summary>
    /// 登陆
    /// </summary>
    class Login : StepBase
    {
        LoginBase loginBase = null;
        public Login()
        {
            Sid = "login";
            State = StepStatus.undo;
            stepInfo = new StepInfo() {
                stepName = Sid,
                isAutoGo = true,
                mustDoStep = new System.Collections.Generic.List<string>() { },
                cannotDoStep = new System.Collections.Generic.List<string>() { },
                pStepClass = this,
            };
        }
        public override void stepBefore()
        {
            if (GameObject.Find("Canvas").transform.FindChild("WaitLoading") != null)
            {
                GameObject.Find("Canvas").transform.FindChild("WaitLoading").gameObject.SetActive(false);
            }
        }

        public override void stepRunner()
        {
            if (LoadingScene.isLoaded)
            {
                State = StepStatus.did;
            }
        }

        public override void stepAfter()
        {

        }

        public override void stepReSet()
        {
            base.stepReSet();
            LoadingScene.isLoaded = false;
        }
        public override void stepSetEnd()
        {
            State = StepStatus.did;
        }
    }
