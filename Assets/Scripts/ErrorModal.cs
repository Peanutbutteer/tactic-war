using UnityEngine;
using System;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace Prototype.NetworkLobby
{
	public class ErrorModal : MonoBehaviour
	{
		protected float m_Timer;

        public Text text;

		private static ErrorModal s_instance;

		public static ErrorModal s_Instance
		{
			get
			{
				return s_instance;
			}
		}

		public void CloseModal()
		{
			gameObject.SetActive(false);
            this.text.text = "Server Disconnect";
        }

        public void Show()
		{
            gameObject.SetActive(true);

        }

        public void Show(String text)
        {
            gameObject.SetActive(true);
            this.text.text = text;
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
			m_Timer = timer;
			m_Callback = callback;
			m_IsTiming = true;
		}
	}
}