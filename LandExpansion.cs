using UnityEngine;

public class LandExpansion : MonoBehaviour
{
    [Header("Land Expansion Settings")]
    public float checkDistance = 1f; // Distance to check for neighboring land pieces

    [Header("Expansion Indicators")]
    public GameObject leftIndicator;
    public GameObject rightIndicator;
    public GameObject forwardIndicator;
    public GameObject backwardIndicator;

    private void Start()
    {
        CheckForExpansion();
    }

    private void CheckForExpansion()
    {
        // Check in each direction and enable/disable the corresponding indicator
        CheckDirection(Vector3.left, leftIndicator);
        CheckDirection(Vector3.right, rightIndicator);
        CheckDirection(Vector3.forward, forwardIndicator);
        CheckDirection(Vector3.back, backwardIndicator);
    }

    private void CheckDirection(Vector3 direction, GameObject indicator)
    {
        // Calculate the end point of the check
        Vector3 checkPoint = transform.position + direction * checkDistance;

        // Perform a sphere check at the end point to detect neighboring land pieces
        Collider[] colliders = Physics.OverlapSphere(checkPoint, 0.1f);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Land"))
            {
                // If a neighboring land piece is found, disable the indicator
                indicator.SetActive(false);
                return;
            }
        }

        // If no neighboring land piece is found, enable the indicator
        indicator.SetActive(true);
    }

    // Debugging: Visualize raycast directions and endpoints in the Scene view
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Vector3 leftPoint = transform.position + Vector3.left * checkDistance;
        Vector3 rightPoint = transform.position + Vector3.right * checkDistance;
        Vector3 forwardPoint = transform.position + Vector3.forward * checkDistance;
        Vector3 backPoint = transform.position + Vector3.back * checkDistance;

        // Draw lines for directions
        Gizmos.DrawLine(transform.position, leftPoint);
        Gizmos.DrawLine(transform.position, rightPoint);
        Gizmos.DrawLine(transform.position, forwardPoint);
        Gizmos.DrawLine(transform.position, backPoint);

        // Draw small spheres at the endpoints
        Gizmos.DrawSphere(leftPoint, 0.1f);
        Gizmos.DrawSphere(rightPoint, 0.1f);
        Gizmos.DrawSphere(forwardPoint, 0.1f);
        Gizmos.DrawSphere(backPoint, 0.1f);
    }
}