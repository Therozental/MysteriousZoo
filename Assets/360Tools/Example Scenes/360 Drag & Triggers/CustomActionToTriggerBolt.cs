using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using IST360Tools; 

public class CustomActionToTriggerBolt : CustomActionScript
{

    public UnityEvent eventTrigger;

    public override void DoAction() {

        if(eventTrigger != null)
        {
            eventTrigger.Invoke();
        }

    }
}
