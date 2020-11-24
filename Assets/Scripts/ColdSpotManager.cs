using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColdSpotManager : MonoBehaviour
{
    public bool hasChecked;
    public GameObject demon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            hasChecked = true;
            demon.SetActive(true);
        }
    }
}
