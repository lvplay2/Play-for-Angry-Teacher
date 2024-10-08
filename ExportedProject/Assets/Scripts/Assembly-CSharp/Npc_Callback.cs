using UnityEngine;

public class Npc_Callback : MonoBehaviour
{
	private INpcEvent npcEvent;

	private void Start()
	{
		npcEvent = base.transform.parent.GetComponent<INpcEvent>();
	}

	private void OnTriggerEnter(Collider other)
	{
		if (other.tag == "Door" && (bool)other.GetComponent<Door>())
		{
			other.GetComponent<Door>().MY_OpenDoor(_isOn: true);
		}
		else
		{
			npcEvent.MY_MarkerEnter(other.transform.position);
		}
	}

	private void OnTriggerExit(Collider other)
	{
		if (other.tag == "Door" && (bool)other.GetComponent<Door>())
		{
			other.GetComponent<Door>().MY_OpenDoor(_isOn: false);
		}
	}
}
