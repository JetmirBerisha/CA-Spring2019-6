using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 offset;
    public Transform t;
    public bool attached;
    private Camera cam;
    private GameObject player;
    private Vector3 moveCam;
    private Vector3 forward;
    public float spinMult, fwdMult, scrollMult;

    void Start() {
        offset = new Vector3(0.5f, 2, -1);
        attached = false;
        cam = GetComponent<Camera>();
        moveCam = Vector3.zero;
        forward = Vector3.forward;
    }

    void Update() {
        if (attached) {
            cam.transform.position = player.transform.TransformPoint(0.5f, 2, -1.5f);
            cam.transform.rotation = Quaternion.LookRotation(player.transform.forward);
        }
        else {
            // move freely script
            float horizontal = spinMult * Input.GetAxis("Horizontal");
            float vertical = fwdMult * Input.GetAxis("Vertical");
            float scroll = scrollMult * Input.GetAxis("Mouse ScrollWheel");
            // Move in local coordinates but move in the y direction by cos(a) where a is the angle by 
            // which the camera is rotated about the x axis
            forward.y = Mathf.Sin(cam.transform.localEulerAngles.x);
            cam.transform.Translate(vertical * forward * Time.deltaTime, Space.Self);
            //Debug.Log("cam speeD: " + (vertical * forward * Time.deltaTime));
            moveCam.y = scroll;
            cam.transform.Translate(moveCam * Time.deltaTime, Space.World);
            moveCam.y = horizontal;
            cam.transform.Rotate(moveCam * Time.deltaTime, Space.World);

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
