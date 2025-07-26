using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PublicBoolForPauseMenuOpen : MonoBehaviour
{
    public bool isAnUIOpened = false;

    public bool Check()
    {
        return isAnUIOpened;
    }

    public void TurnFalse()
    { isAnUIOpened = false; }

    public void TurnTrue()
    { isAnUIOpened = true; }
}
