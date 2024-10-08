using UnityEngine;

public class StudentCallback : MonoBehaviour
{
	public StudentAI studentController;

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Door")
		{
			Door component = other.GetComponent<Door>();
			if (component != null)
			{
				component.MY_OpenDoor(_isOn: true);
			}
		}
		else if (other.tag == "Notebook")
		{
			studentController.MY_NotebookGet(other.transform.position);
		}
		else if (other.tag == "ExitDoor")
		{
			studentController.MY_GetExit(other.transform.position, other.name);
		}
		else if (other.tag == "PlayTime")
		{
			Sing_Game.This.playTime.MY_GetPlay();
		}
		else if (other.tag == "Teacher")
		{
			studentController.MY_Win();
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Door")
		{
			Door component = other.GetComponent<Door>();
			if (component != null)
			{
				component.MY_OpenDoor(_isOn: false);
			}
		}
	}
}
