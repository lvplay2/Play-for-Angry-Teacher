using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Rule : MonoBehaviour
{
	public Image ruleFillImage;

	public Button stepButton;

	public Image toggleAutoImage;

	public Sprite toggleOnSprite;

	public Sprite toggleOffSprite;

	private TeacherController teacherContr;

	private CameraMove cameraMove;

	private float teacherMoveTime;

	private float fillValue;

	private float ruleSpeed;

	private bool autoStep = true;

	private bool stepClicked;

	private void Start()
	{
		teacherContr = Sing_Game.This.teacherContr;
		teacherMoveTime = Sing_Game.This.teacherMoveTime;
		cameraMove = Sing_Game.This.cameraMove;
	}

	public void MY_EnableRule()
	{
		stepButton.interactable = false;
		StartCoroutine("RuleTick");
	}

	public void MY_DisableRule()
	{
		stepButton.interactable = false;
		StopCoroutine("RuleTick");
		fillValue = 0f;
		ruleFillImage.fillAmount = fillValue;
	}

	public void MY_ChangeSpeed(float _speed)
	{
		ruleSpeed = _speed;
	}

	public void UA_StepClick()
	{
		stepClicked = true;
		if (autoStep)
		{
			UA_AutoToggleSwitch(isOn: false);
		}
	}

	public void UA_AutoToggleSwitch(bool isOn)
	{
		toggleAutoImage.sprite = (isOn ? toggleOnSprite : toggleOffSprite);
		autoStep = isOn;
		if (fillValue > 1f)
		{
			stepClicked = true;
		}
	}

	private IEnumerator RuleTick()
	{
		while (true)
		{
			stepButton.interactable = false;
			stepClicked = false;
			fillValue = 0f;
			ruleFillImage.fillAmount = fillValue;
			while (fillValue < 1f)
			{
				fillValue += Time.deltaTime * ruleSpeed;
				ruleFillImage.fillAmount = fillValue;
				yield return new WaitForEndOfFrame();
			}
			stepButton.interactable = true;
			if (!autoStep)
			{
				while (!stepClicked)
				{
					yield return new WaitForEndOfFrame();
				}
			}
			teacherContr.MY_SetMove();
			yield return new WaitForSeconds(teacherMoveTime);
			teacherContr.MY_SetStop();
			cameraMove.MY_CheckDistance();
		}
	}
}
