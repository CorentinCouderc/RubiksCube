using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum RubiksColor
{
	NONE,
	GREEN,
	BLUE,
	YELLOW,
	WHITE,
	RED,
	ORANGE
}

public enum RubiksPosition
{
	NONE,
	MIDDLE,
	TOP,
	RIGHT,
	LEFT,
	BOTTOM,
	TOPRIGHT,
	TOPLEFT,
	BOTTOMRIGHT,
	BOTTOMLEFT
}

public class RubiksFace
{
	public RubiksColor Color { get { return m_color; } }
	public Dictionary<RubiksPosition, RubiksColor> Face { get { return m_face; } }
	public RubiksColor LinkedColorTop { get { return m_linkedColorTop; } }
	public RubiksColor LinkedColorRight { get { return m_linkedColorRight; } }
	public RubiksColor LinkedColorLeft { get { return m_linkedColorLeft; } }
	public RubiksColor LinkedColorBottom { get { return m_linkedColorBottom; } }

	public RubiksFace(RubiksColor faceColor)
	{
		m_color = faceColor;
		SetSidesLinkedColor(faceColor);

		for (int i = 0; i < CUBE_HEIGHT; i++)
		{
			for (int j = 0; j < CUBE_WIDTH; j++)
			{
				m_face[GetPositionFromCoordinates(i, j)] = faceColor;
			}
		}
	}

	public void RotateFaceClockWise()
	{
		RubiksColor tempTopLeftColor = m_face[RubiksPosition.TOPLEFT];
		RubiksColor tempLeftColor = m_face[RubiksPosition.LEFT];
		m_face[RubiksPosition.TOPLEFT] = m_face[RubiksPosition.BOTTOMLEFT];
		m_face[RubiksPosition.BOTTOMLEFT] = m_face[RubiksPosition.BOTTOMRIGHT];
		m_face[RubiksPosition.BOTTOMRIGHT] = m_face[RubiksPosition.TOPRIGHT];
		m_face[RubiksPosition.TOPRIGHT] = tempTopLeftColor;
		m_face[RubiksPosition.LEFT] = m_face[RubiksPosition.BOTTOM];
		m_face[RubiksPosition.BOTTOM] = m_face[RubiksPosition.RIGHT];
		m_face[RubiksPosition.RIGHT] = m_face[RubiksPosition.TOP];
		m_face[RubiksPosition.TOP] = tempLeftColor;
	}

	public void RotateFaceCounterClockWise()
	{
		RubiksColor tempTopLeftColor = m_face[RubiksPosition.TOPLEFT];
		RubiksColor tempLeftColor = m_face[RubiksPosition.LEFT];
		m_face[RubiksPosition.TOPLEFT] = m_face[RubiksPosition.TOPRIGHT];
		m_face[RubiksPosition.TOPRIGHT] = m_face[RubiksPosition.BOTTOMRIGHT];
		m_face[RubiksPosition.BOTTOMRIGHT] = m_face[RubiksPosition.BOTTOMLEFT];
		m_face[RubiksPosition.BOTTOMLEFT] = tempTopLeftColor;
		m_face[RubiksPosition.LEFT] = m_face[RubiksPosition.TOP];
		m_face[RubiksPosition.TOP] = m_face[RubiksPosition.RIGHT];
		m_face[RubiksPosition.RIGHT] = m_face[RubiksPosition.BOTTOM];
		m_face[RubiksPosition.BOTTOM] = tempLeftColor;
	}

	public RubiksColor GetColor(RubiksPosition position)
	{
		return m_face[position];
	}

