using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class JoystickX : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IGetInput
{
	public Slider touchSlider;

	public bool isDrag;

	public bool isFloatValue;

	public void OnPointerDown(PointerEventData eventData)
	{
		isDrag = true;
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		isDrag = false;
		touchSlider.value = 0f;
	}

	public void SliderChangeValue()
	{
		if (!isFloatValue)
		{
			if (touchSlider.value < 0f)
			{
				touchSlider.value = -1f;
			}
			else if (touchSlider.value > 0f)
			{
				touchSlider.value = 1f;
			}
		}
	}

	public bool MY_IsDrag()
	{
		return isDrag;
	}

	public float MY_GetDirection()
	{
		return touchSlider.value;
	}
}
