using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleController : MonoBehaviour
{
    public float xmin = -11.8f, xmax = -20.5f;
    // Update is called once per frame
    void Update() {
        transform.position = new Vector3(xmin - Mathf.PingPong(Time.time, xmin - xmax), transform.position.y, transform.position.z);
    }
}
