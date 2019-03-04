using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 offset;
    public Transform t;
    private bool attached;
    private Camera cam;
    private GameObject player;

    void Start() {
        offset = new Vector3(0.5f, 2, -1);
        attached = false;
        cam = GetComponent<Camera>();
    }

    void Update() {
        if (attached) {
            cam.transform.position = player.transform.TransformPoint(0.5f, 2, -1.2f);
            cam.transform.rotation = Quaternion.LookRotation(player.transform.forward);
        }
        else {
            // move freely script

            // click on player
            if (Input.GetMouseButtonDown(0)) {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit)) {
                    if (hit.collider.tag == "Player") {
                        attached = true;
                        player = hit.collider.gameObject;
                        cam.transform.position = player.transform.position + new Vector3(player.transform.forward.x + 0.5f, player.transform.forward.y + 2, player.transform.forward.z - 2);
                        cam.transform.rotation = Quaternion.LookRotation(hit.collider.transform.forward);
                    }
                }
            }
        }
    }
}
