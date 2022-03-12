using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweenAnimatorBase : MonoBehaviour
{
    [Header("Shared")]

    [Header("Parameters")]

    [Tooltip("Should this transform be locally or world animated ?")]
    [SerializeField]
    protected bool isLocal = true;

    [Tooltip("Should this transform be relatively animated ?")]
    [SerializeField]
    protected bool isRelative = true;

    [Space]

    [Tooltip("The time it will take for this animation to reach the target value")]
    [SerializeField]
    protected float targetReachDuration;

    [Space]

    [Tooltip("Should this animation play on awake ?")]
    [SerializeField]
    protected bool playOnAwake = true;

    [Space]

    [Tooltip("The ease type used on this animation")]
    [SerializeField]
    protected Ease easeType = Ease.Linear;

    [Space]

    [InfoBox("-1 for endless looping")]
    [Tooltip("The loop type used on this animation")]
    [SerializeField]
    protected int loopCount;

    [Tooltip("The loop type used on this animation")]
    [SerializeField]
    protected LoopType loopType = LoopType.Incremental;
}
