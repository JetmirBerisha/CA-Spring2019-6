using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour {
    private NavMeshAgent player;
    private NavMeshObstacle obstacle;
    public bool clicked;
    public bool selected;
    public NavMeshSurface surface;
    private bool stopped;
    public Vector3 goTo;
    private Material material;
    private Color bases, sels;
    private Rigidbody skeleton;
    private float timer;
    // Update is called once per frame
    private void Start() {
        clicked = false;
        selected = false;
        stopped = false;
        player = GetComponent<NavMeshAgent>();
        material = GetComponent<Renderer>().material;
        skeleton = GetComponent<Rigidbody>();
        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
        bases = new Color(0.3f, 0, 0);
        sels = new Color(1, 0, 0);
        timer = 0;
    }

    void Update() {
        if (player.enabled)
            if (!player.pathPending)
                if (player.remainingDistance <= player.stoppingDistance) {
                    player.enabled = false;
                    stopped = false;
                    player.velocity = Vector3.zero;
                    obstacle.enabled = true;
                    obstacle.velocity = Vector3.zero;
                    skeleton.velocity = Vector3.zero;
                    //surface.BuildNavMesh();
                }
        if (selected)
            material.color = sels;
        else
            material.color = bases;
        if (clicked) {
            stopped = false;
            clicked = false;
            obstacle.enabled = false;
            player.enabled = true;
            player.SetDestination(goTo);
        }
        if (player.enabled && stopped) {
            timer += Time.deltaTime;
            if (timer > 0.5f) {
                player.SetDestination(goTo);
                stopped = false;
                timer = 0f;
            }
        }
    }


    /**
     *  1. Try to set the current poisition as the target position
     *
     *  2. Try to disable the agent
     */
    private void OnCollisionEnter(Collision collision) {
        // We don't care for non player-player collisions and when we're
        // stationary and we get hit
        if (collision.collider.tag != "Player" || skeleton.velocity.magnitude == 0)
            return;
        Debug.Log("Collision between players.\r\n collider velocity: " + collision.rigidbody.velocity.sqrMagnitude + "\r\nMy velocity: " + player.velocity.sqrMagnitude);
        if ( player.velocity.magnitude >= collision.rigidbody.velocity.magnitude) {
            if (player.enabled) {
                stopped = true;
                player.SetDestination(player.transform.position);
                timer = 0f;
            }
        }
        else if (collision.rigidbody.velocity.sqrMagnitude < 4 ) {
            //   player.velocity = Vector3.zero;
            //    player.isStopped = true;
            //    player.velocity = Vector3.zero;
            //player.velocity = Vector3.zero;
            //if (player.enabled)
            //    player.SetDestination(skeleton.position);
            //player.velocity = Vector3.zero;
            //stopped = true;
            //player.velocity = Vector3.zero;
            //skeleton.velocity = Vector3.zero;

        }
    }

    //private void OnCollisionExit(Collision collision) {
    //    if (collision.collider.tag != "Player")
    //        return;
    //    Debug.Log("Collision exit");
    //    player.enabled = true;
    //    player.isStopped = false;
    //}

    private void OnTriggerEnter(Collider other) {
        Debug.Log("Trigger enter");
    }
}
