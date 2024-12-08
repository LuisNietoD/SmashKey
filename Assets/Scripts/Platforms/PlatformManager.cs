using System;
using LTX.Singletons;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlatformManager : MonoSingleton<PlatformManager>
{
    public Transform platformEndPoint;
    public GameObject[] platformPrefabs;
    public int numberOfPlatforms = 5;

    private void Start()
    {
        for (int i = 0; i < numberOfPlatforms; i++)
        {
            InstantiateMap();
        }
    }

    private void Update()
    {
        transform.GetChild(0).transform.position = transform.GetChild(1).GetComponent<PlatformBehavior>().front.position;
    }

    public void InstantiateMap()
    {
        GameObject map = platformPrefabs[Random.Range(0, platformPrefabs.Length)];
        
        GameObject platform = Instantiate(map, transform);
        platform.transform.position = transform.childCount > 1
            ? transform.GetChild(0).GetComponent<PlatformBehavior>().front.position
            : platformEndPoint.position + new Vector3(0, 0, 3);

        platform.transform.SetAsFirstSibling();
    }
}
