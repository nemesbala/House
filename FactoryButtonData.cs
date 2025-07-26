using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FactoryButtonData : MonoBehaviour
{
    public int GoodsID;
    public int ProductAmmount;
    public int WPNeeded;

    public int ID()
        { return GoodsID; }

    public int Ammount()
        { return ProductAmmount; }

    public int WP()
        { return WPNeeded; }
}