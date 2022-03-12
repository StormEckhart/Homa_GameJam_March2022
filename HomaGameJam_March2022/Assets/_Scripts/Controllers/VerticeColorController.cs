using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticeColorController : MonoBehaviour
{
    [Header("References")]

    [Tooltip("The mesh fitler to color")]
    [SerializeField]
    private MeshFilter m_MeshFilterToColor;


    [Header("Parameters")]

    [Tooltip("The base color for non coloured vertices")]
    [SerializeField]
    private Color m_BaseColor;

    [Tooltip("The list of all wanted colors")]
    [SerializeField]
    private Color[] m_Colors;

    [Space]

    [Tooltip("Will color every vertice that's a multiple of this value")]
    [SerializeField]
    private int m_AmountToColorEachTime;



    Vector3[] f_Vertices;
    Vector3[] f_ModifiedVertices;
    int[] f_Triangles;
    int[] f_ModifiedTriangles;
    Color32[] f_Colors;
    [Button]
    public void ColorVertices()
    {
        f_Triangles = m_MeshFilterToColor.mesh.triangles;

        f_Vertices = m_MeshFilterToColor.mesh.vertices;
        f_ModifiedVertices = new Vector3[f_Triangles.Length];

        f_ModifiedTriangles = new int[f_Triangles.Length];

        f_Colors = new Color32[f_Triangles.Length];


        int l_AmountColored = 0;
        for (int i = 0; i < f_ModifiedTriangles.Length; i++)
        {
            // Makes every vertex unique
            f_ModifiedVertices[i] = f_Vertices[f_Triangles[i]];
            f_ModifiedTriangles[i] = i;


            f_Colors[f_Triangles[i]] = m_BaseColor;


            if (l_AmountColored < m_AmountToColorEachTime)
            {
                f_Colors[i] = m_Colors[Random.Range(0, m_Colors.Length)];

                l_AmountColored++;
            }
        }


        // Apply changes to the mesh
        m_MeshFilterToColor.mesh.vertices = f_ModifiedVertices;
        m_MeshFilterToColor.mesh.triangles = f_ModifiedTriangles;
        m_MeshFilterToColor.mesh.SetColors(f_Colors);
    }
}
