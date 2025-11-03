using UnityEngine;

public class FloodWater : MonoBehaviour
{
    // Speed of the water rising (units per second)
    //f: a single-precision floating-point number
    public float riseSpeed = 0.5f;

    // Maximum height the water should reach
    public float maxHeight = 10f;

    // Internal flag to control if flooding is active
    private bool isFlooding = false;

    void Update()
    {
        // Press F to start flooding
        if (Input.GetKeyDown(KeyCode.F))
        {
            isFlooding = true;
            Debug.Log("?? Flooding started!");
        }

        // Press G to stop flooding
        if (Input.GetKeyDown(KeyCode.G))
        {
            isFlooding = false;
            Debug.Log("?? Flooding stopped.");
        }

        // If flooding is active, raise the cube
        if (isFlooding && transform.position.y < maxHeight)
        {
            transform.position += Vector3.up * riseSpeed * Time.deltaTime;

            // Print current water level to console
            Debug.Log($"Current water level: {transform.position.y:F2}");
        }

        // Automatically stop when the max height is reached
        if (isFlooding && transform.position.y >= maxHeight)
        {
            isFlooding = false;
            Debug.Log($"?? Flooding reached max height ({maxHeight:F2}) and stopped.");
        }
    }
}
