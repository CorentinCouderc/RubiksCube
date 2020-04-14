using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputSequenceHandler : MonoBehaviour
{
	public void OnEndEdit(string inputText)
	{
		m_inputSequence = inputText;
	}

	public void ApplyInputSequence()
	{
		if (!string.IsNullOrEmpty(m_inputSequence))
		{
			m_rubiksCubeManager.ApplySequence(m_inputSequence);
		}
	}

	public void ClearInputSequence()
	{
		m_inputSequence = "";
		m_inputFieldText.text = m_inputSequence;
		ResetButtons();
	}

	public void SelectColor(Button selectedButton)
	{
		TextMeshProUGUI textComponent = selectedButton.GetComponentInChildren<TextMeshProUGUI>();
		m_colorToAdd = textComponent.text[0];

		foreach (Button colorButton in m_colorButtons)
		{
			if (colorButton.GetComponentInChildren<TextMeshProUGUI>().text != textComponent.text)
			{
				colorButton.interactable = false;
			}
			else
			{
				if (m_alreadySelectedColor)
				{
					ResetButtons();
					return;
				}
			}
		}

		m_alreadySelectedColor = true;
		EnableDirectionButtons(true);
	}

	public void SelectDirection(Button selectedButton)
	{
		TextMeshProUGUI textComponent = selectedButton.GetComponentInChildren<TextMeshProUGUI>();
		m_directionToAdd = textComponent.text[0];

		foreach (Button directionButton in m_directionButtons)
		{
			if (directionButton.GetComponentInChildren<TextMeshProUGUI>().text != textComponent.text)
			{
				directionButton.interactable = false;
			}
		}

		if (m_colorToAdd != default(char) && m_directionToAdd != default(char))
		{
			AddToSequence(m_colorToAdd, m_directionToAdd);
			ResetButtons();
		}
	}

	private void AddToSequence(char color, char direction)
	{
		m_inputSequence += $"{color}{direction}_";
		m_inputFieldText.text = m_inputSequence;
	}

	private void EnableColorButtons(bool enable)
	{
		foreach (Button colorButton in m_colorButtons)
		{
			colorButton.interactable = enable;
		}
	}

	private void EnableDirectionButtons(bool enable)
	{
		foreach (Button directionButton in m_directionButtons)
		{
			directionButton.interactable = enable;
		}
	}

	private void ResetButtons()
	{
		m_alreadySelectedColor = false;
		EnableColorButtons(true);
		EnableDirectionButtons(false);
	}

	private void Start()
	{
		m_inputFieldText = GameObject.FindGameObjectsWithTag("InputField")[0].GetComponentInChildren<TextMeshProUGUI>();

		foreach (GameObject go in GameObject.FindGameObjectsWithTag("ColorButton"))
		{
			m_colorButtons.Add(go.GetComponentInChildren<Button>());
		}

		foreach (GameObject go in GameObject.FindGameObjectsWithTag("DirectionButton"))
		{
			m_directionButtons.Add(go.GetComponentInChildren<Button>());
		}
	}

	private string m_inputSequence = null;
	private TextMeshProUGUI m_inputFieldText = null;

	private char m_colorToAdd = default(char);
	private char m_directionToAdd = default(char);

	private List<Button> m_colorButtons = new List<Button>();
	private List<Button> m_directionButtons = new List<Button>();

	private bool m_alreadySelectedColor = false;

	[SerializeField]
	private RubiksCubeManager m_rubiksCubeManager = null;
}