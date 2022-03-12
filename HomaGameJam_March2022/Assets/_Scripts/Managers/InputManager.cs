using Sirenix.OdinInspector;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : SingletonManager<InputManager>
{
    #region Events

    //The events related to screen touching and swiping

    //Triggered when the player starts touching the screen
    public delegate void StartTouch(Vector2 i_Position, float i_Time);
    public static event StartTouch OnStartTouchEvent;

    //Triggered when the player finishes touching the screen
    public delegate void EndTouch(Vector2 i_Position, float i_Time);
    public static event EndTouch OnEndTouchEvent;

    #endregion



    [Tooltip("The action inputs script related to touch controls")]
    private ActionInputsTouch m_TouchControls;

    [Tooltip("A direct reference to the main camera, to save some performance as it is used a lot")]
    private Camera m_Camera;



    #region Unity Events

    private void OnEnable()
    {
        //Enable the appropriate action input
        m_TouchControls.Touch.Enable();
    }
    private void OnDisable()
    {
        //Disable the appropriate action input
        m_TouchControls.Touch.Disable();
    }


    public override void Awake()
    {
        //Declare the appropriate action input
        m_TouchControls = new ActionInputsTouch();
    }

    private void Start()
    {
        //Set the direct reference to the 
        if (m_Camera == null) m_Camera = CameraManager.Instance.camera;


        //Subscribe to the appropriate touch action inputs events, on start and end contact
        m_TouchControls.Touch.PrimaryContact.started += PrimaryTouchStart;
        m_TouchControls.Touch.PrimaryContact.canceled += PrimaryTouchEnd;
    }

    #endregion


    //Trigger the events
    private void PrimaryTouchStart(InputAction.CallbackContext i_Context)
    {
        if (OnStartTouchEvent != null) OnStartTouchEvent(m_TouchControls.Touch.PrimaryPosition.ReadValue<Vector2>(), (float)i_Context.startTime);
    }
    private void PrimaryTouchEnd(InputAction.CallbackContext i_Context)
    {
        if (OnEndTouchEvent != null) OnEndTouchEvent(m_TouchControls.Touch.PrimaryPosition.ReadValue<Vector2>(), (float)i_Context.time);
    }


    //To get the current touch position, if the player is currently touching the screen, in screen or world coordinates
    public Vector2 ReturnPrimaryCurrentPosition(bool i_ReturnInWorldCoordinates)
    {
        if (i_ReturnInWorldCoordinates == false)
        {
            return m_TouchControls.Touch.PrimaryPosition.ReadValue<Vector2>();
        }
        else
        {
            return Tools.ScreenToWorld(m_Camera, m_TouchControls.Touch.PrimaryPosition.ReadValue<Vector2>());
        }
    }


    //To get the current delta position, the distance between the current touch position and its position during the last frame
    public Vector2 ReturnPrimaryDeltaPosition()
    {
        return m_TouchControls.Touch.PrimaryDeltaPosition.ReadValue<Vector2>();
    }
}
