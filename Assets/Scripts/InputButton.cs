using UnityEngine;
using UnityEngine.EventSystems;

public class InputButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	public void OnPointerEnter(PointerEventData eventData)
	{
		UpdateFaceToHighlight();
		if (!m_rubiksManager.IsApplyingSequence && !RubiksCubeRotation.IsRotating)
		{
			m_rubiksFaceRotation.SetHighlightMaterial(m_faceToHighlight, true, fromButton: true);
		}
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		m_rubiksFaceRotation.SetHighlightMaterial(m_faceToHighlight, false, fromButton: true);
	}

	#region Private

	private void UpdateFaceToHighlight()
	{
		if (m_rubiksManager.LeftFaceColor == m_faceColor)
		{
			m_faceToHighlight = RubiksFaceRotation.RubiksFace.LEFT;
		}
		else if (m_rubiksManager.Logic.RubiksOppositeColor[m_rubiksManager.LeftFaceColor] == m_faceColor)
		{
			m_faceToHighlight = RubiksFaceRotation.RubiksFace.LEFT_OPPOSITE;
		}
		else if (m_rubiksManager.RightFaceColor == m_faceColor)
		{
			m_faceToHighlight = RubiksFaceRotation.RubiksFace.RIGHT;
		}
		else if (m_rubiksManager.Logic.RubiksOppositeColor[m_rubiksManager.RightFaceColor] == m_faceColor)
		{
			m_faceToHighlight = RubiksFaceRotation.RubiksFace.RIGHT_OPPOSITE;
		}
		else if (m_rubiksManager.UpFaceColor == m_faceColor)
		{
			m_faceToHighlight = RubiksFaceRotation.RubiksFace.UP;
		}
		else if (m_rubiksManager.Logic.RubiksOppositeColor[m_rubiksManager.UpFaceColor] == m_faceColor)
		{
			m_faceToHighlight = RubiksFaceRotation.RubiksFace.BOTTOM;
		}
	}

	private RubiksFaceRotation.RubiksFace m_faceToHighlight = default(RubiksFaceRotation.RubiksFace);

	[SerializeField]
	private RubiksCubeManager m_rubiksManager = null;

	[SerializeField]
	private RubiksFaceRotation m_rubiksFaceRotation = null;

	[SerializeField]
	private RubiksColor m_faceColor = default(RubiksColor);

	#endregion Private
}