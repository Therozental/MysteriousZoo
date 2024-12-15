using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

// Interactive Storytelling Course by Tsach Weinberg
// Visual Communications Department
// Bezalel Academy of Arts and Design
// 2020

namespace IST360Tools {

[ CustomEditor( typeof( Trigger ) ) ]
public class TriggerEditor : Editor
{

    VisualElement rootElement;
    VisualTreeAsset mainVisualTree;
    VisualTreeAsset basicActionItemVisualTree;
    Trigger pointerTrigger;

    Dictionary<string,bool> isAddingAction;

    List<TriggerAction> lookEnterActions;

    List<TriggerAction> lookExitActions;

    List<TriggerAction> pointerEnterActions;

    List<TriggerAction> pointerExitActions;

    List<TriggerAction> triggerClickActions;

    public void OnEnable() {

        // Debug.Log( "Look Trigger Editor Enable" );

        pointerTrigger = ( Trigger ) target;

        isAddingAction = new Dictionary<string, bool>();

        // Hierarchy
        rootElement = new VisualElement();
        mainVisualTree = AssetDatabase.LoadAssetAtPath <VisualTreeAsset> ( 
            
            "Assets/360Tools/Editor/Resources/TriggerMainVT.uxml" 
            
        );

        basicActionItemVisualTree = AssetDatabase.LoadAssetAtPath <VisualTreeAsset> ( 
            
            "Assets/360Tools/Editor/Resources/BasicActionItem.uxml" 
            
        );

        // Styles
        var styleSheet = AssetDatabase.LoadAssetAtPath <StyleSheet> (

            "Assets/360Tools/Editor/Resources/TriggerEditorStyle.uss"

        );

        rootElement.styleSheets.Add( styleSheet );


        if( pointerTrigger.lookEnterActions == null ) pointerTrigger.lookEnterActions = new List<TriggerAction>();
        lookEnterActions = pointerTrigger.lookEnterActions;


        if( pointerTrigger.lookExitActions == null ) pointerTrigger.lookExitActions = new List<TriggerAction>();
        lookExitActions = pointerTrigger.lookExitActions;

        if( pointerTrigger.pointerEnterActions == null ) pointerTrigger.pointerEnterActions = new List<TriggerAction>();
        pointerEnterActions = pointerTrigger.pointerEnterActions;

        if( pointerTrigger.pointerExitActions == null ) pointerTrigger.pointerExitActions = new List<TriggerAction>();
        pointerExitActions = pointerTrigger.pointerExitActions;

        if( pointerTrigger.triggerClickActions == null ) pointerTrigger.triggerClickActions = new List<TriggerAction>();
        triggerClickActions = pointerTrigger.triggerClickActions;

    }

    public override VisualElement CreateInspectorGUI() {

        // Reset root element and reuse.
        rootElement.Clear();

        // Turn the UXML into a VisualElement hierarchy under root.
        mainVisualTree.CloneTree( rootElement );

        var eventContainers = rootElement.Query <Foldout> ().ToList();
        rootElement.Query <Foldout> ().ForEach( foldout => {

            List <TriggerAction> targetEventList = null;

            switch( foldout.name ) {
                case "look-enter-event-container": 
                    foldout.text = "              ON LOOK ENTER";
                    targetEventList = lookEnterActions; 
                    break;

                case "look-exit-event-container": 
                    foldout.text = "              ON LOOK EXIT";
                    targetEventList = lookExitActions; 
                    break;

                case "pointer-enter-event-container": 
                    foldout.text = "              ON POINTER ENTER";
                    targetEventList = pointerEnterActions; 
                    break;

                case "pointer-exit-event-container": 
                    foldout.text = "              ON POINTER EXIT";
                    targetEventList = pointerExitActions; 
                    break;

                case "on-click-event-container": 
                    foldout.text = "              ON CLICK";
                    targetEventList = triggerClickActions; 
                    break;

            }

            // Collapse foldout:
            foldout.value = false;

            isAddingAction[ foldout.name ] = false;
            
            Button addButton = foldout.Query( null, "add-buttons" ).First() as Button;

            addButton.clickable.clicked += () => { 

                if( !isAddingAction[ foldout.name ] ) {

                    // Open an action type selector:
                    foldout.Insert( 1, AddActionTypeSelector( foldout, targetEventList ) );

                    isAddingAction[ foldout.name ] = true;

                } else {

                    // Remove the action type selector:
                    foldout.RemoveAt( 1 );
                    isAddingAction[ foldout.name ] = false;

                }
                
            };

            // Draw Icons:
            rootElement.Query( className: "event-icon" ).ForEach( icon => {
                var iconPath = "Assets/360Tools/Editor/Resources/icons/" + icon.name + ".png";
                var iconAsset = AssetDatabase.LoadAssetAtPath <Texture2D> ( iconPath );

                icon.style.backgroundImage = iconAsset;
            });

            // Draw existing action items:
            for( int i=0; i < targetEventList.Count; i++ ) {

                switch( targetEventList[ i ].actionType ) {
                    case TriggerAction.ActionType.CURSOR: 
                        foldout.Add( ChangeCursorActionItem( targetEventList, i) );
                        break; 
                    case TriggerAction.ActionType.ON_OFF: 
                        foldout.Add( OnOffActionItem( targetEventList, i) );
                        break; 
                    case TriggerAction.ActionType.SPRITE_RENDERER: 
                        foldout.Add( SpriteActionItem( targetEventList, i) );
                        break;
                    case TriggerAction.ActionType.ANIMATOR: 
                        foldout.Add( AnimatorActionItem( targetEventList, i) );
                        break;
                    case TriggerAction.ActionType.SOUND: 
                        foldout.Add( SoundActionItem( targetEventList, i) );
                        break;
                    // case TriggerAction.ActionType.EVENT: 
                    //     foldout.Add( EventActionItem( targetEventList, i) );
                    //     break;
                    case TriggerAction.ActionType.CUSTOM: 
                        foldout.Add( CustomActionItem( targetEventList, i) );
                        break;
                }

            }
        
        } );

        UpdateActionsCounters();

        return rootElement;
    }

