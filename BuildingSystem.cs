using UnityEngine;
using System.Collections;
using System;
using System.IO;
using UnityEngine.EventSystems;

public class BuildingSystem : MonoBehaviour
{
    public GameObject[] buildings; // Array to hold different building prefabs
    private GameObject currentBuilding; // The currently selected building
    public bool isBuildingEnabled = false; // Track if the building mode is enabled
    public LayerMask placementLayer; // LayerMask to filter where buildings can be placed
    public LayerMask placementLayer2;
    public float gridSize = 1f; // The size of the grid cells
    private Renderer objectRenderer;
    private Color originalColor;
    public GameObject ErrorUI;

    [Header("SFX")]
    public AudioClip InvalidPlaceClip;
    public AudioClip PlacementClip;
    
    [Header("Scripts")]
    public House exampleScript;
    public Factory factoryScript;
    public Road roadScript;
    public CityHall cityHallScript;
    public Vector3 accessedVector;
    public Vector3 sizeVector;
    public GameObject ConstructionActiveIndicator;
    public bool A;
    public bool C;
    [SerializeField] private AudioSource uiAudioSource;

    void Starter()
    {
        A = IsPointerOverValidPlacement();
        C = IsPointerOverValidPlacementC();
    }
    // Method to be called when a UI button is pressed to select a building
    public void SelectBuilding(int buildingIndex)
    {
        if (buildingIndex < 0 || buildingIndex >= buildings.Length)
        {
            Debug.LogWarning("Invalid building index selected.");
            return;
        }

        // Deactivate any currently selected building
        if (currentBuilding != null)
        {
            Destroy(currentBuilding);
        }

        // Instantiate the selected building and enable building mode
        currentBuilding = Instantiate(buildings[buildingIndex]);
        isBuildingEnabled = true;

        try
        {
            exampleScript = currentBuilding.GetComponent<House>();
            factoryScript = currentBuilding.GetComponent<Factory>();
            roadScript = currentBuilding.GetComponent<Road>();
            cityHallScript = currentBuilding.GetComponent<CityHall>();
        }
        catch
        {
            Debug.Log("There is something wrong");
        }

        if(exampleScript != null)
        {
            accessedVector = exampleScript.offset;
            sizeVector = exampleScript.size;
        }
        else if(factoryScript != null)
        {
            accessedVector = factoryScript.offset;
            sizeVector = factoryScript.size;
        }
        else if (roadScript != null)
        {
            accessedVector = roadScript.offset;
            sizeVector = roadScript.size;
        }
        else if (cityHallScript != null)
        {
            accessedVector = cityHallScript.offset;
            sizeVector = cityHallScript.size;
        }

        // Get the Renderer component and store the original color
        Transform childTransform = FindChildRecursive(currentBuilding.transform, "BASE");
        if(childTransform != null)
        {
            objectRenderer = childTransform.GetComponent<Renderer>();

            if(objectRenderer != null)
            {
                originalColor = objectRenderer.material.color;
            }
            else
            {
                Debug.LogWarning("No Renderer component found on the target child GameObject.");
            }
        }
        else
        {
            Debug.LogWarning("Target child GameObject not found.");
        }
        ConstructionActiveIndicator.SetActive(true);
    }

