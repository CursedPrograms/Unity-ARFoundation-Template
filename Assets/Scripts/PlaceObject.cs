using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[DisallowMultipleComponent]
[RequireComponent(typeof(ARPlaneManager))]
[RequireComponent(typeof(ARRaycastManager))]
public class PlaceObject : MonoBehaviour
{
    bool Placed;

    GameObject currentObject;

    [SerializeField]
    GameObject prefab;
    ARRaycastManager raycastManager;
    ARPlaneManager planeManager;
    List<ARRaycastHit> hits = new List<ARRaycastHit>();

    void Awake()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();
    }

    private void Start()
    {
        Placed = false;
    }

    void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }

    void FingerDown(EnhancedTouch.Finger finger)
    {
        if (finger.index != 0)
        {
            return;
        }
        else
        {
            if (raycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon))
            {
                foreach (ARRaycastHit hit in hits)
                {
                    Pose pose = hit.pose;

                    if (planeManager.GetPlane(hit.trackableId).alignment == PlaneAlignment.HorizontalUp)
                    {
                        if (!Placed)
                        {
                            Instantiate(pose);
                        }
                        else
                        {
                            //Destroy currentObject
                            Instantiate(pose);
                        }
                    }
                    else if (planeManager.GetPlane(hit.trackableId).alignment == PlaneAlignment.HorizontalUp)
                    {

                    }
                    else
                    {

                    }
                }
            }
        }
    }

    void Instantiate(Pose pose)
    {
        currentObject = Instantiate(prefab, pose.position, pose.rotation);
        Placed = true;
    }
}
