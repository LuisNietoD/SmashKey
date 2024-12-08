using UnityEngine;

public class SetPositionParent : MonoBehaviour
{
    private Transform tr;
    
    private void Awake()
    {
        tr = transform;
    }
    
    // Update is called once per frame
    void Update()
    {
        tr.position = tr.parent.position;
    }
}
