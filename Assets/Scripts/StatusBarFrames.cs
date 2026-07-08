using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class StatusBarFrames : MonoBehaviour
{
    [Header("Image Component")]
    [SerializeField] private Image barImage;

    [Header("Sprite Frames")]
    [SerializeField] private Sprite[] frames;

    [Header("Value Settings")]
    [SerializeField] private float maxValue = 100f;

    [Tooltip("Turn this on if your frames go from empty to full. Example: 0%, 25%, 50%, 75%, 100%.")]
    [SerializeField] private bool framesAreEmptyToFull = true;

    [Header("Animation Settings")]
    [SerializeField] private bool animateChanges = true;
    [SerializeField] private float frameDelay = 0.03f;

    private int currentFrameIndex = -1;
    private Coroutine animationRoutine;

    private void Awake()
    {
        // If no Image was assigned in the Inspector, try to find one on this object.
        if (barImage == null)
        {
            barImage = GetComponent<Image>();
        }

        // Start the bar as full by default.
        SetValue(maxValue);
    }

    public void SetValue(float newValue)
    {
        if (barImage == null || frames == null || frames.Length == 0)
        {
            Debug.LogWarning(name + " is missing an Image or sprite frames.");
            return;
        }

        // Clamp value between 0 and maxValue.
        float clampedValue = Mathf.Clamp(newValue, 0f, maxValue);

        // Convert the current value into a number from 0 to 1.
        float normalizedValue = clampedValue / maxValue;

        // Convert that value into a sprite frame index.
        int targetFrameIndex = Mathf.RoundToInt(normalizedValue * (frames.Length - 1));

        // If your frames are ordered full to empty, reverse the index.
        if (!framesAreEmptyToFull)
        {
            targetFrameIndex = (frames.Length - 1) - targetFrameIndex;
        }

        // If this is the first time, just apply the frame immediately.
        if (currentFrameIndex == -1 || !animateChanges)
        {
            ApplyFrame(targetFrameIndex);
            return;
        }

        // Stop any previous animation so the bar does not fight itself.
        if (animationRoutine != null)
        {
            StopCoroutine(animationRoutine);
        }

        animationRoutine = StartCoroutine(AnimateToFrame(targetFrameIndex));
    }

    private IEnumerator AnimateToFrame(int targetFrameIndex)
    {
        int step = currentFrameIndex < targetFrameIndex ? 1 : -1;

        while (currentFrameIndex != targetFrameIndex)
        {
            ApplyFrame(currentFrameIndex + step);
            yield return new WaitForSeconds(frameDelay);
        }

        animationRoutine = null;
    }

    private void ApplyFrame(int frameIndex)
    {
        currentFrameIndex = Mathf.Clamp(frameIndex, 0, frames.Length - 1);
        barImage.sprite = frames[currentFrameIndex];
    }
}