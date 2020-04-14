using System.Collections;
using UnityEngine;

public class RubiksVertical : RubiksCubeRotation
{
    public void RotateVerticalUp(float rotationDuration)
    {
        if (CurrentRotationState == RotationState.IDLE)
        {
            SetupForRotation();
            StartCoroutine(RotateVertical(RotationDirection.UP, rotationDuration));
        }
    }

    public void RotateVerticalDown(float rotationDuration)
    {
        if (CurrentRotationState == RotationState.IDLE)
        {
            SetupForRotation();
            StartCoroutine(RotateVertical(RotationDirection.DOWN, rotationDuration));
        }
    }

    private IEnumerator RotateVertical(RotationDirection direction, float duration = 1.0f)
    {
        Quaternion from = transform.rotation;
        Quaternion to = transform.rotation;

        to *= Quaternion.Euler(GetMultiplierForRotation(direction) * Vector3.forward * NINETY_DEGREES);

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