    // Recursive function to search for a child by name at any depth
    private Transform FindChildRecursive(Transform parent, string childName)
    {
        foreach (Transform child in parent)
        {
            if (child.name == childName)
                return child;

            Transform result = FindChildRecursive(child, childName);
            if (result != null)
                return result;
        }
        return null;
    }

    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            isBuildingEnabled = false;
            Destroy(currentBuilding);
            currentBuilding = null;
            ConstructionActiveIndicator.SetActive(false);
        }

        if (isBuildingEnabled && currentBuilding != null)
        {
            // Update building position to follow the mouse with grid snapping
            UpdateBuildingPosition();
            // Place the building on left-click if it's over a valid collider
            Starter();
            bool isValid = IsPointerOverValidPlacement() && IsPointerOverValidPlacementC();
            if (isValid)
            {
                // Revert to the original color
                objectRenderer.material.color = originalColor;

                if (Input.GetMouseButtonDown(0) && !IsPointerOverUI())
                {
                    PlaceBuilding();
                    if (PlacementClip != null)
                    {
                        string SFXFilePath;
                        SFXFilePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "Audio.txt");
                        string[] datas = File.ReadAllLines(SFXFilePath);
                        float volume = float.Parse(datas[0]);
                        GameObject Cam = GameObject.Find("Main Camera");
                        uiAudioSource.PlayOneShot(PlacementClip, volume);
                    }
                }
            }
            else
            {
                objectRenderer.material.color = new Color(1, 0, 0, 0.5f);
                //Debug.Log("WORKING!");
                if (Input.GetMouseButtonDown(0))
                {
                    ErrorUI.SetActive(false);
                    ErrorUI.SetActive(true);
                    if (InvalidPlaceClip != null)
                    {
                        string SFXFilePath;
                        SFXFilePath = Path.Combine(Path.Combine(Path.GetDirectoryName(Application.dataPath), "SaveDir"), "Audio.txt");
                        string[] datas = File.ReadAllLines(SFXFilePath);
                        float volume = float.Parse(datas[0]);
                        GameObject Cam = GameObject.Find("Main Camera");
                        uiAudioSource.PlayOneShot(InvalidPlaceClip, volume);
                    }
                }
            }
        }
    }

    // Update the building's position to follow the mouse with grid snapping
    void UpdateBuildingPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, placementLayer))
        {
            Vector3 point = hit.point;

            // Snap to the nearest grid point
            point.x = Mathf.Round(point.x / gridSize) * gridSize;
            point.y = 0;
            point.z = Mathf.Round(point.z / gridSize) * gridSize;

            currentBuilding.transform.position = new Vector3(point.x, point.y, point.z);
        }
    }

    // Place the building and call the OnPlaced function
    void PlaceBuilding()
    {
        isBuildingEnabled = false;

        try
        {
            // Call the OnPlaced function in the building script
            House buildingScriptA = currentBuilding.GetComponent<House>();
            Factory buildingScriptB = currentBuilding.GetComponent<Factory>();
            Road buildingScriptC = currentBuilding.GetComponent<Road>();
            CityHall buildingScriptD = currentBuilding.GetComponent<CityHall>();

            if (buildingScriptA != null)
            {
                buildingScriptA.OnPlaced();
            }
            else if (buildingScriptB != null)
            {
                buildingScriptB.OnPlaced();
            }
            else if (buildingScriptC != null)
            {
                buildingScriptC.OnPlaced();
            }
            else if (buildingScriptD != null)
            {
                buildingScriptD.OnPlaced();
            }
            else
            {
                Debug.LogError("Warning! OnPlaced() was not called as the needed script was not found!");
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error in BuildingSystem (Line: 241): {ex.Message}");
        }
        finally
        {
            currentBuilding = null;
            ConstructionActiveIndicator.SetActive(false);
        }
    }

    bool IsPointerOverValidPlacement()
    {
        RaycastHit m_hit;
        Vector3 halfExtents = objectRenderer.bounds.extents;
        float maxDistance = halfExtents.y + 0.5f;
        Vector3 castDirection = Vector3.down;
        Vector3 origin = currentBuilding.transform.position + Vector3.up * maxDistance;

        bool hitDetected = Physics.BoxCast(
            origin + accessedVector,
            halfExtents + sizeVector,
            castDirection,
            out m_hit,
            currentBuilding.transform.rotation,
            maxDistance,
            placementLayer
        );

        Debug.DrawRay(origin + accessedVector, castDirection * maxDistance, hitDetected ? Color.green : Color.red, 1f);
        Debug.DrawLine(origin + accessedVector - halfExtents, origin + accessedVector + halfExtents, Color.blue, 1f);

        return hitDetected;
    }

    public bool IsPointerOverValidPlacementC()
    {
        //Debug.Log("Checking placement...");
        // Define the center of the box check
        Vector3 center = currentBuilding.transform.position + accessedVector;

        // Define the half-extents of the box (half the size in each dimension)
        Vector3 halfExtents = objectRenderer.bounds.extents + sizeVector;

        // Use the rotation of the building object
        Quaternion rotation = currentBuilding.transform.rotation;

        // Check for any colliders overlapping with the box in the specified placement layer
        Collider[] overlaps = Physics.OverlapBox(center, halfExtents, rotation, placementLayer2);

        // Debug log all colliders found
        if (overlaps.Length > 0)
        {
            //Debug.Log($"Warning! Overlap detected: {overlaps.Length} colliders.");
            foreach (Collider col in overlaps)
            {
                //Debug.Log($"Detected collider: {col.name}, Layer: {LayerMask.LayerToName(col.gameObject.layer)}");
            }
        }
        else
        {
            //Debug.Log("No overlaps detected.");
        }

        // If the overlaps array is empty, the area is clear
        return overlaps.Length == 0;
    }


    void OnDrawGizmos()
    {
        if (currentBuilding == null) return;

        Vector3 center = currentBuilding.transform.position + accessedVector;
        Vector3 halfExtents = objectRenderer.bounds.extents + sizeVector;
        Quaternion rotation = currentBuilding.transform.rotation;

        Gizmos.color = Color.green;
        Gizmos.matrix = Matrix4x4.TRS(center, rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, halfExtents * 2f);
    }


    // Method to check if the mouse is over a UI element
    private bool IsPointerOverUI()
    {
        return EventSystem.current.IsPointerOverGameObject();
    }
}