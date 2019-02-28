using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
    private GameObject selected;
    private Vector3 moveCam;
    private Camera cam;
    public float fwdMult, scrollMult, spinMult;
    Ray ray;
    RaycastHit hit;
    // Start is called before the first frame update
    void Start() {
        moveCam = new Vector3();
        cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void FixedUpdate() {
        float horizontal = spinMult * Input.GetAxis("Horizontal");
        float vertical = fwdMult * Input.GetAxis("Vertical");
        float scroll = scrollMult * Input.GetAxis("Mouse ScrollWheel");
        moveCam.x = 0;
        moveCam.y = 0;
        moveCam.z = vertical;
        cam.transform.Translate(moveCam * Time.deltaTime, Space.Self);
        moveCam.z = 0;
        moveCam.y = scroll;
        cam.transform.Translate(moveCam * Time.deltaTime, Space.World);
        moveCam.y = horizontal;
        cam.transform.Rotate(moveCam * Time.deltaTime, Space.World);

        // Mouse clicks to select and act upon a character
        if (Input.GetMouseButtonDown(0)) {
            // left click   
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                selected = hit.collider.gameObject;
            }
        }

        if (Input.GetMouseButtonDown(1)) {
            // right click
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                selected = hit.collider.gameObject;
            }
        }
    }
}
