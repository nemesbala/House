using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class FactoryEnableCheck : MonoBehaviour
{
    public GameObject[] ProductionStarterButtons;
    public FactoryWPnLevelLock factoryWPnLevelLock;

    public void OnEnable()
    {
        foreach (GameObject obj in ProductionStarterButtons)
        {
            factoryWPnLevelLock = obj.GetComponent<FactoryWPnLevelLock>();
            if(factoryWPnLevelLock != null)
            {
                factoryWPnLevelLock.Start();
            }
        }
    }
}
