using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;

public class Rotator : TweenAnimatorBase
{
    [Header("Unique")]

    [Header("Parameters")]

    [Tooltip("This transform's end rotation value")]
    [SerializeField]
    private Vector3 m_TargetValue;



    [Tooltip("This transform's initial rotation, to be able to reset it when needed")]
    private Quaternion m_InitialRotation;



    private void Awake()
    {
        SetInitialValues();


        if (playOnAwake == true)
            StartAnimation();
    }

    public void SetInitialValues()
    {
        if (isLocal == true)
            m_InitialRotation = transform.localRotation;
        else
            m_InitialRotation = transform.rotation;
    }

    Tween f_RotationTween;
    [Button]
    private void StartAnimation()
    {
        ResetValues();


        f_RotationTween?.Kill();

        if (isLocal)
        {
            f_RotationTween = transform.DOLocalRotate(m_TargetValue, targetReachDuration, RotateMode.LocalAxisAdd)
                            .SetEase(easeType)
                            .SetRelative(isRelative)
                            .SetLoops(loopCount, loopType);
        }
        else
        {
            f_RotationTween = transform.DORotate(m_TargetValue, targetReachDuration, RotateMode.LocalAxisAdd)
                            .SetEase(easeType)
                            .SetRelative(isRelative)
                            .SetLoops(loopCount, loopType);
        }
    }

    public void ResetValues()
    {
        if (isLocal)
            transform.localRotation = m_InitialRotation;
        else
            transform.rotation = m_InitialRotation;
    }
}