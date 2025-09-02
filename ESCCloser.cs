using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ESCCloser : MonoBehaviour
{
    [Header("UI References")]
    public GameObject[] subMenus; // Drag & drop all sub-UI GameObjects here

    [Header("Actions")]
    public UnityEvent onSubMenuAction; // Fires when submenus are closed (additional actions)
    public UnityEvent onMainAction;    // Fires when no submenus are open (close main menu/pause game)

    void Update()
    {
        // Use the universal Cancel input instead of specific key
        if (Input.GetButtonDown("Cancel"))
        {
            HandleCancelPress();
        }
    }

    private void HandleCancelPress()
    {
        // Check if any submenus are active
        bool anySubMenuOpen = false;

        foreach (GameObject subMenu in subMenus)
        {
            if (subMenu != null && subMenu.activeSelf)
            {
                anySubMenuOpen = true;
                break; // Exit early if we found one active submenu
            }
        }

        // Handle the appropriate action
        if (anySubMenuOpen)
        {
            CloseAllSubMenus(); // Hardcoded: always close submenus
            onSubMenuAction.Invoke(); // Additional actions after closing
        }
        else
        {
            onMainAction.Invoke(); // Execute main action
        }
    }

    // Automatically closes all submenus (hardcoded behavior)
    private void CloseAllSubMenus()
    {
        foreach (GameObject subMenu in subMenus)
        {
            if (subMenu != null)
            {
                subMenu.SetActive(false);
            }
        }
    }

    // Optional: Public method if you need to close submenus from other scripts
    public void CloseSubMenus()
    {
        CloseAllSubMenus();
        onSubMenuAction.Invoke();
    }
}