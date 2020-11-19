using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject child;

    private Camera cam;
    private Animator anim;

    bool raising;
    float timeCheck=0f;

    // Start is called before the first frame update
    void Start()
    {
        cam = child.GetComponent<Camera>();
        cam.enabled = false;

        anim = GetComponent<Animator>();

        if (gameObject.name != "HandCamera")
            GetComponent<CameraController>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Tab))
        {
            if (cam.enabled)
                cam.enabled = false;
        }
        
    }

    public void TurnOn()
    {
        if (gameObject.name == "HandCamera")
            cam.enabled = true;
    }
    
}
