using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioController : MonoBehaviour
{
    public AudioClip staticClip;

    // Start is called before the first frame update
    void Start()
    {
        if (name != "HandRadio")
            GetComponent<RadioController>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOnRadio()
    {
        if (name == "HandRadio")
        {
            GetComponent<AudioSource>().clip = staticClip;
            GetComponent<AudioSource>().Play();
        }
    }
}
