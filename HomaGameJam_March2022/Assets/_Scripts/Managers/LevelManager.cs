using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : SingletonManager<LevelManager>
{
    [Header("Parameters")]

    [Tooltip("An array of all the levels")]
    [ReadOnly]
    [SerializeField]
    private LevelReferences[] m_Levels;

    [Space]

    [Tooltip("The current level being played")]
    [ReadOnly]
    [SerializeField]
    private LevelReferences m_CurrentLevel;
    public LevelReferences currentLevel => m_CurrentLevel;

    [Tooltip("The index of the current level being played")]
    [ReadOnly]
    [SerializeField]
    private int m_CurrentLevelIndex;
    public int CurrentLevelIndex => m_CurrentLevelIndex;

    [Space]

    [Tooltip("The amount of points the player currently has in the current level")]
    [ReadOnly]
    [SerializeField]
    private int m_CurrentPoints;
    public int CurrentPoints => m_CurrentPoints;

    public void UpdateCurrentPoints(int i_PointsToAdd)
    {
        m_CurrentPoints += i_PointsToAdd;

        UIManager.Instance.UpdateInGamePointsText(m_CurrentPoints);
    }


    [Header("References")]

    [Tooltip("A reference to the level's database, where all of its related values can be found")]
    [SerializeField]
    private LevelData m_LevelData;
    public LevelData LevelData => m_LevelData;



#if UNITY_EDITOR
    //To set all the possible references through code
    //(useful when you have multiple different objects with the same script attached (multiple type of enemies etc...))
    [Button]
    public void SetRefs()
    {
        m_Levels = GameObject.FindObjectsOfType<LevelReferences>(true);
    }
#endif


    #region Unity Events

    //Subscribing and unsubscribing the appropriate level related functions to the appropriate game manager events
    private void OnEnable()
    {
        GameManager.OnLevelStartEvent += OnLevelStart;
        GameManager.OnLevelSuccessEvent += OnLevelSuccess;
        GameManager.OnLevelFailureEvent += OnLevelFailed;
    }
    private void OnDisable()
    {
        GameManager.OnLevelStartEvent -= OnLevelStart;
        GameManager.OnLevelSuccessEvent -= OnLevelSuccess;
        GameManager.OnLevelFailureEvent -= OnLevelFailed;
    }

    //What happens on level start related to the levels
    private void OnLevelStart()
    {
        SetCurrentLevel(m_Levels[m_CurrentLevelIndex]);

        m_CurrentLevel.ResetLevel();
        UpdateCurrentPoints(-CurrentPoints);

        PlayerManager.Instance.SpawnPlayer();
    }
    //What happens on level success related to the levels
    private void OnLevelSuccess()
    {
        m_CurrentLevel.OnLevelSucess();
    }
    //What happens on level failed related to the levels
    private void OnLevelFailed()
    {
        //PlayerManager.Instance.DisablePlayer();
        
        
        m_CurrentLevel.PauseLevelMovement();

        SetCurrentLevel(null);
    }

    #endregion


    //To set which level is the current level
    private void SetCurrentLevel(LevelReferences i_CurrentLevelReferences)
    {
        m_CurrentLevel = i_CurrentLevelReferences;
    }
}
