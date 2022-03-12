using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Data", menuName = "Databases/Create PlayerData", order = 1)]
public class PlayerData : ScriptableObject
{
    [Header("Parameters")]

    [Tooltip("How fast by default the player can move sideways")]
    public float MovingSidewaysBaseSpeed = 2f;

    [Tooltip("How much does the ball's scale slow down the player moving sideways " +
             "(m_PlayerData.MovingSidewaysBaseSpeed - m_PlayerData.MovingSidewaysScaleSlowness * m_BallReferences.transform.localScale.x * 0.01f)")]
    public float MovingSidewaysScaleSlowness = .5f;

    [Tooltip("How far should the sideways target move during swiping")]
    public float TargetHorizontalSensitivity = 2f;

    [Space]

    [Tooltip("How much the ball can become before losing on the next obstacle hit (the ball will start at this scale on level start)")]
    public float MinimumSize = .25f;
    [Tooltip("How big the ball can become")]
    public float MaximumSize = 10f;

    [Tooltip("How big the ball must be to be able to crush walls")]
    public float WallCrushingSize = 1.1f;

    [Tooltip("The amount the ball's scale increments when collecting a pot of modelling clay")]
    public float GrowthPerCollection = .25f;
    [Tooltip("The amount the ball's scale decrements when collising with an obstacle")]
    public float LossPerObstacle = .5f;


    [Header("Visuals")]

    [Tooltip("The time it takes to animate the ball's change of scale")]
    public float SizeChangeAnimationDuration = .5f;

    [Space]

    [Tooltip("The constant distance the ball's edge must have with the center of the character")]
    public float DistanceBallEdgeAndCharacter = .125f;

    [Space]

    [Tooltip("The time to wait before calling the camera into view animation")]
    public float WaitDurationCameraIntoViewAnimation = 2f;

    [Tooltip("The time it takes to animate the camera moving to the camera view")]
    public float CameraIntoViewAnimationDuration = 1f;
}
