using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerControllerB2 : MonoBehaviour {
    public Cam cam;
    public NavMeshAgent agent;
    public Animator anim;
    public Rigidbody rig;
    public float a;
    public float mh;
    public float mv;
    public float velocity;
    public float jumpVelocity;
    private float horizontal;
    private float vertical;
    private float turn;
    private Vector3 forward;
    //private bool IsGrounded;
    private float distToGround;

    private void Start() {
        forward = Vector3.forward;
        //IsGrounded = true;
        anim.SetBool("Jump", false);
        //anim.SetBool("Land", false);
        distToGround = GetComponent<CapsuleCollider>().bounds.extents.y;
    }

    // Update is called once per frame
    void Update() {
        if (!cam.attached)
            return;
        anim.SetBool("Strafe", false);
        //Debug.Log("is grounded:"+IsGrounded());
        //Debug.Log("Jump:" + anim.GetBool("Jump"));
        //Debug.Log("Land:" + anim.GetBool("Land"));
        //Debug.Log("Speed:" + anim.GetFloat("Speed"));
        //Debug.Log("Turn:" + anim.GetFloat("Turn"));
        //Debug.Log("vy:" + rig.velocity.y);
        //Debug.Log("vertical:" + vertical);
        //Debug.Log("horizontal:" + horizontal);
        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space)) {
            anim.SetBool("Jump",true);
            //anim.SetFloat("Speed", 0);
            //anim.SetFloat("Turn", 0);
            rig.velocity += Vector3.up * jumpVelocity;
            rig.velocity += transform.TransformVector(Vector3.forward) * jumpVelocity;
        }
        if (rig.velocity.y<=0.1f && !IsGrounded())
        {
            anim.SetBool("Land", true);
        }
        if (IsGrounded() && anim.GetBool("Land") == true)
        {
            anim.SetBool("Jump", false);
            anim.SetBool("Land", false);
        }

        horizontal = mh * Input.GetAxis("Horizontal");
        vertical = mv * Input.GetAxis("Vertical");
        if (vertical > 0) {
            // Sprint
            anim.SetFloat("Speed", 1);
            if (IsGrounded() && (Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftShift))) {
                anim.SetFloat("Speed", 5);
                vertical *= 2;
                horizontal *= 2;
            }
            // move forward
            transform.Translate(vertical * Time.deltaTime * forward, Space.Self);
            //anim.SetFloat("Speed", vertical);
            if (IsGrounded()) {
                //rotate
                transform.Rotate(horizontal * Time.deltaTime * Vector3.up, Space.World);
                turn = Mathf.Cos(Mathf.Atan2(vertical, horizontal));
                anim.SetFloat("Turn", turn);
                Debug.Log("idle rotate, v>0");
            }
        }
        else if (vertical < 0) {
            // walk back and strafe
            // opposite turn value while strafing
            //anim.SetFloat("Speed", velocity);
            anim.SetBool("Strafe", true);
            anim.SetFloat("Turn", Mathf.Cos(Mathf.Atan2(vertical, horizontal)));
            forward = forward * vertical +  Vector3.right * horizontal/100;
            transform.Translate(forward * Time.deltaTime);
            forward = Vector3.forward;
        }
        if (System.Math.Abs(vertical) < 0.1) {
            anim.SetFloat("Speed", 0);
            if (System.Math.Abs(horizontal) > 0.1f) {
                anim.SetFloat("Turn", Mathf.Cos(Mathf.Atan2(vertical, horizontal)));
                transform.Rotate(horizontal * Time.deltaTime * Vector3.up, Space.World);
                Debug.Log("idle rotate, v<0.1, h>0.1");
            }
            else {
                anim.SetFloat("Turn", 0);
                rig.constraints = RigidbodyConstraints.FreezeRotation;
                Debug.Log("idle rotate, v<0.1, h<=0.1");
                //Debug.Log("Speed: " + vertical);
            }
        }
    }

    bool IsGrounded() {
        return Physics.Raycast(transform.position + Vector3.up*1, -Vector3.up, distToGround + 0.1f);
    }

    //void OnCollisionEnter(Collision collide)
    //{
        
    //    if (collide.gameObject.tag == "Obstacle")
    //    {
    //        IsGrounded = true;
    //    }
    //}

    //void OnCollisionStay(Collision collide)
    //{
    //    if (collide.gameObject.tag == "Obstacle")
    //    {
    //        IsGrounded = true;
    //    }
    //}

    //void OnCollisionExit(Collision collide)
    //{
    //    if (collide.gameObject.tag == "Obstacle")
    //    {
    //        IsGrounded = false;
    //    }
    //}
}