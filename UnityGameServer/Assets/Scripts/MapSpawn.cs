using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawn : MonoBehaviour
{
    public GameObject world;

    public enum mapType
    {
        Cube=1,
        Plane,
        Sphere,
        Capsule
    }

    public void MapInfo()
    {
        int CubeCount = world.transform.childCount;

        for (int i=0; i<CubeCount; i++)
        {
            Transform Cube = world.transform.GetChild(i);
            ServerSend.BlockPosition(Cube.position, (int)mapType.Sphere);
        }
    }
}