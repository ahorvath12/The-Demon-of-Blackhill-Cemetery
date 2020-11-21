using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlight : MonoBehaviour
{
    public GameObject light;

    // Start is called before the first frame update
    void Start()
    {
        if (name != "HandLight")
        {
            GetComponent<Flashlight>().enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Tab))
        {
            if (light.GetComponent<Light>().enabled)
                light.GetComponent<Light>().enabled = false;
        }
    }

    public void TurnOnLight()
    {
        if (gameObject.name == "HandLight")
            light.GetComponent<Light>().enabled = true;
    }
}
