using DG.Tweening;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum e_ObstacleTypes
{
    Wall
}
public class ObstacleReferences : MonoBehaviour
{
    [Header("Parameters")]

    [Tooltip("Which type of obstacle is this ?")]
    public e_ObstacleTypes ObstacleType;



    [Header("References")]

    [ShowIf("@" + nameof(ObstacleType) + " == e_ObstacleTypes.Wall")]
    [Tooltip("A reference to the wall's mesh renderer component, to change its color when the ball switches from being big enough or too small to crush walls")]
    [SerializeField]
    private MeshRenderer m_WallMeshRenderer;

    [Space]

    [ShowIf("@" + nameof(ObstacleType) + " == e_ObstacleTypes.Wall")]
    [Tooltip("A reference to the wall's original euler rotation, to be able to reset its current rotation in case the player crushes it whilst playing")]
    [SerializeField]
    private Vector3 m_WallOriginalEulerRotation;



    [Tooltip("If true, the wall is already flattened")]
    private bool m_WallIsFlattened = false;



#if UNITY_EDITOR
    //To set all the possible references through code
    //(useful when you have multiple different objects with the same script attached (multiple type of enemies etc...))
    [Button]
    public void SetRefs()
    {
        switch (ObstacleType)
        {
            case e_ObstacleTypes.Wall:
                m_WallOriginalEulerRotation = transform.localEulerAngles;

                m_WallMeshRenderer = GetComponentInChildren<MeshRenderer>();
                break;
        }
    }
#endif


    //Reset the obstacle to how it was on level start
    public void ResetObstacle()
    {
        switch (ObstacleType)
        {
            case e_ObstacleTypes.Wall:
                SwitchWallColor(false);
                SwitchWallFlattenedState(false);
                break;
        }
    }


    #region Gameplay Mechanics

    //Change the wall's color
    private Tween f_WallColorSwitchingTween;
    private Color f_WantedColor;
    public void SwitchWallColor(bool i_SwitchToCrushableColor)
    {
        f_WallColorSwitchingTween?.Kill();

        if (i_SwitchToCrushableColor == true)
        {
            f_WantedColor = LevelManager.Instance.LevelData.WallCrushableColor;
        }
        else
        {
            f_WantedColor = LevelManager.Instance.LevelData.WallNonCrushableColor;
        }

        f_WallColorSwitchingTween = m_WallMeshRenderer.material.DOColor(f_WantedColor, LevelManager.Instance.LevelData.WallColorChangeDuration);
    }

    private Tween f_WallFlatteningTween;
    public void SwitchWallFlattenedState(bool i_FlattenWall)
    {
        if (m_WallIsFlattened == i_FlattenWall) return;


        m_WallIsFlattened = i_FlattenWall;

        f_WallFlatteningTween?.Kill();


        if (i_FlattenWall == true)
        {
            f_WallFlatteningTween = transform.DOLocalRotate(new Vector3(90f, 0f, 0f), LevelManager.Instance.LevelData.WallFlatteningDuration);
        }
        else
        {
            transform.localEulerAngles = m_WallOriginalEulerRotation;
        }
    }

    #endregion
}
