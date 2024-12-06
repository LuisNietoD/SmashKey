using System;
using System.Collections.Generic;
using LTX.Singletons;
using NUnit.Framework;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformManager : MonoSingleton<PlatformManager>
{
    public Transform platformEndPoint;
    public GameObject platformPrefab;
    public int numberOfPlatforms = 5;
    public List<Mesh> platformMesh;

    private void Start()
    {
        for (int i = 0; i < numberOfPlatforms; i++)
        {
            GameObject platform = Instantiate(platformPrefab, transform.childCount > 0 ? 
                transform.GetChild(0).GetComponent<PlatformBehavior>().front.position : platformEndPoint.position,
                Quaternion.identity);

            platform.transform.parent = transform;
            platform.transform.SetAsFirstSibling();
        }
    }

    public GameObject GetFarPlatform()
    {
        return transform.GetChild(0).gameObject;
    }

    public Mesh GetRandomMesh()
    {
        if (transform.GetChild(0).childCount > 0)
        {
            Mesh oldMesh = transform.GetChild(0).GetChild(0).GetComponent<MeshFilter>().mesh;
            List<Mesh> meshList = new List<Mesh>(platformMesh);
            meshList.Remove(oldMesh);
            return meshList[Random.Range(0, platformMesh.Count)];
        }

        return platformMesh[Random.Range(0, platformMesh.Count)];
    }
}
