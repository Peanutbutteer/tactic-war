﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using CnControls;
	public class HoldButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		public string ButtonName = "CancelSkill";
		private VirtualButton _virtualButton;
        public Image imageButton;
		private void OnEnable()
		{
			_virtualButton = _virtualButton ?? new VirtualButton(ButtonName);
			CnInputManager.RegisterVirtualButton(_virtualButton);
		}

		private void OnDisable()
		{
			CnInputManager.UnregisterVirtualButton(_virtualButton);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
        if (imageButton != null)
        {
            imageButton.gameObject.SetActive(true);
        }
        _virtualButton.Press();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
            if(imageButton!=null)
            {
                 imageButton.gameObject.SetActive(false);
            }
			_virtualButton.Release();
		}
	}