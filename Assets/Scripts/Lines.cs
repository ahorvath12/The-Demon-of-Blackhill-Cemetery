using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Lines
{
    public string line;
    public AudioClip audio;

    public Lines(string line, AudioClip audio)
    {
        this.line = line;
        this.audio = audio;
    }

    public IEnumerator PlayDialogue(Text text, AudioSource audioSource)
    {
        text.text = line;
        text.enabled = true;
        audioSource.clip = audio;
        yield return new WaitForSeconds(audio.length);
        text.enabled = false;
    }

    //public void PlayDialogue(Text text, AudioSource audioSource)
    //{
    //    text.text = line;
    //    text.enabled = true;
    //    audioSource.clip = audio;
    //}
}