    void UpdateActionsCounters() {

        rootElement.Query( null, "actions-counter" ).ForEach( counter => {

            var label = counter as Label;

            switch( label.name ) {
                case "look-enter-actions-counter": 
                    label.text = "(" + lookEnterActions.Count + ")";
                    break;

                case "look-exit-actions-counter": 
                    label.text = "(" + lookExitActions.Count + ")";
                    break;

                case "pointer-enter-actions-counter": 
                    label.text = "(" + pointerEnterActions.Count + ")";
                    break;

                case "pointer-exit-actions-counter": 
                    label.text = "(" + pointerExitActions.Count + ")";
                    break;

                case "on-click-actions-counter": 
                    label.text = "(" + triggerClickActions.Count + ")";
                    break;

            }

        });
    }

    VisualElement AddActionTypeSelector( VisualElement targetContainer, List<TriggerAction> targetEventList ) {

        var menu = new VisualElement() {

            style = {
                marginBottom = 8,

            }

        };

        foreach( var type in Enum.GetValues( typeof( TriggerAction.ActionType ) ) ) {

            var selector = new Button ( () => {

                // Debug.Log( type.ToString() );

                VisualElement actionItem = null;

                switch( type ) {

                    case TriggerAction.ActionType.CURSOR: 

                        targetEventList.Add( new TriggerAction() {

                            actionType = TriggerAction.ActionType.CURSOR,
                            changeCursorAction = new ChangeCursorAction()

                        });

                        actionItem = ChangeCursorActionItem( targetEventList, targetEventList.Count - 1 ); 
                        break;

                    case TriggerAction.ActionType.ON_OFF: 

                        targetEventList.Add( new TriggerAction() {

                            actionType = TriggerAction.ActionType.ON_OFF,
                            onOffAction = new OnOffAction()

                        });

                        actionItem = OnOffActionItem( targetEventList, targetEventList.Count - 1 ); 
                        break;

                    case TriggerAction.ActionType.SPRITE_RENDERER:
                        targetEventList.Add( new TriggerAction() {

                            actionType = TriggerAction.ActionType.SPRITE_RENDERER,
                            spriteRendererAction = new SpriteRendererAction()

                        }); 
                        actionItem = SpriteActionItem( targetEventList, targetEventList.Count - 1 ); 
                        break;

                    case TriggerAction.ActionType.ANIMATOR:
                        targetEventList.Add( new TriggerAction() {

                            actionType = TriggerAction.ActionType.ANIMATOR,
                            animatorAction = new AnimatorAction()

                        }); 
                        actionItem = AnimatorActionItem( targetEventList, targetEventList.Count - 1 ); 
                        break;

                    case TriggerAction.ActionType.SOUND:
                        targetEventList.Add( new TriggerAction() {

                            actionType = TriggerAction.ActionType.SOUND,
                            soundAction = new SoundAction()

                        }); 
                        actionItem = SoundActionItem( targetEventList, targetEventList.Count - 1 ); 
                        break;

                    // case TriggerAction.ActionType.EVENT:
                    //     targetEventList.Add( new TriggerAction() {

                    //         actionType = TriggerAction.ActionType.EVENT,
                    //         eventAction = new EventAction()

                    //     }); 
                    //     actionItem = EventActionItem( targetEventList, targetEventList.Count - 1 ); 
                    //     break;

                    case TriggerAction.ActionType.CUSTOM:
                        targetEventList.Add( new TriggerAction() {

                            actionType = TriggerAction.ActionType.CUSTOM,
                            customAction = new CustomAction()

                        }); 
                        actionItem = CustomActionItem( targetEventList, targetEventList.Count - 1 ); 
                        break;


                }

                targetContainer.Add( actionItem );
                UpdateActionsCounters();

                menu.RemoveFromHierarchy();

                isAddingAction[ targetContainer.name ] = false;

            });

            selector.text = type.ToString().Replace( "_", " " );
            selector.AddToClassList( "selector-buttons" );

            menu.Add( selector );
        }



        return menu;
    }