	public RubiksPosition GetPositionFromCoordinates(int verticalIndex, int horizontalIndex)
	{
#if UNITY_EDITOR || DEVELOPMENT_BUILD
		Debug.Assert(verticalIndex < CUBE_HEIGHT && horizontalIndex < CUBE_WIDTH, "Invalid indexers (out of cube size)");
#endif

		if (verticalIndex == 0)
		{
			if (horizontalIndex == 0)
			{
				return RubiksPosition.TOPLEFT;
			}
			else if (horizontalIndex == 1)
			{
				return RubiksPosition.TOP;
			}
			else
			{
				return RubiksPosition.TOPRIGHT;
			}
		}
		else if (verticalIndex == 1)
		{
			if (horizontalIndex == 0)
			{
				return RubiksPosition.LEFT;
			}
			else if (horizontalIndex == 1)
			{
				return RubiksPosition.MIDDLE;
			}
			else
			{
				return RubiksPosition.RIGHT;
			}
		}
		else
		{
			if (horizontalIndex == 0)
			{
				return RubiksPosition.BOTTOMLEFT;
			}
			else if (horizontalIndex == 1)
			{
				return RubiksPosition.BOTTOM;
			}
			else
			{
				return RubiksPosition.BOTTOMRIGHT;
			}
		}
	}

	private void SetSidesLinkedColor(RubiksColor faceColor)
	{
		switch (faceColor)
		{
			case RubiksColor.GREEN:
				m_linkedColorTop = RubiksColor.ORANGE;
				m_linkedColorRight = RubiksColor.WHITE;
				m_linkedColorLeft = RubiksColor.YELLOW;
				m_linkedColorBottom = RubiksColor.RED;
				break;

			case RubiksColor.BLUE:
				m_linkedColorTop = RubiksColor.ORANGE;
				m_linkedColorRight = RubiksColor.YELLOW;
				m_linkedColorLeft = RubiksColor.WHITE;
				m_linkedColorBottom = RubiksColor.RED;
				break;

			case RubiksColor.ORANGE:
				m_linkedColorTop = RubiksColor.YELLOW;
				m_linkedColorRight = RubiksColor.BLUE;
				m_linkedColorLeft = RubiksColor.GREEN;
				m_linkedColorBottom = RubiksColor.WHITE;
				break;

			case RubiksColor.RED:
				m_linkedColorTop = RubiksColor.WHITE;
				m_linkedColorRight = RubiksColor.BLUE;
				m_linkedColorLeft = RubiksColor.GREEN;
				m_linkedColorBottom = RubiksColor.YELLOW;
				break;

			case RubiksColor.WHITE:
				m_linkedColorTop = RubiksColor.ORANGE;
				m_linkedColorRight = RubiksColor.BLUE;
				m_linkedColorLeft = RubiksColor.GREEN;
				m_linkedColorBottom = RubiksColor.RED;
				break;

			case RubiksColor.YELLOW:
				m_linkedColorTop = RubiksColor.ORANGE;
				m_linkedColorRight = RubiksColor.GREEN;
				m_linkedColorLeft = RubiksColor.BLUE;
				m_linkedColorBottom = RubiksColor.RED;
				break;

			default:
				Debug.LogError("Should not access default of switch");
				break;
		}
	}

	public bool CheckFace(RubiksColor faceColor)
	{
		foreach (RubiksColor color in Face.Values)
		{
			if (color != faceColor)
			{
				return false;
			}
		}
		return true;
	}

	private const int CUBE_HEIGHT = 3;
	private const int CUBE_WIDTH = 3;
	private Dictionary<RubiksPosition, RubiksColor> m_face = new Dictionary<RubiksPosition, RubiksColor>();
	private RubiksColor m_color = RubiksColor.NONE;
	private RubiksColor m_linkedColorTop = RubiksColor.NONE;
	private RubiksColor m_linkedColorRight = RubiksColor.NONE;
	private RubiksColor m_linkedColorLeft = RubiksColor.NONE;
	private RubiksColor m_linkedColorBottom = RubiksColor.NONE;
}

public class RubiksLogic
{
	public Dictionary<RubiksColor, RubiksFace> RubiksData { get { return m_rubiksData; } }
	public Dictionary<RubiksColor, RubiksColor> RubiksOppositeColor { get { return m_rubiksOppositeColor; } }

