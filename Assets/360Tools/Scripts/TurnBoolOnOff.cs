using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1) Use the 360Tools library.
using IST360Tools;
using UnityEngine.Serialization;

// 2) Our class should inherit from CustomActionScript instead of Monobehavior
//    Learn more about inheritance here: https://learn.unity.com/tutorial/inheritance
public class TurnBoolOnOff : CustomActionScript
{
    [FormerlySerializedAs("isOn")] public bool wasInteracted = false;    // Create a public bool variable to store the state of the object.

    // Override the DoAction method.
    public override void DoAction()
    {
        wasInteracted = true;
        /*
        // Check if the object is on or off and change the state.
        if (wasInteracted == true)
        {
            wasInteracted = false;
        }
        else
        {
            wasInteracted = true;
        }
        */
    }
}