    VisualElement BasicActionItem( List <TriggerAction> targetEventList, TriggerAction item ) {

        var basicActionItem = new VisualElement();

        basicActionItemVisualTree.CloneTree( basicActionItem );

        var removeButton = basicActionItem.Q( null, "remove-button" ) as Button;
        removeButton.clickable.clicked += () => {

            targetEventList.Remove( item );
            basicActionItem.RemoveFromHierarchy();
            UpdateActionsCounters();

        };

        return basicActionItem;
    }


    VisualElement ChangeCursorActionItem( List <TriggerAction> targetEventList, int itemInx ) {

        var item = targetEventList[ itemInx ];
        var actionItem = BasicActionItem( targetEventList, item );

        var title = actionItem.Q( null,"action-type-title" ) as Label;
        title.text = "Change Cursor Action";

        var box = actionItem.Q( null, "basic-action-item" );

        ObjectField cursorTexture = new ObjectField( "Texture:" );
        cursorTexture.objectType = typeof( Texture2D );
        cursorTexture.value = item.changeCursorAction.cursorTexture;
        cursorTexture.RegisterValueChangedCallback( e => {

            var texture = ( e.target as ObjectField ).value as Texture2D;
            item.changeCursorAction.cursorTexture = texture;

        });
        box.Add( cursorTexture );

        return actionItem;
    }
    VisualElement OnOffActionItem( List <TriggerAction> targetEventList, int itemInx ) {

        var item = targetEventList[ itemInx ];
        var actionItem = BasicActionItem( targetEventList, item );

        var title = actionItem.Q( null,"action-type-title" ) as Label;
        title.text = "ON-OFF Action";

        var box = actionItem.Q( null, "basic-action-item" );

        ObjectField receiver = new ObjectField( "Rceiver:" );
        receiver.objectType = typeof( GameObject );
        receiver.value = item.onOffAction.actionReceiver;
        receiver.RegisterValueChangedCallback( e => {

            var go = ( e.target as ObjectField ).value as GameObject;
            item.onOffAction.actionReceiver = go;

        });
        box.Add( receiver );

        Toggle setActive = new Toggle( "Set Active:" );
        setActive.value = item.onOffAction.setActive;
        setActive.RegisterValueChangedCallback( e => {

            var flag = ( e.target as Toggle ).value;
            item.onOffAction.setActive = flag;
        });
        box.Add( setActive );

        return actionItem;
    }

    VisualElement SpriteActionItem( List <TriggerAction> targetEventList, int itemInx ) {
        
        var item = targetEventList[ itemInx ];
        var actionItem = BasicActionItem( targetEventList, item );

        var title = actionItem.Q( null,"action-type-title" ) as Label;
        title.text = "Sprite Renderer Action";

        var box = actionItem.Q( null, "basic-action-item" );

        ObjectField receiver = new ObjectField( "Rceiver:" );
        receiver.objectType = typeof( SpriteRenderer );
        receiver.value = item.spriteRendererAction.receiverRenderer;
        receiver.RegisterValueChangedCallback( e => {

            var renderer = ( e.target as ObjectField ).value as SpriteRenderer;
            item.spriteRendererAction.receiverRenderer = renderer;

        });
        box.Add( receiver );

        ColorField newColor = new ColorField( "Change to color:" );
        newColor.value = item.spriteRendererAction.newColor;
        newColor.RegisterValueChangedCallback( e => {

            var color = ( e.target as ColorField ).value;
            item.spriteRendererAction.newColor = color;

        });
        box.Add( newColor );

        ObjectField newSprite = new ObjectField( "Change to sprite:" );
        newSprite.objectType = typeof( Sprite );
        newSprite.value = item.spriteRendererAction.newSprite;
        newSprite.RegisterValueChangedCallback( e => {

            var sprite = ( e.target as ObjectField ).value as Sprite;
            item.spriteRendererAction.newSprite = sprite;

        });
        box.Add( newSprite );


        return actionItem;
    }

