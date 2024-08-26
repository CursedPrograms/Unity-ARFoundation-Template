using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTrackedImageStateChecker : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager trackedImageManager;

    private void OnEnable()
    {
        if (trackedImageManager == null)
        {
            trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        }

        if (trackedImageManager != null)
        {
            // Subscribe to the event
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }
    }

    private void OnDisable()
    {
        if (trackedImageManager != null)
        {
            // Unsubscribe from the event
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        // Check for added or updated images
        foreach (var trackedImage in eventArgs.added)
        {
            // Image has been detected
            Debug.Log($"Image detected: {trackedImage.referenceImage.name}");
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            // Image has been updated (e.g., it's being tracked again after being lost)
            Debug.Log($"Image updated: {trackedImage.referenceImage.name}");
        }

        // Optionally, handle removed images if needed
        foreach (var trackedImage in eventArgs.removed)
        {
            Debug.Log($"Image removed: {trackedImage.referenceImage.name}");
        }
    }
}
