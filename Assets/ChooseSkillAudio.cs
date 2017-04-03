using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Button))]

public class ChooseSkillAudio : MonoBehaviour
{

    private AudioSource source;
    private AudioClip menuSound;
    // Use this for initialization
    void Start()
    {
        source = transform.root.GetComponent<AudioSource>();
        menuSound = (AudioClip)Resources.Load("Audio/ChooseSkill", typeof(AudioClip));
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TaskOnClick()
    {
        source.clip = menuSound;
        source.Play();
    }

}
