using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public enum e_SidewayMovementTypes
    {
        OnlyDrag,
        TapAndDrag
    }

    public enum e_CharacterAnimatorStates
    {
        Idle,
        Pushing,
        Running,
        Dancing,
        Defeated
    }



    [Header("Parameters")]

    [Tooltip("The type of movement that the sideways movement is currently setup as")]
    [SerializeField]
    private e_SidewayMovementTypes m_CurrentMovementType;

    [Space]

    [ReadOnly]
    [Tooltip("The target point that the player will always move sideways towards")]
    [SerializeField]
    private float m_SidewaysMovementTarget;


    [Header("References")]

    [Tooltip("A reference to the player's database, where all his behaviour values can be found")]
    [SerializeField]
    private PlayerData m_PlayerData;

    [Space]

    [ReadOnly]
    [Tooltip("A reference to the player's render animator controller component")]
    [SerializeField]
    private Animator m_Animator;

    [Space]

    [ReadOnly]
    [Tooltip("A reference to the player's ball references script component")]
    [SerializeField]
    private BallReferences m_BallReferences;

    [Space]

    [ReadOnly]
    [Tooltip("A reference to the point where the camera will move to on level failure or success to see the player's animation")]
    [SerializeField]
    private Transform m_CameraViewPoint;



    //[Tooltip("The current sideways movement speed, relative to the respective player data values and the ball's current scale")]
    private float m_CurrentMovingSidewaysSpeed
    {
        get { return m_PlayerData.MovingSidewaysBaseSpeed - m_PlayerData.MovingSidewaysScaleSlowness * m_BallReferences.transform.localScale.x * 0.01f; }
    }

    [Tooltip("The coroutine handling the player's sideway movement relative to the user's swiping")]
    private Coroutine m_PlayerSidewaysMovementCoroutine;

    [Tooltip("The tween sequence handling changing the ball's colour when in crushing state")]
    private bool m_IsInCrushingState = false;

    [Tooltip("The tween sequence handling changing the ball's colour when in crushing state")]
    private Sequence m_BallColorSwitchingSequence;

    [Tooltip("The tween handling the delayed call to move and rotate the camera on level failure or success")]
    private Tween m_DelayedCameraCallTween;

    [Tooltip("The tween animation rotating the balls forwards")]
    private Tween m_BallRotationTween;

    [Tooltip("All the colors the player has currently collected in the current level")]
    private List<Color> m_CollectedColors = new List<Color>();



#if UNITY_EDITOR
    //To set all the possible references through code
    //(useful when you have multiple different objects with the same script attached (multiple type of enemies etc...))
    [Button]
    public void SetRefs()
    {
        m_Animator = GetComponentInChildren<Animator>();

        m_BallReferences = GetComponentInChildren<BallReferences>();

        m_CameraViewPoint = transform.FindDeepChild(StaticStrings.Names.CameraView);
    }
