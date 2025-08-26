using UnityEngine;
using UnityEngine.EventSystems;

public class RaycastHoverHandlerWithDelay : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask layer;
    public float hoverTimeThreshold = 1.5f;
    public BuildingSystem buildingSystem;

    private GameObject currentHoveredObject;
    private float hoverTimer;

    void Update()
    {
        if (buildingSystem.isBuildingEnabled)
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layer))
        {
            GameObject hitObject = hit.collider.gameObject;

            if (hitObject != currentHoveredObject)
            {
                // Mouse exited previous object
                CallNoHoverFunction(currentHoveredObject);

                // Entering new object
                currentHoveredObject = hitObject;
                hoverTimer = 0f;
            }
            else
            {
                hoverTimer += Time.deltaTime;

                if (hoverTimer >= hoverTimeThreshold)
                {
                    CallHoverFunction(currentHoveredObject);
                    hoverTimer = 0f; // Optional: reset to prevent repeat firing
                }
            }
        }
        else
        {
            // No object under cursor anymore
            CallNoHoverFunction(currentHoveredObject);
            currentHoveredObject = null;
            hoverTimer = 0f;
        }
    }

    void CallHoverFunction(GameObject obj)
    {
        if (obj == null) return;

        obj.GetComponent<House>()?.OnMouseHover();
        obj.GetComponent<Factory>()?.OnMouseHover();
        obj.GetComponent<Road>()?.OnMouseHover();
        obj.GetComponent<CityHall>()?.OnMouseHover();
        obj.GetComponent<Obstacle>()?.OnMouseHover();
        obj.GetComponent<Info>()?.OnMouseHover();
    }

    void CallNoHoverFunction(GameObject obj)
    {
        if (obj == null) return;

        obj.GetComponent<House>()?.NoMouseHover();
        obj.GetComponent<Factory>()?.NoMouseHover();
        obj.GetComponent<Road>()?.NoMouseHover();
        obj.GetComponent<CityHall>()?.NoMouseHover();
        obj.GetComponent<Obstacle>()?.NoMouseHover();
        obj.GetComponent<Info>()?.NoMouseHover();
    }
}