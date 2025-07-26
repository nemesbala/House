using System;
using System.Collections.Generic;
using UnityEngine;

public class RoadConnectionChecker : MonoBehaviour
{
    public static event Action<Vector3> OnRoadPlaced; // Event to notify nearby roads

    public LayerMask roadLayer; // Assign the road layer in the inspector
    public float checkDistance = 1.1f; // Adjust based on road size
    public Vector3 checkOffset = Vector3.zero; // Offset for raycast origin

    public GameObject straightRoad;
    public GameObject cornerRoad;
    public GameObject tJunctionRoad;
    public GameObject intersectionRoad;

    private Dictionary<string, bool> roadConnections = new Dictionary<string, bool>
    {
        {"Left", false},
        {"Right", false},
        {"Forward", false},
        {"Backward", false}
    };

    public void Start()
    {
        straightRoad.SetActive(true);
        CheckNearbyRoads();
        UpdateRoadPrefab();
        OnRoadPlaced?.Invoke(transform.position); // Notify others that a road is placed
    }

    void OnEnable()
    {
        OnRoadPlaced += HandleNewRoadPlaced;
    }

    void OnDisable()
    {
        OnRoadPlaced -= HandleNewRoadPlaced;
    }

    void CheckNearbyRoads()
    {
        roadConnections["Left"] = CheckDirection(Vector3.left);
        roadConnections["Right"] = CheckDirection(Vector3.right);
        roadConnections["Forward"] = CheckDirection(Vector3.forward);
        roadConnections["Backward"] = CheckDirection(Vector3.back);
    }

    bool CheckDirection(Vector3 direction)
    {
        RaycastHit hit;
        Vector3 origin = transform.position + checkOffset;
        Debug.DrawRay(origin, direction * checkDistance, Color.blue, 2f); // Visualize raycast

        if (Physics.Raycast(origin, direction, out hit, checkDistance))
        {
            //Debug.Log($"Raycast hit {hit.collider.gameObject.name} in {direction} direction.");
            if (hit.collider.CompareTag("Road"))
            {
                return true;
            }
        }
        else
        {
            //Debug.Log($"Raycast in {direction} direction detected NOTHING.");
        }
        return false;
    }


    void UpdateRoadPrefab()
    {
        // Disable all prefabs first
        straightRoad.SetActive(false);
        cornerRoad.SetActive(false);
        tJunctionRoad.SetActive(false);
        intersectionRoad.SetActive(false);

        // Determine the correct road piece to activate
        int connectionCount = 0;
        foreach (var connection in roadConnections.Values)
        {
            if (connection) connectionCount++;
        }

        GameObject selectedRoad = null;
        float rotationAngle = 0f;

        if (connectionCount == 4)
        {
            selectedRoad = intersectionRoad;
        }
        else if (connectionCount == 3)
        {
            selectedRoad = tJunctionRoad;
            if (!roadConnections["Forward"]) rotationAngle = 180f;
            else if (!roadConnections["Left"]) rotationAngle = 90f;
            else if (!roadConnections["Right"]) rotationAngle = -90f;
        }
        else if (connectionCount == 2)
        {
            if ((roadConnections["Left"] && roadConnections["Right"]) || (roadConnections["Forward"] && roadConnections["Backward"]))
            {
                selectedRoad = straightRoad;
                if (roadConnections["Left"]) rotationAngle = 90f;
            }
            else
            {
                selectedRoad = cornerRoad;
                if (roadConnections["Forward"] && roadConnections["Right"]) rotationAngle = 90f;
                else if (roadConnections["Right"] && roadConnections["Backward"]) rotationAngle = 180f;
                else if (roadConnections["Backward"] && roadConnections["Left"]) rotationAngle = -90f;
                else if (roadConnections["Left"] && roadConnections["Forward"]) rotationAngle = 0f;
            }
        }
        else if (connectionCount == 1)
        {
            selectedRoad = straightRoad;
            if (roadConnections["Forward"]) rotationAngle = 0f;
            else if (roadConnections["Right"]) rotationAngle = -90f;
            else if (roadConnections["Backward"]) rotationAngle = 180f;
            else if (roadConnections["Left"]) rotationAngle = 90f;
        }
        else
        {
            // Default case: If no connections exist, place a single road piece
            selectedRoad = straightRoad;
            rotationAngle = 0f;
        }

        if (selectedRoad != null)
        {
            selectedRoad.SetActive(true);
            selectedRoad.transform.rotation = Quaternion.Euler(0, rotationAngle, 0);
        }
    }


    void HandleNewRoadPlaced(Vector3 position)
    {
        if (Vector3.Distance(transform.position, position) <= checkDistance * 1.5f)
        {
            CheckNearbyRoads();
            UpdateRoadPrefab();
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Vector3 origin = transform.position + checkOffset;
        Gizmos.DrawRay(origin, Vector3.left * checkDistance);
        Gizmos.DrawRay(origin, Vector3.right * checkDistance);
        Gizmos.DrawRay(origin, Vector3.forward * checkDistance);
        Gizmos.DrawRay(origin, Vector3.back * checkDistance);
    }
}