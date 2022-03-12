using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;

public class Mover : TweenAnimatorBase
{
    [Header("Unique")]

    [Header("Parameters")]

    [Tooltip("This transform's end position value")]
    [SerializeField]
    private Vector3 m_TargetValue;



    [Tooltip("This transform's initial position, to be able to reset it when needed")]
    private Vector3 m_InitialPosition;



    private void Awake()
    {
        SetInitialValues();


        if (playOnAwake == true)
            StartAnimation();
    }

    public void SetInitialValues()
    {
        if (isLocal == true)
            m_InitialPosition = transform.localPosition;
        else
            m_InitialPosition = transform.position;
    }

    Tween f_MovingTween;
    [Button]
    private void StartAnimation()
    {
        ResetValues();


        f_MovingTween?.Kill();

        if (isLocal)
        {
            f_MovingTween = transform.DOLocalMove(m_TargetValue, targetReachDuration)
                            .SetEase(easeType)
                            .SetRelative(isRelative)
                            .SetLoops(loopCount, loopType);
        }
        else
        {
            f_MovingTween = transform.DOMove(m_TargetValue, targetReachDuration)
                            .SetEase(easeType)
                            .SetRelative(isRelative)
                            .SetLoops(loopCount, loopType);
        }
    }

    public void ResetValues()
    {
        if (isLocal)
            transform.localPosition = m_InitialPosition;
        else
            transform.position = m_InitialPosition;
    }
}