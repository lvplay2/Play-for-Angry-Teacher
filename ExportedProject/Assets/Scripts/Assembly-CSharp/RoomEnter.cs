using UnityEngine;

public class RoomEnter : MonoBehaviour
{
	private Transform bsodaPos;

	private bool isExit;

	private void Awake()
	{
		bsodaPos = base.transform.GetChild(0);
	}

	public void MY_Enter()
	{
		if (!isExit)
		{
			isExit = true;
			Sing_Game.This.npcBSoda.MY_Enable(bsodaPos.position);
		}
		else
		{
			base.gameObject.SetActive(value: false);
			Sing_Game.This.npcBSoda.MY_Disable();
		}
	}

	public bool IsEnter()
	{
		return !isExit;
	}
}
