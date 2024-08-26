using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[DisallowMultipleComponent]
public class Autoplace : MonoBehaviour
{
    public enum PlacementType { Floor, Roof, Wall }
    public enum PlacementStrategy
    {
        None,
        Sequential,
        Random
    }

    [SerializeField] private PlacementType placementType;
    [SerializeField] private GameObject prefab;
    [SerializeField] private PlacementStrategy placementStrategy;
    [SerializeField] private GameObject[] prefabs;
    [SerializeField] private float generationInterval = 1f;

    private ARRaycastManager raycastManager;
    private ARPlaneManager planeManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private int prefabIndex = 0; 

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
    }

    void Start()
    {
        InvokeRepeating("RandomRay", 0f, generationInterval);
    }

    void RandomRay()
    {
        Vector2 randomScreenPoint = new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
        Ray ray = Camera.main.ScreenPointToRay(randomScreenPoint);

        if (raycastManager.Raycast(ray, hits, TrackableType.PlaneWithinPolygon))
        {
            ARRaycastHit hit = hits[0];
            PlaneAlignment alignment = planeManager.GetPlane(hit.trackableId).alignment;

            if (IsCorrectPlacement(alignment))
            {
                GameObject chosenPrefab = ChoosePrefab();
                Instantiate(chosenPrefab, hit.pose.position, Quaternion.identity);
            }
        }
    }

    GameObject ChoosePrefab()
    {
        switch (placementStrategy)
        {
            case PlacementStrategy.None:
                return prefab;
            case PlacementStrategy.Sequential:
                if (prefabs.Length == 0)
                    return prefab;
                GameObject sequentialPrefab = prefabs[prefabIndex];
                prefabIndex = (prefabIndex + 1) % prefabs.Length;
                return sequentialPrefab;
            case PlacementStrategy.Random:
                if (prefabs.Length == 0)
                    return prefab;
                int randomIndex = Random.Range(0, prefabs.Length);
                return prefabs[randomIndex];
            default:
                return prefab;
        }
    }

    private bool IsCorrectPlacement(PlaneAlignment alignment)
    {
        switch (placementType)
        {
            case PlacementType.Floor:
                return alignment == PlaneAlignment.HorizontalUp;
            case PlacementType.Roof:
                return alignment == PlaneAlignment.HorizontalDown;
            case PlacementType.Wall:
                return alignment == PlaneAlignment.Vertical;
            default:
                return false;
        }
    }
}
