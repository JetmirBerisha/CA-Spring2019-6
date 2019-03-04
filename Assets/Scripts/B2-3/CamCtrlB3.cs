using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamCtrlB3 : MonoBehaviour
{
    public Camera left;
    public Camera front;
    public Camera right;
    float time;
    // Start is called before the first frame update
    void Start() {
        right.enabled = front.enabled = false;
        left.enabled = true;
        time = 0;
    }

    // Update is called once per frame
    void Update() {
        time += Time.deltaTime;
        if (time > 4f && time < 8.4f) {
            front.enabled = true;
            left.enabled = false;
        }
        else if (time > 8.9f) {
            right.enabled = true;
            front.enabled = false;
        }
    }
}
