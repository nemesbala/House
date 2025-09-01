using UnityEngine;
using System.Collections.Generic;

public class RoadNetworkManager : MonoBehaviour
{
    public static RoadNetworkManager Instance;

    [Header("Tags")]
    public string cityHallTag = "CityHall";
    public string targetRoadTag = "Road";
    public string disconnectedRoadTag = "DisconnectedRoad";

    [Header("Settings")]
    public float gridSize = 6.0f;
    public float checkRadius = 0.4f; // Reduced to prevent diagonal connections

    [Header("City Hall Detection Settings")]
    public Vector3[] cityHallOffsets = new Vector3[]
    {
        new Vector3(0f, 0f, 3f),
        new Vector3(0f, 0f, -3f),
        new Vector3(3f, 0f, 0f),
        new Vector3(-3f, 0f, 0f)
    };

    public Vector3[] cityHallSizes = new Vector3[]
    {
        new Vector3(6f, 2f, 1f),
        new Vector3(6f, 2f, 1f),
        new Vector3(1f, 2f, 6f),
        new Vector3(1f, 2f, 6f)
    };

    [Header("Debug Settings")]
    public bool enableDebugLogs = false;
    public bool enableDebugVisualization = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void OnRoadNetworkChanged()
    {
        if (enableDebugLogs) Debug.Log("Recalculating road network");

        RecalculateConnections();

        if (MainRoadChecker.Instance != null)
        {
            MainRoadChecker.Instance.CollectBuildings();
            MainRoadChecker.Instance.RecheckAllBuildings();
        }
    }

    public void RecalculateConnections()
    {
        // Reset all roads
        GameObject[] allCurrentRoads = GameObject.FindGameObjectsWithTag(targetRoadTag);
        foreach (GameObject road in allCurrentRoads)
        {
            if (road != null) road.tag = disconnectedRoadTag;
        }

        // Find City Halls
        GameObject[] cityHalls = GameObject.FindGameObjectsWithTag(cityHallTag);
        if (cityHalls.Length == 0) return;

        // BFS Setup
        Queue<GameObject> nodesToCheck = new Queue<GameObject>();
        HashSet<GameObject> checkedRoads = new HashSet<GameObject>();

        foreach (GameObject cityHall in cityHalls)
        {
            if (cityHall != null)
            {
                nodesToCheck.Enqueue(cityHall);
                checkedRoads.Add(cityHall);
            }
        }

        // BFS Algorithm
        while (nodesToCheck.Count > 0)
        {
            GameObject currentNode = nodesToCheck.Dequeue();
            if (currentNode == null) continue;

            List<GameObject> adjacentRoads = GetAdjacentRoads(currentNode.transform.position, currentNode);

            foreach (GameObject adjacentRoad in adjacentRoads)
            {
                if (adjacentRoad != null && !checkedRoads.Contains(adjacentRoad))
                {
                    adjacentRoad.tag = targetRoadTag;
                    nodesToCheck.Enqueue(adjacentRoad);
                    checkedRoads.Add(adjacentRoad);
                }
            }
        }
    }

    private List<GameObject> GetAdjacentRoads(Vector3 centerPosition, GameObject sourceObject = null)
    {
        if (sourceObject != null && sourceObject.CompareTag(cityHallTag))
        {
            return GetRoadsAroundBuilding(sourceObject);
        }

        return GetAdjacentRoadsFromPoint(centerPosition);
    }

    private List<GameObject> GetRoadsAroundBuilding(GameObject building)
    {
        List<GameObject> allRoads = new List<GameObject>();

        for (int i = 0; i < cityHallOffsets.Length; i++)
        {
            Vector3 boxCenter = building.transform.position + cityHallOffsets[i];
            Vector3 halfSize = cityHallSizes[i] / 2f;

            if (enableDebugVisualization)
            {
                DrawDebugBox(boxCenter, halfSize, Color.green);
            }

            Collider[] hitColliders = Physics.OverlapBox(boxCenter, halfSize, Quaternion.identity);

            foreach (Collider col in hitColliders)
            {
                if (col != null && col.gameObject != null && col.gameObject != building)
                {
                    bool isRoad = col.CompareTag(disconnectedRoadTag) || col.CompareTag(targetRoadTag);
                    if (isRoad && !allRoads.Contains(col.gameObject))
                    {
                        allRoads.Add(col.gameObject);
                    }
                }
            }
        }

        return allRoads;
    }

