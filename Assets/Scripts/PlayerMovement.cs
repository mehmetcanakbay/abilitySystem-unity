using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//very simple movement script w/o states
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;

    private float horizontal;
    private float vertical;
    public Vector3 moveDirection;

    [SerializeField]
    private float speed;
    // Start is called before the first frame update

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        moveDirection = new Vector3(horizontal, 0.0f, vertical); //y axis is not needed in this example project
        if (rb!=null) {
            rb.AddForce(moveDirection*speed*Time.fixedDeltaTime, ForceMode.Impulse);
        }
    }
}
