using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachItem : MonoBehaviour {

    public Transform controller;
    private Transform transform;
    private Rigidbody rb;

    // Use this for initialization
    void Start () {
        transform = gameObject.GetComponent<Transform>();
        rb= gameObject.GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        //transform.position = controller.position + controller.forward*5;
        //Debug.Log(message: controller.position);

        //transform.position = Vector3.MoveTowards(transform.position, controller.position + controller.forward * 5, 100);
        //rb.MovePosition((controller.position + controller.forward * 5));
        rb.AddForce(((controller.position + controller.forward * 5) - (transform.position)) * 100);
        Debug.Log(message: "controller: " + controller.position);
        Debug.Log(message: transform.position);
    }
}
