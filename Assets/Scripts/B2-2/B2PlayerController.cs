using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class B2PlayerController : MonoBehaviour
{
	public float speed;
	public bool selected;
	public bool move;
    public bool arrived;
	public Vector3 goTo;
    private NavMeshAgent agent;
    // Start is called before the first frame update
    void Start() {
        speed = 3;	// Walk
		selected = false;
		move = false;
        arrived = true;
		goTo = Vector3.zero;
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (move) {
            agent.speed = speed;
            agent.SetDestination(goTo);
            move = false;
            arrived = false;
        }
        if (!agent.pathPending)
            if (agent.remainingDistance <= agent.stoppingDistance && (!agent.hasPath || agent.velocity.sqrMagnitude <= 1)) {
                arrived = true;
                speed = 3;
            }
    }
}
