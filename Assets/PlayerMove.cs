using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed, sense;
    public Rigidbody rb;
    public Camera camera;
    public float rotY, rotX;

    private void Update()
    {
        rb.AddRelativeForce(Vector3.forward * speed * Time.deltaTime * Input.GetAxis("Vertical"), ForceMode.Acceleration);
        rb.AddRelativeForce(Vector3.right * speed * Time.deltaTime * Input.GetAxis("Horizontal"), ForceMode.Acceleration);
        rotY += (Input.GetAxis("Mouse X") * Time.deltaTime * sense);
        rotX -= (Input.GetAxis("Mouse Y") * Time.deltaTime * sense);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, rotY)), 15 * Time.deltaTime);
        camera.transform.localRotation = Quaternion.Lerp(camera.transform.localRotation, Quaternion.Euler(new Vector3(Mathf.Clamp(rotX, -90, 90), 0, 0 )), 15 * Time.deltaTime);

        
    }
}
