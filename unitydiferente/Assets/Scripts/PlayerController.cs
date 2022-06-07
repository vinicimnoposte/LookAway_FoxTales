using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody),typeof (BoxCollider))]
public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private FixedJoystick _joystick;

    [SerializeField] private float _moveSpeed;
    public bool inRange = false;
    public bool onHover = false;

    public GameObject hb;
    private void FixedUpdate()
    {
        _rigidbody.velocity = new Vector3(_joystick.Horizontal * _moveSpeed, _rigidbody.velocity.y, _joystick.Vertical * _moveSpeed);
    }
    private void OnTriggerEnter(Collider other)
    {
        inRange = true;
    }
    private void OnTriggerExit(Collider other)
    {
        inRange = false;
    }
    public void BotaoEntraHover()
    {
        if (inRange)
        {
            gameObject.GetComponent<PlayerController>().enabled = false;
            PlayerEntra();
        }else
            gameObject.GetComponent<PlayerController>().enabled = true;
        hb.GetComponent<hbController>().enabled = true;
       // if (onHover)
       //     PlayerSai();
    }
    public void PlayerEntra()
    {
        gameObject.transform.position = new Vector3(hb.transform.position.x, hb.transform.position.y + gameObject.transform.position.y, 0);
        onHover = true;
    }public void PlayerSai()
    {
        gameObject.transform.position = new Vector3(hb.transform.position.x + 2, hb.transform.position.y, hb.transform.position.z);
        onHover = false;
    }
    public void Start()
    {
        hb.GetComponent<hbController>().enabled = false;
    }
}
