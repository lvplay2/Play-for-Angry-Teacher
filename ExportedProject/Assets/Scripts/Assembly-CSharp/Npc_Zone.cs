using UnityEngine;

public class Npc_Zone : MonoBehaviour
{
	private INpcZone npcZone;

	private void Start()
	{
		npcZone = base.transform.parent.GetComponent<INpcZone>();
	}

	private void OnTriggerEnter(Collider other)
	{
		npcZone.MY_ZoneEnter(other);
	}

	private void OnTriggerExit(Collider other)
	{
		npcZone.MY_ZoneExit(other);
	}

	private void OnTriggerStay(Collider other)
	{
		npcZone.MY_ZoneStay(other);
	}
}
