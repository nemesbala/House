using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickPreventer : MonoBehaviour
{
    public LayerMask Layer;
    private Camera mainCamera;
    // Update is called once per frame
    void Update()
    {
        mainCamera = Camera.main;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        // Perform the raycast
        if (Physics.Raycast(ray, out hit, Mathf.Infinity, Layer))
        {

        }
    }
}
