using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1) Use the 360Tools library.
using IST360Tools;

// 2) Our class should inherit from CustomActionScript instead of Monobehavior
//    Learn more about inheritance here: https://learn.unity.com/tutorial/inheritance
public class CheckOtherObjectsBool : CustomActionScript
{
    public List<TurnBoolOnOff> objectsToCheck;    // Create a public list of GameObjects to store the objects we want to check.
    // These objects should have the TurnBoolOnOff script attached to them.
    public List<TurnBoolOnOff> objectsThatAreOn;

    public bool areAllObjectsOn;    // Create a public bool variable to store the state of the objects.

    // Override the DoAction method.
    public override void DoAction()
    {
        // Loop through the list of objects.
        foreach (TurnBoolOnOff checkOnOff in objectsToCheck)
        {
            if (checkOnOff != null)
            {
                Debug.Log("Checking object: " + checkOnOff.gameObject.name + ", is " + checkOnOff.wasInteracted);
                // Check if the object is on or off.
                if (checkOnOff.wasInteracted == true && !objectsThatAreOn.Contains(checkOnOff))
                {
                    // Add the object to the list of objects that are on.
                    objectsThatAreOn.Add(checkOnOff);
                }
                else if (checkOnOff.wasInteracted == false && objectsThatAreOn.Contains(checkOnOff))
                {
                    // Remove the object from the list of objects that are on.
                    objectsThatAreOn.Remove(checkOnOff);
                }
            }

            // Check if all objects are on.
            if (objectsThatAreOn.Count == objectsToCheck.Count)
            {
                areAllObjectsOn = true;
                //set active to the ending sprites and story animation
            }
            else
            {
                areAllObjectsOn = false;
            }
            
        }
    }
}
