using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class DebugController : MonoBehaviour
{
    [Tooltip("The action inputs script related to touch controls")]
    private ActionInputsTouch m_TouchControls;



    #region Unity Events

    //Subscribing and unsubscribing the appropriate debug related functions to the appropriate events
    private void OnEnable()
    {
        //Enable the appropriate action input
        m_TouchControls.Debug.Enable();


        m_TouchControls.Debug.LevelReset.performed += OnLevelReset;
    }
    private void OnDisable()
    {
        //Disable the appropriate action input
        m_TouchControls.Debug.Disable();


        m_TouchControls.Debug.LevelReset.performed -= OnLevelReset;
    }

    private void Awake()
    {
        //Declare the appropriate action input
        m_TouchControls = new ActionInputsTouch();
    }

    #region Custom Events

    private void OnLevelReset(InputAction.CallbackContext i_Context)
    {
        GameManager.UpdateLevelState(e_LevelStates.Started);
    }

    #endregion

    #endregion


}
