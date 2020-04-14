using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RubiksCubeManager : MonoBehaviour
{
	public RubiksLogic Logic { get { return m_rubiksLogic; } }
	public RubiksColor LeftFaceColor { get; set; }
	public RubiksColor RightFaceColor { get; set; }
	public RubiksColor UpFaceColor { get; set; }
	public bool IsApplyingSequence { get { return m_isApplyingSequence; } }

	public static string[] FaceColors = new string[] { "W", "Y", "B", "G", "R", "O" };
	public static string[] FaceDirections = new string[] { "C", "A" };

	public void ShuffleRubiksCube()
	{
		string sequence = GenerateShuffleSequence();
		ApplySequence(sequence);
	}

	public void ApplySequence(string sequence)
	{
		if (!m_isApplyingSequence)
		{
			m_isApplyingSequence = true;
			ParseSequence(sequence);
			StartCoroutine(ApplySequence(m_colors, m_directions));
		}
	}

	public void Retry()
	{
		m_victoryPopup.SetActive(false);
		ShuffleRubiksCube();
		m_startTime = DateTime.UtcNow;
	}

	#region Private

	private void Awake()
	{
		LeftFaceColor = RubiksColor.BLUE;
		RightFaceColor = RubiksColor.WHITE;
		UpFaceColor = RubiksColor.RED;
		m_rubiksLogic = new RubiksLogic();

		ShuffleRubiksCube();
		m_startTime = DateTime.UtcNow;

#if UNITY_EDITOR || DEVELOPMENT_BUILD
		Debug.Log(m_rubiksLogic.ToString());
#endif
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}

		if ((!m_isApplyingSequence && m_rubiksLogic.IsSolved))
		{
			// Solved
			DateTime now = DateTime.UtcNow;
			TimeSpan timeSpan = now - m_startTime;
			if (timeSpan.Hours > 0)
			{
				m_timeText.text = $"{TEXT_TIME} : {timeSpan.Hours}:{timeSpan.Minutes}:{timeSpan.Seconds}";
			}
			else
			{
				m_timeText.text = $"{TEXT_TIME} : {timeSpan.Minutes}:{timeSpan.Seconds}.{timeSpan.Milliseconds}";
			}
			m_victoryPopup.SetActive(true);
		}
	}

	private string GenerateShuffleSequence()
	{
		string outputSequence = "";
		int limit = s_rand.Next(10, 15);
		for (int i = 0; i < limit; i++)
		{
			int colorIndex = s_rand.Next(FaceColors.Length);
			int directionIndex = s_rand.Next(FaceDirections.Length);

			outputSequence += $"{FaceColors[colorIndex]}{FaceDirections[directionIndex]}_";
		}

		return outputSequence;
	}

	public void ParseSequence(string actionSequence)
	{
		m_colors.Clear();
		m_directions.Clear();
		string[] sequenceElements = actionSequence.Split('_');

		for (int i = 0; i < sequenceElements.Length; i++)
		{
			if (sequenceElements[i].Length == 0)
				continue;

			RubiksColor faceColor = RubiksColor.NONE;
			switch (sequenceElements[i][0])
			{
				case 'W':
					faceColor = RubiksColor.WHITE;
					break;

				case 'Y':
					faceColor = RubiksColor.YELLOW;
					break;

				case 'B':
					faceColor = RubiksColor.BLUE;
					break;

				case 'G':
					faceColor = RubiksColor.GREEN;
					break;

				case 'R':
					faceColor = RubiksColor.RED;
					break;

				case 'O':
					faceColor = RubiksColor.ORANGE;
					break;

				default: break;
			}

			FaceRotation direction = sequenceElements[i][1] == 'C' ? FaceRotation.CLOCKWISE : FaceRotation.COUNTERCLOCKWISE;

			m_colors.Add(faceColor);
			m_directions.Add(direction);
		}
	}

	private IEnumerator ApplySequence(List<RubiksColor> colors, List<FaceRotation> directions)
	{
		for (int i = 0; i < colors.Count; i++)
		{
			yield return new WaitUntil(() => RubiksCubeRotation.CurrentRotationState == RubiksCubeRotation.RotationState.IDLE);

			RubiksFaceRotation.RubiksFace faceToRotate = RubiksFaceRotation.RubiksFace.NONE;
			GameObject faceGO = null;

			if (colors[i] == UpFaceColor)
			{
				faceToRotate = RubiksFaceRotation.RubiksFace.UP;
				faceGO = GameObject.FindGameObjectWithTag("UP");
			}
			else if (colors[i] == LeftFaceColor)
			{
				faceToRotate = RubiksFaceRotation.RubiksFace.LEFT;
				faceGO = GameObject.FindGameObjectWithTag("LEFT");
			}
			else if (colors[i] == RightFaceColor)
			{
				faceToRotate = RubiksFaceRotation.RubiksFace.RIGHT;
				faceGO = GameObject.FindGameObjectWithTag("RIGHT");
			}
			else if (colors[i] == m_rubiksLogic.RubiksOppositeColor[UpFaceColor])
			{
				m_rubiksVertical.RotateVerticalUp(m_shuffleRotationDuration);

				faceToRotate = RubiksFaceRotation.RubiksFace.LEFT;
				faceGO = GameObject.FindGameObjectWithTag("LEFT");
			}
			else if (colors[i] == m_rubiksLogic.RubiksOppositeColor[LeftFaceColor])
			{
				m_rubiksHorizontal.RotateHorizontalLeft(m_shuffleRotationDuration);

				faceToRotate = RubiksFaceRotation.RubiksFace.RIGHT;
				faceGO = GameObject.FindGameObjectWithTag("RIGHT");
			}
			else if (colors[i] == m_rubiksLogic.RubiksOppositeColor[RightFaceColor])
			{
				m_rubiksHorizontal.RotateHorizontalRight(m_shuffleRotationDuration);

				faceToRotate = RubiksFaceRotation.RubiksFace.LEFT;
				faceGO = GameObject.FindGameObjectWithTag("LEFT");
			}

			yield return new WaitUntil(() => RubiksCubeRotation.CurrentRotationState == RubiksCubeRotation.RotationState.IDLE);

			if (faceToRotate != RubiksFaceRotation.RubiksFace.NONE && faceGO != null)
			{
				Rotate(faceToRotate, colors[i], faceGO, directions[i]);
			}
		}

		yield return new WaitUntil(() => RubiksCubeRotation.CurrentRotationState == RubiksCubeRotation.RotationState.IDLE);
		m_isApplyingSequence = false;
	}

	private void Rotate(RubiksFaceRotation.RubiksFace faceToRotate, RubiksColor faceColor, GameObject faceGO, FaceRotation direction)
	{
		m_rubiksFaceRotation.Rotate(faceToRotate, direction, faceGO, m_shuffleRotationDuration);
		if (direction == FaceRotation.CLOCKWISE)
		{
			m_rubiksLogic.RotateFaceClockWise(faceColor);
		}
		else
		{
			m_rubiksLogic.RotateFaceCounterClockWise(faceColor);
		}
	}

	private System.Random s_rand = new System.Random();

	private RubiksLogic m_rubiksLogic = null;

	private List<RubiksColor> m_colors = new List<RubiksColor>();
	private List<FaceRotation> m_directions = new List<FaceRotation>();

	private bool m_isApplyingSequence = false;
	private DateTime m_startTime = new DateTime();
	private const string TEXT_TIME = "Rubiks solved in";

	[SerializeField]
	private RubiksFaceRotation m_rubiksFaceRotation = null;

	[SerializeField]
	private RubiksHorizontal m_rubiksHorizontal = null;

	[SerializeField]
	private RubiksVertical m_rubiksVertical = null;

	[SerializeField]
	private float m_shuffleRotationDuration = 0.2f;

	[SerializeField]
	private GameObject m_victoryPopup = null;

	[SerializeField]
	private TextMeshProUGUI m_timeText = null;

	#endregion Private
}