	public bool IsSolved
	{
		get
		{
			bool isSolved = true;
			foreach (RubiksColor faceColor in Enum.GetValues(typeof(RubiksColor)))
			{
				if (faceColor != RubiksColor.NONE)
				{
					isSolved &= RubiksData[faceColor].CheckFace(faceColor);
				}
			}
			return isSolved;
		}
	}

	public RubiksLogic()
	{
		foreach (RubiksColor color in Enum.GetValues(typeof(RubiksColor)))
		{
			if (color == RubiksColor.NONE)
				continue;

			FillRubiksOppositeColor(color);
			m_rubiksData[color] = new RubiksFace(color);
		}
	}

	private void FillRubiksOppositeColor(RubiksColor color)
	{
		RubiksColor oppositeColor = RubiksColor.NONE;
		switch (color)
		{
			case RubiksColor.BLUE:
				oppositeColor = RubiksColor.GREEN;
				break;

			case RubiksColor.GREEN:
				oppositeColor = RubiksColor.BLUE;
				break;

			case RubiksColor.YELLOW:
				oppositeColor = RubiksColor.WHITE;
				break;

			case RubiksColor.WHITE:
				oppositeColor = RubiksColor.YELLOW;
				break;

			case RubiksColor.RED:
				oppositeColor = RubiksColor.ORANGE;
				break;

			case RubiksColor.ORANGE:
				oppositeColor = RubiksColor.RED;
				break;

			default: break;
		}

		m_rubiksOppositeColor[color] = oppositeColor;
	}

	#region Rotation

	public void RotateFaceClockWise(RubiksColor faceColor)
	{
		RubiksFace toRotate = m_rubiksData[faceColor];

		toRotate.RotateFaceClockWise();
		RotateSidesClockWise(toRotate);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
		Debug.Log(this.ToString());
#endif
	}

	public void RotateFaceCounterClockWise(RubiksColor faceColor)
	{
		RubiksFace toRotate = m_rubiksData[faceColor];

		toRotate.RotateFaceCounterClockWise();
		RotateSidesCounterClockWise(toRotate);

#if UNITY_EDITOR || DEVELOPMENT_BUILD
		Debug.Log(this.ToString());
#endif
	}

	private void RotateSidesClockWise(RubiksFace toRotate)
	{
		RubiksFace topLinkedFace = m_rubiksData[toRotate.LinkedColorTop];
		RubiksFace rightLinkedFace = m_rubiksData[toRotate.LinkedColorRight];
		RubiksFace leftLinkedFace = m_rubiksData[toRotate.LinkedColorLeft];
		RubiksFace bottomLinkedFace = m_rubiksData[toRotate.LinkedColorBottom];

		RubiksPosition[] topLinkedPositions = GetTopLinkedPositions(toRotate.Color);
		RubiksPosition[] leftLinkedPositions = GetLeftLinkedPositions(toRotate.Color);
		RubiksPosition[] bottomLinkedPositions = GetBottomLinkedPositions(toRotate.Color);
		RubiksPosition[] rightLinkedPositions = GetRightLinkedPositions(toRotate.Color);

		RubiksColor tempTopLinkedFirstColor = topLinkedFace.GetColor(topLinkedPositions[0]);
		RubiksColor tempTopLinkedSecondColor = topLinkedFace.GetColor(topLinkedPositions[1]);
		RubiksColor tempTopLinkedThirdColor = topLinkedFace.GetColor(topLinkedPositions[2]);

		topLinkedFace.Face[topLinkedPositions[0]] = leftLinkedFace.GetColor(leftLinkedPositions[0]);
		topLinkedFace.Face[topLinkedPositions[1]] = leftLinkedFace.GetColor(leftLinkedPositions[1]);
		topLinkedFace.Face[topLinkedPositions[2]] = leftLinkedFace.GetColor(leftLinkedPositions[2]);

		leftLinkedFace.Face[leftLinkedPositions[0]] = bottomLinkedFace.GetColor(bottomLinkedPositions[0]);
		leftLinkedFace.Face[leftLinkedPositions[1]] = bottomLinkedFace.GetColor(bottomLinkedPositions[1]);
		leftLinkedFace.Face[leftLinkedPositions[2]] = bottomLinkedFace.GetColor(bottomLinkedPositions[2]);

		bottomLinkedFace.Face[bottomLinkedPositions[0]] = rightLinkedFace.GetColor(rightLinkedPositions[0]);
		bottomLinkedFace.Face[bottomLinkedPositions[1]] = rightLinkedFace.GetColor(rightLinkedPositions[1]);
		bottomLinkedFace.Face[bottomLinkedPositions[2]] = rightLinkedFace.GetColor(rightLinkedPositions[2]);

		rightLinkedFace.Face[rightLinkedPositions[0]] = tempTopLinkedFirstColor;
		rightLinkedFace.Face[rightLinkedPositions[1]] = tempTopLinkedSecondColor;
		rightLinkedFace.Face[rightLinkedPositions[2]] = tempTopLinkedThirdColor;
	}

