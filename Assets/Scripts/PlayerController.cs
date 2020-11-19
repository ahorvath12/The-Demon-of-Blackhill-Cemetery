using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Animator[] hands;

    private int index = 0;
    private bool isPlaying;

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
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Tab))
        {
            hands[index].SetTrigger("Lower");
            isPlaying = true;
        }

        
    }
}
