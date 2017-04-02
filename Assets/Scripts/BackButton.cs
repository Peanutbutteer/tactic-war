using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]

/// <summary>
/// Tracks opening order of menus and modals to fire the correct back button delegate when Android back button/Esc key is pressed.
/// </summary>
public class BackButton : MonoBehaviour
{
    public AudioSource source;

    private static List<BackButton> s_buttonStack = new List<BackButton>();
    private AudioClip backSound;
	protected Button m_BackButton;

    void Start()
    {
        backSound = (AudioClip)Resources.Load("Audio/Back", typeof(AudioClip));
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }


    protected virtual void TaskOnClick()
    {
        source.clip = backSound;
        source.Play();
    }


    protected virtual void OnEnable()
	{
		m_BackButton = GetComponent<Button>();

		s_buttonStack.Add(this);
	}

	protected virtual void OnDisable()
	{
		// Pop this button
		s_buttonStack.Remove(this);
	}

	protected virtual void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			// Make sure we're on top of the stack and are enabled and interactive
			if (s_buttonStack.Count > 0 &&
				s_buttonStack[s_buttonStack.Count - 1] == this &&
				m_BackButton.interactable &&
				m_BackButton.enabled)
			{
				OnBackPressed();
			}
		}
	}

	//We use the back button logic on this screen to ensure clean closure.
	protected virtual void OnBackPressed()
	{
		m_BackButton.onClick.Invoke();
	}
}
