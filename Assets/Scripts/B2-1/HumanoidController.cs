using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanoidController : MonoBehaviour {
    private Rigidbody rb;
    private Animator animator;
    private NavMeshAgent agent;
    public float acceleration;
    public float multiplier;

    void Start() {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update() {
        float moveHorizontal = multiplier * Input.GetAxis("Horizontal");
        float moveVertical = multiplier * Input.GetAxis("Vertical");
        Vector3 move = new Vector3(moveHorizontal, 0, moveVertical);
        rb.AddForce(acceleration * move);
        animator.SetFloat("Velocity",rb.velocity.sqrMagnitude);
        //rb.velocity = move;
        Debug.Log(rb.velocity.sqrMagnitude);

        //animator.SetFloat("Velocity", acceleration * move.sqrMagnitude);
        //Debug.Log("multiplier * |move|: " + acceleration * move.sqrMagnitude + "\nVelocity param: " + animator.GetFloat("Velocity"));
        //animator.SetFloat("Altitude", rb.position.y);
        if (Input.GetKeyDown(KeyCode.D)) {
            animator.SetFloat("Turn", 1);
        }
    }
}
