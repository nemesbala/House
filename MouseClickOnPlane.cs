using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using UnityEngine.EventSystems;

public class MouseClickOnPlane : MonoBehaviour
{
    [Header("Plane Settings")]
    public LayerMask planeLayer; // Layer mask for the planes
    public LayerMask GroundLayer;
    public GameObject[] replacementPlanes; // Array of replacement planes to choose from

    [Header("Pricing Settings")]
    public int baseCost = 100; // Base cost for the first piece
    public float costMultiplier = 2.0f; // Multiplier for exponential cost increase

    [Header("Click Feedback")]
    public GameObject clickMarkerPrefab; // Optional: Marker to show click location
    public GameObject confirmationUI; // UI confirmation panel
    public TMP_Text costText; // Text field to display the cost

    private Camera mainCamera;
    private GameObject selectedPlane; // The plane that was clicked
    private GameObject activeUI; // The currently active confirmation UI
    ObstacleSpawner obstacle;
    private string saveFilePath;
    private string cashFilePath;
    public int currentCash;
    private int piecesBought = 0; // Tracks how many pieces have been purchased

    void Start()
    {
        mainCamera = Camera.main;
        saveFilePath = Path.Combine(Application.persistentDataPath, "plane_data.txt");
        cashFilePath = Path.Combine(Application.persistentDataPath, "Cash.txt");

        LoadCash();
        UpdateCashDisplay();
    }

    void Update()
    {
        DetectMouseClick();
    }

    private void DetectMouseClick()
    {
        // Check for mouse input
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            // Ignore clicks if the pointer is over a UI element
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.Log("Pointer is over a UI element. Ignoring click.");
                return;
            }

            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, GroundLayer))
            {

            }
            else if (Physics.Raycast(ray, out hit, Mathf.Infinity, planeLayer))
            {
                //Debug.Log($"Mouse clicked on plane at: {hit.point}");

                // Optional: Show a marker at the clicked position
                if (clickMarkerPrefab != null)
                {
                    Instantiate(clickMarkerPrefab, hit.point, Quaternion.identity);
                }

                // Store the clicked plane
                selectedPlane = hit.collider.gameObject;

                // Activate the associated UI for the clicked plane
                PlaneUIHandler planeUIHandler = selectedPlane.GetComponent<PlaneUIHandler>();
                if (planeUIHandler != null)
                {
                    if (activeUI != null) activeUI.SetActive(false); // Hide the previous UI
                    activeUI = planeUIHandler.confirmationUI;
                    activeUI.SetActive(true);

                    // Update the cost display
                    if (costText != null)
                    {
                        int currentCost = CalculateCost();
                        costText.text = "This expansion costs: " + currentCost.ToString() + "$. Would You like to purchase it?";
                    }
                }
            }
        }
    }

    public void Return()
    {
        activeUI.SetActive(false);
    }

    private void ReplaceSelectedPlane()
    {
        int currentCost = CalculateCost();
        LoadCash();
        if (selectedPlane != null && replacementPlanes.Length > 0 && currentCash >= currentCost)
        {
            activeUI.SetActive(false);
            // Subtract the replacement cost
            currentCash -= currentCost;
            SaveCash();
            UpdateCashDisplay();

            // Select a random replacement plane from the array
            GameObject replacementPlane = replacementPlanes[Random.Range(0, replacementPlanes.Length)];

            // Instantiate the replacement plane at the selected plane's position and rotation
            Vector3 parent = selectedPlane.transform.position;
            parent.y = 0f;
            GameObject newPlane = Instantiate(replacementPlane, parent, selectedPlane.transform.rotation);

            // Save the new plane's position and type
            SavePlaneData(newPlane);

            // Hide the confirmation UI
            if (activeUI != null)
            {
                activeUI.SetActive(false);
            }
            Destroy(selectedPlane);
            //selectedPlane = null; // Clear the selection
        }
        else if(currentCash < currentCost)
        {
            costText.text = "You do not have enough cash for this expansion!";
        }
        else
        {
            Debug.LogWarning("Current Cash: " + currentCash + ", replacementPlanes.Length: " + replacementPlanes.Length);
        }
    }

    private int CalculateCost()
    {
        piecesBought = File.ReadAllLines(saveFilePath).Length;
        return Mathf.CeilToInt(baseCost * Mathf.Pow(costMultiplier, piecesBought));
    }

    private void SavePlaneData(GameObject newPlane)
    {
        string CleanName = newPlane.name.Replace("(Clone)", "");
        string data = $"{newPlane.transform.position.x},0,{newPlane.transform.position.z},{CleanName}\n";
        File.AppendAllText(saveFilePath, data);
        obstacle = null;
        obstacle = newPlane.GetComponent<ObstacleSpawner>();
        obstacle.SpawnObstacles();
        obstacle = null;
    }

    private void SaveCash()
    {
        File.WriteAllText(cashFilePath, currentCash.ToString());
        //Debug.Log("Saved cash: " + currentCash);
    }

    private void LoadCash()
    {
        if (File.Exists(cashFilePath))
        {
            string cashString = File.ReadAllText(cashFilePath);
            if (int.TryParse(cashString, out int loadedCash))
            {
                currentCash = loadedCash;
            }
            else
            {
                Debug.LogWarning("Failed to parse cash data. Defaulting to 0.");
                currentCash = 185;
            }
        }
        else
        {
            currentCash = 185; // Default cash value
            SaveCash();
        }
    }

    private void UpdateCashDisplay()
    {
        CashDisplay cashDisplay = FindObjectOfType<CashDisplay>();
        if (cashDisplay != null)
        {
            cashDisplay.Start();
            //Debug.Log("Cash text updated!");
        }
    }
}