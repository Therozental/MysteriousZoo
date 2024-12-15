using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Interactive Storytelling Course by Tsach Weinberg
// Visual Communications Department
// Bezalel Academy of Arts and Design
// 2020

namespace IST360Tools {
    
[AddComponentMenu("360 Tools/Look Indicator")]
[DisallowMultipleComponent]
    public class LookIndicator : MonoBehaviour {

        public Camera targetCamera;
        public float indicationRadius = 0.4f;
        private Vector2 resolution;
        private Vector2 screenCenter;
        private float pxRadius;
        private Trigger[] triggers;
        private List<Trigger> insideFieldOfView;
        
        void Start()
        {
            if(!targetCamera) {
                Debug.LogError("Please attach a Camera to the CursorController component");
                this.gameObject.SetActive(false);
            }

            triggers = FindObjectsOfType<Trigger>();
            insideFieldOfView = new List<Trigger>();
            
            ResetScreenCenter();
        }

        void Update()
        {
            // Ray ray = targetCamera.ViewportPointToRay(new Vector3(0.5F, 0.5F, 0));

            if( resolution.x != Screen.width || resolution.y != Screen.height ) 
            {
                ResetScreenCenter();
            }

            foreach (Trigger trigger in triggers)
            {
                // Debug.Log( trigger.transform.position );
                Vector3 screenPoint = targetCamera.WorldToScreenPoint( trigger.transform.position );
                
                if( screenPoint.x > 0 && screenPoint.y > 0 && screenPoint.z > 0 ) 
                {
                    float centerDistance = Vector2.Distance( screenPoint, screenCenter );

                    if( centerDistance < pxRadius ) {

                        if( !insideFieldOfView.Contains( trigger ) ) 
                        {
                            trigger.LookEnter();
                            insideFieldOfView.Add( trigger );
                        }

                    } else {

                        if( insideFieldOfView.Contains( trigger ) ) 
                        {
                            trigger.LookExit();
                            insideFieldOfView.Remove( trigger );
                        }
                    }
                }

            }

        }

        void ResetScreenCenter() 
        {
            resolution = new Vector2( Screen.width, Screen.height );
            screenCenter = new Vector2( Screen.width / 2 , Screen.height / 2 );

            float screenSize = Mathf.Min( resolution.x, resolution.y );
            pxRadius =  indicationRadius * screenSize;
        }

    }
}