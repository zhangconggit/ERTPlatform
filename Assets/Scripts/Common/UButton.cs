using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CFramework
{
    public enum ButtonTransition
    {
        None,
        ColorTint,
        SpriteSwap
    }

    public class UButton : MonoBehaviour, IPointerClickHandler, IEventSystemHandler, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
	{
		[Tooltip("虚拟键位")]
		public KeyCode code;

		private VoidDelegate OnKeyEvent;

		[HideInInspector]
		public Color normalColor = Color.white;

		[HideInInspector]
		public Color pressColor = new Color(0.784313738f, 0.784313738f, 0.784313738f, 1f);

		[HideInInspector]
		public Sprite normalImage;

		[HideInInspector]
		public Sprite pressImage;

		private Image buttonImage;

		[HideInInspector]
		public Text text;

		[Tooltip("是否启用保持按钮属性")]
		public bool enabledKeep;

		[Range(0.1f, 2f), Tooltip("点击后改变scale的倍率")]
		public float magnification = 1f;

		[Tooltip("是否启用长按事件")]
		public bool enabledLongDown;

		[HideInInspector]
		public float durationThreshold = 0.1f;

		public ButtonTransition transition;

		private bool isDown;

		private bool longPressTrigger;

		private float timePressStarted;

		private bool firstLongClick = true;

		private Vector3 scale;

		private void Start()
		{
			this.buttonImage = base.GetComponent<Image>();
            this.scale = base.transform.localScale;
		}

		private void Update()
		{
			if (Input.GetKeyDown(this.code))
			{
				this.Down();
			}
			if (Input.GetKeyUp(this.code))
			{
				this.Up();
			}
			if (this.enabledLongDown)
			{
				if (this.isDown && !this.longPressTrigger && this.OnKeyEvent != null && (Time.time - this.timePressStarted > this.durationThreshold || this.firstLongClick))
				{
					this.timePressStarted = Time.time;
					this.firstLongClick = false;
					this.OnKeyEvent();
					return;
				}
			}
			else if (Input.GetKeyDown(this.code) && this.OnKeyEvent != null)
			{
				this.OnKeyEvent();
			}
		}

		public void OnPointerClick(PointerEventData eventData)
		{
			if (!this.enabledLongDown && this.OnKeyEvent != null)
			{
				this.OnKeyEvent();
			}
		}

		public void AddListionEvent(VoidDelegate CallBack)
		{
			this.OnKeyEvent = (VoidDelegate)Delegate.Combine(this.OnKeyEvent, CallBack);
		}

		public void RemoveListionEvent(VoidDelegate CallBack)
		{
			this.OnKeyEvent = (VoidDelegate)Delegate.Remove(this.OnKeyEvent, CallBack);
		}

		public void OnPointerDown(PointerEventData eventData)
		{
			this.Down();
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			this.Up();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if (this.enabledLongDown)
			{
				this.isDown = false;
				this.longPressTrigger = true;
			}
		}

		public void ClearAllListener()
		{
			this.OnKeyEvent = null;
		}

		private void Down()
		{
			if (this.enabledLongDown)
			{
				this.timePressStarted = Time.time;
				this.isDown = true;
				this.longPressTrigger = false;
				this.firstLongClick = true;
			}
			if (this.enabledKeep)
			{
				switch (this.transition)
				{
				case ButtonTransition.ColorTint:
					if (this.buttonImage.color == this.pressColor)
					{
						this.buttonImage.color = this.normalColor;
					}
					else
					{
						this.buttonImage.color=this.pressColor;
					}
					break;
				case ButtonTransition.SpriteSwap:
					if (this.buttonImage.sprite == this.pressImage)
					{
						this.buttonImage.sprite = this.normalImage;
					}
					else
					{
						this.buttonImage.sprite =this.pressImage;
					}
					break;
				}
			}
			else
			{
				switch (this.transition)
				{
				case ButtonTransition.ColorTint:
					if (this.buttonImage != null)
					{
						this.buttonImage.color=this.pressColor;
					}
					break;
				case ButtonTransition.SpriteSwap:
					if (this.buttonImage != null && this.pressImage != null)
					{
						this.buttonImage.sprite = this.pressImage;
					}
					break;
				}
			}
			base.transform.localScale = this.scale * this.magnification;
		}

		private void Up()
		{
			if (this.enabledLongDown)
			{
				this.isDown = false;
				this.longPressTrigger = true;
			}
			if (!this.enabledKeep)
			{
				switch (this.transition)
				{
				case ButtonTransition.ColorTint:
					if (this.buttonImage != null)
					{
						this.buttonImage.color = this.normalColor;
					}
					break;
				case ButtonTransition.SpriteSwap:
					if (this.buttonImage != null && this.pressImage != null)
					{
						this.buttonImage.sprite=this.normalImage;
					}
					break;
				}
			}
			base.transform.localScale = this.scale;
		}
	}
}
