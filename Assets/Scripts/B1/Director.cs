using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Director : MonoBehaviour {

    private GameObject[] players;
    private List<PlayerController> selected;
    private Material obstacle;
    private Collider obstacleCollider;
    private Color baseColor, targetColor;
    public Camera cam;
    private float timer;
    private float criticalPoint = 0.23f;
    private Vector3 start, end;
    private bool toDraw;
    private Ray ray;
    private RaycastHit hit;
    private Rect rect;
    public NavMeshSurface surface;
    private Vector3 TL, TR, BL, BR;
    // Start is called before the first frame update
    void Start() {
        surface = GetComponent<NavMeshSurface>();
        players = GameObject.FindGameObjectsWithTag("Player");
        selected = new List<PlayerController>();
        timer = 0;
        toDraw = false;
        rect = new Rect();
        baseColor = new Color(0.8f, 0.5f, 0.5f);
        targetColor = new Color(0.8f, 0, 0.8f);
    }

    // Update is called once per frame
    void FixedUpdate() {
        if (Input.GetKeyUp(KeyCode.C))
            removeAll();
        ClickObstacle();
        MoveObstacle();
        if (!Cursor.visible)
            return;
        if (Input.GetMouseButtonDown(1)) {
            timer = 0;
        }
        if (Input.GetMouseButton(1)) {
            /* Update the selection square */
            if (timer == 0) {
                start = new Vector3(Input.mousePosition.x, Screen.height - Input.mousePosition.y, Input.mousePosition.z);
                rect.x = start.x;
                rect.y = start.y;
            }
            timer += Time.deltaTime;
            if (timer > criticalPoint) {
                toDraw = true;
                end = Input.mousePosition;
                end.y = Screen.height - end.y;
                rect.width = end.x - start.x;
                rect.height = end.y - start.y;
            }
        }
        if (Input.GetMouseButtonUp(1)) {
            /* End timer and figure out the selected players */
            if (timer > criticalPoint) {
                // hold
                // select all of the objects within the rectangle and add some effect to them
                Ray ray1;
                ray1 = cam.ScreenPointToRay(new Vector3(Math.Min(rect.xMax, rect.xMin), Math.Min(rect.yMin, rect.yMax), 0));
                Physics.Raycast(ray1, out RaycastHit hit1);
                TL = hit1.point;
                ray1 = cam.ScreenPointToRay(new Vector3(Math.Max(rect.xMax, rect.xMin), Math.Min(rect.yMin, rect.yMax), 0));
                Physics.Raycast(ray1, out hit1);
                TR = hit1.point;
                ray1 = cam.ScreenPointToRay(new Vector3(Math.Min(rect.xMax, rect.xMin), Math.Max(rect.yMin, rect.yMax), 0));
                Physics.Raycast(ray1, out hit1);
                BL = hit1.point;
                ray1 = cam.ScreenPointToRay(new Vector3(Math.Max(rect.xMax, rect.xMin), Math.Max(rect.yMin, rect.yMax), 0));
                Physics.Raycast(ray1, out hit1);
                BR = hit1.point;
                for (int i = 0; i < players.Length; i++) {
                    if (isWithinPolygon(players[i].transform.position))
                        addSelection(players[i]);
                    else
                        removeSelection(players[i]);
                }
            }
            else {
                // click: select one dude or send all selected dudes to this location
                //RaycastHit[] hits;
                RaycastHit singleHit;
                bool found = false;
                ray = cam.ScreenPointToRay(Input.mousePosition);
                //hits = Physics.RaycastAll(cam.transform.position, cam.transform.forward);
                Physics.Raycast(ray, out singleHit);
                //foreach (RaycastHit h in hits) {
                //    Debug.Log(h.point + "\r\n");
                //}
                //for (int i = 0; !found && i < hits.Length; i++)
                if (singleHit.collider.tag == "Player") {
                    removeAll();
                    addSelection(singleHit.collider.gameObject);
                    found = true;
                }
                if (!found)
                    foreach(PlayerController i in selected) {
                        i.goTo = singleHit.point;
                        i.clicked = true;
                    }
            }
            toDraw = false;
            timer = 0;
        }
    }

    private void OnGUI() {
        if (toDraw)
            GUI.Box(rect, "");
    }

    // Handle single left clicks for selecting 
    void ClickObstacle() {
        if (!Cursor.visible || !Input.GetMouseButtonUp(0))
            return;
        // We have a visible cursor and a left click up
        ray = cam.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        if (hit.collider.tag != "Obstacle")
            return;
        if (obstacleCollider != null)
            obstacle.color = baseColor;
        obstacleCollider = hit.collider;
        obstacle = obstacleCollider.GetComponent<Renderer>().material;
        obstacle.color = targetColor;
    }

    // Move the selected obstacle
    void MoveObstacle() {
        if (obstacleCollider == null || obstacle == null)
            return;
        Vector3 trans = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        obstacleCollider.gameObject.transform.position -= trans / 25;
    }

    private void addSelection(GameObject item) {
        PlayerController i = item.GetComponent<PlayerController>();
        i.selected = true;
        selected.Add(i);
    }

    private void removeSelection(GameObject item) {
        PlayerController i = item.GetComponent<PlayerController>();
        i.selected = false;
        selected.Remove(i);
    }

    private void removeAll() {
        if (obstacle != null)
            obstacle.color = baseColor;
        obstacleCollider = null;
        obstacle = null;
        foreach (PlayerController i in selected)
            i.selected = false;
        selected.Clear();
    }

    bool IsWithinTriangle(Vector3 p, Vector3 p1, Vector3 p2, Vector3 p3) {
        //Need to set z -> y because of other coordinate system
        float denominator = ((p2.z - p3.z) * (p1.x - p3.x) + (p3.x - p2.x) * (p1.z - p3.z));
        float a = ((p2.z - p3.z) * (p.x - p3.x) + (p3.x - p2.x) * (p.z - p3.z)) / denominator;
        float b = ((p3.z - p1.z) * (p.x - p3.x) + (p1.x - p3.x) * (p.z - p3.z)) / denominator;
        float c = 1 - a - b;
        //The point is within the triangle if 0 <= a <= 1 and 0 <= b <= 1 and 0 <= c <= 1
        if (a >= 0f && a <= 1f && b >= 0f && b <= 1f && c >= 0f && c <= 1f)
            return true;
        return false;
    }

    bool isWithinPolygon(Vector3 point) {
        return IsWithinTriangle(point, TL, BL, TR) || IsWithinTriangle(point, TR, BL, BR);
    }
}