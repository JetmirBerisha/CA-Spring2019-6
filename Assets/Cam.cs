using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cam : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 offset;
    public Transform t;

    void Start()
    {
        offset = transform.position - t.position;    
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = t.position + offset;
    }
}
