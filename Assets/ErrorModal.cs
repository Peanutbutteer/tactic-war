using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ErrorModal : MonoBehaviour {
	protected float m_Timer;

		private static ErrorModal s_instance;

		public static ErrorModal s_Instance
		{
			get
			{
				return s_instance;
			}
		}

		public void CloseModal() {

		}

		public void Show() {
			
		}

		protected Action m_Callback;
	
		protected bool m_IsTiming = true;

		//Closes self automatically
		protected void Awake()
		{
			//Singleton modal
			s_instance = this;
			CloseModal();
		}

		//Handle timer
		protected void Update()
		{
			if (m_IsTiming)
			{
				m_Timer = m_Timer - Time.unscaledDeltaTime;
				if (m_Timer <= 0f)
				{
					m_IsTiming = false;
					if (m_Callback != null)
					{
						m_Callback();
					}
				}
			}
		}

		//Setup how the modal behaves
		public void SetupTimer(float timer, Action callback)
		{
			this.m_Timer = timer;
			this.m_Callback = callback;
			m_IsTiming = true;
		}
}