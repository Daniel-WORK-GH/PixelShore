using UnityEngine;

public class FishingRod : MonoBehaviour
{
    public LineRenderer lineRenderer;    // The LineRenderer to display the fishing rod.
    public float throwSpeed = 15f;        // The speed at which the fishing rod is thrown.
    public float maxThrowDistance = 3f; // The maximum distance the fishing rod can travel.
    
    public FishSpawner spawner;

    private Vector3 startPoint;          // The starting point of the fishing rod (the player's position).
    private Vector3 targetPoint;         // The current target point where the fishing rod is thrown.
    private Vector3 currentPoint;         // The current target point where the fishing rod is thrown.
    private bool isThrowing = false;     // Whether the fishing rod is in motion.

    void Start()
    {
        // Ensure the line renderer is set up in the editor or manually.
        if (lineRenderer == null)
        {
            lineRenderer = GetComponent<LineRenderer>();
        }

        // Set up the initial state of the LineRenderer
        lineRenderer.positionCount = 2; // We need two points: start and end of the fishing line.
        lineRenderer.enabled = false;   // Start with the line hidden.
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click to throw the fishing rod
        {
            if(isThrowing)
            {
                isThrowing = false;
                lineRenderer.enabled = false;

                if(spawner.TryCatchFish())
                {
                    Debug.Log("Caught fish!");
                }
            }
            else if(!isThrowing)
            {
                ThrowFishingRod();
            }
        }

        if (isThrowing)
        {
            // Update the fishing line positions
            float step = throwSpeed * Time.deltaTime; // Speed of movement
            currentPoint = Vector3.MoveTowards(currentPoint, targetPoint, step);

            Vector3 topMiddlePosition = new Vector3(
                transform.position.x + transform.localScale.x / 2, // Add half the collider's width to Y position
                transform.position.y + transform.localScale.y / 2, // Add half the collider's height to Y position
                transform.position.z // Same Z position
            );

            startPoint = topMiddlePosition; // The start of the rod is the player's position.

            // Update line renderer positions
            lineRenderer.SetPosition(0, startPoint); // Set the starting point to the player's position.
            lineRenderer.SetPosition(1, currentPoint); // Update the endpoint as the target point.

            if (IsTouchingWater(currentPoint) && currentPoint == targetPoint)
            {
                if(spawner.TryCatchFish())
                {
                    lineRenderer.SetPosition(1, new Vector3(currentPoint.x, currentPoint.y - 0.1f, currentPoint.z));
                }
            }

            // Stop throwing when the fishing rod reaches the max distance
            if (Vector3.Distance(startPoint, currentPoint) >= maxThrowDistance)
            {
                isThrowing = false;
                lineRenderer.enabled = false;
            }
        }
    }

    void ThrowFishingRod()
    {
        Vector3 topMiddlePosition = new Vector3(
            transform.position.x + transform.localScale.x / 2, // Add half the collider's width to Y position
            transform.position.y + transform.localScale.y / 2, // Add half the collider's height to Y position
            transform.position.z // Same Z position
        );

        startPoint = topMiddlePosition; // The start of the rod is the player's position.
        targetPoint = GetMousePosition();        // Initially, the target is the same as the start.
        currentPoint = startPoint;        // Initially, the target is the same as the start.

        // Show the fishing line
        lineRenderer.enabled = true;

        // Set the throwing flag to true
        isThrowing = true;
    }

    bool IsTouchingWater(Vector3 endPosition)
    {
        // Use a small overlap check around the target point to detect collisions with water
        Collider2D hit = Physics2D.OverlapPoint(endPosition);
        if (hit != null && hit.CompareTag("Water"))
        {
            return true;
        }
        return false;
    }


    Vector3 GetMousePosition()
    {
        // Convert the mouse position to world space
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; // Ensure the Z coordinate stays at 0 (2D space)
        return mousePosition;
    }
}