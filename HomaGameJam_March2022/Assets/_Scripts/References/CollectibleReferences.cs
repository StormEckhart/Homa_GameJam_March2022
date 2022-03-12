using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum e_CollectibleTypes
{
    Container
}
public class CollectibleReferences : MonoBehaviour
{
    [Header("Parameters")]

    [Tooltip("Which type of collectible is this ?")]
    public e_CollectibleTypes CollectibleType;

    [Space]

    [ShowIf("@" + nameof(CollectibleType) + " == e_CollectibleTypes.Container")]
    [OnValueChanged(nameof(OnContainerColorValueChanged))]
    [Tooltip("Which color container is this ?")]
    public Color ContainerColor;

    //Change the container's color when changing the ContainerColor value
    private void OnContainerColorValueChanged()
    {
        m_ContainerMeshRenderer.sharedMaterials[1].color = ContainerColor;
    }   


    [Header("References")]

    [ShowIf("@" + nameof(CollectibleType) + " == e_CollectibleTypes.Container")]
    [Tooltip("A reference to the container's mesh renderer component, to change its color when changing the ContainerColor variable")]
    [SerializeField]
    private MeshRenderer m_ContainerMeshRenderer;



#if UNITY_EDITOR
    //To set all the possible references through code
    //(useful when you have multiple different objects with the same script attached (multiple type of enemies etc...))
    [Button]
    public void SetRefs()
    {
        switch (CollectibleType)
        {
            case e_CollectibleTypes.Container:
                m_ContainerMeshRenderer = GetComponentInChildren<MeshRenderer>();
                break;
        }
    }
#endif



    //Reset the collectible to how it was on level start
    public void ResetCollectible()
    {
        switch (CollectibleType)
        {
            case e_CollectibleTypes.Container:
                m_ContainerMeshRenderer.materials[1].color = ContainerColor;
                break;
        }
    }
}
