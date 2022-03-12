using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : SingletonManager<UIManager>
{
    public enum e_UIScreens
    {
        None,
        Welcome,
        InGame,
        Success,
        Failure
    }


    [Header("Parameters")]

    [ReadOnly]
    [Tooltip("The current UI screen displayed")]
    [SerializeField]
    private e_UIScreens m_CurrentDisplayedUIScreen;


    [Header("References")]

    [ReadOnly]
    [Tooltip("A reference to the UI's welcome screen")]
    [SerializeField]
    private GameObject m_WelcomeScreen;

    [ReadOnly]
    [Tooltip("A reference to the UI's success screen")]
    [SerializeField]
    private GameObject m_SuccessScreen;

    [ReadOnly]
    [Tooltip("A reference to the UI's failure screen")]
    [SerializeField]
    private GameObject m_FailureScreen;

    [Space]

    [ReadOnly]
    [Tooltip("A reference to the UI's in game screen")]
    [SerializeField]
    private GameObject m_InGameScreen;

    [ReadOnly]
    [Tooltip("A reference to the UI's in game points text")]
    [SerializeField]
    private TextMeshProUGUI m_InGamePointsText;
    [ReadOnly]
    [Tooltip("A reference to the UI's success screen total points text")]
    [SerializeField]
    private TextMeshProUGUI m_SuccessPointsText;



#if UNITY_EDITOR
    //To set all the possible references through code
    //(useful when you have multiple different objects with the same script attached (multiple type of enemies etc...))
    [Button]
    public void SetRefs()
    {
        m_WelcomeScreen = transform.FindDeepChild(StaticStrings.Names.WelcomeScreen).gameObject;
        m_SuccessScreen = transform.FindDeepChild(StaticStrings.Names.SuccessScreen).gameObject;
        m_FailureScreen = transform.FindDeepChild(StaticStrings.Names.FailureScreen).gameObject;

        m_InGameScreen = transform.FindDeepChild(StaticStrings.Names.InGameScreen).gameObject;

        m_InGamePointsText = m_InGameScreen.transform.FindDeepChild(StaticStrings.Names.InGamePointsText).GetComponent<TextMeshProUGUI>();
        m_SuccessPointsText = m_SuccessScreen.transform.FindDeepChild(StaticStrings.Names.SuccessPointsText).GetComponent<TextMeshProUGUI>();
    }
#endif


    #region Unity Events

    //Subscribing and unsubscribing the appropriate level related functions to the appropriate game manager events
    private void OnEnable()
    {
        GameManager.OnGameLaunchEvent += OnGameLaunch;

        GameManager.OnLevelStartEvent += OnLevelStart;
        GameManager.OnLevelSuccessEvent += OnLevelSuccess;
        GameManager.OnLevelFailureEvent += OnLevelFailed;
    }
    private void OnDisable()
    {
        GameManager.OnGameLaunchEvent -= OnGameLaunch;

        GameManager.OnLevelStartEvent -= OnLevelStart;
        GameManager.OnLevelSuccessEvent -= OnLevelSuccess;
        GameManager.OnLevelFailureEvent -= OnLevelFailed;
    }

    #region Custom Events

    private void OnGameLaunch()
    {
        ShowSpecificUIScreen(e_UIScreens.Welcome);
    }

    private void OnLevelStart()
    {
        ShowSpecificUIScreen(e_UIScreens.InGame);
    }
    private void OnLevelSuccess()
    {
        ShowSpecificUIScreen(e_UIScreens.Success);
    }
    private void OnLevelFailed()
    {
        ShowSpecificUIScreen(e_UIScreens.Failure);
    }

    #endregion

    #endregion

    #region Button Methods

    //What happens when the user presses the button on welcome screen
    public void OnWelcomeButtonPress()
    {
        GameManager.UpdateLevelState(e_LevelStates.Started);
    }
    //What happens when the user presses the button on success screen
    public void OnSuccessButtonPress()
    {
        GameManager.UpdateLevelState(e_LevelStates.Started);
    }
    //What happens when the user presses the button on failure screen
    public void OnFailureButtonPress()
    {
        GameManager.UpdateLevelState(e_LevelStates.Started);
    }

    #endregion


    #region UI

    //Show the wanted UI screen whilst closing the others
    private void ShowSpecificUIScreen(e_UIScreens i_WantedUIScreen)
    {
        if (m_CurrentDisplayedUIScreen == i_WantedUIScreen) return;


        m_WelcomeScreen.SetActive(false);
        m_SuccessScreen.SetActive(false);
        m_FailureScreen.SetActive(false);
        m_InGameScreen.SetActive(false);

        switch (i_WantedUIScreen)
        {
            case e_UIScreens.Welcome:
                m_WelcomeScreen.SetActive(true);
                break;

            case e_UIScreens.InGame:
                m_InGameScreen.SetActive(true);
                break;

            case e_UIScreens.Success:
                m_SuccessPointsText.text = m_InGamePointsText.text + " Points";

                m_SuccessScreen.SetActive(true);
                break;

            case e_UIScreens.Failure:
                m_FailureScreen.SetActive(true);
                break;
        }

        m_CurrentDisplayedUIScreen = i_WantedUIScreen;
    }

    //Update the shown points in the points text component
    public void UpdateInGamePointsText(int i_UpdatedValue)
    {
        m_InGamePointsText.text = i_UpdatedValue.ToString();
    }

    #endregion
}
