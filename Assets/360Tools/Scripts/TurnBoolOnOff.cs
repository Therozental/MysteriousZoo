using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1) Use the 360Tools library.
using IST360Tools;

// 2) Our class should inherit from CustomActionScript instead of Monobehavior
//    Learn more about inheritance here: https://learn.unity.com/tutorial/inheritance
public class TurnBoolOnOff : CustomActionScript
{
    public bool isOn;    // Create a public bool variable to store the state of the object.

    // Override the DoAction method.
    public override void DoAction()
    {
        // Check if the object is on or off and change the state.
        if (isOn == true)
        {
            isOn = false;
        }
        else
        {
            isOn = true;
        }
    }
}
