using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FaceRotation { CLOCKWISE, COUNTERCLOCKWISE }

public class RubiksFaceRotation : MonoBehaviour
{
	public void Awake()
	{
		if (m_cubes == null)
		{
			m_cubes = GameObject.FindGameObjectsWithTag("Cube");
		}
	}

	public enum RubiksFace { NONE, UP, LEFT, RIGHT, BOTTOM, RIGHT_OPPOSITE, LEFT_OPPOSITE }

	public void Rotate(RubiksFace faceToRotate, FaceRotation direction, GameObject faceGO, float rotationDuration)
	{
		RubiksCubeRotation.SetRotationState(RubiksCubeRotation.RotationState.ROTATING);
		SetRubiksFaceParents(faceToRotate, faceGO);
		StartCoroutine(RotateFace(faceToRotate, direction, faceGO, rotationDuration));
	}

	public void SetHighlightMaterial(RubiksFace face, bool set, bool fromButton = false)
	{
		if (fromButton)
		{
			m_canResetHighlight = !set;
		}

		GameObject[] faceCubes = GetCubesForFace(m_cubes, face);

		foreach (GameObject cube in faceCubes)
		{
			Renderer cubeRenderer = cube.GetComponent<Renderer>();
			if (set)
			{
				if (cubeRenderer.sharedMaterial != m_highlightMaterial)
				{
					cubeRenderer.sharedMaterial = m_highlightMaterial;
				}
			}
			else
			{
				if (cubeRenderer.sharedMaterial != m_defaultMaterial)
				{
					cubeRenderer.sharedMaterial = m_defaultMaterial;
				}
			}
		}
	}

	private void SetRubiksFaceParents(RubiksFace faceToRotate, GameObject faceGO)
	{
		GameObject[] faceCubes = GetCubesForFace(m_cubes, faceToRotate);

		foreach (GameObject cube in faceCubes)
		{
			cube.transform.parent = faceGO.transform;
		}
	}

	private GameObject[] GetCubesForFace(GameObject[] cubes, RubiksFace face, float distance = 0.9f)
	{
		List<GameObject> faceCubes = new List<GameObject>();

		foreach (GameObject cube in m_cubes)
		{
			if (face == RubiksFace.UP && cube.transform.position.y > distance)
			{
				faceCubes.Add(cube);
			}
			else if (face == RubiksFace.BOTTOM && cube.transform.position.y < -distance)
			{
				faceCubes.Add(cube);
			}
			else if (face == RubiksFace.LEFT && cube.transform.position.x < -distance)
			{
				faceCubes.Add(cube);
			}
			else if (face == RubiksFace.LEFT_OPPOSITE && cube.transform.position.x > distance)
			{
				faceCubes.Add(cube);
			}
			else if (face == RubiksFace.RIGHT && cube.transform.position.z < -distance)
			{
				faceCubes.Add(cube);
			}
			else if (face == RubiksFace.RIGHT_OPPOSITE && cube.transform.position.z > distance)
			{
				faceCubes.Add(cube);
			}
		}

		return faceCubes.ToArray();
	}

	private void ResetRubiksFaceParents()
	{
		foreach (GameObject cube in m_cubes)
		{
			cube.transform.parent = GameObject.FindGameObjectWithTag("CubeContainer").transform;
		}
	}

