
using System.Collections.Generic;
using UnityEngine;

namespace CFramework
{
    public class ChangeCamera : UPageBase
    {
        public List<UPageButton> buttons = new List<UPageButton>();

        public ChangeCamera(List<_CLASS_CameraShortCut> shortcutL)
        {
            SetAnchored(AnchoredPosition.full);
            SetBorderSpace(0, 0, 0, 0);

            //List<_CLASS_CameraShortCut> shortcutL = DataContainer.Instance.getCameraShortCut();

            int index = 0;
            foreach (_CLASS_CameraShortCut csc in shortcutL)
            {
                UPageButton bt = new UPageButton(AnchoredPosition.bottom);
                bt.button.magnification = 0.8f;
                buttons.Add(bt);
                bt.SetParent(this);
                bt.LoadSprite("shijiaok");
                bt.LoadPressSprite("shijiaok-h");
                bt.button.text.color = Color.white;
                bt.rect = new UnityEngine.Rect(100 * shortcutL.Count / 2f - 100 * index++, -70, 80, 80);
                bt.text = csc.shortcutName;
                Vector3 pos = new Vector3(float.Parse(csc.position[0]), float.Parse(csc.position[1]), float.Parse(csc.position[2]));
                Vector3 rot = new Vector3(float.Parse(csc.euler[0]), float.Parse(csc.euler[1]), float.Parse(csc.euler[2]));
                bt.onClick.AddListener(() => {
                    Camera.main.transform.position = pos;
                    Camera.main.transform.rotation = Quaternion.Euler(rot);
                });
            }
        }
    }
}

