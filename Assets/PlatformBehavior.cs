using System;
using DG.Tweening;
using UnityEngine;

public class PlatformBehavior : MonoBehaviour
{
    public Transform front;
    [SerializeField]
    private MeshFilter _meshFilter;

    [SerializeField] private Rigidbody _rb;
    private void Start()
    { 
        _meshFilter.mesh = PlatformManager.Instance.GetRandomMesh();
    }

    private void Update()
    {
        _rb.linearVelocity = new Vector3(0, 0, GameMetrics.Instance.platformTravelTime);
        if (transform.position.z < PlatformManager.Instance.platformEndPoint.position.z)
        {
            SetAtSpawn();
        }
    }

    private void SetAtSpawn()
    {
        _meshFilter.mesh = PlatformManager.Instance.GetRandomMesh();
        transform.position = PlatformManager.Instance.GetFarPlatform().GetComponent<PlatformBehavior>().front.position;
        transform.SetAsFirstSibling();
    }
}
