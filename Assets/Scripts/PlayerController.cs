using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public Animator[] hands;
    public Text text;

    [HideInInspector]

    private GameObject ecto;

    private int index = 0;
    private bool isPlaying, canCollectSample;

    // Start is called before the first frame update
    void Start()
    {
        hands[index].SetTrigger("Raise");
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlaying && !hands[index].GetCurrentAnimatorStateInfo(0).IsName("Raise"))
        {
            isPlaying = false;
            index++;
            if (index >= hands.Length)
                index = 0;
            
            hands[index].SetTrigger("Raise");
            
        }
        if (!isPlaying && Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Tab))
        {
            hands[index].SetTrigger("Lower");
            isPlaying = true;
            
        }

        if (canCollectSample && Input.GetKeyDown(KeyCode.E))
        {
            if (hands[index].name == "HandBeaker")
            {
                Destroy(ecto);
                text.enabled = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Ectoplasm")
        {
            text.text = "Press [e] to collect sample with flask";
            text.enabled = true;
            canCollectSample = true;
            ecto = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ectoplasm")
        {
            text.enabled = false;
            canCollectSample = false;
            ecto = null;
        }
    }
}
