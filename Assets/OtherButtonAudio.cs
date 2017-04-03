using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

[RequireComponent(typeof(Button))]
public class OtherButtonAudio : MonoBehaviour {

    public AudioClip sound;
    private AudioSource source;
    // Use this for initialization
    void Start()
    {
        source = transform.root.GetComponent<AudioSource>();
        GetComponent<Button>().onClick.AddListener(TaskOnClick);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TaskOnClick()
    {
        source.clip = sound;
        source.Play();
    }
}
