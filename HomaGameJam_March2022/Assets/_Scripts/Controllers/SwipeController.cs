using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeController : MonoBehaviour
{
    [Header("Parameters")]

    [Tooltip("The minimum swipe distance (in screen coordinates) for it to be registered as such")]
    [SerializeField]
    private float m_MinimumDistance = 100f;

    [Tooltip("The maximum swipe time for it to be registered as such")]
    [SerializeField]
    private float m_MaximumTime = 1f;

    [Tooltip("The swipe's threshold to determine in which direction the swipe was made")]
    [SerializeField, Range(0f, 1f)]
    private float m_DirectionThreshold = .9f;



    [Tooltip("The current start position for the player's finger on the screen")]
    private Vector2 m_StartPosition;
    [Tooltip("The time when the player starts touching the screen")]
    private float m_StartTime;

    [Tooltip("The current end position for the player's finger on the screen")]
    private Vector2 m_EndPosition;
    [Tooltip("The time when the player finishes touching the screen")]
    private float m_EndTime;



    #region Unity Events

    //Subscribing and unsubscribing to the appropriate events
    private void OnEnable()
    {
        InputManager.OnStartTouchEvent += SwipeStart;
        InputManager.OnEndTouchEvent += SwipeEnd;
    }
    private void OnDisable()
    {
        InputManager.OnStartTouchEvent -= SwipeStart;
        InputManager.OnEndTouchEvent -= SwipeEnd;
    }

    #endregion

    //The start of the swipe, when the player touches the screen
    private void SwipeStart(Vector2 i_Position, float i_Time)
    {
        m_StartPosition = i_Position;
        m_StartTime = i_Time;
    }
    //The end of the swipe, when the player lifts his finger off of the screen
    private void SwipeEnd(Vector2 i_Position, float i_Time)
    {
        m_EndPosition = i_Position;
        m_EndTime = i_Time;

        DetectSwipe();
    }


    //To determine if the player has swiped or not, depending on several parameters
    Vector3 f_Direction3D;
    Vector2 f_Direction2D;
    private void DetectSwipe()
    {
        if (Vector3.Distance(m_StartPosition, m_EndPosition) >= m_MinimumDistance 
           && 
           (m_EndTime - m_StartTime) <= m_MaximumTime)
        {
            Debug.LogError("Swipe detected");

            //Calculate the total 3D direction
            f_Direction3D = m_EndPosition - m_StartPosition;
            //Translate it to a 2D value between 0 and 1
            f_Direction2D = new Vector2(f_Direction3D.x, f_Direction3D.y).normalized;
            DetermineSwipeDirection(f_Direction2D);
        }
    }

    //To determine in which direction the player has swiped
    private void DetermineSwipeDirection(Vector2 i_Direction)
    {
        if (Vector2.Dot(Vector2.up, i_Direction) > m_DirectionThreshold)
        {
            Debug.LogError("Swipe up");
        }
        else if (Vector2.Dot(Vector2.down, i_Direction) > m_DirectionThreshold)
        {
            Debug.LogError("Swipe down");
        }
        else if (Vector2.Dot(Vector2.right, i_Direction) > m_DirectionThreshold)
        {
            Debug.LogError("Swipe right");
        }
        else if (Vector2.Dot(Vector2.left, i_Direction) > m_DirectionThreshold)
        {
            Debug.LogError("Swipe left");
        }
    }
}