	private void RotateSidesCounterClockWise(RubiksFace toRotate)
	{
		RubiksFace topLinkedFace = m_rubiksData[toRotate.LinkedColorTop];
		RubiksFace rightLinkedFace = m_rubiksData[toRotate.LinkedColorRight];
		RubiksFace leftLinkedFace = m_rubiksData[toRotate.LinkedColorLeft];
		RubiksFace bottomLinkedFace = m_rubiksData[toRotate.LinkedColorBottom];

		RubiksPosition[] topLinkedPositions = GetTopLinkedPositions(toRotate.Color);
		RubiksPosition[] leftLinkedPositions = GetLeftLinkedPositions(toRotate.Color);
		RubiksPosition[] bottomLinkedPositions = GetBottomLinkedPositions(toRotate.Color);
		RubiksPosition[] rightLinkedPositions = GetRightLinkedPositions(toRotate.Color);

		RubiksColor tempTopLinkedFirstColor = topLinkedFace.GetColor(topLinkedPositions[0]);
		RubiksColor tempTopLinkedSecondColor = topLinkedFace.GetColor(topLinkedPositions[1]);
		RubiksColor tempTopLinkedThirdColor = topLinkedFace.GetColor(topLinkedPositions[2]);

		topLinkedFace.Face[topLinkedPositions[0]] = rightLinkedFace.GetColor(rightLinkedPositions[0]);
		topLinkedFace.Face[topLinkedPositions[1]] = rightLinkedFace.GetColor(rightLinkedPositions[1]);
		topLinkedFace.Face[topLinkedPositions[2]] = rightLinkedFace.GetColor(rightLinkedPositions[2]);

		rightLinkedFace.Face[rightLinkedPositions[0]] = bottomLinkedFace.GetColor(bottomLinkedPositions[0]);
		rightLinkedFace.Face[rightLinkedPositions[1]] = bottomLinkedFace.GetColor(bottomLinkedPositions[1]);
		rightLinkedFace.Face[rightLinkedPositions[2]] = bottomLinkedFace.GetColor(bottomLinkedPositions[2]);

		bottomLinkedFace.Face[bottomLinkedPositions[0]] = leftLinkedFace.GetColor(leftLinkedPositions[0]);
		bottomLinkedFace.Face[bottomLinkedPositions[1]] = leftLinkedFace.GetColor(leftLinkedPositions[1]);
		bottomLinkedFace.Face[bottomLinkedPositions[2]] = leftLinkedFace.GetColor(leftLinkedPositions[2]);

		leftLinkedFace.Face[leftLinkedPositions[0]] = tempTopLinkedFirstColor;
		leftLinkedFace.Face[leftLinkedPositions[1]] = tempTopLinkedSecondColor;
		leftLinkedFace.Face[leftLinkedPositions[2]] = tempTopLinkedThirdColor;
	}

