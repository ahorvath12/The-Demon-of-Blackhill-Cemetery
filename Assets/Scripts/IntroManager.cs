using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroManager : MonoBehaviour
{
    public AudioClip introClip;
    public string dialogue;

    public Text text;
    public AudioSource audioSource;

    private Lines line;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;

        line = new Lines(dialogue, introClip);
        line.PlayDialogue(text, audioSource);
        audioSource.Play();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return) || !audioSource.isPlaying)
            SceneManager.LoadScene("Main");
        
    }
}
