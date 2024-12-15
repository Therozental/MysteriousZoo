using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 1) Use the 360Tools library.
using IST360Tools; 

// 2) Our class should inherit from CustomActionScript instead of Monobehavior
//    Learn more about inheritance here: https://learn.unity.com/tutorial/inheritance
public class CustomActionExample : CustomActionScript 
{
    // 3) Override the DoAction method. Learn more: https://learn.unity.com/tutorial/overriding
    public override void DoAction() {

        // 4) Do anything here...
        Debug.Log( gameObject.name + " has been triggered" );

    }
}