	private RubiksPosition[] GetTopLinkedPositions(RubiksColor color)
	{
		RubiksPosition[] topLinkedCoords = new RubiksPosition[3];
		switch (color)
		{
			case RubiksColor.WHITE:
				topLinkedCoords[0] = RubiksPosition.BOTTOMLEFT;
				topLinkedCoords[1] = RubiksPosition.BOTTOM;
				topLinkedCoords[2] = RubiksPosition.BOTTOMRIGHT;
				break;

			case RubiksColor.BLUE:
				topLinkedCoords[0] = RubiksPosition.BOTTOMRIGHT;
				topLinkedCoords[1] = RubiksPosition.RIGHT;
				topLinkedCoords[2] = RubiksPosition.TOPRIGHT;
				break;

			case RubiksColor.YELLOW:
				topLinkedCoords[0] = RubiksPosition.TOPRIGHT;
				topLinkedCoords[1] = RubiksPosition.TOP;
				topLinkedCoords[2] = RubiksPosition.TOPLEFT;
				break;

			case RubiksColor.GREEN:
				topLinkedCoords[0] = RubiksPosition.TOPLEFT;
				topLinkedCoords[1] = RubiksPosition.LEFT;
				topLinkedCoords[2] = RubiksPosition.BOTTOMLEFT;
				break;

			case RubiksColor.ORANGE:
				topLinkedCoords[0] = RubiksPosition.TOPRIGHT;
				topLinkedCoords[1] = RubiksPosition.TOP;
				topLinkedCoords[2] = RubiksPosition.TOPLEFT;
				break;

			case RubiksColor.RED:
				topLinkedCoords[0] = RubiksPosition.BOTTOMLEFT;
				topLinkedCoords[1] = RubiksPosition.BOTTOM;
				topLinkedCoords[2] = RubiksPosition.BOTTOMRIGHT;
				break;

			default:
				Debug.LogError("Should not be here");
				break;
		}

		return topLinkedCoords;
	}

	private RubiksPosition[] GetLeftLinkedPositions(RubiksColor color)
	{
		RubiksPosition[] leftLinkedCoords = new RubiksPosition[3];
		switch (color)
		{
			case RubiksColor.WHITE:
				leftLinkedCoords[0] = RubiksPosition.BOTTOMRIGHT;
				leftLinkedCoords[1] = RubiksPosition.RIGHT;
				leftLinkedCoords[2] = RubiksPosition.TOPRIGHT;
				break;

			case RubiksColor.BLUE:
				leftLinkedCoords[0] = RubiksPosition.BOTTOMRIGHT;
				leftLinkedCoords[1] = RubiksPosition.RIGHT;
				leftLinkedCoords[2] = RubiksPosition.TOPRIGHT;
				break;

			case RubiksColor.YELLOW:
				leftLinkedCoords[0] = RubiksPosition.BOTTOMRIGHT;
				leftLinkedCoords[1] = RubiksPosition.RIGHT;
				leftLinkedCoords[2] = RubiksPosition.TOPRIGHT;
				break;

			case RubiksColor.GREEN:
				leftLinkedCoords[0] = RubiksPosition.BOTTOMRIGHT;
				leftLinkedCoords[1] = RubiksPosition.RIGHT;
				leftLinkedCoords[2] = RubiksPosition.TOPRIGHT;
				break;

			case RubiksColor.ORANGE:
				leftLinkedCoords[0] = RubiksPosition.TOPRIGHT;
				leftLinkedCoords[1] = RubiksPosition.TOP;
				leftLinkedCoords[2] = RubiksPosition.TOPLEFT;
				break;

			case RubiksColor.RED:
				leftLinkedCoords[0] = RubiksPosition.BOTTOMLEFT;
				leftLinkedCoords[1] = RubiksPosition.BOTTOM;
				leftLinkedCoords[2] = RubiksPosition.BOTTOMRIGHT;
				break;

			default:
				Debug.LogError("Should not be here");
				break;
		}

		return leftLinkedCoords;
	}

