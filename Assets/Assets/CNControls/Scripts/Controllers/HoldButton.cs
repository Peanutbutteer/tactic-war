using UnityEngine;
using UnityEngine.EventSystems;
using CnControls;
	public class HoldButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		public string ButtonName = "CancelSkill";
		private VirtualButton _virtualButton;
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
			_virtualButton.Press();
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			_virtualButton.Release();
		}
	}