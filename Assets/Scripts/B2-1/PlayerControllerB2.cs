using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using UnityStandardAssets.Characters.ThirdPerson;

public class PlayerControllerB2 : MonoBehaviour {
    public Camera cam;
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
    private bool jumping;

    private void Start() {
        //agent.updateRotation = false;
        //anim.SetFloat("Turn", 0.0f);
        //anim.SetFloat("Forward", 0.0f);
        //time = 0f;
        forward = Vector3.forward;
        jumping = false;
    }

    // Update is called once per frame
    void Update() {
        anim.SetBool("Strafe", false);
        if (transform.position.y < 0.1) {
            jumping = false;
            //anim.SetBool("Jump", false);
        }
        if (!jumping && Input.GetKeyDown(KeyCode.Space)) {
            anim.SetFloat("Speed", 0);
            anim.SetTrigger("Jump");
            //anim.SetBool("Jump", true);
            //anim.SetFloat("Turn", 0);
            jumping = true;
            rig.velocity += Vector3.up * jumpVelocity;
            rig.velocity += transform.TransformVector(Vector3.forward) * jumpVelocity;
        }
        else if (jumping)
            return;


        //else if (!jumping)
        //    rig.velocity = new vector3(0, rig.velocity.y, 0);

        horizontal = mh * Input.GetAxis("Horizontal");
        vertical = mv * Input.GetAxis("Vertical");
        if (vertical > 0) {
            // Sprint
            if (!jumping && (Input.GetKey(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.LeftShift))) {
                vertical *= 2;
                horizontal *= 2;
            }
            // move forward
            transform.Translate(vertical * Time.deltaTime * forward, Space.Self);
            anim.SetFloat("Speed", vertical);
            if (!jumping) {
                //rotate
                transform.Rotate(horizontal * Time.deltaTime * Vector3.up, Space.World);
                turn = Mathf.Cos(Mathf.Atan2(vertical, horizontal));
                anim.SetFloat("Turn", turn);
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
        if (System.Math.Abs(vertical) < 0.2) {
            anim.SetFloat("Speed", 0);
            if (System.Math.Abs(horizontal) > 0.1f) {
                anim.SetFloat("Turn", Mathf.Cos(Mathf.Atan2(vertical, horizontal)));
                transform.Rotate(horizontal * Time.deltaTime * Vector3.up, Space.World);
            }
            else {
                anim.SetFloat("Turn", 0);
                //Debug.Log("Speed: " + vertical);
            }
        }
    }
}