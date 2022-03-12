using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallReferences : MonoBehaviour
{
    #region Events

    //A delegate called when this ball triggers with another collider
    public delegate void OnCollisionDelegate(Collider i_Collider);

    //An event called when this ball triggers with a collectible, for all necessary functions to subscribe to
    public event OnCollisionDelegate OnCollectibleCollisionEvent;
    //A event called when this ball triggers with an obstacle, for all necessary functions to subscribe to
    public event OnCollisionDelegate OnObstacleCollisionEvent;
    //A event called when this ball triggers with the current level's finish line, for all necessary functions to subscribe to
    public event OnCollisionDelegate OnLevelFinishCollisionEvent;

    #endregion



    [Header("References")]

    [Tooltip("A reference to the ball's mesh renderer component, to be able to change the ball's color")]
    public MeshRenderer MeshRenderer;



#if UNITY_EDITOR
    //To set all the possible references through code
    //(useful when you have multiple different objects with the same script attached (multiple type of enemies etc...))
    [Button]
    private void SetReferences()
    {
        MeshRenderer = GetComponentInChildren<MeshRenderer>();
    }
#endif


    #region Unity Events

    //To handle all trigger contact, with obstacles, pickables or the level finish line
    private void OnTriggerEnter(Collider i_Other)
    {
        if (i_Other.gameObject.tag == StaticStrings.Tags.Collectible)
        {
            if (OnCollectibleCollisionEvent != null) OnCollectibleCollisionEvent(i_Other);
        }
        else if (i_Other.gameObject.tag == StaticStrings.Tags.Obstacle)
        {
            if (OnObstacleCollisionEvent != null) OnObstacleCollisionEvent(i_Other);
        }
        else if (i_Other.gameObject.tag == StaticStrings.Tags.FinishLine)
        {
            if (OnLevelFinishCollisionEvent != null) OnLevelFinishCollisionEvent(i_Other);
        }
    }

    #endregion
}
