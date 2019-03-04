using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private B2PlayerController selected;
    private bool haveSelection;
    private Vector3 moveCam;
    private Vector3 forward;
    private Camera cam;
    public float fwdMult, scrollMult, spinMult;
    Ray ray;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start() {
        moveCam = Vector3.zero;
        cam = GetComponent<Camera>();
        haveSelection = false;
        forward = Vector3.forward;
    }

    // Update is called once per frame
    void FixedUpdate() {
        float horizontal = spinMult * Input.GetAxis("Horizontal");
        float vertical = fwdMult * Input.GetAxis("Vertical");
        float scroll = scrollMult * Input.GetAxis("Mouse ScrollWheel");
        // Move in local coordinates but move in the y direction by cos(a) where a is the angle by 
        // which the camera is rotated about the x axis
        forward.y = Mathf.Cos(cam.transform.localEulerAngles.x);
        cam.transform.Translate(vertical * forward * Time.deltaTime, Space.Self);
		Debug.Log("cam speeD: " + (vertical * forward * Time.deltaTime));
        moveCam.y = scroll;
        cam.transform.Translate(moveCam * Time.deltaTime, Space.World);
        moveCam.y = horizontal;
        cam.transform.Rotate(moveCam * Time.deltaTime, Space.World);

        // Mouse clicks to select and act upon a character
        if (Input.GetMouseButtonDown(0)) {
            // left click act
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (haveSelection && Physics.Raycast(ray, out hit)) {
                if (Vector3.Distance(selected.goTo, hit.point) < 2)
                    selected.speed = 22;
                else
                    selected.speed = 3;
                selected.goTo = hit.point;
                selected.move = true;
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            // right click select
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.tag == "Player") {
                    selected = hit.collider.GetComponent<B2PlayerController>();
                    selected.selected = true;
                    haveSelection = true;
                }
                else
                    haveSelection = false;
            }
        }
    }
}
