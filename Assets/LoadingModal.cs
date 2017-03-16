using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingModal : MonoBehaviour {

	private FadingGroup m_Fader;

	public virtual void CloseModal()
	{
		gameObject.SetActive(false);
	}

	public virtual void Show()
	{
		gameObject.SetActive(true);
	}

	[SerializeField]
	protected float m_FadeTime = 0.5f;

	public static LoadingModal s_Instance
	{
		get;
		private set;
	}

	public bool readyToTransition
	{
		get
		{
			return m_Fader.currentFade == Fade.None && gameObject.activeSelf;
		}
	}

	public void FadeIn()
	{
		Show();
		m_Fader.StartFade(Fade.In, m_FadeTime);
	}

	public void FadeOut()
	{
		Show();
		m_Fader.StartFade(Fade.Out, m_FadeTime, CloseModal);
	}

	protected virtual void Awake()
	{
		if (s_Instance != null)
		{
			Debug.Log("<color=lightblue>Trying to create a second instance of LoadingModal</color");
			Destroy(gameObject);
		}
		else
		{
			s_Instance = this;
		}

		m_Fader = GetComponent<FadingGroup>();
	}

	protected virtual void OnDestroy()
	{
		if (s_Instance == this)
		{
			s_Instance = null;
		}
	}
}
