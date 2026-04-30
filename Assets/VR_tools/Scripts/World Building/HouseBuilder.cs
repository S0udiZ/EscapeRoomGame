using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HouseBuilder : MonoBehaviour
{
    public GameObject wall_prefab, corner_prefab, tile_prefab;
    public float wall_height = 2.4f;
    public Vector3[] corners;
    public Vector3[] roof_top;
    public Vector3[] roof_bottom;

    public void Awake()
    {
        for (int i = 0; i < corners.Length; i++)
        {
            GameObject inst = Instantiate(corner_prefab, transform.position + corners[i],Quaternion.identity,transform);
            Vector3 dir;
            float dist;
            if (i < corners.Length - 1)
            {
                dir = (corners[i + 1] - corners[i]).normalized;
                dist = Vector3.Distance(corners[i], corners[i + 1]);
            }
            else
            {
                dir = (corners[0] - corners[i]).normalized;
                dist = Vector3.Distance(corners[i], corners[0]);
            }
            inst.transform.LookAt(inst.transform.position + dir);
            if(dist > 2)
            {
                int amount = Mathf.RoundToInt(dist / 2);
                for (int x = 0; x < amount; x++)
                {
                    inst = Instantiate(wall_prefab, transform.position + corners[i] + dir * (dist / amount * x), Quaternion.identity, transform);
                    inst.transform.LookAt(inst.transform.position + dir);
                    inst.transform.localScale = new Vector3(1, 1, (dist / 2) / amount);
                }
            }
            else
            {
                inst = Instantiate(wall_prefab, transform.position + corners[i], Quaternion.identity, transform);
                inst.transform.LookAt(inst.transform.position + dir);
                inst.transform.localScale = new Vector3(1,1,dist/2);
            }
        }
        //implement house roof
        //will be fucked to do but can be done
        for (int i = 0; i < roof_top.Length; i++)
        {
            float dist = Vector3.Distance(roof_top[i], roof_top[i + 1]);
            float height_dist1 = Vector3.Distance(roof_top[i], roof_bottom[i * 2]);
            float height_dist2 = Vector3.Distance(roof_top[i], roof_bottom[i * 2 + 1]);
        }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        for (int i = 0; i < corners.Length; i++)
        {
            Gizmos.DrawCube(transform.position + corners[i], Vector3.one * 0.1f);
            Gizmos.DrawLine(transform.position + corners[i], transform.position + corners[i] + Vector3.up * wall_height);
            if (i < corners.Length - 1)
            {
                Gizmos.DrawLine(transform.position + corners[i], transform.position + corners[i+1]);
            }
            else
            {
                Gizmos.DrawLine(transform.position + corners[i], transform.position + corners[0]);
            }
        }
        Gizmos.color = Color.blue;
        for (int i = 0; i < roof_top.Length; i++)
        {
            Gizmos.DrawCube(transform.position + roof_top[i] + Vector3.up * wall_height, Vector3.one * 0.1f);
            Gizmos.DrawLine(transform.position + roof_bottom[i * 2] + Vector3.up * wall_height, transform.position + roof_top[i] + Vector3.up * wall_height);
            Gizmos.DrawLine(transform.position + roof_bottom[i * 2 + 1] + Vector3.up * wall_height, transform.position + roof_top[i] + Vector3.up * wall_height);
            Gizmos.DrawLine(transform.position + roof_bottom[i * 2] + Vector3.up * wall_height, transform.position + roof_bottom[i * 2 + 1] + Vector3.up * wall_height);
            if (i < roof_top.Length - 1)
            {
                Gizmos.DrawLine(transform.position + roof_top[i] + Vector3.up * wall_height, transform.position + roof_top[i + 1] + Vector3.up * wall_height);
                Gizmos.DrawLine(transform.position + roof_bottom[(i + 1) * 2] + Vector3.up * wall_height, transform.position + roof_bottom[i * 2] + Vector3.up * wall_height);
                Gizmos.DrawLine(transform.position + roof_bottom[(i+ 1) * 2 + 1] + Vector3.up * wall_height, transform.position + roof_bottom[i * 2 + 1] + Vector3.up * wall_height);
            }
        }
    }
}
