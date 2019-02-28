using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeLook : MonoBehaviour
{
    // Use hashtable to store selected players

    // Implement free look
    public int forwardMultiplier;
    public int rotateMultiplier;
    private Camera cam;
    private Vector3 offset;
    private Vector3 lastHit;
    private Ray ray;
    private RaycastHit hit;
    private float forward, side;
    // All game objects with tag Player
    private GameObject[] players;

    // Start is called before the first frame update
    void Start() {
        cam = GetComponent<Camera>();
        Ray ray = cam.ScreenPointToRay(new Vector3(Screen.width/2, Screen.height/2, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit)) {
            lastHit = hit.point;
            offset = cam.transform.position - lastHit;
            //Debug.Log("Offset initialized at: " + offset);
        }
        else
            Debug.LogError("The initial offset ray didn't hit anything");
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (Input.GetKey(KeyCode.Space)) {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            //forward = Input.GetAxis("Mouse Y") * Time.deltaTime * forwardMultiplier;
            //cam.transform.Translate(new Vector3(side, 0, forward), Space.World);
            side = Input.GetAxis("Mouse X") * Time.deltaTime * rotateMultiplier;
            cam.transform.RotateAround(lastHit, Vector3.up, side);
            ray = cam.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
            if (Physics.Raycast(ray, out hit)) {
                lastHit = hit.point;
                offset = cam.transform.position - lastHit;
            }
        }
        else {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        if (Cursor.visible && Input.GetMouseButtonDown(0)) {
            // Move the camera
            ray = cam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit)) {
                lastHit = hit.point;
                // check where the ray hit 
                if (lastHit.x < -30)
                    lastHit.x = -30;
                else if (lastHit.x > 30)
                    lastHit.x = 30;
                if (lastHit.z < -30)
                    lastHit.z = -30;
                else if (lastHit.z > 30)
                    lastHit.z = 30;
                cam.transform.position = lastHit + offset;
            }
        }
    }
}
