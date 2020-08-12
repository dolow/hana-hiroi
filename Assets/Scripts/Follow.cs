using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    [Tooltip("Following speed")]
    public float moveSpeed = 20.0f;

    [Tooltip("Rotate speed")]
    public float rotateSpeed = 10.0f;

    [Tooltip("Range from exact destication point considered as destination")]
    public float destinationRange = 0.1f;

    private Vector3 currentDestination = Vector3.zero;
    private Vector3 originPosition = Vector3.zero;

    private Vector3 moveVelocity = Vector3.zero;
    private Vector3 rotateVelocity = Vector3.zero;

    void Start()
    {
        this.SetDestination(this.transform.position);
    }

    void Update()
    {
        float currentDistance = Vector3.Distance(this.transform.position, this.currentDestination);
        if (currentDistance < this.destinationRange)
        {
            this.StopFollowing();
            return;
        }

        // NOTE: This is one of the patterns of character movement, it won't be a common logic
        {
            float originDistance = Vector3.Distance(this.originPosition, this.currentDestination);

            float moveSpeedByDistance = originDistance / this.moveSpeed;
            this.transform.position = Vector3.SmoothDamp(this.transform.position, this.currentDestination, ref this.moveVelocity, moveSpeedByDistance);

            float restDistance = originDistance - currentDistance;
            //float rotateSpeedByDistance = Vector3.Distance(this.originPosition, this.currentDestination) / this.rotateSpeed;
            float rotateSpeedByDistance = restDistance / originDistance;
            Vector3 diff = this.transform.position - this.currentDestination;
            Quaternion newRotation = Quaternion.FromToRotation(Vector3.up, diff);
            this.transform.rotation = Quaternion.Slerp(this.transform.rotation, newRotation, rotateSpeedByDistance);
        }
    }

    public bool IsFollowing()
    {
        return this.moveVelocity != Vector3.zero || this.rotateVelocity != Vector3.zero;
    }

    public void StopFollowing()
    {
        this.moveVelocity = Vector3.zero;
        this.rotateVelocity = Vector3.zero;
    }

    public Vector3 GetDestination()
    {
        return this.currentDestination;
    }

    public void SetDestination(Vector3 destination)
    {
        // 2D adjustment
        destination.z = this.transform.position.z;

        this.currentDestination = destination;
        this.originPosition = this.transform.position;
    }

    public void ExtendDestination(Vector3 destination)
    {
        // 2D adjustment
        destination.z = this.transform.position.z;

        Vector3 oldDestination = this.currentDestination;

        // NOTE: This is one of the patterns of character movement, it won't be a common logic
        {
            if (
                (oldDestination.x > this.transform.position.x && destination.x < this.transform.position.x) ||
                (oldDestination.x < this.transform.position.x && destination.x > this.transform.position.x)
            )
            {
                this.originPosition.x = this.transform.position.x;
            }

            if (
                (oldDestination.y > this.transform.position.y && destination.y < this.transform.position.y) ||
                (oldDestination.y < this.transform.position.y && destination.y > this.transform.position.y)
            )
            {
                this.originPosition.y = this.transform.position.y;
            }
        }

        this.currentDestination = destination;
    }
}
