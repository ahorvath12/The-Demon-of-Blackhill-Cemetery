using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchHands : MonoBehaviour
{

    [HideInInspector]
    public bool active;

    public void SwapOn()
    {
        active = true;
    }

    public void SwapOff()
    {
        active = false;
    }
}