	private RubiksPosition[] GetBottomLinkedPositions(RubiksColor color)
	{
		RubiksPosition[] bottomLinkedCoords = new RubiksPosition[3];
		switch (color)
		{
			case RubiksColor.WHITE:
				bottomLinkedCoords[0] = RubiksPosition.TOPRIGHT;
				bottomLinkedCoords[1] = RubiksPosition.TOP;
				bottomLinkedCoords[2] = RubiksPosition.TOPLEFT;
				break;

			case RubiksColor.BLUE:
				bottomLinkedCoords[0] = RubiksPosition.BOTTOMRIGHT;
				bottomLinkedCoords[1] = RubiksPosition.RIGHT;
				bottomLinkedCoords[2] = RubiksPosition.TOPRIGHT;
				break;

			case RubiksColor.YELLOW:
				bottomLinkedCoords[0] = RubiksPosition.BOTTOMLEFT;
				bottomLinkedCoords[1] = RubiksPosition.BOTTOM;
				bottomLinkedCoords[2] = RubiksPosition.BOTTOMRIGHT;
				break;

			case RubiksColor.GREEN:
				bottomLinkedCoords[0] = RubiksPosition.TOPLEFT;
				bottomLinkedCoords[1] = RubiksPosition.LEFT;
				bottomLinkedCoords[2] = RubiksPosition.BOTTOMLEFT;
				break;

			case RubiksColor.ORANGE:
				bottomLinkedCoords[0] = RubiksPosition.TOPRIGHT;
				bottomLinkedCoords[1] = RubiksPosition.TOP;
				bottomLinkedCoords[2] = RubiksPosition.TOPLEFT;
				break;

			case RubiksColor.RED:
				bottomLinkedCoords[0] = RubiksPosition.BOTTOMLEFT;
				bottomLinkedCoords[1] = RubiksPosition.BOTTOM;
				bottomLinkedCoords[2] = RubiksPosition.BOTTOMRIGHT;
				break;

			default:
				Debug.LogError("Should not be here");
				break;
		}

		return bottomLinkedCoords;
	}

	private RubiksPosition[] GetRightLinkedPositions(RubiksColor color)
	{
		RubiksPosition[] rightLinkedCoords = new RubiksPosition[3];
		switch (color)
		{
			case RubiksColor.WHITE:
				rightLinkedCoords[0] = RubiksPosition.TOPLEFT;
				rightLinkedCoords[1] = RubiksPosition.LEFT;
				rightLinkedCoords[2] = RubiksPosition.BOTTOMLEFT;
				break;

			case RubiksColor.BLUE:
				rightLinkedCoords[0] = RubiksPosition.TOPLEFT;
				rightLinkedCoords[1] = RubiksPosition.LEFT;
				rightLinkedCoords[2] = RubiksPosition.BOTTOMLEFT;
				break;

			case RubiksColor.YELLOW:
				rightLinkedCoords[0] = RubiksPosition.TOPLEFT;
				rightLinkedCoords[1] = RubiksPosition.LEFT;
				rightLinkedCoords[2] = RubiksPosition.BOTTOMLEFT;
				break;

			case RubiksColor.GREEN:
				rightLinkedCoords[0] = RubiksPosition.TOPLEFT;
				rightLinkedCoords[1] = RubiksPosition.LEFT;
				rightLinkedCoords[2] = RubiksPosition.BOTTOMLEFT;
				break;

			case RubiksColor.ORANGE:
				rightLinkedCoords[0] = RubiksPosition.TOPRIGHT;
				rightLinkedCoords[1] = RubiksPosition.TOP;
				rightLinkedCoords[2] = RubiksPosition.TOPLEFT;
				break;

			case RubiksColor.RED:
				rightLinkedCoords[0] = RubiksPosition.BOTTOMLEFT;
				rightLinkedCoords[1] = RubiksPosition.BOTTOM;
				rightLinkedCoords[2] = RubiksPosition.BOTTOMRIGHT;
				break;

			default:
				Debug.LogError("Should not be here");
				break;
		}

		return rightLinkedCoords;
	}

