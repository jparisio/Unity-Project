using UnityEngine;

public class RotatePlane : MonoBehaviour
{
    public float rotationSpeed = 700f; // Adjust the speed of rotation

    void Update()
    {
        // Get mouse horizontal movement
        float mouseX = Input.GetAxis("Mouse X");

        // Calculate the rotation amount
        float rotationAmount = mouseX * rotationSpeed * Time.deltaTime;

        // Rotate around the Z-axis
        transform.Rotate(Vector3.forward, rotationAmount);
    }
}
