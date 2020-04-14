using UnityEngine;
using UnityEngine.EventSystems;

public class ArrowButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
	public void OnPointerEnter(PointerEventData eventData)
	{
		m_buttonAnimator.SetTrigger(POINTER_ENTER);
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		m_buttonAnimator.SetTrigger(POINTER_EXIT);
	}

	public void OnPointerDown(PointerEventData eventData)
	{
		m_buttonAnimator.SetBool(POINTER_DOWN, true);
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		m_buttonAnimator.SetBool(POINTER_DOWN, false);
	}

	[SerializeField]
	private Animator m_buttonAnimator = null;

	private const string POINTER_ENTER = "Enter";
	private const string POINTER_EXIT = "Exit";
	private const string POINTER_DOWN = "Click";
}