using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MainRoadChecker : MonoBehaviour
{
    public static MainRoadChecker Instance;

    private List<House> houses = new List<House>();
    private List<Factory> factories = new List<Factory>();
    private List<CityHall> citihalls = new List<CityHall>();

    void Awake()
    {
        Instance = this;
        CollectBuildings();
    }

    public void CollectBuildings()
    {
        houses.Clear();
        factories.Clear();
        citihalls.Clear();

        houses.AddRange(FindObjectsOfType<House>());
        factories.AddRange(FindObjectsOfType<Factory>());
        citihalls.AddRange(FindObjectsOfType<CityHall>());
    }

    public void RecheckAllBuildings()
    {
        StartCoroutine(RemoveRoadWithDelay());
    }

    private IEnumerator RemoveRoadWithDelay()
    {
        yield return null;

        foreach (var house in houses)
        {
            if (house != null)
                house.CheckRoadConnection();
        }

        foreach (var factory in factories)
        {
            if (factory != null)
                factory.CheckRoadConnection();
        }

        foreach (var cityhall in citihalls)
        {
            if (cityhall != null)
                cityhall.CheckRoadConnection();
        }
    }

}