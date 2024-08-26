using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARTrackedImageAudioController : MonoBehaviour
{
    [SerializeField]
    private ARTrackedImageManager trackedImageManager;
    private AudioSource audioSource;

    private void OnEnable()
    {
        if (trackedImageManager == null)
        {
            trackedImageManager = FindObjectOfType<ARTrackedImageManager>();
        }

        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
        }

        audioSource = GetComponent<AudioSource>();

        if (audioSource == null)
        {
            Debug.LogError("AudioSource component missing from the GameObject.");
        }
    }

    private void OnDisable()
    {
        if (trackedImageManager != null)
        {
            trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
        }
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (var trackedImage in eventArgs.added)
        {
            HandleTrackedImage(trackedImage);
        }

        foreach (var trackedImage in eventArgs.updated)
        {
            HandleTrackedImage(trackedImage);
        }

        foreach (var trackedImage in eventArgs.removed)
        {
            if (trackedImage.trackingState == TrackingState.None && audioSource.isPlaying)
            {
                Debug.Log($"Image {trackedImage.referenceImage.name} removed, stopping audio.");
                audioSource.Stop();           
            }
        }
    }

    private void HandleTrackedImage(ARTrackedImage trackedImage)
    {
        if (trackedImage.trackingState == TrackingState.Tracking)
        {
            if (!audioSource.isPlaying)
            {
                Debug.Log($"Image {trackedImage.referenceImage.name} detected, playing audio.");
                audioSource.Play();      
            }
        }
        else if (trackedImage.trackingState == TrackingState.Limited)
        {
            if (audioSource.isPlaying)
            {
                Debug.Log($"Image {trackedImage.referenceImage.name} tracking is limited, pausing audio.");
                audioSource.Pause();    
            }
        }
        else if (trackedImage.trackingState == TrackingState.None)
        {
            if (audioSource.isPlaying)
            {
                Debug.Log($"Image {trackedImage.referenceImage.name} is not being tracked, pausing audio.");
                audioSource.Pause();    
            }
        }
    }
}
