using UnityEngine;
#if UNITY_ANDROID && !UNITY_EDITOR
using Unity.XR.Oculus;
#endif

public class MetaQuestFrameRateSetter : MonoBehaviour
{
    [SerializeField] private int targetFrameRate = 90;

    void Start()
    {
        // Lock Unity's Application frame rate
        Application.targetFrameRate = targetFrameRate;

        // Optional: Match vsync count
        QualitySettings.vSyncCount = 0; // Disable vsync, since we are manually setting FPS

#if UNITY_ANDROID && !UNITY_EDITOR
        // Attempt to set preferred refresh rate (for Meta Quest)
        try
        {
            OVRManager.display.displayFrequency = targetFrameRate;
            Debug.Log($"[MetaQuest] Target refresh rate set to {targetFrameRate}Hz");
        }
        catch
        {
            Debug.LogWarning("[MetaQuest] Failed to set refresh rate. Ensure OVRManager is initialized and the device supports it.");
        }
#endif
    }
}