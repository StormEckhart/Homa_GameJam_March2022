using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Level Data", menuName = "Databases/Create LevelData", order = 2)]
public class LevelData : ScriptableObject
{
    [Header("Parameters")]

    [Tooltip("How far before the player reached the far end border of the level should it stop moving towards the player")]
    [SerializeField]
    private float m_TargetValue = 2f;
    public float LevelTargetValue => m_TargetValue;

    [Tooltip("The time it takes for the level to move one unit (1 meter)")]
    [SerializeField]
    private float m_MovePerUnitDuration = .5f;
    public float MovePerUnitDuration => m_MovePerUnitDuration;

    [Space]

    [Tooltip("The time it takes to complete the wall color switching animation")]
    [SerializeField]
    private float m_WallColorChangeDuration = .5f;
    public float WallColorChangeDuration => m_WallColorChangeDuration;

    [Tooltip("The color all the walls in the current level will be switched to when the ball becomes big enough to crush them down")]
    [SerializeField]
    private Color m_WallCrushableColor;
    public Color WallCrushableColor => m_WallCrushableColor;

    [Tooltip("The color all the walls in the current level will be switched to when the ball becomes too small to crush them down")]
    [SerializeField]
    private Color m_WallNonCrushableColor;
    public Color WallNonCrushableColor => m_WallNonCrushableColor;


    [Tooltip("The time it takes to complete the wall flattening animation")]
    [SerializeField]
    private float m_WallFlatteningDuration = .25f;
    public float WallFlatteningDuration => m_WallFlatteningDuration;

    [Space]

    [Tooltip("How many points when the player receives when collecting a container")]
    [SerializeField]
    private int m_ContainerCollectionPoints = 50;
    public int ContainerCollectionPoints => m_ContainerCollectionPoints;

    [Tooltip("How many points when the player receives when flattening a wall")]
    [SerializeField]
    private int m_WallFlatteningPoints = 125;
    public int WallFlatteningPoints => m_WallFlatteningPoints;

    [Tooltip("How many points are removed when the player collides with a wall")]
    [SerializeField]
    private int m_WallCollisionMinusPoints = 75;
    public int ObstacleCollisionMinusPoints => m_WallCollisionMinusPoints;
}
