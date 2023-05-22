using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    public float smoothTime = 0.25f;
    Vector3 currVelocity;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LateUpdate() {

        transform.position = Vector3.SmoothDamp(
            transform.position, 
            target.position + offset, 
            ref currVelocity, 
            smoothTime);
        
    }
}
