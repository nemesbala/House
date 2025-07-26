using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ESCCloser : MonoBehaviour
{
    public UnityEvent onAction;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            onAction.Invoke();
        }
    }
}