	private void Update()
	{
		if (!RubiksCubeRotation.IsRotating && !m_rubiksCubeManager.IsApplyingSequence)
		{
			RubiksFace previousFace = m_faceToRotate;
			bool didRaycastHit = TryRaycast();

			if (m_canResetHighlight && (!didRaycastHit || previousFace != m_faceToRotate))
			{
				SetHighlightMaterial(previousFace, false);
			}

			if (didRaycastHit)
			{
				SetHighlightMaterial(m_faceToRotate, true);
			}

			if (Input.GetMouseButtonDown(0))
			{
				if (didRaycastHit)
				{
					Rotate(m_faceToRotate, FaceRotation.CLOCKWISE, m_faceGO, m_rotationSpeed);
					m_rubiksCubeManager.Logic.RotateFaceClockWise(m_faceColor);
				}
			}
			else if (Input.GetMouseButtonDown(1))
			{
				if (didRaycastHit)
				{
					Rotate(m_faceToRotate, FaceRotation.COUNTERCLOCKWISE, m_faceGO, m_rotationSpeed);
					m_rubiksCubeManager.Logic.RotateFaceCounterClockWise(m_faceColor);
				}
			}
		}
	}

	private bool TryRaycast()
	{
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit))
		{
			if (hit.transform.name == "UpFace")
			{
				m_faceToRotate = RubiksFace.UP;
				m_faceGO = GameObject.FindGameObjectWithTag("UP");
				m_faceColor = m_rubiksCubeManager.UpFaceColor;
				return true;
			}
			else if (hit.transform.name == "RightFace")
			{
				m_faceToRotate = RubiksFace.RIGHT;
				m_faceGO = GameObject.FindGameObjectWithTag("RIGHT");
				m_faceColor = m_rubiksCubeManager.RightFaceColor;
				return true;
			}
			else if (hit.transform.name == "LeftFace")
			{
				m_faceToRotate = RubiksFace.LEFT;
				m_faceGO = GameObject.FindGameObjectWithTag("LEFT");
				m_faceColor = m_rubiksCubeManager.LeftFaceColor;
				return true;
			}
		}
		return false;
	}

	private IEnumerator RotateFace(RubiksFace faceToRotate, FaceRotation direction, GameObject faceGO, float duration = 1.0f)
	{
		Quaternion from = faceGO.transform.localRotation;
		Quaternion to = faceGO.transform.localRotation;

		to *= Quaternion.Euler(GetMultiplierForRotation(faceToRotate, direction) * GetAxisForFace(faceToRotate) * NINETY_DEGREES);

		float elapsed = 0.0f;
		while (elapsed < duration)
		{
			faceGO.transform.rotation = Quaternion.Slerp(from, to, elapsed / duration);
			elapsed += Time.deltaTime;
			yield return null;
		}
		faceGO.transform.rotation = to;

		ResetRubiksFaceParents();
		RubiksCubeRotation.SetRotationState(RubiksCubeRotation.RotationState.IDLE);
	}

	private float GetMultiplierForRotation(RubiksFace faceToRotate, FaceRotation direction)
	{
		float multiplier = 1.0f;

		if (faceToRotate == RubiksFace.LEFT || faceToRotate == RubiksFace.RIGHT)
		{
			multiplier *= -1.0f;
		}

		switch (direction)
		{
			case FaceRotation.COUNTERCLOCKWISE:
				multiplier *= -1.0f;
				break;

			default: break;
		}

		return multiplier;
	}

	private Vector3 GetAxisForFace(RubiksFace faceToRotate)
	{
		switch (faceToRotate)
		{
			case RubiksFace.UP:
				return Vector3.up;

			case RubiksFace.LEFT:
				return Vector3.right;

			case RubiksFace.RIGHT:
				return Vector3.forward;

			default: break;
		}

		return new Vector3();
	}

	private GameObject[] m_cubes = null;
	private const float NINETY_DEGREES = 90.0f;

	private GameObject m_faceGO = null;
	private RubiksFace m_faceToRotate = RubiksFace.NONE;
	private RubiksColor m_faceColor = RubiksColor.NONE;
	private bool m_canResetHighlight = true;

	[SerializeField]
	protected RubiksCubeManager m_rubiksCubeManager = null;

	[SerializeField]
	private float m_rotationSpeed = 0.5f;

	[SerializeField]
	private Material m_defaultMaterial = null;

	[SerializeField]
	private Material m_highlightMaterial = null;
}