using UnityEngine;
using UnityEngine.EventSystems;

public class TouchPanel : MonoBehaviour, IPointerUpHandler, IEventSystemHandler, IPointerDownHandler, IGetInput
{
	public bool isDrag;

	private Vector2 _lastPos = Vector2.zero;

	private PointerEventData pointer;

	public void OnPointerDown(PointerEventData eventData)
	{
		pointer = eventData;
		isDrag = true;
		_lastPos = eventData.position;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		isDrag = false;
	}

	public bool MY_IsDrag()
	{
		return isDrag;
	}

	public float MY_GetDirection()
	{
		float num = pointer.position.x - _lastPos.x;
		_lastPos = pointer.position;
		return num / 5f;
	}
}
