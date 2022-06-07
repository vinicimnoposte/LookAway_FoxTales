﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MoveChanPhisical : MonoBehaviour
{
    public Rigidbody rdb;
    public Animator anim;
    Vector3 movaxis, turnaxis;
    public GameObject currentCamera;
    public float jumpspeed = 8;
    public float gravity = 20;

    float jumptime;
    float flyvelocity = 3;
    public GameObject wing;
    public Transform rightHandObj, leftHandObj;
    bool jumpbtn = false;
    bool jumpbtndown = false;
    bool jumpbtnrelease = false;
    GameObject closeThing;
    float weight;

    [SerializeField]
    bool hbEnabled = false;

    

    
    // Start is called before the first frame update
    void Start()
    {
      
        if (SceneManager.GetActiveScene().name.Equals("Land"))
        {
            if (PlayerPrefs.HasKey("OldPlayerPosition"))
            {
                print("movendo "+ PlayerPrefsX.GetVector3("OldPlayerPosition"));
                transform.position = PlayerPrefsX.GetVector3("OldPlayerPosition");
               
            }
        }
        currentCamera = Camera.main.gameObject;
        GameObject.FindObjectOfType<hbController>().enabled = hbEnabled;

        


        

    }
    private void Update()
    {
        if(Input.GetButtonDown("Jump"))
        {
            jumpbtn = true;
            jumpbtndown = true;
        }
        if (Input.GetButtonUp("Jump"))
        {
            jumpbtn = false;
            jumptime = 0;
        }
        movaxis = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

       
        if(Input.GetKeyDown(KeyCode.O))
        {
            GameObject.FindObjectOfType<hbController>().enabled = !hbEnabled;
            hbEnabled = !hbEnabled;
        }
            

    }

    void FixedUpdate()
    {

   
        Vector3 relativedirection = currentCamera.transform.TransformVector(movaxis);
        relativedirection = new Vector3(relativedirection.x, jumptime, relativedirection.z);

        Vector3 relativeDirectionWOy = relativedirection;
        relativeDirectionWOy = new Vector3(relativedirection.x,0, relativedirection.z);

        
        anim.SetFloat("Speed", rdb.velocity.magnitude);

        if (wing.activeSelf)
        {

            float velocity = Mathf.Abs(rdb.velocity.x)+ Mathf.Abs(rdb.velocity.z);
            velocity = Mathf.Clamp(velocity, 0, 10);

            rdb.AddRelativeForce(new Vector3(0, velocity*120, 1000));
            
             Vector3 movfly = new Vector3(Vector3.forward.x* flyvelocity, 0, Vector3.forward.z* flyvelocity);

             float angz = Vector3.Dot(transform.right, Vector3.up);
             float angx = Vector3.Dot(transform.forward, Vector3.up);
             movfly = new Vector3(movaxis.z+ angx*2, -angz, -movaxis.x- angz);

             transform.Rotate(movfly);

             wing.transform.localRotation = Quaternion.Euler(0, 0, angz*50);


             flyvelocity -= angx*0.01f;
             flyvelocity = Mathf.Lerp(flyvelocity, 3, Time.fixedDeltaTime);
             flyvelocity = Mathf.Clamp(flyvelocity,0,5);
             
        }
        else
        {
            //rdb.velocity = relativeDirectionWOy*5 + new Vector3(0,rdb.velocity.y,0);
            rdb.AddForce(relativeDirectionWOy *10000/(rdb.velocity.magnitude+1));
            Quaternion rottogo = Quaternion.LookRotation(relativeDirectionWOy * 2 + transform.forward);
            transform.rotation = Quaternion.Lerp(transform.rotation, rottogo, Time.fixedDeltaTime * 50);
        }
        if (Input.GetButtonDown("Fire1"))
        {
            anim.SetTrigger("PunchA");
        }

      

        RaycastHit hit;
        if (Physics.Raycast(transform.position-(transform.forward*0.1f)+transform.up*0.3f, Vector3.down,out hit, 1000))
        {
            anim.SetFloat("JumpHeight", hit.distance);
            if (hit.distance < 0.5f && jumpbtn)
            {
                jumptime = 0.25f;
            }
            if (hit.distance>0.5f && jumpbtndown && !wing.activeSelf)
            {
                
                wing.SetActive(true);
                jumpbtndown = false;
                return;
            }
            if (hit.distance > 0.5f && jumpbtndown && wing.activeSelf)
            {
               wing.SetActive(false);
                
            }

           

        }

        

        if (jumpbtn)
        {
            jumptime -= Time.fixedDeltaTime;
            jumptime = Mathf.Clamp01(jumptime);
            rdb.AddForce(Vector3.up * jumptime * jumpspeed);

        }

        jumpbtndown = false;

    }


    //a callback for calculating IK
    void OnAnimatorIK()
    {
        if (wing.activeSelf)
        {

            if (rightHandObj != null)
            {
               
                anim.SetIKPositionWeight(AvatarIKGoal.RightHand, 1);
                anim.SetIKRotationWeight(AvatarIKGoal.RightHand, 1);
                anim.SetIKPosition(AvatarIKGoal.RightHand, rightHandObj.position);
                anim.SetIKRotation(AvatarIKGoal.RightHand, rightHandObj.rotation);

                anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, 1);
                anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, 1);
                anim.SetIKPosition(AvatarIKGoal.LeftHand, leftHandObj.position);
                anim.SetIKRotation(AvatarIKGoal.LeftHand, leftHandObj.rotation);

            }
        }

        if (closeThing)
        {
            //calcula a direcao do ponto de toque para a personagem
            Vector3 handDirection = closeThing.transform.position - transform.position;
            //verifica se o objeto ta na frente do personagem >0
            float lookto = Vector3.Dot(handDirection.normalized, transform.forward);
            //calcula e interpola o peso pela formula (l*3)/distancia^3
            weight=Mathf.Lerp(weight,(lookto*3 / (Mathf.Pow(handDirection.magnitude,3))),Time.fixedDeltaTime*2);
           
            anim.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
            anim.SetIKRotationWeight(AvatarIKGoal.RightHand, weight);
            anim.SetIKPosition(AvatarIKGoal.RightHand, closeThing.transform.position + transform.right * 0.1f);
            anim.SetIKRotation(AvatarIKGoal.RightHand, Quaternion.identity);

            anim.SetIKPositionWeight(AvatarIKGoal.LeftHand, weight);
            anim.SetIKRotationWeight(AvatarIKGoal.LeftHand, weight);
            anim.SetIKPosition(AvatarIKGoal.LeftHand, closeThing.transform.position- transform.right*0.1f);
            anim.SetIKRotation(AvatarIKGoal.LeftHand, Quaternion.identity);

            if (weight <= 0)
            {
                Destroy(closeThing);
            }
           
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        wing.SetActive(false);
        if (collision.transform.position.y > transform.position.y + .05f) {
            if(!closeThing)
            closeThing = new GameObject("Handpos");

            weight = 0;
            closeThing.transform.parent = collision.gameObject.transform;
            closeThing.transform.position= collision.GetContact(0).point;

        }

    }
    private void OnCollisionExit(Collision collision)
    {


    }
}
