using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class TeleportArea : MonoBehaviour
{

    public enum telport_types
    {
        area,
        point
    }
    public telport_types type;
    public Vector3 size;

    private void Awake()
    {
        VR_Teleport.teleport_areas.Add(this);
    }
#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, size);
        if (type == telport_types.point)
        {
            Gizmos.DrawSphere(transform.position, 0.1f);
        }
    }
#endif
}

public static class VR_Teleport
{

    public static List<TeleportArea> teleport_areas = new List<TeleportArea>();

    public static Vector3 Teleport(Vector3 position)
    {
        for (int i = 0; i < teleport_areas.Count; i++)
        {
            if (InsideBounds(position, teleport_areas[i].size, teleport_areas[i].transform.position))
            {
                if (teleport_areas[i].type == TeleportArea.telport_types.area)
                {
                    return (new Vector3(position.x, teleport_areas[i].transform.position.y, position.z));
                }
                else
                {
                    return teleport_areas[i].transform.position;
                }
            }
        }
        return new Vector3(9999,9999,9999);
    }

    private static bool InsideBounds(Vector3 position, Vector3 size, Vector3 center)
    {
        if (position.y > -size.y / 2 + center.y && position.y < size.y / 2 + center.y &&
            position.x > -size.x / 2 + center.x && position.x < size.x / 2 + center.x &&
            position.z > -size.z / 2 + center.z && position.z < size.z / 2 + center.z)
        {
            return true;
        }
        return false;
    }
}