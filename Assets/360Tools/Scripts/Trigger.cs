using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interactive Storytelling Course by Tsach Weinberg
// Visual Communications Department
// Bezalel Academy of Arts and Design
// 2020

namespace IST360Tools {

[AddComponentMenu("360 Tools/Trigger")]
[DisallowMultipleComponent]
public class Trigger : MonoBehaviour {

    public enum TriggerType { LookEnter, LookExit, PointerEnter, PointerExit, TriggerClick }

    [Space(10)]
    public List<TriggerAction> lookEnterActions;
    [Space(10)]
    public List<TriggerAction> lookExitActions;
    [Space(10)]
    public List<TriggerAction> pointerEnterActions;
    [Space(10)]
    public List<TriggerAction> pointerExitActions;
    [Space(10)]
    public List<TriggerAction> triggerClickActions;

    private Boolean isDragging = false;

    public void LookEnter()
    {
        //print("Look Enter " + this.gameObject.name);
        foreach (TriggerAction action in lookEnterActions)
        {
            action.DoAction();
        }

    }

    public void LookExit()
    {
        //print("Look Exit " + this.gameObject.name);
        foreach (TriggerAction action in lookExitActions)
        {
            action.DoAction();
        }
    }

    void Start() {

        ResetSpriteActionsColors();
        
    }

    private void OnMouseDown() {

        isDragging = false;
        StartCoroutine( ActivateTrigger() );

    }

    private void OnMouseDrag() {

        // Debug.Log("mouse drag");
        
        Vector2 delta = Vector2.zero; 

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved ) {
            delta = Input.touches[0].deltaPosition;
        }
        else if ( Input.touchCount == 0 && Input.GetMouseButton(0) ) {
            delta = new Vector2(
                Input.GetAxis("Mouse X"),
                Input.GetAxis("Mouse Y")
            );
        }

        if( delta.magnitude > 0.01 ) {
            isDragging = true;
        }
    }

    private void OnMouseEnter() {

        //print("Pointer Exit " + this.gameObject.name);
        foreach (TriggerAction action in pointerEnterActions )
        {
            action.DoAction();
        }
    }

    private void OnMouseExit() {

        //print("Pointer Exit " + this.gameObject.name);
        foreach (TriggerAction action in pointerExitActions)
        {
            action.DoAction();
        }
    }


    IEnumerator ActivateTrigger() {

        yield return new WaitForSeconds( 0.1f );

        if( !isDragging ) {

            //print("Trigger Clicked: " + this.gameObject.name);
            foreach (TriggerAction action in triggerClickActions)
            {
                action.DoAction();
            }

        }
    }

    void ResetSpriteActionsColors() {

        //make sure no sprite color is transparent:
        foreach (TriggerAction action in lookEnterActions)
        {
            if (action.spriteRendererAction.newColor.a == 0f)
                action.spriteRendererAction.newColor.a = 1f;
        }
        foreach (TriggerAction action in lookExitActions)
        {
            if (action.spriteRendererAction.newColor.a == 0f)
                action.spriteRendererAction.newColor.a = 1f;
        }
        foreach (TriggerAction action in triggerClickActions)
        {
            if (action.spriteRendererAction.newColor.a == 0f)
                action.spriteRendererAction.newColor.a = 1f;
        }

    }
}

}