    private List<GameObject> GetAdjacentRoadsFromPoint(Vector3 point)
    {
        List<GameObject> roads = new List<GameObject>();
        Vector3[] directions = { Vector3.forward, Vector3.back, Vector3.right, Vector3.left };

        foreach (Vector3 dir in directions)
        {
            Vector3 checkPos = point + (dir * gridSize);

            if (enableDebugVisualization)
            {
                Debug.DrawLine(point, checkPos, Color.cyan, 2f);
                Debug.DrawRay(checkPos, Vector3.up * 2, Color.yellow, 2f);
            }

            Collider[] hitColliders = Physics.OverlapSphere(checkPos, checkRadius);

            foreach (Collider col in hitColliders)
            {
                if (col != null && col.gameObject != null &&
                    col.CompareTag(disconnectedRoadTag) &&
                    !roads.Contains(col.gameObject))
                {
                    if (IsOrthogonalNeighbor(point, col.transform.position))
                    {
                        roads.Add(col.gameObject);

                        if (enableDebugVisualization)
                        {
                            Debug.DrawLine(point, col.transform.position, Color.green, 2f);
                        }
                    }
                    else if (enableDebugVisualization)
                    {
                        Debug.DrawLine(point, col.transform.position, Color.red, 2f);
                    }
                }
            }
        }

        return roads;
    }

    private bool IsOrthogonalNeighbor(Vector3 pos1, Vector3 pos2)
    {
        Vector2 gridPos1 = new Vector2(
            Mathf.Round(pos1.x / gridSize) * gridSize,
            Mathf.Round(pos1.z / gridSize) * gridSize
        );

        Vector2 gridPos2 = new Vector2(
            Mathf.Round(pos2.x / gridSize) * gridSize,
            Mathf.Round(pos2.z / gridSize) * gridSize
        );

        Vector2 difference = gridPos2 - gridPos1;
        bool isOrthogonal =
            (Mathf.Abs(difference.x) == gridSize && Mathf.Abs(difference.y) == 0) ||
            (Mathf.Abs(difference.x) == 0 && Mathf.Abs(difference.y) == gridSize);

        return isOrthogonal;
    }

    private void DrawDebugBox(Vector3 center, Vector3 halfSize, Color color)
    {
        Vector3[] corners = new Vector3[8];
        corners[0] = center + new Vector3(-halfSize.x, -halfSize.y, -halfSize.z);
        corners[1] = center + new Vector3(-halfSize.x, -halfSize.y, halfSize.z);
        corners[2] = center + new Vector3(-halfSize.x, halfSize.y, -halfSize.z);
        corners[3] = center + new Vector3(-halfSize.x, halfSize.y, halfSize.z);
        corners[4] = center + new Vector3(halfSize.x, -halfSize.y, -halfSize.z);
        corners[5] = center + new Vector3(halfSize.x, -halfSize.y, halfSize.z);
        corners[6] = center + new Vector3(halfSize.x, halfSize.y, -halfSize.z);
        corners[7] = center + new Vector3(halfSize.x, halfSize.y, halfSize.z);

        Debug.DrawLine(corners[0], corners[1], color, 2f);
        Debug.DrawLine(corners[0], corners[2], color, 2f);
        Debug.DrawLine(corners[0], corners[4], color, 2f);
        Debug.DrawLine(corners[1], corners[3], color, 2f);
        Debug.DrawLine(corners[1], corners[5], color, 2f);
        Debug.DrawLine(corners[2], corners[3], color, 2f);
        Debug.DrawLine(corners[2], corners[6], color, 2f);
        Debug.DrawLine(corners[3], corners[7], color, 2f);
        Debug.DrawLine(corners[4], corners[5], color, 2f);
        Debug.DrawLine(corners[4], corners[6], color, 2f);
        Debug.DrawLine(corners[5], corners[7], color, 2f);
        Debug.DrawLine(corners[6], corners[7], color, 2f);
    }
}