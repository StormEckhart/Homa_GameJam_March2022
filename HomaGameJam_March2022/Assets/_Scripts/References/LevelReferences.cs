using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelReferences : MonoBehaviour
{
    [Header("References")]

    [Tooltip("This level's player spawn point")]
    [ReadOnly]
    [SerializeField]
    private Vector3 m_SpawnPoint;
    public Vector3 SpawnPoint => m_SpawnPoint;

    [Space]

    [Tooltip("This level's player")]
    [ReadOnly]
    [SerializeField]
    private PlayerController m_Player;
    public PlayerController Player => m_Player;

    [Space]

    [Tooltip("This level's collectibles")]
    [ReadOnly]
    [SerializeField]
    private CollectibleReferences[] m_Collectibles;
    public CollectibleReferences[] Collectibles => m_Collectibles;

    [Tooltip("This level's collectibles")]
    [ReadOnly]
    [SerializeField]
    private ObstacleReferences[] m_Obstacles;
    public ObstacleReferences[] Obstacles => m_Obstacles;

    [Space]

    [Tooltip("This level's ground transform")]
    [ReadOnly]
    [SerializeField]
    private Transform m_Ground;
    public Transform Ground => m_Ground;

    [Tooltip("The part of the level that moves towards the player")]
    [ReadOnly]
    [SerializeField]
    private Transform m_MovingGroup;
    [Tooltip("The part of the level that moves towards the player's original position, to be able to reset it")]
    private Vector3 m_MovingGroupOriginalPosition;

    [Tooltip("The level's decoration that also moves towards the player")]
    [ReadOnly]
    [SerializeField]
    private Transform m_DecorationGroup;
    [Tooltip("The level's decoration that also moves towards the player's original position, to be able to reset it")]
    private Vector3 m_DecorationGroupOriginalPosition;

    [Space]

    [Tooltip("A list of all FXs gameObject components related to level success")]
    [ReadOnly]
    [SerializeField]
    private List<GameObject> m_FXOnSuccessObjects = new List<GameObject>();



    [Tooltip("The tween animation moving the level towards the player")]
    private Tween m_MoveLevelTowardsPlayerTween;
    [Tooltip("The tween animation moving the decoration towards the player")]
    private Tween m_MoveDecorationTowardsPlayerTween;



#if UNITY_EDITOR
    //To set all the possible references through code
    //(useful when you have multiple different objects with the same script attached (multiple type of enemies etc...))
    [Button]
    public void SetRefs()
    {
        m_SpawnPoint = transform.FindDeepChild(StaticStrings.Names.SpawnPoint).position;

        m_Player = GetComponentInChildren<PlayerController>(true);

        m_Collectibles = GetComponentsInChildren<CollectibleReferences>(true);
        m_Obstacles = GetComponentsInChildren<ObstacleReferences>(true);

        m_Ground = transform.FindDeepChild(StaticStrings.Names.Ground);
        m_MovingGroup = transform.FindDeepChild(StaticStrings.Names.MovingGroup);
        m_DecorationGroup = transform.FindDeepChild(StaticStrings.Names.DecorationGroup);

        m_MovingGroupOriginalPosition = m_MovingGroup.transform.localPosition;
        m_DecorationGroupOriginalPosition = m_DecorationGroup.transform.localPosition;

        m_FXOnSuccessObjects.Clear();

        Transform FXOnSuccessParent = transform.FindDeepChild(StaticStrings.Names.FXsOnSuccess);
        for (int i = 0; i < FXOnSuccessParent.childCount; i++)
        {
            m_FXOnSuccessObjects.Add(FXOnSuccessParent.GetChild(i).gameObject);

            m_FXOnSuccessObjects[i].SetActive(false);
        }


        m_Player.SetRefs();

        for (int i = 0; i < m_Collectibles.Length; i++)
        {
            m_Collectibles[i].SetRefs();
        }
        for (int i = 0; i < m_Obstacles.Length; i++)
        {
            m_Obstacles[i].SetRefs();
        }
    }
#endif


    #region Level

    //Trigger what happens on level success
    public void OnLevelSucess()
    {
        for (int i = 0; i < m_FXOnSuccessObjects.Count; i++)
        {
            m_FXOnSuccessObjects[i].SetActive(true);
        }
    }

    //Reset all level components as they should be on level start
    public void ResetLevel()
    {
        for (int i = 0; i < m_Collectibles.Length; i++)
        {
            m_Collectibles[i].gameObject.SetActive(true);

            m_Collectibles[i].ResetCollectible();
        }

        for (int i = 0; i < m_Obstacles.Length; i++)
        {
            m_Obstacles[i].gameObject.SetActive(true);

            m_Obstacles[i].ResetObstacle();
        }

        for (int i = 0; i < m_FXOnSuccessObjects.Count; i++)
        {
            m_FXOnSuccessObjects[i].SetActive(false);
        }


        m_MovingGroup.transform.localPosition = m_MovingGroupOriginalPosition;
        m_DecorationGroup.transform.localPosition = m_DecorationGroupOriginalPosition;

        MoveLevelTowardsPlayer();
    }


    //Start the tween animation that moves the level towards the player
    private void MoveLevelTowardsPlayer()
    {
        m_MoveLevelTowardsPlayerTween?.Kill();
        m_MoveDecorationTowardsPlayerTween?.Kill();


        m_MoveLevelTowardsPlayerTween = m_MovingGroup.DOLocalMove
            (
            new Vector3(0, 0, -m_Ground.localScale.z + 1 + LevelManager.Instance.LevelData.LevelTargetValue),
            LevelManager.Instance.LevelData.MovePerUnitDuration * m_Ground.localScale.z
            )
            .SetEase(Ease.Linear);


        m_MoveDecorationTowardsPlayerTween = m_DecorationGroup.DOLocalMove
            (
            new Vector3(0, 0, -m_Ground.localScale.z + 1 + LevelManager.Instance.LevelData.LevelTargetValue),
            LevelManager.Instance.LevelData.MovePerUnitDuration * m_Ground.localScale.z * 2
            )
            .SetEase(Ease.Linear);
    }
    public void PauseLevelMovement()
    {
        m_MoveLevelTowardsPlayerTween?.Pause();
        m_MoveDecorationTowardsPlayerTween?.Pause();
    }

    #endregion

    #region Level Components

    public void ChangeWallColors(bool i_SwitchToCrushableColor)
    {
        for (int i = 0; i < m_Obstacles.Length; i++)
        {
            if (m_Obstacles[i].ObstacleType == e_ObstacleTypes.Wall)
            {
                m_Obstacles[i].SwitchWallColor(i_SwitchToCrushableColor);
            }
        }
    }

    #endregion
}
