using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

// Interactive Storytelling Course by Tsach Weinberg
// Visual Communications Department
// Bezalel Academy of Arts and Design
// 2020

namespace IST360Tools {

[System.Serializable]
public class TriggerAction
{
    public enum ActionType { CURSOR, ON_OFF, SPRITE_RENDERER, ANIMATOR, SOUND, EVENT, CUSTOM }

    public string actionName;

    public ActionType actionType;
    
    public ChangeCursorAction changeCursorAction;
    public OnOffAction onOffAction;
    public SpriteRendererAction spriteRendererAction;
    public AnimatorAction animatorAction;
    public SoundAction soundAction;
    public EventAction eventAction;
    public CustomAction customAction;


    public void DoAction()
    {
        switch ( actionType ) {

            case ActionType.CURSOR: changeCursorAction.DoAction(); break;

            case ActionType.ON_OFF: onOffAction.DoAction(); break;

            case ActionType.SPRITE_RENDERER: spriteRendererAction.DoAction(); break;

            case ActionType.ANIMATOR: animatorAction.DoAction(); break;

            case ActionType.SOUND: soundAction.DoAction(); break;

            // case ActionType.EVENT: eventAction.DoAction(); break;

            case ActionType.CUSTOM: customAction.DoAction(); break;

        }

    }
}

[System.Serializable]
public class ChangeCursorAction : ITriggerAction
{
    public Texture2D cursorTexture;

    private CursorMode cursorMode = CursorMode.Auto; 
    private Vector2 hotSpot = new Vector2( 16, 16 );   


    public void DoAction()
    {
        if( cursorTexture != null ) {

            Cursor.SetCursor( cursorTexture, hotSpot, cursorMode );
        }

    }

}

[System.Serializable]
public class OnOffAction : ITriggerAction
{
    public GameObject actionReceiver;
    public bool setActive;

    public void DoAction()
    {
        if(actionReceiver != null)
        {
            actionReceiver.SetActive(setActive);
        }
    }
}

[System.Serializable]
public class SpriteRendererAction : ITriggerAction
{
    public SpriteRenderer receiverRenderer;
    public Color newColor = new Color(1,1,1,1);
    public Sprite newSprite;

    public void DoAction()
    {
        if(receiverRenderer != null)
        {
            if (newSprite != null)
            {
                receiverRenderer.sprite = newSprite;
            }
            receiverRenderer.color = newColor;
        }
    }
}

[System.Serializable]
public class AnimatorAction : ITriggerAction
{
    public enum AnimatorParameterType { Trigger, Int, Bool, Float }

    public Animator receiverAnimator;
    public AnimatorParameterType parameterType;
    public string parameterName;
    public string parameterValue;

    public void DoAction()
    {
        if(receiverAnimator != null)
        {
            if (parameterName != null)
            {
                switch(parameterType)
                {
                    case AnimatorParameterType.Trigger:
                        receiverAnimator.SetTrigger(parameterName);
                        break;
                    case AnimatorParameterType.Int:
                        int i;
                        bool isInt = int.TryParse(parameterValue, out i);
                        if (isInt) { receiverAnimator.SetInteger(parameterName, i); }
                        else { Debug.LogError("Error in Aminator Action: Wrong parameterValue. '" + parameterValue + "' is not an Int."); }
                        break;
                    case AnimatorParameterType.Bool:
                        if(parameterValue == "true" || parameterValue == "True" || parameterValue == "1")
                        {
                            receiverAnimator.SetBool(parameterName, true);
                        }
                        else if (parameterValue == "false" || parameterValue == "False" || parameterValue == "0")
                        {
                            receiverAnimator.SetBool(parameterName, false);
                        }
                        else
                        {
                            Debug.LogError("Error in Aminator Action: Wrong Bool parameterValue. Use 'true' or 'false'");
                        }    
                        break;
                    case AnimatorParameterType.Float:
                        float f;
                        bool isFloat = float.TryParse(parameterValue, out f);
                        if (isFloat){ receiverAnimator.SetFloat(parameterName,f); }
                        else { Debug.LogError("Error in Aminator Action: Wrong parameterValue. '" + parameterValue + "' is not a float."); }
                        break;
                }
            }
            else { Debug.LogError("Missing parameter name for AnimatorAction"); }
        }
        else { Debug.LogError("Missing receiverAnimator for AnimatorAction"); }

    }
}

[System.Serializable]
public class SoundAction : ITriggerAction
{
    public enum SoundActionType { PLAY, PAUSE, STOP }
    public AudioSource source;
    public SoundActionType actionType;

    public void DoAction() {

        if( source != null ) {

            switch( actionType ) {
                case SoundActionType.PLAY: source.Play(); break;
                case SoundActionType.PAUSE: source.Pause(); break;
                case SoundActionType.STOP: source.Stop(); break;
            }
        }

    }
}

[System.Serializable]
public class EventAction : ITriggerAction
{
    public UnityEvent eventTrigger;

    public void DoAction()
    {
        if(eventTrigger != null)
        {
            eventTrigger.Invoke();
        }
    }
}

[System.Serializable]
public class CustomAction : ITriggerAction
{
    public CustomActionScript customActionScript;

    public void DoAction()
    {
        if(customActionScript != null)
        {
            customActionScript.DoAction();
        }
    }
}

public interface ITriggerAction { void DoAction(); }

public class CustomActionScript : MonoBehaviour {
    public virtual void DoAction() {}
}

}