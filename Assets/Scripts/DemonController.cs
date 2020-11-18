using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonController : MonoBehaviour
{
    public GameObject mesh1, mesh2;
    public bool walk, crouch;

    // Start is called before the first frame update
    void Start()
    {
        if (walk)
        {
            GetComponent<Animator>().SetTrigger("Walk");
        }
        else if(crouch)
        {
            GetComponent<Animator>().SetTrigger("Crouch");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("Player enter");
            GetComponent<Animator>().SetTrigger("Fade");
        }
    }
}