    VisualElement AnimatorActionItem( List <TriggerAction> targetEventList, int itemInx ) {
        
        var item = targetEventList[ itemInx ];
        var actionItem = BasicActionItem( targetEventList, item );

        var title = actionItem.Q( null,"action-type-title" ) as Label;
        title.text = "Animator Action";

        var box = actionItem.Q( null, "basic-action-item" );

        ObjectField receiver = new ObjectField( "Rceiver:" );
        receiver.objectType = typeof( Animator );
        receiver.value = item.animatorAction.receiverAnimator;
        receiver.RegisterValueChangedCallback( e => {

            var animator = ( e.target as ObjectField ).value as Animator;
            item.animatorAction.receiverAnimator = animator;

        });
        box.Add( receiver );

        EnumField parameterType = new EnumField( "Parameter Type:", AnimatorAction.AnimatorParameterType.Trigger );
        parameterType.value = item.animatorAction.parameterType;
        parameterType.RegisterValueChangedCallback( e => {

            var type = ( e.target as EnumField ).value;
            item.animatorAction.parameterType = (AnimatorAction.AnimatorParameterType)type;

        });
        box.Add( parameterType );

        TextField parameterName = new TextField( "Parameter Name: " );
        parameterName.value = item.animatorAction.parameterName;
        parameterName.RegisterValueChangedCallback( e => {

            var name = ( e.target as TextField ).value;
            item.animatorAction.parameterName = name;

        });
        box.Add( parameterName );

        TextField parameterValue = new TextField( "Parameter Value: " );
        parameterValue.value = item.animatorAction.parameterValue;
        parameterValue.RegisterValueChangedCallback( e => {

            var val = ( e.target as TextField ).value;
            item.animatorAction.parameterValue = val;

        });
        box.Add( parameterValue );

        return actionItem;
    }

    VisualElement SoundActionItem( List <TriggerAction> targetEventList, int itemInx ) {
        
        var item = targetEventList[ itemInx ];
        var actionItem = BasicActionItem( targetEventList, item );

        var title = actionItem.Q( null,"action-type-title" ) as Label;
        title.text = "Play Sound Action";

        var box = actionItem.Q( null, "basic-action-item" );

        ObjectField audioSource = new ObjectField( "Audio Source:" );
        audioSource.objectType = typeof( AudioSource );
        audioSource.value = item.soundAction.source;
        audioSource.RegisterValueChangedCallback( e => {

            var source = ( e.target as ObjectField ).value as AudioSource;
            item.soundAction.source = source;

        });
        box.Add( audioSource );

        EnumField soundActionType = new EnumField( "Action Type:", SoundAction.SoundActionType.PLAY );
        soundActionType.value = item.soundAction.actionType;
        soundActionType.RegisterValueChangedCallback( e => {

            var type = ( e.target as EnumField ).value;
            item.soundAction.actionType = (SoundAction.SoundActionType)type;

        });
        box.Add( soundActionType );


        return actionItem;
    }


    VisualElement EventActionItem( List <TriggerAction> targetEventList, int itemInx ) {
        
        var item = targetEventList[ itemInx ];
        var actionItem = BasicActionItem( targetEventList, item );

        var title = actionItem.Q( null,"action-type-title" ) as Label;
        title.text = "UNDER CONSTRUCTION: Event Action";

        var box = actionItem.Q( null, "basic-action-item" );

        // PropertyField eventTrigger = new PropertyField( , "Event Trigger:" );
        // eventTrigger.objectType = typeof( CustomEvent );
        // eventTrigger.value = item.eventAction.eventTrigger as CustomEvent;
        // eventTrigger.RegisterValueChangedCallback( e => {

        //     var eventValue = ( e.target as ObjectField ).value;
        //     item.eventAction.eventTrigger = eventValue;

        // });
        // box.Add( eventTrigger );

        return actionItem;
    }

    VisualElement CustomActionItem( List <TriggerAction> targetEventList, int itemInx ) {
        
        var item = targetEventList[ itemInx ];
        var actionItem = BasicActionItem( targetEventList, item );

        var title = actionItem.Q( null,"action-type-title" ) as Label;
        title.text = "Custom Action";

        var box = actionItem.Q( null, "basic-action-item" );

        ObjectField receiver = new ObjectField( "Rceiver:" );
        receiver.objectType = typeof( CustomActionScript );
        receiver.value = item.customAction.customActionScript;
        receiver.RegisterValueChangedCallback( e => {

            var customScript = ( e.target as ObjectField ).value as CustomActionScript;
            item.customAction.customActionScript = customScript;

        });
        box.Add( receiver );

        return actionItem;
    }


}

}