#endif


    #region Unity Events

    //Subscribing and unsubscribing the appropriate player related functions to the appropriate events
    private void OnEnable()
    {
        InputManager.OnStartTouchEvent += HandleMovingPlayerRelativeToSwipe;
        InputManager.OnEndTouchEvent += StopPlayerSidewaysMovementCoroutine;

        GameManager.OnLevelSuccessEvent += OnLevelSuccess;
        GameManager.OnLevelFailureEvent += OnLevelFailure;

        m_BallReferences.OnCollectibleCollisionEvent += OnCollectibleCollision;
        m_BallReferences.OnObstacleCollisionEvent += OnObstacleCollision;
        m_BallReferences.OnLevelFinishCollisionEvent += OnLevelFinishCollision;
    }
    private void OnDisable()
    {
        InputManager.OnStartTouchEvent -= HandleMovingPlayerRelativeToSwipe;
        InputManager.OnEndTouchEvent -= StopPlayerSidewaysMovementCoroutine;

        GameManager.OnLevelSuccessEvent -= OnLevelSuccess;
        GameManager.OnLevelFailureEvent -= OnLevelFailure;

        m_BallReferences.OnCollectibleCollisionEvent -= OnCollectibleCollision;
        m_BallReferences.OnObstacleCollisionEvent -= OnObstacleCollision;
        m_BallReferences.OnLevelFinishCollisionEvent -= OnLevelFinishCollision;
    }

    private void Update()
    {
        //Move the player towards the sideways target, relative to his current sideways speed
        transform.position = new Vector3
            (Mathf.MoveTowards(transform.position.x, m_SidewaysMovementTarget, m_CurrentMovingSidewaysSpeed * Time.deltaTime),
            transform.position.y, transform.position.z);
    }

    #region Custom Events

    private void OnLevelSuccess()
    {
        ChangeCharacterAnimatorState(e_CharacterAnimatorStates.Dancing);
        PauseBallRotation();

        StopPlayerSidewaysMovementCoroutine(Vector2.zero, 0f);

        m_DelayedCameraCallTween = DOVirtual.DelayedCall(m_PlayerData.WaitDurationCameraIntoViewAnimation, () =>
        {
            CameraManager.Instance.MoveCamera(m_CameraViewPoint.transform.position, m_PlayerData.CameraIntoViewAnimationDuration, Ease.InOutSine);
            CameraManager.Instance.RotateCamera(m_CameraViewPoint.transform.eulerAngles, m_PlayerData.CameraIntoViewAnimationDuration, Ease.InOutSine);
        });
    }
    private void OnLevelFailure()
    {
        ChangeCharacterAnimatorState(e_CharacterAnimatorStates.Defeated);
        PauseBallRotation();

        StopPlayerSidewaysMovementCoroutine(Vector2.zero, 0f);

        m_DelayedCameraCallTween = DOVirtual.DelayedCall(m_PlayerData.WaitDurationCameraIntoViewAnimation, () =>
        {
            CameraManager.Instance.MoveCamera(m_CameraViewPoint.transform.position, m_PlayerData.CameraIntoViewAnimationDuration, Ease.InOutSine);
            CameraManager.Instance.RotateCamera(m_CameraViewPoint.transform.eulerAngles, m_PlayerData.CameraIntoViewAnimationDuration, Ease.InOutSine);
        });
    }


    public void ResetPlayer()
    {
        m_DelayedCameraCallTween?.Kill();

        m_CollectedColors.Clear();

        ChangeCharacterAnimatorState(e_CharacterAnimatorStates.Pushing);
        ChangeBallSize(Vector3.one * m_PlayerData.MinimumSize);

        SwitchBallCrushingState(false);

        RotateBall();
    }

    #endregion

    #endregion


    #region Movement

    //Handle the player's sideway movement, through a Coroutine, called on start touch, and stopped on end touch
    private void HandleMovingPlayerRelativeToSwipe(Vector2 i_Position, float i_Time)
    {
        if (GameManager.Instance.CurrentLevelState != e_LevelStates.Started) return;

        m_PlayerSidewaysMovementCoroutine = StartCoroutine(HandleMovingPlayerRelativeToSwipeCoroutine(i_Position));
    }
    private IEnumerator HandleMovingPlayerRelativeToSwipeCoroutine(Vector2 i_Position)
    {
        while (true)
        {
            switch (m_CurrentMovementType)
            {
                case e_SidewayMovementTypes.OnlyDrag:
                    //The target starts on the player's position, and then moves relative to the user's horizontal dragging 
                    m_SidewaysMovementTarget += InputManager.Instance.ReturnPrimaryDeltaPosition().x * (m_PlayerData.TargetHorizontalSensitivity * Time.deltaTime);
                    break;

                case e_SidewayMovementTypes.TapAndDrag:
                    //The target starts on the starting touch position (i_Position) and then moves relative to the user's horizontal dragging 
                    m_SidewaysMovementTarget = InputManager.Instance.ReturnPrimaryCurrentPosition(true).x;
                    break;
            }


            //Clamp the sideways target so that the player stays within the level bounds
            m_SidewaysMovementTarget = Mathf.Clamp
                (
                m_SidewaysMovementTarget, 
                - (LevelManager.Instance.currentLevel.Ground.transform.localScale.x / 2) + m_BallReferences.transform.localScale.x / 2, 
                LevelManager.Instance.currentLevel.Ground.transform.localScale.x / 2 - m_BallReferences.transform.localScale.x / 2
                );


            yield return null;
        }
    }
    private void StopPlayerSidewaysMovementCoroutine(Vector2 i_Position, float i_Time)
    {
        if (m_PlayerSidewaysMovementCoroutine != null) StopCoroutine(m_PlayerSidewaysMovementCoroutine);

        m_SidewaysMovementTarget = transform.position.x;
    }

    #endregion


    #region Triggers

    //What happens on collectible collision
    private void OnCollectibleCollision(Collider i_CollectibleCollider)
    {
        if (i_CollectibleCollider.TryGetComponent(out CollectibleReferences l_CollectibleReferences))
        {
            switch (l_CollectibleReferences.CollectibleType)
            {
                //Add the parameter color onto the player's ball and increment its scale
                case e_CollectibleTypes.Container:
                    ChangeBallSize(m_BallReferences.transform.localScale + Vector3.one * m_PlayerData.GrowthPerCollection, l_CollectibleReferences.ContainerColor);

                    LevelManager.Instance.UpdateCurrentPoints(LevelManager.Instance.LevelData.ContainerCollectionPoints);
                    break;
            }


            //Deactivate the used collectible
            l_CollectibleReferences.gameObject.SetActive(false);
        }
    }

    //What happens on obstacle collision
    private Tween f_CameraShakeTween;
    private void OnObstacleCollision(Collider i_ObstacleCollider)
    {
        if (f_CameraShakeTween.IsActive() == false)
            f_CameraShakeTween = CameraManager.Instance.camera.DOShakePosition(0.25f, 0.5f);


        if (m_IsInCrushingState == true)
        {
            if (i_ObstacleCollider.TryGetComponent(out ObstacleReferences l_ObstacleReferences))
            {
                switch (l_ObstacleReferences.ObstacleType)
                {
                    //Add the parameter color onto the player's ball and increment its scale
                    case e_ObstacleTypes.Wall:
                        l_ObstacleReferences.SwitchWallFlattenedState(true);

                        LevelManager.Instance.UpdateCurrentPoints(LevelManager.Instance.LevelData.WallFlatteningPoints);
                        break;
                }
            }

            return;
        }


        if ((m_BallReferences.transform.localScale - Vector3.one * m_PlayerData.LossPerObstacle).IsGreaterOrEqual(Vector3.one * m_PlayerData.MinimumSize))
        {
            ChangeBallSize(m_BallReferences.transform.localScale - Vector3.one * m_PlayerData.LossPerObstacle);
        }
        else
        {
            ChangeBallSize(Vector3.zero);

            GameManager.UpdateLevelState(e_LevelStates.Failed);
        }

        LevelManager.Instance.UpdateCurrentPoints(-LevelManager.Instance.LevelData.ObstacleCollisionMinusPoints);
    }

    //What happens on level finish collision
    private void OnLevelFinishCollision(Collider i_LevelFinishCollider)
    {
        GameManager.UpdateLevelState(e_LevelStates.Won);
    }

    #endregion


    #region Gameplay Mechanics

    //Methods to call when the ball needs to change size (bigger or smaller), that handle everything that should happen during that process
    private Tween f_ChangeBallScaleTween;
    private Tween f_ChangeBallColorTween;
    private Tween f_ChangeBallPositionTween;
    private void ChangeBallSize(Vector3 i_WantedSize)
    {
        //Kill previous ongoing animations
        f_ChangeBallScaleTween?.Kill();
        f_ChangeBallColorTween?.Kill();
        f_ChangeBallPositionTween?.Kill();


        //Setup and start the scale changing animation
        f_ChangeBallScaleTween = m_BallReferences.transform.DOScale(i_WantedSize, m_PlayerData.SizeChangeAnimationDuration);

        //Setup and start the position changing animation
        f_ChangeBallPositionTween = m_BallReferences.transform.DOLocalMove(
            new Vector3
            (
            0f,
            i_WantedSize.y / 2,
            i_WantedSize.z / 2 + m_PlayerData.DistanceBallEdgeAndCharacter
            ),
            m_PlayerData.SizeChangeAnimationDuration);


        //Check if it's time to change the ball's current state, if so change it
        if (i_WantedSize.IsGreaterOrEqual(Vector3.one * m_PlayerData.WallCrushingSize))
        {
            SwitchBallCrushingState(true);
        }
        else
        {
            SwitchBallCrushingState(false);
        }
    }
    private void ChangeBallSize(Vector3 i_WantedSize, Color i_WantedColor)
    {
        //Kill previous ongoing animations
        f_ChangeBallScaleTween?.Kill();
        f_ChangeBallColorTween?.Kill();
        f_ChangeBallPositionTween?.Kill();


        //Setup and start the scale changing animation
        f_ChangeBallScaleTween = m_BallReferences.transform.DOScale(i_WantedSize, m_PlayerData.SizeChangeAnimationDuration);

        //Setup and start the color changing animation
        f_ChangeBallColorTween = m_BallReferences.MeshRenderer.material.DOColor(i_WantedColor, m_PlayerData.SizeChangeAnimationDuration);

        //Setup and start the position changing animation
        f_ChangeBallPositionTween = m_BallReferences.transform.DOLocalMove(
            new Vector3
            (
            0f,
            i_WantedSize.y / 2,
            i_WantedSize.z / 2 + m_PlayerData.DistanceBallEdgeAndCharacter
            ),
            m_PlayerData.SizeChangeAnimationDuration);


        //Add the collected color to the collected colors list, for later use
        m_CollectedColors.Add(i_WantedColor);


        //Check if it's time to change the ball's current state, if so change it
        if (i_WantedSize.IsGreaterOrEqual(Vector3.one * m_PlayerData.WallCrushingSize))
        {
            SwitchBallCrushingState(true);
        }
        else
        {
            SwitchBallCrushingState(false);
        }
    }

    private void SwitchBallCrushingState(bool i_SwitchToCrushableState)
    {
        if (m_IsInCrushingState == i_SwitchToCrushableState) return;


        if (i_SwitchToCrushableState == true)
        {
            LevelManager.Instance.currentLevel.ChangeWallColors(true);


            if (m_BallColorSwitchingSequence == null)
            {
                SetupBallColorSwitchingSequence();
            }
            else
            {
                m_BallColorSwitchingSequence.Restart();
            }
        }
        else
        {
            LevelManager.Instance.currentLevel.ChangeWallColors(false);


            if (m_BallColorSwitchingSequence != null)
            {
                m_BallColorSwitchingSequence.Kill();
            }
        }


        m_IsInCrushingState = i_SwitchToCrushableState;
    }

    #endregion

    #region Only Visuals

    //Handles setting up the ball color swithching feedback that plays on switching to crushing state
    private Tween f_ColorSwitchingTween;
    private void SetupBallColorSwitchingSequence()
    {
        m_BallColorSwitchingSequence = DOTween.Sequence();


        for (int i = 0; i < m_CollectedColors.Count; i++)
        {
            f_ColorSwitchingTween = m_BallReferences.MeshRenderer.material.DOColor(m_CollectedColors[i], m_PlayerData.SizeChangeAnimationDuration);

            m_BallColorSwitchingSequence.Append(f_ColorSwitchingTween);
        }


        m_BallColorSwitchingSequence.SetLoops(-1, LoopType.Yoyo)
                                    .Play();
    }


    //Change the character's current animation
    private void ChangeCharacterAnimatorState(e_CharacterAnimatorStates i_WantedState)
    {
        m_Animator.ResetTrigger(StaticStrings.Animator.DanceTrigger);
        m_Animator.ResetTrigger(StaticStrings.Animator.DefeatTrigger);
        m_Animator.ResetTrigger(StaticStrings.Animator.ResetTrigger);

        m_Animator.SetBool(StaticStrings.Animator.IsPushingBool, false);
        m_Animator.SetBool(StaticStrings.Animator.IsRunningBool, false);


        switch (i_WantedState)
        {
            case e_CharacterAnimatorStates.Idle:
                m_Animator.SetTrigger(StaticStrings.Animator.ResetTrigger);
                break;

            case e_CharacterAnimatorStates.Pushing:
                m_Animator.SetBool(StaticStrings.Animator.IsPushingBool, true);
                break;

            case e_CharacterAnimatorStates.Running:
                m_Animator.SetBool(StaticStrings.Animator.IsRunningBool, true);
                break;

            case e_CharacterAnimatorStates.Dancing:
                m_Animator.SetTrigger(StaticStrings.Animator.DanceTrigger);
                break;

            case e_CharacterAnimatorStates.Defeated:
                m_Animator.SetTrigger(StaticStrings.Animator.DefeatTrigger);
                break;
        }
    }

    //To start the ball's rotation
    private void RotateBall()
    {
        m_BallRotationTween?.Kill();


        m_BallRotationTween = m_BallReferences.transform.DOLocalRotate(Vector3.right * 360f, 2f, RotateMode.LocalAxisAdd)
                                                        .SetEase(Ease.Linear)
                                                        .SetLoops(-1, LoopType.Incremental);
    }
    //To pause the ball's rotation
    private void PauseBallRotation()
    {
        m_BallRotationTween?.Pause();
    }

    #endregion
}
