using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class hbController : MonoBehaviour
{
    Rigidbody hb;
    public float multiplier;
    public float moveForce, turnTorque;
    // Start is called before the first frame update
    void Start()
    {
        hb = GetComponent<Rigidbody>();
    }
    

    public Transform[] anchors = new Transform[4];
    RaycastHit[] hits = new RaycastHit[4];

    // Update is called once per frame
    private void FixedUpdate()
    {
        for (int i = 0; i < 4; i++)
            ApplyForce(anchors[i], hits[i]);
        hb.AddForce(Input.GetAxis("Vertical") * moveForce * transform.forward);
        hb.AddTorque(Input.GetAxis("Horizontal") * turnTorque * transform.up);
    }
    void ApplyForce(Transform anchor, RaycastHit hit)
    {
        if(Physics.Raycast(anchor.position, -anchor.up, out hit))
        {
            float force = 0;
            force = Mathf.Abs(1 / (hit.point.y - anchor.position.y));
            hb.AddForceAtPosition(transform.up * force * multiplier, anchor.position, ForceMode.Acceleration);
        }
    }
}
