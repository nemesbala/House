using UnityEngine;
using EPOOutline;
using UnityEngine.EventSystems;

public class RaycastHoverHandler : MonoBehaviour
{
    public Camera mainCamera;
    public LayerMask Layer;
    private Outlinable outlinableToUse;
    private Outlinable lastOutline;

    void Update()
    {
        // Skip hover detection if mouse is over UI
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Layer))
        {
            GameObject hoveredObject = hit.collider.gameObject;
            outlinableToUse = hoveredObject.GetComponent<Outlinable>();

            if (outlinableToUse != null)
            {
                if (outlinableToUse != lastOutline)
                {
                    DisableLastOutline();

                    outlinableToUse.OutlineParameters.Enabled = true;
                    lastOutline = outlinableToUse;
                }
            }
            else
            {
                DisableLastOutline();
            }
        }
        else
        {
            DisableLastOutline();
        }
    }

    private void DisableLastOutline()
    {
        if (lastOutline != null)
        {
            lastOutline.OutlineParameters.Enabled = false;
            lastOutline = null;
        }
    }
}
