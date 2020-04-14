using System.Collections;
using UnityEngine;

public class RubiksHorizontal : RubiksCubeRotation
{
    public void RotateHorizontalRight(float rotationDuration)
    {
        if (CurrentRotationState == RotationState.IDLE)
        {
            SetupForRotation();
            StartCoroutine(RotateHorizontal(RotationDirection.RIGHT, rotationDuration));
        }
    }

    public void RotateHorizontalLeft(float rotationDuration)
    {
        if (CurrentRotationState == RotationState.IDLE)
        {
            SetupForRotation();
            StartCoroutine(RotateHorizontal(RotationDirection.LEFT, rotationDuration));
        }
    }

    private IEnumerator RotateHorizontal(RotationDirection direction, float duration = 1.0f)
    {
        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation;

        to *= Quaternion.Euler(GetMultiplierForRotation(direction) * Vector3.up * NINETY_DEGREES);

        float elapsed = 0.0f;

        while (elapsed < duration)
        {
            transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.rotation = to;

        ResetAfterRotation(direction);
    }
}