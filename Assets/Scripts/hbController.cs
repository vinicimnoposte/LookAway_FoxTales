using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hbController : MonoBehaviour
{
    Rigidbody hb;
    public float multiplier;
    public float moveForce, turnTorque;
    public FixedJoystick joystick;
<<<<<<< Updated upstream
    public Quaternion originalRotationValue;
    float rotationResetSpeed = 1.0f;
=======
    
>>>>>>> Stashed changes
    // Start is called before the first frame update
    void Start()
    {
        hb = GetComponent<Rigidbody>();
        originalRotationValue = transform.rotation;
    }
    public Transform[] anchors = new Transform[4];
    RaycastHit[] hits = new RaycastHit[4];

    // Update is called once per frame
    private void FixedUpdate()
    {
        for (int i = 0; i < 4; i++)
            ApplyForce(anchors[i], hits[i]);
<<<<<<< Updated upstream
        hb.AddForce(joystick.Vertical * moveForce * transform.forward);
        hb.AddTorque(joystick.Horizontal * turnTorque * transform.up);
=======
        hb.AddForce(Input.GetAxis("Vertical") * moveForce * transform.forward);
        hb.AddTorque(Input.GetAxis("Horizontal") * turnTorque * transform.up);

        //joystick
        
        hb.velocity = new Vector3(joystick.Horizontal * moveForce, hb.velocity.y, joystick.Vertical * moveForce);

        if(joystick.Horizontal != 0 || joystick.Vertical != 0)
        {
            transform.rotation = Quaternion.LookRotation(hb.velocity);
            
        }      
>>>>>>> Stashed changes
    }
    void ApplyForce(Transform anchor, RaycastHit hit)
    {
        if(Physics.Raycast(anchor.position, -anchor.up, out hit))
        {
            float force = 0;
            force = Mathf.Abs(10f / (hit.point.y - anchor.position.y));
            hb.AddForceAtPosition(transform.up * force * multiplier, anchor.position, ForceMode.Acceleration);
            
        }
<<<<<<< Updated upstream
    }
    public void ResetaPosition()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, originalRotationValue, Time.time * rotationResetSpeed);
    }
    public void ResetaPeso()
    {
        if(hb.drag > 0)
            hb.drag = 0;        
    }
    public void Normaliza()
    {
        if(hb.drag == 0)
            hb.drag = 2;        
    }
=======
    }   
>>>>>>> Stashed changes
}

