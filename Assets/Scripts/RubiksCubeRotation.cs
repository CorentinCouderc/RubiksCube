using UnityEngine;

public class RubiksCubeRotation : MonoBehaviour
{
	public enum RotationDirection { LEFT, RIGHT, UP, DOWN }

	public enum RotationState { IDLE, ROTATING }

	public static RotationState CurrentRotationState { get; set; } = RotationState.IDLE;

	public static bool IsRotating { get { return CurrentRotationState == RotationState.ROTATING; } }

	#region RotationState

	protected void SetupForRotation()
	{
		SetRotationState(RotationState.ROTATING);
		SetRubiksCubeParents();
	}

	protected void ResetAfterRotation(RotationDirection direction)
	{
		ChangeFacesOnRotation(direction);
		ResetRubiksCubeParents();
		SetRotationState(RotationState.IDLE);
	}

	protected void SetRubiksCubeParents()
	{
		if (m_cubes == null)
		{
			m_cubes = GameObject.FindGameObjectsWithTag("Cube");
		}

		foreach (GameObject cube in m_cubes)
		{
			cube.transform.parent = transform;
		}
	}

	protected void ResetRubiksCubeParents()
	{
		foreach (GameObject cube in m_cubes)
		{
			cube.transform.parent = GameObject.FindGameObjectWithTag("CubeContainer").transform;
		}
	}

	public static void SetRotationState(RotationState next)
	{
		RotationState previous = CurrentRotationState;
		if (next == previous)
			return;

		switch (previous)
		{
			case RotationState.ROTATING:
				break;

			default: break;
		}

		CurrentRotationState = next;

		switch (next)
		{
			case RotationState.ROTATING:
				break;

			default: break;
		}
	}

	#endregion RotationState

	protected float GetMultiplierForRotation(RotationDirection direction)
	{
		float multiplier = 0.0f;
		switch (direction)
		{
			case RotationDirection.UP:
				multiplier = -1.0f;
				break;

			case RotationDirection.DOWN:
				multiplier = 1.0f;
				break;

			case RotationDirection.RIGHT:
				multiplier = -1.0f;
				break;

			case RotationDirection.LEFT:
				multiplier = 1.0f;
				break;

			default: break;
		}

		return multiplier;
	}

	protected void ChangeFacesOnRotation(RotationDirection direction)
	{
		RubiksColor currentLeftFaceColor = m_rubiksCubeManager.LeftFaceColor;
		RubiksColor currentRightFaceColor = m_rubiksCubeManager.RightFaceColor;
		RubiksColor currentUpFaceColor = m_rubiksCubeManager.UpFaceColor;
		RubiksColor currentOppositeLeftFaceColor = m_rubiksCubeManager.Logic.RubiksOppositeColor[currentLeftFaceColor];
		RubiksColor currentOppositeRightFaceColor = m_rubiksCubeManager.Logic.RubiksOppositeColor[currentRightFaceColor];
		RubiksColor currentOppositeUpFaceColor = m_rubiksCubeManager.Logic.RubiksOppositeColor[currentUpFaceColor];

		RubiksColor nextLeftFaceColor = RubiksColor.NONE;
		RubiksColor nextRightFaceColor = RubiksColor.NONE;
		RubiksColor nextUpFaceColor = RubiksColor.NONE;

		switch (direction)
		{
			case RotationDirection.UP:
				nextLeftFaceColor = currentOppositeUpFaceColor;
				nextRightFaceColor = currentRightFaceColor;
				nextUpFaceColor = currentLeftFaceColor;
				break;

			case RotationDirection.DOWN:
				nextLeftFaceColor = currentUpFaceColor;
				nextRightFaceColor = currentRightFaceColor;
				nextUpFaceColor = currentOppositeLeftFaceColor;
				break;

			case RotationDirection.RIGHT:
				nextLeftFaceColor = currentOppositeRightFaceColor;
				nextRightFaceColor = currentLeftFaceColor;
				nextUpFaceColor = currentUpFaceColor;
				break;

			case RotationDirection.LEFT:
				nextLeftFaceColor = currentRightFaceColor;
				nextRightFaceColor = currentOppositeLeftFaceColor;
				nextUpFaceColor = currentUpFaceColor;
				break;

			default: break;
		}

		m_rubiksCubeManager.LeftFaceColor = nextLeftFaceColor;
		m_rubiksCubeManager.RightFaceColor = nextRightFaceColor;
		m_rubiksCubeManager.UpFaceColor = nextUpFaceColor;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
		Debug.Log($"Transitioning: Left color {currentLeftFaceColor} -> {nextLeftFaceColor}");
		Debug.Log($"Transitioning: Right color {currentRightFaceColor} -> {nextRightFaceColor}");
		Debug.Log($"Transitioning: Up color {currentUpFaceColor} -> {nextUpFaceColor}");
#endif
	}

	protected GameObject[] m_cubes = null;
	protected const float NINETY_DEGREES = 90.0f;

	[SerializeField]
	protected RubiksCubeManager m_rubiksCubeManager = null;
}