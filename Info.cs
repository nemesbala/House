using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Info : MonoBehaviour
{
    [Tooltip("This is the UI, that is going to be enabled, when the mouse hover over the building. (Assign the IMAGE!(Child of the Canvas) NOT THE CANVAS!)")]
    public GameObject objectToHoverEnable;

    public void OnMouseHover()
    {
        objectToHoverEnable.SetActive(true);
    }

    public void NoMouseHover()
    {
        objectToHoverEnable.SetActive(false);
    }
}
