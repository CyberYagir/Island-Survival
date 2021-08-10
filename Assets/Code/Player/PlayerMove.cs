using StylizedWater2;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] float speed, sense;
    [SerializeField] Rigidbody rb;
    [SerializeField] Camera camera;
    [SerializeField] float rotY, rotX;
    [SerializeField] float angle;
    [SerializeField] bool inWater;
    [SerializeField] float jumpForce;
    [SerializeField] LayerMask mask;
    [SerializeField] bool inJump;
    [SerializeField] float gravModifayer;
    [SerializeField] float forwardSpeed;


    public float GetForwardSpeed()
    {
        return Vector3.Dot(rb.velocity, transform.forward);
    }

    private void Update()
    {
        if (!WindowManager.menu)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.Confined;
        }

        if (!WindowManager.menu)
        {
            rotY += (Input.GetAxis("Mouse X") * Time.deltaTime * sense);
            rotX -= (Input.GetAxis("Mouse Y") * Time.deltaTime * sense);
            rotX = Mathf.Clamp(rotX, -85, 85);

            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(new Vector3(0, rotY)), 15 * Time.deltaTime);
            camera.transform.localRotation = Quaternion.Lerp(camera.transform.localRotation, Quaternion.Euler(new Vector3(rotX, 0, 0)), 15 * Time.deltaTime);
            rb.AddRelativeForce((Vector3.forward + new Vector3(0, angle * 5f, 0)) * speed * Time.deltaTime * Input.GetAxis("Vertical"), ForceMode.Acceleration);
            rb.AddRelativeForce((Vector3.right + new Vector3(0, angle * 2f, 0)) * speed * Time.deltaTime * Input.GetAxis("Horizontal"), ForceMode.Acceleration);
        }
        if (Physics.SphereCastAll(transform.position - new Vector3(0, 1, 0) + new Vector3(0, 0.4f, 0), 0.45f, Vector3.down, 0.1f, mask).Length != 0)
        {
            inJump = false;
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (Physics.Raycast(transform.position - new Vector3(0, 0.9f, 0), Vector3.down, out RaycastHit hitf, 0.2f))
                {
                    if (!WindowManager.menu)
                    {
                        angle = Vector3.Angle(hitf.normal, Vector3.up) / 180f;
                        if (angle < 0.29f)
                        {
                            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                        }
                    }
                }
            }
        }
        else
        {
            angle = 0;
            inJump = true;
        }

        if (!inWater)
        {
            if (transform.position.y < 15)
            {
                rb.useGravity = false;
                inWater = true;
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY;
                GetComponent<Rigidbody>().freezeRotation = true;
                return;
            }
            RaycastHit hit;
            Debug.DrawRay(transform.position - new Vector3(0, 0.9f, 0), transform.forward, Color.red);
            if (Physics.Raycast(transform.position - new Vector3(0, 0.9f, 0), transform.forward, out hit, 1f))
            {
                angle = Vector3.Angle(hit.normal, Vector3.up) / 180f;
                if (angle > 0.31)
                {
                    angle = 0;
                }
            }
            if (!Physics.Raycast(transform.position - new Vector3(0, 0.9f, 0), Vector3.down, out RaycastHit hitb, 3f)){
                print("Fall");
                rb.velocity -= new Vector3(0, Time.deltaTime * gravModifayer, 0);
            }


        }
        else
        {
            transform.position = new Vector3(transform.position.x, WaterPoint.waterPoint.transform.position.y + 1, transform.position.z);
            if (!inJump && WaterPoint.waterPoint.transform.position.y > 15)
            {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
                GetComponent<Rigidbody>().useGravity = true;
                inWater = false;
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position - new Vector3(0, 1, 0) + new Vector3(0,0.4f,0), 0.45f);
    }
}
