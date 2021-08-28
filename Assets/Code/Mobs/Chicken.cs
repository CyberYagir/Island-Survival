using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chicken : Mob
{
    public float jumpForce;
    Rigidbody rb;
    bool inJump;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StartCoroutine(randomPoses());
    }
    private void Update()
    {
        if (inJump)
        {
            rb.AddRelativeForce(Vector3.forward * speed, ForceMode.Acceleration);
        }
        transform.LookAt(new Vector3(newPosition.x, transform.position.y, newPosition.z));
        if (Vector3.Distance(new Vector3(newPosition.x, transform.position.y, newPosition.z), transform.position) < 1)
        {
            SetRandomPos();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        inJump = false;
        rb.AddRelativeForce(Vector3.up * jumpForce, ForceMode.Impulse);
    }

    private void OnCollisionExit(Collision collision)
    {
        inJump = true;
    }

    IEnumerator randomPoses()
    {
        while (true)
        {
            SetRandomPos();
            yield return new WaitForSeconds(5f);
        }
    }

    public void SetRandomPos()
    {
        var newPos = transform.position + new Vector3(Random.Range(-movingRadius, movingRadius), 50, Random.Range(-movingRadius, movingRadius));
        if (Physics.Raycast(newPos, Vector3.down, out RaycastHit hit))
        {
            newPosition = hit.point + new Vector3(0, 0.5f, 0);
        }
    }
}
