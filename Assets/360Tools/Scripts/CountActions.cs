using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1) Use the 360Tools library.
using IST360Tools;

// 2) Our class should inherit from CustomActionScript instead of Monobehavior
//    Learn more about inheritance here: https://learn.unity.com/tutorial/inheritance

public class CountActions : CustomActionScript
{
    public int actionsCount = 0;    // Create a public int variable to store the number of actions. For example: clicks.

    public int maxActions = 999; // Change this to suit your needs.

    public bool limitNumberOfActions = false;    // Optional: Create a public bool variable to limit the number of actions.
    public bool resetActionsAfterMax = false;   // Optional: Create a public bool variable to reset the action count after reaching the maximum number of clicks.

    // Override the DoAction method.
    public override void DoAction()
    {
        // If the resetActionsAfterMax is true AND the click count is greater than or equal to the maxActions, reset the actions count.
        if (resetActionsAfterMax && actionsCount >= maxActions)
        {
            actionsCount = 0;
        }
        // If the limitNumberOfActions is true AND the click count is greater than or equal to the maxActions, set the count to be equal to max actions.
        else if (limitNumberOfActions && actionsCount >= maxActions)
        {
            actionsCount = maxActions;
        }
        else
        {
            // If both conditions are false, increment the action count.
            actionsCount++;
        }
    }
}