	#endregion Rotation

	#region Print

	private string PrintColor(RubiksColor color)
	{
		return Enum.GetName(typeof(RubiksColor), color)[0].ToString();
	}

	public override string ToString()
	{
		StringBuilder sb = new StringBuilder();
		sb.AppendLine($"_ |_ |_|{PrintColor(m_rubiksData[RubiksColor.ORANGE].Face[RubiksPosition.TOPLEFT])}|{PrintColor(m_rubiksData[RubiksColor.ORANGE].Face[RubiksPosition.TOP])}|{PrintColor(m_rubiksData[RubiksColor.ORANGE].Face[RubiksPosition.TOPRIGHT])}|_|_|_|_|_|_");
		sb.AppendLine($"_ |_ |_|{PrintColor(m_rubiksData[RubiksColor.ORANGE].Face[RubiksPosition.LEFT])}|{PrintColor(m_rubiksData[RubiksColor.ORANGE].Face[RubiksPosition.MIDDLE])}|{PrintColor(m_rubiksData[RubiksColor.ORANGE].Face[RubiksPosition.RIGHT])}|_|_|_|_|_|_");
		sb.AppendLine($"_ |_ |_|{PrintColor(m_rubiksData[RubiksColor.ORANGE].Face[RubiksPosition.BOTTOMLEFT])}|{PrintColor(m_rubiksData[RubiksColor.ORANGE].Face[RubiksPosition.BOTTOM])}|{PrintColor(m_rubiksData[RubiksColor.ORANGE].Face[RubiksPosition.BOTTOMRIGHT])}|_|_|_|_|_|_");

		sb.AppendLine($"{PrintColor(m_rubiksData[RubiksColor.GREEN].Face[RubiksPosition.TOPLEFT])}|{PrintColor(m_rubiksData[RubiksColor.GREEN].Face[RubiksPosition.TOP])}|{PrintColor(m_rubiksData[RubiksColor.GREEN].Face[RubiksPosition.TOPRIGHT])}" +
			$"|{PrintColor(m_rubiksData[RubiksColor.WHITE].Face[RubiksPosition.TOPLEFT])}|{PrintColor(m_rubiksData[RubiksColor.WHITE].Face[RubiksPosition.TOP])}|{PrintColor(m_rubiksData[RubiksColor.WHITE].Face[RubiksPosition.TOPRIGHT])}" +
			$"|{PrintColor(m_rubiksData[RubiksColor.BLUE].Face[RubiksPosition.TOPLEFT])}|{PrintColor(m_rubiksData[RubiksColor.BLUE].Face[RubiksPosition.TOP])}|{PrintColor(m_rubiksData[RubiksColor.BLUE].Face[RubiksPosition.TOPRIGHT])}" +
			$"|{PrintColor(m_rubiksData[RubiksColor.YELLOW].Face[RubiksPosition.TOPLEFT])}|{PrintColor(m_rubiksData[RubiksColor.YELLOW].Face[RubiksPosition.TOP])}|{PrintColor(m_rubiksData[RubiksColor.YELLOW].Face[RubiksPosition.TOPRIGHT])}");

		sb.AppendLine($"{PrintColor(m_rubiksData[RubiksColor.GREEN].Face[RubiksPosition.LEFT])}|{PrintColor(m_rubiksData[RubiksColor.GREEN].Face[RubiksPosition.MIDDLE])}|{PrintColor(m_rubiksData[RubiksColor.GREEN].Face[RubiksPosition.RIGHT])}" +
			$"|{PrintColor(m_rubiksData[RubiksColor.WHITE].Face[RubiksPosition.LEFT])}|{PrintColor(m_rubiksData[RubiksColor.WHITE].Face[RubiksPosition.MIDDLE])}|{PrintColor(m_rubiksData[RubiksColor.WHITE].Face[RubiksPosition.RIGHT])}" +
			$"|{PrintColor(m_rubiksData[RubiksColor.BLUE].Face[RubiksPosition.LEFT])}|{PrintColor(m_rubiksData[RubiksColor.BLUE].Face[RubiksPosition.MIDDLE])}|{PrintColor(m_rubiksData[RubiksColor.BLUE].Face[RubiksPosition.RIGHT])}" +
			$"|{PrintColor(m_rubiksData[RubiksColor.YELLOW].Face[RubiksPosition.LEFT])}|{PrintColor(m_rubiksData[RubiksColor.YELLOW].Face[RubiksPosition.MIDDLE])}|{PrintColor(m_rubiksData[RubiksColor.YELLOW].Face[RubiksPosition.RIGHT])}");

		sb.AppendLine($"{PrintColor(m_rubiksData[RubiksColor.GREEN].Face[RubiksPosition.BOTTOMLEFT])}|{PrintColor(m_rubiksData[RubiksColor.GREEN].Face[RubiksPosition.BOTTOM])}|{PrintColor(m_rubiksData[RubiksColor.GREEN].Face[RubiksPosition.BOTTOMRIGHT])}" +
			$"|{PrintColor(m_rubiksData[RubiksColor.WHITE].Face[RubiksPosition.BOTTOMLEFT])}|{PrintColor(m_rubiksData[RubiksColor.WHITE].Face[RubiksPosition.BOTTOM])}|{PrintColor(m_rubiksData[RubiksColor.WHITE].Face[RubiksPosition.BOTTOMRIGHT])}" +
			$"|{PrintColor(m_rubiksData[RubiksColor.BLUE].Face[RubiksPosition.BOTTOMLEFT])}|{PrintColor(m_rubiksData[RubiksColor.BLUE].Face[RubiksPosition.BOTTOM])}|{PrintColor(m_rubiksData[RubiksColor.BLUE].Face[RubiksPosition.BOTTOMRIGHT])}" +
			$"|{PrintColor(m_rubiksData[RubiksColor.YELLOW].Face[RubiksPosition.BOTTOMLEFT])}|{PrintColor(m_rubiksData[RubiksColor.YELLOW].Face[RubiksPosition.BOTTOM])}|{PrintColor(m_rubiksData[RubiksColor.YELLOW].Face[RubiksPosition.BOTTOMRIGHT])}");

		sb.AppendLine($"_ |_ |_|{PrintColor(m_rubiksData[RubiksColor.RED].Face[RubiksPosition.TOPLEFT])}|{PrintColor(m_rubiksData[RubiksColor.RED].Face[RubiksPosition.TOP])}|{PrintColor(m_rubiksData[RubiksColor.RED].Face[RubiksPosition.TOPRIGHT])}|_|_|_|_|_|_");
		sb.AppendLine($"_ |_ |_|{PrintColor(m_rubiksData[RubiksColor.RED].Face[RubiksPosition.LEFT])}|{PrintColor(m_rubiksData[RubiksColor.RED].Face[RubiksPosition.MIDDLE])}|{PrintColor(m_rubiksData[RubiksColor.RED].Face[RubiksPosition.RIGHT])}|_|_|_|_|_|_");
		sb.AppendLine($"_ |_ |_|{PrintColor(m_rubiksData[RubiksColor.RED].Face[RubiksPosition.BOTTOMLEFT])}|{PrintColor(m_rubiksData[RubiksColor.RED].Face[RubiksPosition.BOTTOM])}|{PrintColor(m_rubiksData[RubiksColor.RED].Face[RubiksPosition.BOTTOMRIGHT])}|_|_|_|_|_|_");

		return sb.ToString();
	}

	#endregion Print

	private Dictionary<RubiksColor, RubiksFace> m_rubiksData = new Dictionary<RubiksColor, RubiksFace>();
	private Dictionary<RubiksColor, RubiksColor> m_rubiksOppositeColor = new Dictionary<RubiksColor, RubiksColor>();
}