using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastClickHandler : MonoBehaviour
{
    public Camera mainCamera; // Assign your main camera in the Inspector
    public LayerMask Layer;
    Factory factoryScript;
    House houseScript;
    Road roadScript;
    CityHall cityHallScript;
    Obstacle obstacleScript;
    public BuildingSystem buildingSystem;

    void Update()
    {
        // Check for left mouse button click
        if (Input.GetMouseButtonDown(0) && !buildingSystem.isBuildingEnabled)
        {
            // Ignore clicks if the pointer is over a UI element
            if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            {
                //Debug.Log("Pointer is over a UI element. Ignoring click.");
                return;
            }

            // Perform the raycast
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity, Layer))
            {
                // Retrieve the GameObject that was clicked
                
                GameObject clickedObject = hit.collider.gameObject;

                // Try to find the House script on the clicked object
                houseScript = clickedObject.GetComponent<House>();
                factoryScript = clickedObject.GetComponent<Factory>();
                roadScript = clickedObject.GetComponent<Road>();
                cityHallScript = clickedObject.GetComponent<CityHall>();
                obstacleScript = clickedObject.GetComponent<Obstacle>();


                if (houseScript != null)
                {
                    houseScript.OnClick(); // Call the method in House.cs
                }

                if(factoryScript != null)
                {
                    factoryScript.OnClick();
                }

                if(roadScript != null)
                {
                    roadScript.OnClick();
                }

                if (cityHallScript != null)
                {
                    cityHallScript.OnClick();
                }

                if(obstacleScript != null)
                {
                    obstacleScript.OnClick();
                }
            }
        }
    }
}