using UnityEngine;
using System.Collections;
 
// Interactive Storytelling Course by Tsach Weinberg
// Visual Communications Department
// Bezalel Academy of Arts and Design
// 2020

namespace IST360Tools {

[AddComponentMenu("360 Tools/Drag Look")]
[DisallowMultipleComponent]
public class DragLook : MonoBehaviour {
 
    public Texture2D cursorTexture;
    public Texture2D onDragLookTexture;
    [Tooltip("Drag sensitivity. Change befor play.")]
    [Range(0.5f,5.0f)]
    public float sensitivity = 1.0f;
    [Tooltip("How fast will the camera stop moving after drag ends.")]
    [Range(0.85f,1)]
    public float damping = 0.95f;


    private float dpi_sensitivity = 1.0f;

    private float minimumX = -360F;
    private float maximumX = 360F;
    private float minimumY = -85F;
    private float maximumY = 85F;

    private float rotationX;
    private float rotationY;

    private float inertiaX;
    private float inertiaY;

    private CursorMode cursorMode = CursorMode.Auto; 
    private Vector2 hotSpot = new Vector2( 16, 16 );   
    private bool prevIsDragging = true;

    Quaternion originalRotation;

	// Use this for initialization
	void Start () {

        Application.targetFrameRate = 60;
 
        dpi_sensitivity = sensitivity * 96 / Screen.dpi;

        originalRotation = transform.localRotation;

        HandelCursor( false );

	}

    void HandelCursor( bool isDragging ) {

        if( isDragging != prevIsDragging ) {
        
            if( !isDragging && cursorTexture != null ) {

                Cursor.SetCursor( cursorTexture, hotSpot, cursorMode );
            }

            if( isDragging && onDragLookTexture != null ) {

                Cursor.SetCursor( onDragLookTexture, hotSpot, cursorMode );
            }

            prevIsDragging = isDragging;
        }

    }
 
	void Update () {

        if (Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Moved )
        {
            // Handle touch interaction:

            float pointer_x = Input.touches[0].deltaPosition.x;
            float pointer_y = Input.touches[0].deltaPosition.y;

            // print( "touches[0].deltaPosition: " + pointer_x + ", " + pointer_y );

            float deltaX = 10 * pointer_x * dpi_sensitivity * Time.deltaTime;
            float deltaY = 10 * pointer_y * dpi_sensitivity * Time.deltaTime;

            rotationX += deltaX;
            rotationY += deltaY;

            inertiaX = deltaX;
            inertiaY = deltaY;


        }
        else if ( Input.touchCount == 0 && Input.GetMouseButton(0) )
        {
            HandelCursor( true );

            // Handle mouse interaction:

            float pointer_x = Input.GetAxis("Mouse X");
            float pointer_y = Input.GetAxis("Mouse Y");

            // print( "Input.GetAxis: " + pointer_x + ", " + pointer_y );

            float deltaX = -1 * pointer_x * dpi_sensitivity;
            float deltaY = -1 * pointer_y * dpi_sensitivity;

            #if UNITY_EDITOR
                deltaX *= 10;
                deltaY *= 10;
            #endif

            rotationX += deltaX;
            rotationY += deltaY;

            inertiaX = deltaX;
            inertiaY = deltaY;

        } 
        else 
        {
            HandelCursor( false ); 

            inertiaX *= damping;
            inertiaY *= damping;

            rotationX += inertiaX;
            rotationY += inertiaY;
        }

        rotationY = ClampAngle(rotationY, minimumY, maximumY);
        rotationX = ClampAngle(rotationX, minimumX, maximumX);

        Quaternion yQuaternion = Quaternion.AngleAxis(rotationY, Vector3.left);
        Quaternion xQuaternion = Quaternion.AngleAxis(rotationX, Vector3.up);

        transform.localRotation = originalRotation * xQuaternion * yQuaternion;
	}

    public static float ClampAngle(float angle, float min, float max)
        {
            angle = angle % 360;
            if ((angle >= -360F) && (angle <= 360F))
            {
                if (angle < -360F)
                {
                    angle += 360F;
                }
                if (angle > 360F)
                {
                    angle -= 360F;
                }
            }
            return Mathf.Clamp(angle, min, max);
        }

}

}