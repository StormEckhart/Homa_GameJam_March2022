using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tools
{
    public static Vector3 ScreenToWorld(Camera i_UsedCamera, Vector3 i_Position)
    {
        return i_UsedCamera.ScreenPointToRay(i_Position).GetPoint(-i_UsedCamera.transform.position.z);
    }
}
