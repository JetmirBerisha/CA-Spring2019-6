using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class test : MonoBehaviour {
    public Transform destination;
    private NavMeshAgent agent;
    private NavMeshObstacle obstacle;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(destination.position);
        obstacle = GetComponent<NavMeshObstacle>();
        obstacle.enabled = false;
    }

    private void FixedUpdate() {
        if (agent.enabled && !agent.pathPending)
            if (agent.remainingDistance <= agent.stoppingDistance && (!agent.hasPath || agent.velocity.sqrMagnitude <= 1)) {
                agent.enabled = false;
                obstacle.enabled = true;
            }
    }
}